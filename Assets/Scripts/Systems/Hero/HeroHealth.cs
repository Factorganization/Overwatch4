using UnityEngine;

public class HeroHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        GameUIManager.Instance.UpdateHealth();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
        GameUIManager.Instance.UpdateHealth();
    }

    private void Die()
    {
        Debug.Log("Hero has died.");
    }
}
