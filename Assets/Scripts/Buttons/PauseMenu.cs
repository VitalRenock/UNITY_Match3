using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool _paused = false;

    public void TogglePause()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        _paused = !_paused;
    }
}
