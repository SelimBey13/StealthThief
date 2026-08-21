using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject[] panels;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject mapSelectPanel;
    [SerializeField] private GameObject envanterPanel;
    [SerializeField] private GameObject settingsPanel;

    void Awake()
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        MainMenuPanel();
    }
    public void ShowPanel(GameObject panelToShow)
    {
        foreach(GameObject panel in panels)
        {
            panel.SetActive(panel == panelToShow);
        }
    }

    public void MapSelectPanel()
    {
        ShowPanel(mapSelectPanel);
    }
    public void EnvanterPanel()
    {
        ShowPanel(envanterPanel);
    }
    public void MainMenuPanel()
    {
        ShowPanel(mainMenuPanel);
    }
    public void SettingsPanel()
    {
        ShowPanel(settingsPanel);
    }

    public void LoadMap(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        InputManager.Instance.SwitchToPlayerMap();
        
    }


}
