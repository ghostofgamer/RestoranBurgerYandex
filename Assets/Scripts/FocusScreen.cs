using Agava.WebUtility;
using UnityEngine;

public class FocusScreen : MonoBehaviour
{
    private int _stop = 0;
    private int _play = 1;
    private bool _isWork;

    private void OnEnable()
    {
        Application.focusChanged += OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
    }

    private void OnDisable()
    {
        Application.focusChanged -= OnInBackgroundChangeApp;
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
    }

    private void OnInBackgroundChangeApp(bool inApp)
    {
        SetValueAudio(!inApp);
        PauseGame(!inApp);
    }

    private void OnInBackgroundChangeWeb(bool isBackground)
    {
        SetValueAudio(isBackground);
        PauseGame(isBackground);
    }

    private void SetValueAudio(bool value)
    {
        AudioListener.pause = value;
    }

    private void PauseGame(bool value)
    {
        if (!_isWork) return;
        Time.timeScale = value ? _stop : _play;
    }

    public void SetValueWork(bool value)
    {
        _isWork = value;
    }
}
