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
    private float moneyLoss;
    public static event Action<float> OnAccountMoneyChanged;
    public static event Action<float> OnTotalChallangeMoneyChanged;

    void OnEnable()
    {
        ValuableInteractable.OnItemTaken += SetChallangeMoney;
        GameManager.OnChallengeCompletedSuccesfully += ControlChallengeSuccess;
    }

    void OnDisable()
    {
        ValuableInteractable.OnItemTaken -= SetChallangeMoney;
        GameManager.OnChallengeCompletedSuccesfully -= ControlChallengeSuccess;
    }

    void DepositCollectedMoney()
    {
            accountMoney += totalChallangeMoney*(1f-employerTax);
            moneyLoss = totalChallangeMoney*employerTax;
            OnAccountMoneyChanged?.Invoke(accountMoney);
    }

    void SetChallangeMoney(float value)
    {
        totalChallangeMoney += value;
        OnTotalChallangeMoneyChanged?.Invoke(totalChallangeMoney);
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
        if(value)
        {
            DepositCollectedMoney();
        }
        totalChallangeMoney = 0f;
    }


}
