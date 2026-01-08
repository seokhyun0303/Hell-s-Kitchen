//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI; // UI ����� ���� �ʿ�
//using UnityEngine.EventSystems; // ��ġ �̺�Ʈ�� ó���ϱ� ���� �ʿ�

//public class MobilePlayermovement : MonoBehaviour
//{
//    private SprintDustController dustController;

//    private Animator animator;
//    public float speed; // �̵� �ӵ�
//    private Rigidbody rb;
//    public bool canMove = true;
//    private bool alreadyup = false;
//    private AudioSource audioSource;
//    public AudioClip footstepClip;

//    private Vector3 movement = Vector3.zero; // �̵� ����
//    private Quaternion targetRotation; // ��ǥ ȸ�� ����
//    private bool isMoving = false;

//    public Button sprintButton; // �޸��� ��ư �߰�
//    private bool isSprinting = false; // �޸��� ���¸� ����
//    private string activeButton = "";

//    public RectTransform upButton;    // Up 버튼 영역
//    public RectTransform downButton;  // Down 버튼 영역
//    public RectTransform leftButton;  // Left 버튼 영역
//    public RectTransform rightButton; // Right 버튼 영역

//    void Start()
//    {
//        dustController = GetComponentInChildren<SprintDustController>();
//        rb = GetComponent<Rigidbody>();
//        animator = GetComponent<Animator>();

//        GameObject audioObject = GameObject.Find("AudioSourceObject2");
//        if (audioObject != null)
//        {
//            audioSource = audioObject.GetComponent<AudioSource>();
//            audioSource.clip = footstepClip;
//        }
//        else
//        {
//            Debug.LogError("Object with the specified name not found.");
//        }

//        // �޸��� ��ư �̺�Ʈ ������ ����
//        if (sprintButton != null)
//        {
//            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

//            // PointerDown �̺�Ʈ �߰�
//            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
//            pointerDownEntry.eventID = EventTriggerType.PointerDown;
//            pointerDownEntry.callback.AddListener((eventData) => StartSprint());
//            trigger.triggers.Add(pointerDownEntry);

//            // PointerUp �̺�Ʈ �߰�
//            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
//            pointerUpEntry.eventID = EventTriggerType.PointerUp;
//            pointerUpEntry.callback.AddListener((eventData) => StopSprint());
//            trigger.triggers.Add(pointerUpEntry);
//        }
//    }

//    void Update()
//    {
//        if (!canMove)
//        {
//            return;
//        }

//        // �ӵ� ó��
//        if (speed < 3)
//        {
//            if (GameManager.instance.currentPlayer == GameManager.PlayerType.player1)
//                speed = 3f;
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player2)
//                speed = 3.5f;
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player3)
//                speed = 4f;
//        }
//        if (speed > 6)
//        {
//            speed = 6;
//        }

//        if (GameManager.instance.isspeedup && !alreadyup)
//        {
//            speed *= 1.5f;
//            alreadyup = true;
//        }
//        else if (!GameManager.instance.isspeedup && alreadyup)
//        {
//            speed /= 1.5f;
//            alreadyup = false;
//        }

//        // �̵� ó��
//        if (movement != Vector3.zero)
//        {
//            isMoving = true;
//            var velocity = movement * speed;
//            targetRotation = Quaternion.LookRotation(movement);

//            // �̵� �� ȸ��
//            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
//            rb.MovePosition(newPosition);
//            transform.rotation = targetRotation;

//            animator.SetFloat("speed", velocity.magnitude);
//        }
//        else
//        {
//            isMoving = false;
//        }

//        // ���� ó��
//        if (isMoving)
//        {
//            if (!audioSource.isPlaying)
//            {
//                audioSource.Play();
//            }
//        }
//        else
//        {
//            if (audioSource.isPlaying)
//            {
//                audioSource.Stop();
//            }
//        }

//        CheckTouchOutsideButtons();
//    }

//    // �޸��� ����
//    public void StartSprint()
//    {
//        if (!isSprinting && isMoving) // Only start sprint if the player is moving
//        {
//            isSprinting = true;
//            speed *= 2; // Double the speed for sprinting
//            audioSource.pitch = 1.8f; // Increase audio pitch for sprinting sound
//            animator.SetBool("shift", true); // Enable sprinting animation

//            // Play the dust effect only once
//            if (dustController != null)
//            {
//                dustController.PlayDustOnce();
//            }
//        }
//    }

//    public void StopSprint()
//    {
//        if (isSprinting)
//        {
//            isSprinting = false;
//            speed /= 2;
//            audioSource.pitch = 1.3f;
//            animator.SetBool("shift", false);
//            if (dustController != null)
//            {
//                dustController.StopDust();
//            }
//        }
//    }

//    // �̵� ���� ����
//    public void SetMovement(Vector3 direction, string buttonName)
//    {
//        if (activeButton != "" && activeButton != buttonName)
//        {
//            StopMovement();
//        }

//        activeButton = buttonName; // 현재 활성화된 버튼 업데이트
//        movement = direction;
//    }

//    // ��ư���� ���� ���� �� ����
//    public void StopMovement()
//    {
//        movement = Vector3.zero; // 이동 멈춤
//        activeButton = ""; // 활성화된 버튼 초기화
//    }

//    // 버튼 이벤트 함수
//    public void MoveUp(BaseEventData data)
//    {
//        SetMovement(Vector3.back, "Up");
//    }

//    public void MoveDown(BaseEventData data)
//    {
//        SetMovement(Vector3.forward, "Down");
//    }

//    public void MoveLeft(BaseEventData data)
//    {
//        SetMovement(Vector3.right, "Left");
//    }

//    public void MoveRight(BaseEventData data)
//    {
//        SetMovement(Vector3.left, "Right");
//    }

//    public void Stop(BaseEventData data)
//    {
//        StopMovement();
//    }

//    private void CheckTouchOutsideButtons()
//    {
//        if (Input.touchCount > 0) // 터치가 하나 이상 있을 때만 확인
//        {
//            Touch touch = Input.GetTouch(0);
//            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
//            {
//                // 현재 터치가 버튼 영역 안에 있는지 확인
//                if (!IsTouchInsideRect(touch.position, upButton) &&
//                    !IsTouchInsideRect(touch.position, downButton) &&
//                    !IsTouchInsideRect(touch.position, leftButton) &&
//                    !IsTouchInsideRect(touch.position, rightButton))
//                {
//                    StopMovement(); // 버튼 영역 밖이면 이동 중지
//                }
//            }
//        }
//    }

//    // 터치가 RectTransform 내부에 있는지 확인
//    private bool IsTouchInsideRect(Vector2 touchPosition, RectTransform rectTransform)
//    {
//        Vector2 localPoint;
//        RectTransformUtility.ScreenPointToLocalPointInRectangle(
//            rectTransform,
//            touchPosition,
//            null,
//            out localPoint
//        );
//        return rectTransform.rect.Contains(localPoint);
//    }
//}





using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobilePlayermovement : MonoBehaviour
{
    private SprintDustController dustController;

    private Animator animator;
    public float speed;
    private Rigidbody rb;
    public bool canMove = true;
    private bool alreadyup = false;
    private AudioSource audioSource;
    public AudioClip footstepClip;

    private Vector3 movement = Vector3.zero;
    private Quaternion targetRotation;
    private bool isMoving = false;

    public Button sprintButton;
    private bool isSprinting = false;
    private string activeButton = "";

    public RectTransform upButton;
    public RectTransform downButton;
    public RectTransform leftButton;
    public RectTransform rightButton;

    void Start()
    {
        dustController = GetComponentInChildren<SprintDustController>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        GameObject audioObject = GameObject.Find("AudioSourceObject2");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
            audioSource.clip = footstepClip;
        }
        else
        {
            Debug.LogError("Object with the specified name not found.");
        }

        if (sprintButton != null)
        {
            EventTrigger trigger = sprintButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => StartSprint());
            trigger.triggers.Add(pointerDownEntry);

            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((eventData) => StopSprint());
            trigger.triggers.Add(pointerUpEntry);
        }
    }

    void Update()
    {
        if (!canMove)
        {
            animator.SetFloat("speed", 0f);
            return;
        }

        HandleSpeedBoost();

        if (movement != Vector3.zero)
        {
            isMoving = true;
            Vector3 velocity = movement * speed;
            targetRotation = Quaternion.LookRotation(movement);

            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
            transform.rotation = targetRotation;

            animator.SetFloat("speed", velocity.magnitude);
        }
        else
        {
            isMoving = false;
            animator.SetFloat("speed", 0f);
        }

        HandleFootstepSound();
        CheckTouchOutsideButtons();
    }

    private void HandleSpeedBoost()
    {
        if (speed < 3)
        {
            speed = GameManager.instance.currentPlayer switch
            {
                GameManager.PlayerType.player1 => 3.9f,
                GameManager.PlayerType.player2 => 4.55f,
                GameManager.PlayerType.player3 => 5.2f,
                _ => 3.9f,
            };
        }

        if (speed > 7.8f) speed = 7.8f;

        if (GameManager.instance.isspeedup && !alreadyup)
        {
            speed *= 1.5f;
            alreadyup = true;
        }
        else if (!GameManager.instance.isspeedup && alreadyup)
        {
            speed /= 1.5f;
            alreadyup = false;
        }
    }

    private void HandleFootstepSound()
    {
        if (isMoving)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }

    public void StartSprint()
    {
        if (!isSprinting && isMoving)
        {
            isSprinting = true;
            speed *= 2;
            audioSource.pitch = 1.8f;
            animator.SetBool("shift", true);
            dustController?.PlayDustOnce();
        }
    }

    public void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            speed /= 2;
            audioSource.pitch = 1.3f;
            animator.SetBool("shift", false);
            dustController?.StopDust();
        }
    }

    public void SetMovement(Vector3 direction, string buttonName)
    {
        if (activeButton != "" && activeButton != buttonName)
        {
            StopMovement();
        }

        activeButton = buttonName;
        movement = direction;
    }

    public void StopMovement()
    {
        movement = Vector3.zero;
        activeButton = "";
    }

    public void MoveUp(BaseEventData data)
    {
        SetMovement(Vector3.back, "Up");
    }

    public void MoveDown(BaseEventData data)
    {
        SetMovement(Vector3.forward, "Down");
    }

    public void MoveLeft(BaseEventData data)
    {
        SetMovement(Vector3.right, "Left");
    }

    public void MoveRight(BaseEventData data)
    {
        SetMovement(Vector3.left, "Right");
    }

    public void Stop(BaseEventData data)
    {
        StopMovement();
    }

    private void CheckTouchOutsideButtons()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (!IsTouchInsideRect(touch.position, upButton) &&
                    !IsTouchInsideRect(touch.position, downButton) &&
                    !IsTouchInsideRect(touch.position, leftButton) &&
                    !IsTouchInsideRect(touch.position, rightButton))
                {
                    StopMovement();
                }
            }
        }
    }

    private bool IsTouchInsideRect(Vector2 touchPosition, RectTransform rectTransform)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            touchPosition,
            null,
            out localPoint
        );
        return rectTransform.rect.Contains(localPoint);
    }
}
