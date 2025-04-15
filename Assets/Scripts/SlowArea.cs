using UnityEngine;
using UnityEngine.AI;

public class SlowArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NavMeshAgent agent))
        {
            agent.speed /= 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NavMeshAgent agent))
        {
            agent.speed *= 2f;
        }
    }
}
