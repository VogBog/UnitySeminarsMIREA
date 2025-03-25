using UnityEngine;

public class ButtonForPlatform : Button
{
    [SerializeField] private MovablePlatform platform;

    protected override void OnActivate()
    {
        platform.isActive = true;
    }
}
