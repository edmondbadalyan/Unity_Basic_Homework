using Assets.Scipts;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int Multiplier = 10;
    public float BaseDamage = 1.0f;
    public bool IsDamaged = false;
    private DamageCalculator _damageCalculator;

    public void Start()
    {
        Debug.Log($"Base Damage (float): {BaseDamage}");
        Debug.Log($"Multiplier (int): {Multiplier}");
        Debug.Log($"Is Damaged (bool): {IsDamaged}");

        _damageCalculator = new DamageCalculator();
        _damageCalculator.CalculateDamage(BaseDamage, Multiplier);
    }
}
