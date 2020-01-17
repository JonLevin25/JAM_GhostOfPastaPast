using System;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = target as Health;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Space();
        EditorGUILayout.IntField("Curr HP", script.CurrentHP);
        EditorGUI.EndDisabledGroup();
    }
}
#endif

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