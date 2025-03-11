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

            transform.Rotate(transform.up * x * rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * y * moveSpeed * Time.deltaTime;
        }
    }
}
