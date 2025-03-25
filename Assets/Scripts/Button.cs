using UnityEngine;

public abstract class Button : MonoBehaviour
{
    private bool isActivated = false;

    public void Activate()
    {
        if (isActivated)
        {
            return;
        }

        isActivated = true;
        OnActivate();
    }

    protected abstract void OnActivate();
}
