using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class HeroMovementController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private Camera _mainCamera;
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    
    [Header("Movement Settings")]
    [Tooltip("Скорость перемещения персонажа")]
    public float moveSpeed = 5f;
    [Tooltip("Скорость поворота персонажа (градусы в секунду)")]
    public float rotationSpeed = 10f;

    [Header("Jump & Gravity Settings")]
    [Tooltip("Сила прыжка")]
    public float jumpForce = 5f;
    [Tooltip("Гравитация, применяемая к персонажу")]
    public float gravity = -9.81f;
    [Tooltip("Дополнительное смещение вниз при нахождении на земле")]
    public float groundedOffset = -0.5f;

    // Вертикальная скорость (для прыжков и гравитации)
    private Vector3 _velocity;
    private bool _isGrounded;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError("CharacterController не найден на объекте " + gameObject.name);
        }
        
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator не найден на объекте " + gameObject.name);
        }
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        // Проверяем, находится ли персонаж на земле
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            // Сбрасываем вертикальную скорость для избежания накопления отрицательных значений
            _velocity.y = groundedOffset;
        }

        // Чтение ввода: WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = _mainCamera.transform.forward;
        Vector3 cameraRight = _mainCamera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        bool isMoving = moveDirection.sqrMagnitude > 0.1f;
        _animator.SetBool(IsRunningHash, isMoving);
        
        if (isMoving)
        {
            // Плавный поворот персонажа в сторону движения
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            
            _controller.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }
        // Прыжок: если персонаж на земле и нажата кнопка прыжка
        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _velocity.y = jumpForce;
            
        }
        // Применяем гравитацию
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
    
    private void OnControllerColliderHit(ControllerColliderHit  other)
    {
        // Сохраняем объекты с заданным компонентом 
        GiveHealth giveHealth = other.gameObject.GetComponent<GiveHealth>();

        if (giveHealth != null) // Если аптечка существует
        {
            // Уничтожаем аптечку
            Destroy(other.gameObject);
        }
    }
}
