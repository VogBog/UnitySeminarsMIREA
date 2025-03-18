public class RedMole : Mole
{
    protected override void OnHit()
    {
        gameManager.AddScore();
    }
}
