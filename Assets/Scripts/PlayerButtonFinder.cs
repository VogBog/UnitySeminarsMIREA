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
        else
        {
            CheckHighlighter();
        }
    }

    private void CheckHighlighter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 6f, mask))
        {
            if (hit.collider.CompareTag("Button"))
            {
                Highlighter highlighter = hit.collider.GetComponent<Highlighter>();
                if (highlighter != null)
                {
                    highlighter.Highlight();
                }
            }
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
