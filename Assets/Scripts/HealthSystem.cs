using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HealthSystem : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth = 100;

    public event EventHandler onDamage;
    public event EventHandler onDeath;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(int damageAmount)
    {
       
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        if (onDamage != null)
        {
            onDamage(this, EventArgs.Empty);
        }
       
        if (currentHealth == 0)
        {
            Die();
        }
    }
    private void Die()
    {
        if (onDeath != null)
        { 
            onDeath(this, EventArgs.Empty); 
        }
    }
    public float GetHealthPoints()
    {
        return  (float)currentHealth/maxHealth;
    }
}
