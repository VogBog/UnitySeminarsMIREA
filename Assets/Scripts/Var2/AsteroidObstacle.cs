using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Var2
{
    public class AsteroidObstacle : MonoBehaviour
    {
        public float Speed;
        
        private void Update()
        {
            transform.position += Speed * Time.deltaTime * Vector3.left;

            if (transform.position.x < -10f)
            {
                transform.position = new Vector3(10f, Random.Range(-5f, 5f), transform.position.z);
                Speed = Random.Range(0.5f, 2f);
                transform.localScale = Vector3.one * Random.Range(.2f, .4f);
            }
        }
    }
}