

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    public static GameManager instance;

    // Player Type Enum
    public enum PlayerType { player1, player2, player3 }
    public PlayerType currentPlayer;
    private int currentmoney;
    public int CurrentMoney { get { return currentmoney; } }

    // Game Timer
    public float gameDuration = 90f; //for ppt
    private float remainingTime;
    public bool isspeedup = false;
    public bool iscookup = false;

    // Order Tracking
    public enum FoodType { Kimbap1, Kimbap2, Kimbap3, Kimbap4, Kimbap5, Kimbap6 }
    public FoodType currentOrder;
    private FoodType beforefood;

    // Audio
    public AudioClip correctSound;
    public AudioClip errorSound;
    private AudioSource audioSource;

    // Stage
    public int currentStage = 0;
    public bool[] stageClearStatus = new bool[4];
    public bool isWin = false;

    // Dish Class to Track Ingredients
    [System.Serializable]
    public class Dish
    {
        public List<string> ingredients = new List<string>();

        public void AddIngredient(string ingredient)
        {
            ingredients.Add(ingredient);
        }

        public void ClearDish()
        {
            ingredients.Clear();
        }
    }

    public Dish currentDish = new Dish();

    private void Awake()
    {
        // Singleton instance setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep GameManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unregister event when GameManager is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        stageClearStatus[0] = true;
        stageClearStatus[1] = true;
        StartGame();
    }

    // Game Start
    public void StartGame()
    {
        remainingTime = gameDuration; // Reset timer
        GenerateNewOrder(); // Generate first order
        if (currentStage != 0) StartCoroutine(GameTimer()); // Start timer coroutine
        currentmoney = 0;
        isWin = false;
        beforefood = FoodType.Kimbap6;
    }

    // Game Timer Coroutine
    private IEnumerator GameTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        EndGame(); // End game when time runs out
    }

    public void EndGame()
    {
        Debug.Log("Game Over");
        switch (currentStage)
        {
            case 0: // Tutorial
                if (currentmoney >= 0) 
                {
                    isWin = true;
                    stageClearStatus[1] = true;
                    Debug.Log("level 0 clear");
                }
                break;
            case 1: // Easy
                if (currentmoney >= 5000) 
                {
                    isWin = true;
                    stageClearStatus[2] = true;
                    Debug.Log("level 1 clear");
                }
                break;
            case 2: // Normal까지는 Clear하면 다음 단계 해금
                if (currentmoney >= 5000) 
                {
                    isWin = true;
                    stageClearStatus[3] = true;
                    Debug.Log("level 2 clear");
                }
                break;
            case 3:
                if (currentmoney >= 5000) 
                {
                    isWin = true;
                    Debug.Log("level 3 clear");
                }
                break;
        }
        SceneManager.LoadScene("EndScene");
    }

    private void ActivatePlayer()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");
        GameObject player3 = GameObject.Find("Player3");

        player1?.SetActive(currentPlayer == PlayerType.player1);
        player2?.SetActive(currentPlayer == PlayerType.player2);
        player3?.SetActive(currentPlayer == PlayerType.player3);
    }

    // Generate New Order
    public void GenerateNewOrder()
    {
        FoodType newOrder;
        do
        {
            newOrder = (FoodType)Random.Range(0, System.Enum.GetValues(typeof(FoodType)).Length);
        } while (newOrder == beforefood); // 이전 음식과 동일한 값이면 다시 생성

        currentOrder = newOrder;
        beforefood = currentOrder;
        Debug.Log("New Order: " + currentOrder);
        UIManager.instance.UpdateOrderDisplay(); // Assuming you have a UI to update the order display
    }

    // Verify the Dish (check if dish matches the order)
    public void VerifyDish(GameObject dish)
    {
        // Check if the tag of the dish matches the current order
        string expectedTag = currentOrder.ToString(); // Expected tag like "Kimbap1", "Kimbap2", etc.
        GameObject audioObject = GameObject.Find("AudioSourceObject");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }
        if (dish.CompareTag(expectedTag))
        {
            Debug.Log("Order Completed: " + currentOrder);
            currentmoney += 4000; // Reward for correct dish
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            Debug.Log("Incorrect Order Submitted.");
            currentmoney -= 1000; // Penalty for incorrect dish
            audioSource.PlayOneShot(errorSound);
        }

        // Clear the current dish (if applicable)
        currentDish.ClearDish();

        // Generate a new order after submission
        GenerateNewOrder();
    }

    // Called if the order is missed
    public void MissOrder()
    {
        Debug.Log("Order Missed: " + currentOrder);
        currentmoney -= 1000;
        GenerateNewOrder();
    }

    // Adds an ingredient to the current dish
    public void AddIngredientToDish(string ingredient)
    {
        currentDish.AddIngredient(ingredient);
    }

    // Returns remaining game time
    public float GetRemainingTime()
    {
        return remainingTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene2")
        {
            ActivatePlayer();
            StartGame();
            Debug.Log("onSceneLoad");
        }
    }

    public int GetCurrentMoney()
    {
        return currentmoney;
    }

    public void ActivateSpeedUp()
    {
        StartCoroutine(TemporarySpeedUp());
    }

    private IEnumerator TemporarySpeedUp()
    {
        isspeedup = true;
        yield return new WaitForSeconds(5f);
        isspeedup = false;
    }

    // Function to set iscookup to true for 5 seconds
    public void ActivateCookUp()
    {
        StartCoroutine(TemporaryCookUp());
    }

    private IEnumerator TemporaryCookUp()
    {
        iscookup = true;
        yield return new WaitForSeconds(10f);
        iscookup = false;
    }

    // Function to set isaddmoney to true for 5 seconds
    public void ActivateAddMoney()
    {
        currentmoney += 500;
    }

}
