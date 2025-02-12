using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform target; // Цель (аптечка)
    public float rotationSpeed = 2.0f; //скорость вращения 
    public float moveSpeed = 2.0f; // скорость движения

    private Vector3 _lastKnownTargetPosition; // Последняя известная позиция цели
    private bool _isTargetDestroyed = false; // Проверка что цель уничтожена

    private void Update()
    {
        // Если цель существует, обновляем последнюю известную позицию
        if (target != null)
        {
            _lastKnownTargetPosition = target.position;
        }

        // Вычисляем направление к последней известной позиции цели
        
        Vector3 direction = _lastKnownTargetPosition - transform.position;

        // Если цель уничтожена и мы достигли последней известной позиции, останавливаемся
        if (_isTargetDestroyed && direction.sqrMagnitude < 0.1f)
        {
            return; // Прекращаем движение
        }

        // Поворачиваемся к цели
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Двигаемся вперед
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
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
            _isTargetDestroyed = true;

            // Очищаем ссылку на цель?
            target = null;
        }
    }
}