using UnityEngine;

public class PlayerButtonFinder : MonoBehaviour
{
    [SerializeField] private LayerMask mask;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Interact();
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 6f, mask))
        {
            if(hit.collider.CompareTag("Button"))
            {
                hit.collider.GetComponent<Button>().Activate();
            }
        }
    }
}
