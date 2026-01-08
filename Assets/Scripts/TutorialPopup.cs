using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPopup : MonoBehaviour
{
    public GameObject tutorialPopup1;
    public GameObject tutorialPopup2;
    public GameObject tutorialPopup3;

    public void OnClickedNextButton()
    {
        if (tutorialPopup3.activeSelf)
        {
            SceneManager.LoadScene("StageScene");
        }
        
        if (!tutorialPopup1.activeSelf)
        {
            tutorialPopup1.SetActive(true);
        }
        else if (!tutorialPopup2.activeSelf)
        {
            tutorialPopup2.SetActive(true);
        }
        else if (!tutorialPopup3.activeSelf)
        {
            tutorialPopup3.SetActive(true);
        }
    }
}
