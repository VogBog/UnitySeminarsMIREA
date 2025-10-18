using System;
using Damage;
using Extensions;
using InputSystems;
using Lobby;
using MainGame;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private ScoreCounter _scoreCounter;
        
        [field: SerializeField] public Camera Camera { get; private set; }
        [field: SerializeField] public Movement Movement { get; private set; }
        [field: SerializeField] public PlayerModel Model { get; private set; }
        [field: SerializeField] public PlayerAbilityUsage AbilityUsage { get; private set; }
        [field: SerializeField] public PlayerMoveByTiles MoveByTiles { get; private set; }
        [field: SerializeField] public PlayerMarkerView Markers { get; private set; }
        [field: SerializeField] public PlayerGameUI GameUI { get; private set; }
        
        [field: SerializeField] public PlayerHealth Health { get; private set; }
        
        public PlayerInput Input { get; private set; }
        public PlayerHurtBox HurtBox { get; private set; }

        public Transform RealTransform => Movement.ControllerTransform;
        
        public event Action Updated, FixedUpdated;

        public void Initialize(PlayerData data)
        {
            Input = new PlayerInput(data.Index);
            Input.Device = data.Device;
            Movement.Initialize(this);
            Model.Initialize(this);
            AbilityUsage.Initialize(this, data.Data);
            MoveByTiles.Initialize(this);
            Markers.Initialize(this);
            GameUI.Initialize(this);
            Health.Initialize(this.FindFirstObjectByTypeOrException<EventBus>(), this);

            if (data.Index == 1)
                _scoreCounter = new(this);

            HurtBox = GetComponentInChildren<PlayerHurtBox>();
            
            SetMaterial(Model.Renderer, data.Data.Color);

            Health.Died += OnDie;
        }

        private void SetMaterial(MeshRenderer renderer, Color color)
        {
            var material = Instantiate(renderer.sharedMaterial);
            material.color = color;
            renderer.sharedMaterial = material;

            var markerRenderer = Markers.HeavyMarker.GetComponent<MeshRenderer>();
            markerRenderer.sharedMaterial = material;
            
            material = Instantiate(material);
            material.color = Color.grey;
            
            markerRenderer = Markers.Marker.GetComponent<MeshRenderer>();
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

        private void OnDie(PlayerHealth playerHealth)
        {
            Camera.transform.SetParent(null);
            Destroy(gameObject);
        }

        public void TakeDamage(ref GetDamageData data) => Health.TakeDamage(ref data);
    }
}