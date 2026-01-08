using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene1 : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Scene1");
    }
}
