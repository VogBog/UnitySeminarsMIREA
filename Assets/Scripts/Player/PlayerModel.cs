using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerModel
    {
        [field: SerializeField] public Transform Model { get; private set; }
        [SerializeField] private Transform _characterControllerTransform;
        
        public MeshRenderer Renderer { get; private set; }

        public void Initialize(Player player)
        {
            Renderer = Model.GetComponent<MeshRenderer>();
            if (Renderer == null)
                throw new NullReferenceException("PlayerModel: Cannot find MeshRenderer on Model");
            
            player.Updated += Update;
        }

        private void Update()
        {
            Model.position = Vector3.Lerp(
                Model.position, _characterControllerTransform.position, Time.deltaTime / Time.fixedDeltaTime);
        }
    }
}