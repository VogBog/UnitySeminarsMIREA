using System.Collections;
using UnityEngine;

public abstract class Mole : MonoBehaviour
{
    public GameManager gameManager;

    private Coroutine timer;

    private void OnMouseDown()
    {
        OnHit();
        StopCoroutine(timer);
        Destroy(gameObject);
    }

    protected abstract void OnHit();

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
