using UnityEngine;

public class PlayerMovePC : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody _rb;
    private PlayerSteps _steps;
    private PlayerInput _playerInput;
    private float _x;
    private float _z;
    
    private float stepProgress = 0f;
    private float stepThreshold = 0.1f;
    private float stepInterval = 0.03f;
    private float lastStepTime = 0f;
    private float elapsedTime = 0f;
    
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _steps = GetComponent<PlayerSteps>();
    }

    private void Update()
    {
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
        
        // Проверяем, движется ли игрок
        if (input.magnitude > stepThreshold)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= stepInterval)
            {
                _steps.AddValue(input.magnitude * 0.08f);
                elapsedTime = 0f;
            }
        }
        else
        {
            // Сбрасываем прогресс шагов, если игрок стоит на месте
            elapsedTime = 0f;
        }
        
        
        /*// Проверяем, движется ли игрок
        if (input.magnitude > stepThreshold)
        {
            Debug.Log("БОЛЬШЕ!");
            // Уменьшаем частоту передачи данных
            if (Time.time - lastStepTime >= stepInterval)
            {
                _steps.AddValue(input.magnitude * 0.08f);
                lastStepTime = Time.time;
            }
        }
        else
        {
            // Сбрасываем прогресс шагов, если игрок стоит на месте
            stepProgress = 0f;
        }*/
        
        // _steps.AddValue(input.magnitude * 0.08f);
    }
}