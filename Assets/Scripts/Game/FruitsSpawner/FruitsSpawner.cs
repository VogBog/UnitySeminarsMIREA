using System.Collections;
using Base;
using UnityEngine;

namespace Game.FruitsSpawner
{
    public class FruitsSpawner : MonoBehaviour
    {
        [SerializeField] private Fruit _fruitPrefab;
        
        private IFruitsSpawner _spawner;
        
        private void Awake()
        {
            _spawner = GameServices.CreateFruitsSpawner();
            _spawner.SetPrefab(_fruitPrefab);
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                if (_spawner.IsServer())
                {
                    var randX = Random.Range(-40f, 40f);
                    var randZ = Random.Range(-40f, 40f);
                    var pos = new Vector3(randX, 0, randZ);
                    
                    _spawner.Instantiate(pos);
                }
                
                yield return new WaitForSeconds(2f);
            }
        }

        public void EatenBy(Fruit fruit, Player.Player player)
        {
            if (!_spawner.IsServer())
                return;
            
            _spawner.Despawn(fruit);
            player.SnakeTail.AddLength();
        }
    }
}