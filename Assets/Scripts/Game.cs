using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private ulong score = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            score++;
            text.text = score.ToString();
        }
    }
}