using System;
using System.Collections;
using UnityEngine;

public class ClosingScreen : MonoBehaviour
{
    public static ClosingScreen Instance{get; private set;}

    [SerializeField] private GameObject SuccessfulPanel;
    [SerializeField] private GameObject UnSuccessfulPanel;
    [SerializeField] private float closingTime;
    CanvasGroup canvasGroup;
    bool paintedScreenOnce = false;
    public event Action OnScenePainted;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SuccessfulPanel.SetActive(false);
        UnSuccessfulPanel.SetActive(false);
    }
    void OnEnable()
    {
        GameManager.OnChallengeCompletedSuccesfully += PaintScreen;
    }
    void OnDisable()
    {
        GameManager.OnChallengeCompletedSuccesfully -= PaintScreen;
    }
    void PaintScreen(bool value)
    {
        if(paintedScreenOnce){return;}

        if(value)
        {
            canvasGroup = SuccessfulPanel.GetComponent<CanvasGroup>();
            SuccessfulPanel.SetActive(true);
        }
        else
        {
            canvasGroup = UnSuccessfulPanel.GetComponent<CanvasGroup>();
            UnSuccessfulPanel.SetActive(true);
        }

        canvasGroup.alpha = 0f;
        StartCoroutine(ClosingSceneTimer());
        paintedScreenOnce = true;
    }

    IEnumerator ClosingSceneTimer()
    {
        float time = 0f;

        while(time < closingTime)
        {
            time += Time.deltaTime;
            canvasGroup.alpha += Time.deltaTime/closingTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        OnScenePainted?.Invoke();
    }
}
