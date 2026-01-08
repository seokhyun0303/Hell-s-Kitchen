using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EndScene : MonoBehaviour
{
    public TMP_Text totalMoneyText;
    public UnityEngine.UI.Image resultImage;
    public Sprite gameOverImg;
    public Sprite gameClearImg;
    // Start is called before the first frame update
    void Start()
    {
        int money = GameManager.instance.CurrentMoney;
        
        string moneyText = $"Total: {money}G";
        totalMoneyText.text = moneyText;

        GameObject imageObject = GameObject.Find("ResultImage");

        resultImage = imageObject.GetComponent<UnityEngine.UI.Image>();
        gameClearImg = Resources.Load<Sprite>("gameclear");
        gameOverImg = Resources.Load<Sprite>("gameover");
        if (GameManager.instance.isWin)
        {
            resultImage.sprite = gameClearImg;
        }
        else
        {
            resultImage.sprite = gameOverImg;
        }
    }
}
