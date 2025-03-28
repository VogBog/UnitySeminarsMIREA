using UnityEngine;

public abstract class Button : MonoBehaviour
{
    [SerializeField] private bool isSwitch;

    private bool isActivated = false;

    public void Activate()
    {
        if (isActivated && !isSwitch)
        {
            return;
        }

        isActivated = true;
        OnActivate();
    }

    protected abstract void OnActivate();
}
