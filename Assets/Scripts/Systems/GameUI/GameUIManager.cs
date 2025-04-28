using System;
using Systems;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _pauseMenu;
    
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _gameUI.SetActive(true);
        _pauseMenu.SetActive(false);
    }
    
    public void UpdateHealth()
    {
        if (Hero.Instance.Health != null)
        {
            _healthText.text = $"Health: {Hero.Instance.Health.CurrentHealth}/{Hero.Instance.Health.MaxHealth}";
        }
    }

    public void TogglePauseMenu()
    {
        bool isActive = _pauseMenu.activeSelf;
        _pauseMenu.SetActive(!isActive);
        Time.timeScale = isActive ? 1 : 0; // Pause or resume the game
    }
}
