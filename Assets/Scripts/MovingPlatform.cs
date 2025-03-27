using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 5f;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float delta = Mathf.PingPong(Time.time * speed, distance * 2) - distance;
        transform.position = _startPosition + new Vector3(0, 0, delta);
    }
}
