using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _healthPrefab;
    
    void Start()
    {
        if (_healthPrefab != null)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 3f), 0.5f, Random.Range(-10f, 3f));
            Instantiate(_healthPrefab, randomPosition, Quaternion.identity);
            
        }
    }

    
}
