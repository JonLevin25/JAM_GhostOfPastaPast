using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityOnHit : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _inivincibleTime;
    
    private void Start()
    {
        _health.OnHpChanged += OnHpChanged;
    }

    private void OnHpChanged(int currhp, int totalhp, int delta)
    {
        if (delta < 0f)
        {
            StartCoroutine(SetInvincible(_health, _inivincibleTime));
        }
    }

    private IEnumerator SetInvincible(Health health, float inivincibleTime)
    {
        health.IsInvincible = true;
        yield return new WaitForSeconds(inivincibleTime);
        health.IsInvincible = false;
    }
}
