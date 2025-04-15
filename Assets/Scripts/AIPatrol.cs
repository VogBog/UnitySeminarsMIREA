using UnityEngine;
using UnityEngine.AI;

public class AIPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private NavMeshAgent agent;
    private int pointIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance * 2f)
        {
            pointIndex = (pointIndex + 1) % points.Length;
            agent.SetDestination(points[pointIndex].position);
        }
    }
}
