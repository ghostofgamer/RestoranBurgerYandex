using UnityEngine;

public class PlayerMovePC : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody _rb;
    private PlayerSteps _steps;
    private PlayerInput _playerInput;
    private float _x;
    private float _z;
    
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _steps = GetComponent<PlayerSteps>();
    }

    private void Update()
    {
        /*// Получаем ввод с клавиатуры
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(moveHorizontal, moveVertical);*/
        // Move(input);
        Move();
    }

    private void Move()
    {
        _x = _playerInput.X;
        _z = _playerInput.Z;
        Vector2 input = new Vector2(_x, _z);
        
        Vector3 desiredMove = transform.TransformDirection(new Vector3(input.x, 0, input.y));
        var velocity = new Vector3(desiredMove.x * _speed, _rb.linearVelocity.y, desiredMove.z * _speed);
        _rb.linearVelocity = velocity;
        _steps.AddValue(input.magnitude * 0.08f);
    }
}