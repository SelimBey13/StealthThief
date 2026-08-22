using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance{get;private set;}
    [SerializeField] GameObject pausePanel;
    [SerializeField] private GameObject firstButton;
    [SerializeField] bool isEscapeOn = false;
    public static event Action<GameObject> OnFirstButtonChanged;

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
        InputManager.OnPause += PauseInputs;
    }
    void OnDisable()
    {
        InputManager.OnPause -= PauseInputs;
    }

    public void FixIsEscapeOn() // resume butonuna basınca çalışır
    {
        isEscapeOn = false;

        SetPanelAvailable();
    }
    void PauseInputs(bool value)
    {
        if(value)
        {
            isEscapeOn = !isEscapeOn;

            SetPanelAvailable();
            MenuHelper.Instance.SetCursorForMenu();
        }
    }

    public void BackToMenu(string sceneName) // backtomenu butonu ile calısır
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
        InputManager.Instance.SwitchToMenuMap();
    }
    void SetPanelAvailable()
    {
        if(isEscapeOn)
            {
                pausePanel.SetActive(true);
                InputManager.Instance.SwitchToMenuMap();
                Time.timeScale = 0f;
                OnFirstButtonChanged?.Invoke(firstButton);
            }
            else
            {
                pausePanel.SetActive(false);
                InputManager.Instance.SwitchToPlayerMap();
                Time.timeScale = 1f;
            }
    }

    public GameObject PauseFirstButton()
    {
        return firstButton;
    }
}
