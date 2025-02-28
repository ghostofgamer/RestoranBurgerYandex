using UnityEngine;

public class KeyboardInputPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Rigidbody _rb;
    private PlayerSteps _steps;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _steps = GetComponent<PlayerSteps>();
    }

    private void Update()
    {
        // Получаем ввод с клавиатуры
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(-moveHorizontal, -moveVertical);
        KeyboardInputMethod(input);
    }

    private void KeyboardInputMethod(Vector2 input)
    {
        Debug.Log("INPUT " + input);
        var velocity = new Vector3(input.x * _speed, _rb.linearVelocity.y, input.y * _speed);
        _rb.linearVelocity = velocity;
        _steps.AddValue(input.magnitude * 0.08f);
    }
}