using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed;
        
        public bool IsOwner { get; private set; } = false;
        public Vector3 MoveAxis { get; private set; }
        public float Speed => _speed;
        
        public void Initialize(bool isOwner)
        {
            if (!isOwner)
                return;

            MoveAxis = Vector3.forward;
            
            FindFirstObjectByType<GameStarter.GameStarter>().Started += () =>
            {
                IsOwner = true;
            };
        }

        public bool Stop() => IsOwner = false;

        private void Update()
        {
            if (!IsOwner)
                return;
            
            var moveAxis = Vector3.zero;
            
            if(Input.GetKeyDown(KeyCode.W))
                moveAxis.z += 1;
            if(Input.GetKeyDown(KeyCode.S))
                moveAxis.z -= 1;
            if(Input.GetKeyDown(KeyCode.A))
                moveAxis.x -= 1;
            if(Input.GetKeyDown(KeyCode.D))
                moveAxis.x += 1;

            if (moveAxis.x == 0 && moveAxis.z == 0 ||
                Vector3.Angle(moveAxis, MoveAxis) > 100)
            {
                moveAxis = MoveAxis;
            }
            else
            {
                moveAxis.Normalize();
            }
            
            MoveAxis = moveAxis;
        }

        private void FixedUpdate()
        {
            if(!IsOwner)
                return;
            
            transform.position += _speed * Time.fixedDeltaTime * MoveAxis;
        }
    }
}