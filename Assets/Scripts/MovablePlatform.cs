using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;

    public bool isActive = false;

    private Vector3 nextPos;
    private int pointIndex = 0;

    private void Start()
    {
        nextPos = points[0].position;
    }

    private void Update()
    {
        if (isActive)
        {
            if(Vector3.Distance(transform.position, nextPos) < .1f)
            {
                pointIndex = (pointIndex + 1) % points.Length;
                nextPos = points[pointIndex].position;
            }
            else
            {
                Vector3 direction = (nextPos - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
}
