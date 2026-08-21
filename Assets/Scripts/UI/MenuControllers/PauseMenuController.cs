using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] bool isEscapeOn = false;

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
            }
            else
            {
                pausePanel.SetActive(false);
                InputManager.Instance.SwitchToPlayerMap();
                Time.timeScale = 1f;
            }
    }
}
