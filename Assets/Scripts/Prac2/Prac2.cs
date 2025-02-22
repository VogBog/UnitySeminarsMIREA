using UnityEngine;

namespace Practice2
{
    public class Prac2 : MonoBehaviour
    {
        public GameObject objectToMove;
        public GameObject[] points;

        public void Update()
        {
            if (Random.Range(0, 10) == 5)
            {
                int random = Random.Range(0, points.Length);
                objectToMove.transform.position = points[random].transform.position;
            }
        }
    }
}
