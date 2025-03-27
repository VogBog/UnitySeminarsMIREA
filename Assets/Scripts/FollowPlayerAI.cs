using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
