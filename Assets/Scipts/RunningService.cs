using UnityEngine;

public class RunningService : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator не найден");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetBool(IsRunning, true);
        }
    }
}