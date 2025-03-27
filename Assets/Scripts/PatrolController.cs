using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent _agent;
    private int _currentWaypoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentWaypoint = 0;
        SetNextWaypoint();
    }

    private void Update()
    {
        if(_agent.remainingDistance < .5f)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        _agent.SetDestination(waypoints[_currentWaypoint].position);
        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
    }
}
