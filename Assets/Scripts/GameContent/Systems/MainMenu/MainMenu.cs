using System;
using Systems.Persistence;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIDocument mainMenu;
    
    private Button newGameButton, loadGameButton, settingsButton, quitButton;

    private void Awake()
    {
        newGameButton = mainMenu.rootVisualElement.Q<Button>("NewGameButton");
        newGameButton.RegisterCallback<ClickEvent>(NewGame);
        
        /*loadGameButton = mainMenu.rootVisualElement.Q<Button>("LoadButton");
        loadGameButton.RegisterCallback<ClickEvent>(LoadGame);
        
        settingsButton = mainMenu.rootVisualElement.Q<Button>("SettingsButton");
        settingsButton.RegisterCallback<ClickEvent>(OpenSettings);*/
        
        quitButton = mainMenu.rootVisualElement.Q<Button>("QuitButton");
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
    }

    private void LoadGame(ClickEvent evt)
    {
        Debug.Log("Load Game");
    }

    public void NewGame(ClickEvent evt)
    {
        SceneManager.LoadScene("Bootstrapper");
    }
    
    public void OpenSettings(ClickEvent evt)
    {
        Debug.Log("Open Settings");
    }
    
    public void QuitGame(ClickEvent evt)
    {
        Debug.Log("Quit Game");
    }
}
