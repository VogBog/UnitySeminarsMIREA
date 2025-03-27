using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;

    public bool isActive = false;

    private Vector3 nextPos;
    private int pointIndex = 0;
    private PlayerMovement _player;

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
                Vector3 moveVec = direction * speed * Time.deltaTime;
                transform.position += moveVec;

                if(_player != null)
                {
                    _player.characterController.Move(moveVec);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _player = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            _player = null;
        }
    }
}
