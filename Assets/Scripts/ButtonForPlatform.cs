using UnityEngine;

public class ButtonForPlatform : Button
{
    [SerializeField] private MovablePlatform platform;

    protected override void OnActivate()
    {
        platform.isActive = !platform.isActive;
        Debug.Log($"Set platform active to {platform.isActive}");
    }
}
