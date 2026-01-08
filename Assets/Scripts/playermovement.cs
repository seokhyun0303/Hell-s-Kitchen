//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class playermovement : MonoBehaviour
//{
//    private Animator animator;
//    public float speed;            // 이동 속도
//    private Rigidbody rb;
//    public bool canMove = true;
//    private bool alreadyup = false;
//    private AudioSource audioSource;
//    public AudioClip footstepClip;

//    void Start()
//    {
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
//    }
//    // Update is called once per frame
//    void Update()
//    {
//        if (!canMove)
//        {
//            return; // canMove가 false면 이동 로직을 실행하지 않음
//        }
//        if(speed < 3)
//        {
//            if(GameManager.instance.currentPlayer == GameManager.PlayerType.player1)
//            {
//                speed = 3f;
//            }
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player2)
//            {
//                speed = 3.5f;
//            }
//            else if (GameManager.instance.currentPlayer == GameManager.PlayerType.player3)
//            {
//                speed = 4f;
//            }
//        }
//        else if(speed > 6)
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
//        // 이동 벡터 초기화
//        Vector3 movement = Vector3.zero;
//        Quaternion targetRotation = transform.rotation; // 기본적으로 현재 회전 유지
//        bool isMoving = false;
//        // W 또는 UpArrow 키 입력 시 북쪽(+Z)
//        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
//        {
//            isMoving = true;
//            movement = Vector3.back;
//            targetRotation = Quaternion.LookRotation(Vector3.back); // 북쪽(+Z) 바라봄
//        } 
//        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
//        {
//            isMoving = false;
//        }
//        // S 또는 DownArrow 키 입력 시 남쪽(-Z)
//        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
//        {
//            isMoving = true;
//            movement = Vector3.forward;
//            targetRotation = Quaternion.LookRotation(Vector3.forward); // 남쪽(-Z) 바라봄
//        }
//        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
//        {
//            isMoving = false;
//        }
//        // A 또는 LeftArrow 키 입력 시 서쪽(-X)
//        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
//        {
//            isMoving = true;
//            movement = Vector3.right;
//            targetRotation = Quaternion.LookRotation(Vector3.right); // 서쪽(-X) 바라봄
//        }
//        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
//        {
//            isMoving = false;
//        }
//        // D 또는 RightArrow 키 입력 시 동쪽(+X)
//        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
//        {
//            isMoving = true;
//            movement = Vector3.left;
//            targetRotation = Quaternion.LookRotation(Vector3.left); // 동쪽(+X) 바라봄
//        }
//        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
//        {
//            isMoving = false;
//        }

//        // running
//        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
//        {
//            audioSource.pitch = 1.8f;
//            speed = 2 * speed;
//            animator.SetBool("shift", true);
//        } else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
//        {
//            audioSource.pitch = 1.3f;
//            speed = speed/2;
//            animator.SetBool("shift", false);
//        }

//        // 회전 적용
//        var velocity = movement * speed;
//        if (movement != Vector3.zero)
//        {
//            transform.rotation = targetRotation;
//            // 이동
//            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
//            rb.MovePosition(newPosition);

//        }
//        animator.SetFloat("speed", velocity.magnitude);

//        // sound
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
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    private Animator animator;
    public float speed; // 이동 속도
    private Rigidbody rb;
    public bool canMove = true;
    private bool alreadyup = false;
    private AudioSource audioSource;
    public AudioClip footstepClip;

    private bool isSprinting = false; // Tracks if the player is sprinting
    private bool isMoving = false;   // Tracks if the player is moving

    private Vector3 movement = Vector3.zero; // Tracks movement direction
    private Quaternion targetRotation; // Tracks rotation direction

    // Reference to the Sprint Dust Controller
    private SprintDustController dustController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        dustController = GetComponentInChildren<SprintDustController>();

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
    }

    void Update()
    {
        if (!canMove)
        {
            return; // Skip logic if movement is disabled
        }

        // Movement and direction logic
        movement = Vector3.zero;
        targetRotation = transform.rotation;
        isMoving = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            isMoving = true;
            movement = Vector3.back;
            targetRotation = Quaternion.LookRotation(Vector3.back);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            isMoving = true;
            movement = Vector3.forward;
            targetRotation = Quaternion.LookRotation(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            isMoving = true;
            movement = Vector3.right;
            targetRotation = Quaternion.LookRotation(Vector3.right);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            isMoving = true;
            movement = Vector3.left;
            targetRotation = Quaternion.LookRotation(Vector3.left);
        }

        // Sprint logic
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!isSprinting && isMoving) // Only start sprinting if moving
            {
                isSprinting = true;
                speed *= 2;
                audioSource.pitch = 1.8f;
                animator.SetBool("shift", true);

                // Trigger sprint dust
                if (dustController != null)
                {
                    dustController.PlayDustOnce();
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            if (isSprinting)
            {
                isSprinting = false;
                speed /= 2;
                audioSource.pitch = 1.3f;
                animator.SetBool("shift", false);
            }
        }

        // Apply movement and rotation
        var velocity = movement * speed;
        if (movement != Vector3.zero)
        {
            transform.rotation = targetRotation;
            Vector3 newPosition = rb.position + velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
        animator.SetFloat("speed", velocity.magnitude);

        // Footstep sound
        if (isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
