using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [SerializeField] private LookMouse _lookMouse;

    private PlayerMovePC _playerMovement;
    private float _mouseX;
    private float _mouseY;
    
    public float X { get; private set; }

    public float Z { get; private set; }

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovePC>();

        if (Application.isMobilePlatform)
            enabled = false;
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            _mouseX = Input.GetAxis(MouseX);
            _mouseY = Input.GetAxis(MouseY);
            _lookMouse.Rotate(_mouseX, _mouseY);
            X = Input.GetAxis(Horizontal);
            Z = Input.GetAxis(Vertical);
        }
    }

}
