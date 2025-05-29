using UnityEngine;

namespace Practice4
{
    public class CubeMover : MonoBehaviour
    {
        public float moveSpeed, rotationSpeed;

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            transform.Rotate(rotationSpeed * Time.deltaTime * transform.up);
            transform.position += y * moveSpeed * Time.deltaTime * transform.forward;
        }
    }
}
