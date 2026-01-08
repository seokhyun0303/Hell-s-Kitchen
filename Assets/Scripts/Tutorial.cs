using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorialChat; 
    public TMP_Text contents; 
    public Image image; 

    public float typingSpeed = 0.01f;

    private bool isInteractable = false;
    private Coroutine hideCoroutine;
    private Coroutine typingCoroutine;
    private bool isTyping;
    
    private Queue<string> messageQueue = new Queue<string>(); 
    private bool isMessageQueueRunning = false;
   
    void Start()
    {
        if (GameManager.instance.currentStage == 0)
        {
            EnqueueMessage("Press  <voffset=0.4em><size=150%><sprite name=\"button_triangle_left\"> <sprite name=\"button_triangle_right\"></size> <sprite name=\"button_triangle_top\"> <sprite name=\"button_triangle_under\"></voffset>to <color=#FF0000>move</color>.", false, 5f);
            EnqueueMessage("You can run by pressing  <voffset=0.3em><sprite name=\"flash-symbol\"></voffset>", false, 6f);
            EnqueueMessage("Check the order in the upper left corner.", false, 6f);
            EnqueueMessage("Press the   <voffset=0.3em><sprite name=\"grab\"></voffset> to <color=#FF0000>grab the ingredients</color> that correspond to your order.", false, 10f);
            EnqueueMessage("If you grab it wrong, you can press the   <voffset=0.3em><sprite name=\"grab\"></voffset> to throw it in the <color=#FF0000>trash can</color>.", false, 10f);
            EnqueueMessage("And press the   <voffset=0.3em><sprite name=\"grab\"></voffset> to place  <color=#FF0000>the vegetables and fish on the cutting board</color>.", false, 20f);
            EnqueueMessage("Or, place <color=#FF0000>the sausages in a frying pan</color> and take them out when the time is up.", false, 8f);
            EnqueueMessage("Press the   <voffset=0.3em><sprite name=\"sword\"></voffset>to chop ingredients", false, 10f);
            EnqueueMessage("<color=#FF0000>Plate the cooked ingredients, seaweed and rice.</color> The order of the plates of the ingredients does not matter.", false, 10f);
            EnqueueMessage("Kimbap is made when 4 ingredients, including seaweed and rice, are placed together.", false, 6f);
            EnqueueMessage("<color=#FF0000>Submit your kimbap</color> and get paid.", false, 5f);
            EnqueueMessage("<color=#FF0000>Fire!</color> Pick up the <color=#FF0000>fire extinguisher</color>, approach the fire, and press the   <voffset=0.3em><sprite name=\"grab\"></voffset>", false, 20f);
            EnqueueMessage("Please note that a frying pan that has caught fire cannot be used.", false, 10f);
            EnqueueMessage("<color=#FF0000>Black Out!</color> Press the   <voffset=0.3em><sprite name=\"grab\"></voffset> to turn on the <color=#FF0000>light</color>.", false, 15f);
            EnqueueMessage("Now that the tutorial is over, enjoy the game.", false, 4f);
            
        }
        else
        {
            tutorialChat.SetActive(false);
        }
    }

    public void EnqueueMessage(string message, bool requireInteraction, float time)
    {
        messageQueue.Enqueue(message); // 메시지 추가
        if (!isMessageQueueRunning)
        {
            StartCoroutine(ProcessMessageQueue(requireInteraction, time));
        }
    }

    private IEnumerator ProcessMessageQueue(bool requireInteraction, float time)
    {
        isMessageQueueRunning = true;

        while (messageQueue.Count > 0)
        {
            string currentMessage = messageQueue.Dequeue();
            ShowChatBubble(currentMessage, requireInteraction);
            yield return new WaitUntil(() => !isTyping);

            yield return new WaitForSeconds(time);
            // 상호작용이 필요한 경우 사용자가 처리할 때까지 대기
            // if (requireInteraction)
            // {
            //     yield return new WaitUntil(() => !isInteractable);
            // }
            // else
            // {
            //     // 상호작용 필요 없을 경우 자동 숨김 시간 대기
            //     yield return new WaitForSeconds(time);
            //     // HideChatBubble();
            // }
        }

        isMessageQueueRunning = false;

        tutorialChat.SetActive(false);
        GameManager.instance.EndGame();
    }

    public void ShowChatBubble(string message, bool requireInteraction)
    {
        tutorialChat.SetActive(true);

        // 상호작용 여부 설정
        isInteractable = requireInteraction;

        // 이전에 실행 중이던 코루틴 중지
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        // 상호작용이 필요 없다면 일정 시간이 지난 후 자동으로 숨김
        // if (!requireInteraction)
        // {
        //     hideCoroutine = StartCoroutine(HideAfterTime(displayTime));
        // }

        // 타이핑 시작
        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator HideAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        tutorialChat.SetActive(false);
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        contents.text = "";
        string currentText = ""; // 현재 출력된 텍스트

        bool insideTag = false; // HTML 태그 안에 있는지 여부

        for (int i = 0; i < message.Length; i++)
        {
            char currentChar = message[i];

            if (currentChar == '<')
            {
                insideTag = true;
            }

            if (insideTag)
            {
                currentText += currentChar;

                // 태그가 끝났는지 확인
                if (currentChar == '>')
                {
                    insideTag = false;
                }
            }
            else
            {
                currentText += currentChar;
                yield return new WaitForSeconds(typingSpeed);
            }

            contents.text = currentText;
        }

        isTyping = false;
    }
}
