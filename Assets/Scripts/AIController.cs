using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }
}
