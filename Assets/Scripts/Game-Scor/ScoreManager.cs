using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}

    void Awake()
    {
        AwakingController();
    }

    [SerializeField] private float totalChallangeMoney =0f;
    [SerializeField] private int collectedValuable;
    [SerializeField] private float accountMoney; //save sistemi gelince ayarlanacak
    [SerializeField] private float employerTax; // 0-1
    bool challangeCompleted = false;
    public static event Action<float> OnAccountMoneyChanged;

    void OnEnable()
    {
        ValuableInteractable.OnItemTaken += SetChallangeMoney;
        GameManager.OnChallengeCompleted += ControlChallengeSuccess;
    }

    void OnDisable()
    {
        ValuableInteractable.OnItemTaken -= SetChallangeMoney;
        GameManager.OnChallengeCompleted -= ControlChallengeSuccess;
    }

    void DepositCollectedMoney()
    {
            accountMoney += totalChallangeMoney*(1f-employerTax);
            OnAccountMoneyChanged?.Invoke(accountMoney);
    }

    void SetChallangeMoney(float value)
    {
        totalChallangeMoney += value;
    }
    void AwakingController()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void ControlChallengeSuccess(bool value)
    {
        challangeCompleted = value;
        if(value)
        {
            DepositCollectedMoney();
        }
    }


}
