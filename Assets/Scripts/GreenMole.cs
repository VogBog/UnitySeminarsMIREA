public class GreenMole : Mole
{
    protected override void OnHit()
    {
        gameManager.SubScore();
    }
}
