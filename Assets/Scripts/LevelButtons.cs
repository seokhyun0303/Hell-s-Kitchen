using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtons : MonoBehaviour
{
    public Button[] levelButtons; 
    public Color[] buttonColors;  
    public Color deactivatedColor;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int index = i;
            levelButtons[index].onClick.AddListener(() => OnLevelButtonClicked(index));
        }
        UpdateButtonsUI();
    }

    public void OnLevelButtonClicked(int buttonIndex)
    {
        Debug.Log($"Button {buttonIndex} clicked!");
// 누르는 버튼에 따라 현재 스테이지 정보 넘겨주기
        if (GameManager.instance != null)
        {
            GameManager.instance.currentStage = buttonIndex;
        }

        SceneManager.LoadScene("Scene2");
    }

    void UpdateButtonsUI()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (GameManager.instance.stageClearStatus[i] == true)
            {
                levelButtons[i].interactable = true;
            }
            else 
            {
                levelButtons[i].interactable = false; 
            }
        }
    }
}
