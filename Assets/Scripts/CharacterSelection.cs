using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public Color originColor;
    public Color newColor;

    void Start()
    {
        newColor = new Color(1f, 0.67f, 0.46f);
        originColor = new Color(1f, 1f, 1f);
        SetPlayerToPlayer1();
    }
    public void SetPlayerToPlayer1()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player1;
        button1.image.color = newColor;
        button2.image.color = originColor;
        button3.image.color = originColor;
    }

    public void SetPlayerToPlayer2()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player2;
        button1.image.color = originColor;
        button2.image.color = newColor;
        button3.image.color = originColor;
    }

    public void SetPlayerToPlayer3()
    {
        GameManager.instance.currentPlayer = GameManager.PlayerType.player3;
        button1.image.color = originColor;
        button2.image.color = originColor;
        button3.image.color = newColor;
    }
}
