using UnityEngine;

public class SwitchPlatform : MonoBehaviour
{
    public PlatformBase[] activate;
    public PlatformBase[] deactivate;

    private bool state;

    public void Toggle()
    {
        state = !state;

        foreach (var p in activate)
            p.SetActive(state);

        foreach (var p in deactivate)
            p.SetActive(!state);
    }
}
