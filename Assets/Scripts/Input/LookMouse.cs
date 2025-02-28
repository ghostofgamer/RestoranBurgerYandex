using UnityEngine;

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
    private PlayerInput _playerInput;

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    public void Rotate(float mouseX, float mouseY)
    {
        _mouseX = mouseX * _sensivity * Time.deltaTime + _xOffset * Time.deltaTime;
        _mouseY = mouseY * _sensivity * Time.deltaTime + _yOffset * Time.deltaTime;
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