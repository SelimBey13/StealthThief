using System;
using System.Collections;
using UnityEngine;

public class SourceSoundManager : MonoBehaviour
{
    public static SourceSoundManager Instance{get; private set;}

    [SerializeField] private float idleVoice;
    [SerializeField] private float walkVoice;
    [SerializeField] private float crouchVoice;
    [SerializeField] private float crouchWalkVoice;
    [SerializeField] private float fireVoice;
    [SerializeField] private float SprintVoice;
    [SerializeField] float sourceVoice; // kontrol amaclı sf
    bool isCrouching;
    bool isFire;
    float tempVoice;
    Coroutine fireCoroutine;

    public static event Action<float> OnSourceVoiceChanged;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        PlayerStateManager.OnStateChanged += SetSourceVoice;
        PlayerStateManager.OnCrouch += SetCrouchInfo;
        PlayerStateManager.OnFire += SetFireInfo;
    }
    void OnDisable()
    {
        PlayerStateManager.OnStateChanged -= SetSourceVoice;
        PlayerStateManager.OnCrouch -= SetCrouchInfo;
        PlayerStateManager.OnFire -= SetFireInfo;
    }

    void Start()
    {
        sourceVoice = idleVoice;
    }

    void SetSourceVoice(PlayerStateManager.PlayerMainStates mainState)
    {
        if(mainState == PlayerStateManager.PlayerMainStates.Idle)
        {
            if(isFire) { SetFireVoice();}
            else if(isCrouching){ sourceVoice = crouchVoice;} 
            else{ sourceVoice = idleVoice;}   
        }
        else if(mainState == PlayerStateManager.PlayerMainStates.Walk)
        {
            if(isFire){ SetFireVoice();}
            else if(isCrouching){ sourceVoice = crouchWalkVoice;}
            else{sourceVoice = walkVoice;}
        }
        else if(mainState == PlayerStateManager.PlayerMainStates.Sprint)
        {
            sourceVoice = SprintVoice;
        }
        OnSourceVoiceChanged?.Invoke(sourceVoice);
    }

    void SetCrouchInfo(bool value)
    {
        isCrouching = value;
    }
    void SetFireInfo(bool value)
    {
        isFire = value;
    }

    void SetFireVoice()
    {
        if(fireCoroutine != null)
        {   
            sourceVoice = tempVoice; // eski değeri korumak için konuldu
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireVoiceActivation());
    }

    IEnumerator FireVoiceActivation()
    {
        tempVoice = sourceVoice;
        sourceVoice = fireVoice;
        OnSourceVoiceChanged?.Invoke(sourceVoice);
        yield return new WaitForSeconds(0.2f);
        sourceVoice = tempVoice;
        OnSourceVoiceChanged?.Invoke(sourceVoice);
    }

}
 