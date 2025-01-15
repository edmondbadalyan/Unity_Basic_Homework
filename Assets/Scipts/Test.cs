using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCalculator
{
    public void CalculateDamage(float baseDamage, int multiplier)
    {
        var result = baseDamage * multiplier;
        Debug.LogWarning($"Calculated Damage: {baseDamage} * {multiplier} = {result}");
    }
}
public class Test : MonoBehaviour
{

    public int multiplier = 10;
    public float baseDamage = 1.0f;
    public bool isDamaged = false;
    public DamageCalculator damageCalculator;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Base Damage (float): {baseDamage}");
        Debug.Log($"Multiplier (int): {multiplier}");
        Debug.Log($"Is Damaged (bool): {isDamaged}");

        damageCalculator = new DamageCalculator();
        damageCalculator.CalculateDamage(baseDamage, multiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
