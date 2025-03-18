using UnityEngine;

public class Colored : MonoBehaviour
{
    private Color[] colors;
    private Renderer renderer;
    private int colorIndex;
    private GameManager gameManager;

    public void Initialize(Color[] colors, GameManager gameManager)
    {
        this.gameManager = gameManager;
        this.colors = colors;
        renderer = GetComponent<Renderer>();
        colorIndex = Random.Range(0, colors.Length);
        renderer.material.color = colors[colorIndex];
    }

    private void OnMouseDown()
    {
        colorIndex = (colorIndex + 1) % colors.Length;
        renderer.material.color = colors[colorIndex];
        gameManager.ColoredChangeColor();
    }

    public Color GetCurrentColor()
    {
        return colors[colorIndex];
    }
}
