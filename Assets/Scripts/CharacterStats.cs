using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strength;
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();


    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        // 从玩家的角度，对敌人造成的伤害
        int totalDamage = damage.GetValue() + strength.GetValue();

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        // 从敌人的角度，受到伤害
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
