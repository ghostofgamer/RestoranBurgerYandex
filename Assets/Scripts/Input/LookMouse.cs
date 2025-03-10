using System;
using TheSTAR.Data;
using TheSTAR.GUI;
using UnityEngine;
using Zenject;

public class LookMouse : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private float _sensivity;

    private float _mouseX;
    private float _mouseY;
    private float _xRotation;
    private float _xOffset = 0;
    private float _yOffset = 0;
    private float _angleRotation = 90f;
    private bool _islookedLock = true;
    
    private TutorialController _tutorialController;
    private GameScreen _gameScreen;
    private bool _firstLook = false;
    private CameraController _camera; 
    
    public event Action FirstLookChanged;
    
    [Inject]
    private void Constuct(TutorialController tutor,CameraController camera)
    {
        _tutorialController = tutor;
        _camera = camera;
    }
    
    private void Start()
    {
        // _tutorialController = FindObjectOfType<TutorialController>();
        
        // Cursor.lockState = CursorLockMode.Locked;

        if (Application.isMobilePlatform)
            enabled = false;
    }

    public void SetValue(bool value)
    {
        _islookedLock = value;
    }
    
    public void Rotate(float mouseX, float mouseY)
    {
        if (_islookedLock) return;
        
        // FirstLookChanged?.Invoke();
        
        if (!_firstLook&&(mouseX>0||mouseY>0))
        {
            FirstLookChanged?.Invoke();
            // _tutorialController.CompleteTutorial(TutorialType.LookAround);
            _firstLook = true;
        }
        
#if UNITY_WEBGL && !UNITY_EDITOR
 _mouseX = mouseX * (_sensivity * 0.3f) * Time.deltaTime + _xOffset * Time.deltaTime;
        _mouseY = mouseY * (_sensivity * 0.3f) * Time.deltaTime + _yOffset * Time.deltaTime;
        #else
        _mouseX = mouseX * _sensivity * Time.deltaTime + _xOffset * Time.deltaTime;
        _mouseY = mouseY * _sensivity * Time.deltaTime + _yOffset * Time.deltaTime;
#endif
        
        _xOffset = 0f;
        _yOffset = 0f;
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_angleRotation, _angleRotation);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        _body.Rotate(Vector3.up * _mouseX);
    }

    public void ChangeOffset(float x, float y)
    {
        _xOffset = x;
        _yOffset = y;
    }
}