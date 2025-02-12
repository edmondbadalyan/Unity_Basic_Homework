using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class MoveRigidBody : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    [Header("Movement Settings")]
    [Tooltip("Сила, применяемая для перемещения персонажа")]
    public float moveForce = 10f;
    [Tooltip("Максимальная горизонтальная скорость")]
    public float maxSpeed = 5f;
    [Tooltip("Скорость поворота персонажа (градусы в секунду)")]
    public float rotationSpeed = 720f;

    [Header("Jump Settings")]
    [Tooltip("Сила прыжка, применяемая к персонажу")]
    public float jumpForce = 5f;
    [Tooltip("Расстояние для проверки наличия земли")]
    public float groundCheckDistance = 0.2f;
    [Tooltip("Слой, обозначающий землю")]
    public LayerMask groundLayer;

    private Vector3 _input;
    //private bool _isTargetDestroyed = false;
    private bool _jumpPressed;

    
    private Vector3 HorizontalVelocity => new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("Rigidbody не найден на объекте " + gameObject.name);
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator не найден на объекте " + gameObject.name);
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _input = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
        _animator.SetBool(IsRunningHash, _input.sqrMagnitude > 0.01f);
    }

    private void FixedUpdate()
    {
        if (_input.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_input);
            _rb.rotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Применяем силу для движения, если текущая горизонтальная скорость ниже maxSpeed
            if (HorizontalVelocity.magnitude < maxSpeed)
            {
                _rb.AddForce(_input * moveForce, ForceMode.VelocityChange);
            }
            else
            {
                // Ограничиваем горизонтальную скорость до maxSpeed
                Vector3 clampedVelocity = HorizontalVelocity.normalized * maxSpeed;
                _rb.velocity = new Vector3(clampedVelocity.x, _rb.velocity.y, clampedVelocity.z);
            }
        }

        // Проверяем, находится ли персонаж на земле
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);

        if (isGrounded)
        {
            _rb.useGravity = false;
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        }
        else
        {
            _rb.useGravity = true;
        }
        
        
        // Прыжок: если персонаж на земле и нажата кнопка прыжка
        if (isGrounded && _jumpPressed)
        {
            // Сбрасываем вертикальную скорость для стабильного прыжка
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _rb.useGravity = true;
        }
        _jumpPressed = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        // Сохраняем объекты с заданным компонентом 
        GiveHealth giveHealth = other.gameObject.GetComponent<GiveHealth>();

        if (giveHealth != null) // Если аптечка существует
        {
            // Уничтожаем аптечку
            Destroy(other.gameObject);

            // Устанавливаем флаг, что аптечка уничтожена
            //_isTargetDestroyed = true;
        }
    }
}
