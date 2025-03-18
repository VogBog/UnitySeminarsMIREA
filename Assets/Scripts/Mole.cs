using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public GameManager gameManager;

    private Coroutine timer;

    private void OnMouseDown()
    {
        gameManager.HitMole();
        StopCoroutine(timer);
        Destroy(gameObject);
    }

    public void StartTimer(float time)
    {
        timer = StartCoroutine(Timer(time));
    }

    IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
