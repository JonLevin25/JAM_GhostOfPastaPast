using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _startHP;

    private void Awake() => _currHP = _startHP;

    public event Action OnDeath;

    private int _currHP;
    private bool _isDead;

    public int CurrentHP
    {
        get => _currHP;
        private set
        {
            _currHP = value;
            if (_currHP < 0f && !_isDead) Kill();
        } 
    }

    private void Kill()
    {
        _isDead = true;
        OnDeath?.Invoke();
    }

    public void TakeDamage(int hp)
    {
        CurrentHP -= hp;
    }

    public void Heal(int hp)
    {
        CurrentHP += hp;
    }
}