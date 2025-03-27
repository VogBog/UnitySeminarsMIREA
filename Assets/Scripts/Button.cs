using UnityEngine;

public abstract class Button : MonoBehaviour
{
    [SerializeField] private bool _isSwitch;

    private bool isActivated = false;

    public void Activate()
    {
        if (isActivated && !_isSwitch)
        {
            return;
        }

        isActivated = true;
        OnActivate();
    }

    protected abstract void OnActivate();
}
