using UnityEngine;

public class SourceSoundManager : MonoBehaviour
{
    public static SourceSoundManager Instance{get; private set;}
    
    float sourceVoice;
    void Awake()
    {
        Instance = this;
    }



    void SetSourceVoice()
    {
        
    }

}
