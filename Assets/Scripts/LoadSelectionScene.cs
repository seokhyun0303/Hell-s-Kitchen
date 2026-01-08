using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSelectionScene : MonoBehaviour
{
    public Button startButton;
    public Sprite clickedImage;

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("SelectionScene");
        // startButton.image.sprite = clickedImage;
    }

}
