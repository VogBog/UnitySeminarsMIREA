using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RedMole molePrefab;
    [SerializeField] private GreenMole greenMolePrefab;
    [SerializeField] private int greenMoleSpawnChance;
    [SerializeField] private float createMoleTime;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float moleLiveTime;
    [SerializeField] private int rounds;

    private int score = 0;

    public void AddScore()
    {
        score++;
    }

    public void SubScore()
    {
        score--;
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

            Mole prefab = molePrefab;
            if(Random.Range(0, 100) < greenMoleSpawnChance)
            {
                prefab = greenMolePrefab;
            }

            var mole = Instantiate(prefab);
            mole.transform.position = pos;

            mole.gameManager = this;
            mole.StartTimer(moleLiveTime);

            yield return new WaitForSeconds(createMoleTime);
        }

        yield return new WaitForSeconds(moleLiveTime);

        Debug.Log("Конец! Вы прибили " + score + " кротов");
    }
}
