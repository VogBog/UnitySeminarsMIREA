using System.Collections;
using Damage;
using Data;
using GridMap;
using Pool;
using UnityEngine;

namespace Player.Abilities.Thunder
{
    public class ThunderProjectile : MonoBehaviour
    {
        public const float ThunderShowTime = .2f;
        
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private LineRenderer _secondLineRenderer;
        [SerializeField] private LineRenderer _hugeLineRenderer;
        
        private ObjectPool _pool;
        private GridMap.GridMap _gridMap;
        
        public void SetPool(ObjectPool pool)
        {
            _pool = pool;
            _gridMap = Extensions.ObjectExtensions.FindFirstObjectByTypeOrException<GridMap.GridMap>();
        }

        public void Attack(Player player, ThunderAbilityData data, Vector3 direction)
        {
            var ray = new Ray(player.RealTransform.position, direction);
            if (!Physics.Raycast(ray, out var hit, data.QuickFirstDistance))
            {
                var endPos = ray.origin + ray.direction * data.QuickFirstDistance;
                DrawThunder(_lineRenderer, ray.origin, endPos);
                SetQuickThunder(endPos);
                return;
            }
            
            DrawThunder(_lineRenderer, ray.origin, hit.point);
            SetQuickThunder(hit.point);

            if (!hit.collider.TryGetComponent<IDamageable>(out var damageable))
                return;

            DealDamage(player, damageable, hit.collider.gameObject, data.QuickDamage);
            
            var colliders = Physics.OverlapSphere(hit.point, data.QuickSecondDistance);
            float minDist = float.MaxValue;
            (GameObject, IDamageable) target = (null, null);
            
            foreach (var collider in colliders)
            {
                if(!collider.TryGetComponent<IDamageable>(out var iDamageable) || damageable == iDamageable ||
                   player.HurtBox == iDamageable) 
                    continue;
                
                float dist = Vector3.Distance(hit.point, collider.transform.position);
                if(dist > minDist)
                    continue;
                
                var ray2 = new Ray(hit.point, collider.transform.position - hit.point);
                if(!Physics.Raycast(ray2, out var hit2, dist + 1f) ||
                   hit2.collider != collider)
                    continue;
                
                minDist = dist;
                target = (collider.gameObject, iDamageable);
            }

            if (target.Item1 == null)
            {
                var rand = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
                var pos = hit.point + rand;
                DrawThunder(_secondLineRenderer, hit.point, pos);
                SetQuickThunder(pos);
                return;
            }
            
            DealDamage(player, target.Item2, target.Item1, data.QuickDamage);
            SetQuickThunder(target.Item1.transform.position);
            
            DrawThunder(_secondLineRenderer, hit.point, target.Item1.transform.position);
        }

        public void HeavyAttack(Player player, ThunderAbilityData data, Vector3 direction)
        {
            var center = player.RealTransform.position + direction * data.HeavyDistance / 2f;
            var halfExtends = new Vector3(
                data.HeavyWidth / 2f,
                2f,
                data.HeavyDistance / 2f);
            var rotation = Quaternion.LookRotation(direction, Vector3.up);

            var colliders = Physics.OverlapBox(center, halfExtends, rotation);
            foreach (var collider in colliders)
            {
                if(!collider.TryGetComponent<IDamageable>(out var damageable) ||
                   damageable == player.HurtBox)
                    continue;

                DealDamage(player, damageable, collider.gameObject, data.HeavyDamage);
            }

            var rects = _gridMap.FromWorldRectToIndexesRect(
                center.x, center.z, halfExtends.z * 2f, halfExtends.x * 2f, direction, GridMapValues.Thunder10Seconds);
            _gridMap.SetCellsAsync(rects);
            DrawThunder(_hugeLineRenderer,
                player.RealTransform.position,
                player.RealTransform.position + direction * data.HeavyDistance);
        }

        private void DrawThunder(LineRenderer line, Vector3 from, Vector3 to)
        {
            int maxCount = line.positionCount;
            line.SetPosition(0, from);
            line.SetPosition(maxCount - 1, to);

            var direction = to - from;

            for (int i = 1; i < maxCount - 1; i++)
            {
                float mp = (float)i / (maxCount - 1);
                var noise = new Vector3(
                    Random.Range(-.3f, .3f),
                    0f,
                    Random.Range(-.3f, .3f)
                );
                
                line.SetPosition(i, from + direction * mp + noise);
            }
            
            line.gameObject.SetActive(true);

            StartCoroutine(DeActiveLineDelayed(line, ThunderShowTime));
        }

        private IEnumerator DeActiveLineDelayed(LineRenderer line, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            line.gameObject.SetActive(false);
        }

        private void DealDamage(Player player, IDamageable damageable, GameObject damageableObject, int defaultDamage)
        {
            var (x, y) = _gridMap.FromWorldPositionToIndexes(
                damageableObject.transform.position.x, damageableObject.transform.position.z);
            byte cell = _gridMap.GetCell(x, y);
            if ((cell is GridMapValues.Steam or GridMapValues.Water or 
                >= GridMapValues.ThunderWater5Seconds and <= GridMapValues.ThunderWater30Seconds)
                && Random.Range(0, 2) == 0)
                defaultDamage++;

            var damageData = new GetDamageData(defaultDamage, player.gameObject, Elementals.Thunder);
            damageable.TakeDamage(ref damageData);
        }

        private void SetQuickThunder(Vector3 pos)
        {
            var (x, y) = _gridMap.FromWorldPositionToIndexes(pos.x, pos.z);
            _gridMap.SetCell(x, y, GridMapValues.QuickThunder);
        }
    }
}