using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    private bool isMuted;
    public GameObject muteIcon;

    void Update()
    {
        isMuted = SoundManager.instance.isMuted;
        if (isMuted)  
        {
            muteIcon.SetActive(true);
        }
        else
        {
            muteIcon.SetActive(false);
        }
    }
    
    public void Mute()
    {
        SoundManager.instance.ToggleMute();
    }

}
