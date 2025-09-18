using System;
using InputSystems;
using Lobby;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Movement Movement { get; private set; }
        [field: SerializeField] public PlayerModel Model { get; private set; }
        
        public PlayerInput Input { get; private set; }
        
        public event Action Updated, FixedUpdated;

        public void Initialize(PlayerData data)
        {
            Input = new PlayerInput(data.Index);
            Movement.Initialize(this);
            Model.Initialize(this);
            
            SetMaterial(Model.Renderer, data.Data.Color);
        }

        private void SetMaterial(MeshRenderer renderer, Color color)
        {
            var material = Instantiate(renderer.sharedMaterial);
            material.color = color;
            renderer.sharedMaterial = material;

            var markerRenderer = Movement.Marker.GetComponent<MeshRenderer>();
            markerRenderer.sharedMaterial = material;
        }

        private void Update()
        {
            Updated?.Invoke();
        }

        private void FixedUpdate()
        {
            FixedUpdated?.Invoke();
        }

        private void OnDestroy()
        {
            Input.Dispose();
        }
    }
}