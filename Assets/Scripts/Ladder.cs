using UnityEngine;

public class Ladder : MonoBehaviour
{
    private CharacterController controller;
    private PlayerMovement playerMovement;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller = other.gameObject.GetComponent<CharacterController>();
            playerMovement = other.gameObject.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            controller = null;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && controller != null)
        {
            playerMovement.velocity = Vector3.zero;
            controller.Move(5f * Time.deltaTime * Vector3.up);
        }
    }
}
