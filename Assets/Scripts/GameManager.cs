using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Mole molePrefab;
    [SerializeField] private float createMoleTime;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float moleLiveTime;
    [SerializeField] private int rounds;

    private int score = 0;

    public void HitMole()
    {
        score++;
    }

    private void Start()
    {
        StartCoroutine(CreateMoles());
    }

    IEnumerator CreateMoles()
    {
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < rounds; i++)
        {
            var randIndex = Random.Range(0, spawnPoints.Length);
            var pos = spawnPoints[randIndex].position;

            var mole = Instantiate(molePrefab);
            mole.transform.position = pos;

            mole.gameManager = this;
            mole.StartTimer(moleLiveTime);

            yield return new WaitForSeconds(createMoleTime);
        }

        yield return new WaitForSeconds(moleLiveTime);

        Debug.Log("КОНЕЦ! Вы прибили " + score + " кротов");
    }
}
