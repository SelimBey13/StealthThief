using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject[] panels;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject mapSelectPanel;
    [SerializeField] private GameObject envanterPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuFirstButton;
    [SerializeField] private GameObject mapSelectFirstButton;
    [SerializeField] private GameObject envanterFirstButton;
    [SerializeField] private GameObject settingsFirstButton;
    public static event Action<GameObject> OnFirstButtonChanged;


    void Awake()
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        MainMenuPanel();
    }
    public void ShowPanel(GameObject panelToShow , GameObject firstButton)
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(panel == panelToShow);
        }
        Debug.Log("OnFirstButtonChanged tetiklendi, buton: " + (firstButton != null ? firstButton.name : "NULL"));
        OnFirstButtonChanged?.Invoke(firstButton);
    }

    public void MapSelectPanel()
    {
        ShowPanel(mapSelectPanel , mapSelectFirstButton);
    }
    public void EnvanterPanel()
    {
        ShowPanel(envanterPanel, envanterFirstButton);
    }
    public void MainMenuPanel()
    {
        ShowPanel(mainMenuPanel , mainMenuFirstButton);
    }
    public void SettingsPanel()
    {
        ShowPanel(settingsPanel , settingsFirstButton);
    }

    public void LoadMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        InputManager.Instance.SwitchToPlayerMap();
        
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


}
