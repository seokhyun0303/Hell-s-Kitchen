//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FireController : MonoBehaviour
//{
//    public playergrab playergrab;
//    public GameObject fireEffect; 
//    public GameObject fryingPan;  
//    public GameObject extinguisher;
//    public GameObject currentIngredient;
//    public float burnTime = 1f;
//    public bool hasIngredient = false;
//    public bool isOnFire = false;  
//    public float timeRange_1 = 60f;
//    public float timeRange_2 = 120f;
//    public float randomTime;

//    void Start()
//    {
//        float initialDelay = 20.0f;

//        StartCoroutine(DelayedStartFire(initialDelay));
//    }

//    IEnumerator DelayedStartFire(float delay)
//    {
//        // 초기 대기 시간만큼 대기
//        yield return new WaitForSeconds(delay);

//        // 대기 후 Fire 발생 루틴 시작
//        StartCoroutine(TriggerFire());
//    }

//    IEnumerator TriggerFire()
//    {
//        while (true)
//        {
//            randomTime = Random.Range(timeRange_1, timeRange_2);

//            // 설정된 시간만큼 대기
//            yield return new WaitForSeconds(randomTime);

//            // 대기 후 실행할 함수 호출
//            StartFire();
//        }
//    }

//    void StartFire()
//    {
//        isOnFire = true;
//        fireEffect.SetActive(true);
//        if (hasIngredient)
//        {
//            StartCoroutine(BurnIngredient());
//        }

//        Debug.Log("Fire");
//    }

//    public void ExtinguishFire()
//    {
//        isOnFire = false;
//        fireEffect.SetActive(false);

//        Debug.Log("Extinguish");
//    }

//    IEnumerator BurnIngredient()
//    {
//        yield return new WaitForSeconds(burnTime);
//        if (isOnFire && currentIngredient != null)  
//        {
//            Destroy(currentIngredient);
//            hasIngredient = false;
//        }
//    }
//}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class FireController : MonoBehaviour
//{
//    public playergrab playergrab;
//    public GameObject fireEffect;
//    public GameObject fryingPan;
//    public GameObject extinguisher;
//    public GameObject currentIngredient;
//    public GameObject cookedIngredientPrefab; // Prefab for the burnt version
//    public TextMeshProUGUI panTimerText; // UI text to display the timer above the pan
//    public float burnTime = 1f;
//    public float cookingTime = 10f; // Time it takes to cook before burning
//    public float fireStartTime = 5f; // Time after which fire starts if the ingredient is burnt
//    public bool hasIngredient = false;
//    public bool isOnFire = false;
//    public float timeRange_1 = 60f;
//    public float timeRange_2 = 120f;
//    public float randomTime;

//    private bool isCooking = false; // Flag to track if cooking has started
//    private Coroutine cookingCoroutine; // To handle cooking time
//    private Coroutine burnCoroutine; // To handle fire after burnt
//    private float currentTimer;
//    private float originalCookingTime;

//    void Start()
//    {
//        float initialDelay = 20.0f;
//        switch (GameManager.instance.currentPlayer)
//        {
//            case GameManager.PlayerType.player1:
//                cookingTime = 8f; // player1의 기본 시간
//                originalCookingTime = cookingTime;
//                break;
//            case GameManager.PlayerType.player2:
//                cookingTime = 9f; // player2의 기본 시간
//                originalCookingTime = cookingTime;
//                break;
//            case GameManager.PlayerType.player3:
//                cookingTime = 10f; // player3의 기본 시간
//                originalCookingTime = cookingTime;
//                break;
//            default:
//                cookingTime = 8f; // 기본값
//                originalCookingTime = cookingTime;
//                break;
//        }
//        HideTimer(); // Hide the timer UI initially
//        StartCoroutine(DelayedStartFire(initialDelay));
//    }

//    IEnumerator DelayedStartFire(float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        StartCoroutine(TriggerFire());
//    }

//    IEnumerator TriggerFire()
//    {
//        while (true)
//        {
//            randomTime = Random.Range(timeRange_1, timeRange_2);
//            yield return new WaitForSeconds(randomTime);
//            StartFire();
//        }
//    }

//    //void StartFire()
//    //{
//    //    isOnFire = true;
//    //    fireEffect.SetActive(true);
//    //    if (hasIngredient && isCooking) // Check if there is an ingredient and it's cooking
//    //    {
//    //        StartCoroutine(BurnIngredient());
//    //    }

//    //    Debug.Log("Fire started!");
//    //}
//    void StartFire()
//    {
//        isOnFire = true;
//        fireEffect.SetActive(true);

//        if (currentIngredient && isCooking)
//        {
//            // Destroy the ingredient immediately when the fire starts
//            Destroy(currentIngredient);
//            currentIngredient = null; // Clear the reference
//            hasIngredient = false; // Update flag since there is no more ingredient
//            HideTimer(); // Hide the timer since there's no ingredient left
//            StartCoroutine(BurnIngredient());
//        }

//        Debug.Log("Fire started!");
//    }

//    //public void ExtinguishFire()
//    //{
//    //    isOnFire = false;
//    //    fireEffect.SetActive(false);
//    //    hasIngredient = false;
//    //    isCooking = false;
//    //    HideTimer(); // Hide the timer once fire is extinguished
//    //    StopCooking();
//    //    Debug.Log("Fire extinguished!");
//    //}
//    public void ExtinguishFire()
//    {
//        isOnFire = false;
//        fireEffect.SetActive(false);
//        hasIngredient = false;
//        isCooking = false;
//        currentIngredient = null; // Clear any existing ingredient reference
//        HideTimer(); // Hide the timer once fire is extinguished
//        StopCooking(); // Stop any cooking or burning coroutine

//        Debug.Log("Fire extinguished! Ready for new ingredient.");
//    }

//    public void StartCooking()
//    {
//        if (!isCooking && hasIngredient)
//        {
//            isCooking = true;

//            if (GameManager.instance.iscookup)
//            {
//                cookingTime *= 0.7f; // Reduce chopping time by 30% if iscookup is true
//            }

//            if (cookingCoroutine != null)
//                StopCoroutine(cookingCoroutine);

//            cookingCoroutine = StartCoroutine(CookingCoroutine());
//            ShowTimer(); // Show the timer when cooking starts
//        }
//    }


//    IEnumerator CookingCoroutine()
//    {
//        currentTimer = cookingTime;
//        while (currentTimer > 0f)
//        {
//            currentTimer -= Time.deltaTime; // Decrease the timer
//            UpdateTimerText(currentTimer);  // Update the timer UI above the pan
//            yield return null; // Wait for the next frame
//        }

//        // Cooking completed, now the ingredient burns
//        Debug.Log("Cooking completed! Ingredient is now burning.");
//        ReplaceIngredientWithCookedVersion();

//        // Start fire if the burnt ingredient stays too long on the pan
//        burnCoroutine = StartCoroutine(BurnAfterTime());
//    }

//    void ReplaceIngredientWithCookedVersion()
//    {
//        if (currentIngredient != null && cookedIngredientPrefab != null)
//        {
//            Vector3 ingredientPosition = currentIngredient.transform.position;
//            Quaternion ingredientRotation = currentIngredient.transform.rotation;

//            Destroy(currentIngredient);

//            // Instantiate the cooked version at the same position
//            GameObject cookedVersion = Instantiate(cookedIngredientPrefab, ingredientPosition, ingredientRotation);
//            cookedVersion.transform.SetParent(this.transform);
//            cookedVersion.tag = "CookedSalami"; // Set tag to allow it to be grabbed
//            currentIngredient = cookedVersion;

//            Debug.Log("Ingredient replaced with cooked version.");
//        }
//        else
//        {
//            Debug.LogError("Current ingredient or cooked prefab is null.");
//        }
//    }


//    IEnumerator BurnAfterTime()
//    {
//        currentTimer = fireStartTime;
//        while (currentTimer > 0f)
//        {
//            currentTimer -= Time.deltaTime;
//            UpdateTimerText(currentTimer);  // Update the timer during the burning period
//            yield return null;
//        }

//        if (currentIngredient != null && !isOnFire) // If still no fire and ingredient is present
//        {
//            StartFire();
//        }
//    }

//    IEnumerator BurnIngredient()
//    {
//        yield return new WaitForSeconds(burnTime);
//        if (isOnFire && currentIngredient != null)
//        {
//            Destroy(currentIngredient);
//            hasIngredient = false;
//            HideTimer(); // Hide the timer when the ingredient is destroyed
//        }
//    }

//    void OnTriggerEnter(Collider other)
//    {

//        if (other.CompareTag("Salami") && currentIngredient == null)
//        {
//            Debug.Log("Salami added to the frying pan and started cooking.");
//        }
//        else if (other.CompareTag("Salami") && currentIngredient != null)
//        {
//            Debug.Log("Frying pan is already in use. Cannot add another Salami.");
//        }
//    }

//    void OnTriggerExit(Collider other)
//    {
//        // Stop cooking if the ingredient is removed from the pan
//        if (other.CompareTag("Salami") && currentIngredient == other.gameObject)
//        {
//            StopCooking();
//        }
//    }

//    public void StopCooking()
//    {
//        if (isCooking)
//        {
//            isCooking = false;

//            if (cookingCoroutine != null)
//            {
//                StopCoroutine(cookingCoroutine);
//                cookingCoroutine = null;
//            }

//            if (burnCoroutine != null)
//            {
//                StopCoroutine(burnCoroutine);
//                burnCoroutine = null;
//            }

//            if (GameManager.instance.iscookup)
//            {
//                cookingTime = originalCookingTime;
//            }

//            HideTimer(); // Hide the timer when cooking stops
//            Debug.Log("Cooking stopped.");
//        }
//    }

//    public void GrabbedIngredient()
//    {
//        StopCooking(); // Stop the cooking or burning process
//        isOnFire = false; // Ensure fire is turned off
//        fireEffect.SetActive(false); // Hide the fire effect if it was visible
//    }

//    // UI Timer Functions
//    void UpdateTimerText(float time)
//    {
//        // Update the timer text with two decimal places
//        panTimerText.text = Mathf.Max(time, 0f).ToString("F2") + "s";
//    }

//    void HideTimer()
//    {
//        // Hide the timer text by clearing the text content
//        panTimerText.text = "";
//    }

//    void ShowTimer()
//    {
//        // Show the timer with the initial cooking time
//        panTimerText.text = cookingTime.ToString("F2") + "s";
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Import Unity UI for Image handling

public class FireController : MonoBehaviour
{
    public playergrab playergrab;
    public GameObject fireEffect;
    public GameObject fryingPan;
    public GameObject extinguisher;
    public GameObject currentIngredient;
    public GameObject cookedIngredientPrefab; // Prefab for the burnt version
    public GameObject steamEffect; // Steam particles to play after cooking is done
    public Image bgImage; // Background image of the timer bar
    public Image fgImage; // Foreground image of the timer bar
    public float burnTime = 1f;
    public float cookingTime = 10f; // Time it takes to cook before burning
    public float fireStartTime = 5f; // Time after which fire starts if the ingredient is burnt
    public bool hasIngredient = false;
    public bool isOnFire = false;
    public float timeRange_1 = 60f;
    public float timeRange_2 = 120f;
    public float initialDelay = 20.0f;
    private float randomTime;

    private bool isCooking = false; // Flag to track if cooking has started
    private Coroutine cookingCoroutine; // To handle cooking time
    private Coroutine burnCoroutine; // To handle fire after burnt
    private float currentTimer;
    private float originalCookingTime;

    public AudioSource audioSource;
    public AudioSource fireAudioSource;
    public AudioClip fryingClip;
    public AudioClip fireClip;

    void Start()
    {
        GameObject audioObject = GameObject.Find("AudioSourceObject");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on the object.");
            }
        }
        
        switch (GameManager.instance.currentPlayer)
        {
            case GameManager.PlayerType.player1:
                cookingTime = 8f; // player1's default time
                originalCookingTime = cookingTime;
                break;
            case GameManager.PlayerType.player2:
                cookingTime = 9f; // player2's default time
                originalCookingTime = cookingTime;
                break;
            case GameManager.PlayerType.player3:
                cookingTime = 10f; // player3's default time
                originalCookingTime = cookingTime;
                break;
            default:
                cookingTime = 8f; // Default value
                originalCookingTime = cookingTime;
                break;
        }

        switch (GameManager.instance.currentStage)
        {
            case 0:
                initialDelay = 0f;
                timeRange_1 = 97.0f;
                timeRange_2 = 97.0f;
                break;
            case 1:
                initialDelay = 20.0f;
                timeRange_1 = 25f;
                timeRange_2 = 90f;
                break;
            case 2:
                initialDelay = 10.0f;
                timeRange_1 = 20f;
                timeRange_2 = 60f;
                break;
            case 3:
                initialDelay = 0.0f;
                timeRange_1 = 10f;
                timeRange_2 = 40f;
                break;
            default:
                timeRange_1 = 90f;
                timeRange_2 = 120f;
                break;
        }
       
        HideTimer(); // Hide the timer UI initially
        if (steamEffect != null)
        {
            steamEffect.SetActive(false); // Ensure steam is off at the start
        }
        StartCoroutine(DelayedStartFire(initialDelay));
    }

    IEnumerator DelayedStartFire(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TriggerFire());
    }

    IEnumerator TriggerFire()
    {
        while (true)
        {
            randomTime = Random.Range(timeRange_1, timeRange_2);
            yield return new WaitForSeconds(randomTime);
            StartFire();
        }
    }

    void StartFire()
    {
        isOnFire = true;
        fireEffect.SetActive(true);
        fireAudioSource.clip = fireClip;
        fireAudioSource.Play();
        if (currentIngredient && isCooking)
        {
            // Destroy the ingredient immediately when the fire starts
            Destroy(currentIngredient);
            currentIngredient = null; // Clear the reference
            hasIngredient = false; // Update flag since there is no more ingredient
            HideTimer(); // Hide the timer since there's no ingredient left
            StartCoroutine(BurnIngredient());
        }

        Debug.Log("Fire started!");
    }

    public void ExtinguishFire()
    {
        fireAudioSource.Stop();
        isOnFire = false;
        fireEffect.SetActive(false);
        hasIngredient = false;
        isCooking = false;
        currentIngredient = null; // Clear any existing ingredient reference
        HideTimer(); // Hide the timer once fire is extinguished
        StopCooking(); // Stop any cooking or burning coroutine

        if (steamEffect != null)
        {
            steamEffect.SetActive(false); // Stop steam effect when fire is extinguished
        }

        Debug.Log("Fire extinguished! Ready for new ingredient.");
    }

    public void StartCooking()
    {
        if (!isCooking && hasIngredient)
        {
            isCooking = true;
            audioSource.clip = fryingClip;
            audioSource.Play();

            if (GameManager.instance.iscookup)
            {
                cookingTime *= 0.7f; // Reduce cooking time by 30% if iscookup is true
            }

            if (cookingCoroutine != null)
                StopCoroutine(cookingCoroutine);

            cookingCoroutine = StartCoroutine(CookingCoroutine());
            ShowTimer(); // Show the timer when cooking starts
        }
    }

    IEnumerator CookingCoroutine()
    {
        currentTimer = cookingTime;
        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime; // Decrease the timer
            UpdateTimerBar(currentTimer);  // Update the timer bar above the pan
            yield return null; // Wait for the next frame
        }

        // Cooking completed, now the ingredient is cooked
        Debug.Log("Cooking completed! Ingredient is now cooked.");
        ReplaceIngredientWithCookedVersion();

        // Play steam effect after cooking is done
        if (steamEffect != null)
        {
            steamEffect.SetActive(true);
        }

        // Start fire if the cooked ingredient stays too long on the pan
        burnCoroutine = StartCoroutine(BurnAfterTime());
    }

    void ReplaceIngredientWithCookedVersion()
    {
        if (currentIngredient != null && cookedIngredientPrefab != null)
        {
            Vector3 ingredientPosition = currentIngredient.transform.position;
            Quaternion ingredientRotation = currentIngredient.transform.rotation;

            Destroy(currentIngredient);

            // Instantiate the cooked version at the same position
            GameObject cookedVersion = Instantiate(cookedIngredientPrefab, ingredientPosition, ingredientRotation);
            cookedVersion.transform.SetParent(this.transform);
            cookedVersion.tag = "CookedSalami"; // Set tag to allow it to be grabbed
            currentIngredient = cookedVersion;

            Debug.Log("Ingredient replaced with cooked version.");
        }
        else
        {
            Debug.LogError("Current ingredient or cooked prefab is null.");
        }
    }

    IEnumerator BurnAfterTime()
    {
        currentTimer = fireStartTime;
        bgImage.color = Color.red; // Change the background color to red for the second timer
        while (currentTimer > 0f)
        {
            currentTimer -= Time.deltaTime;
            UpdateTimerBar(currentTimer);  // Update the timer during the burning period
            yield return null;
        }

        if (currentIngredient != null && !isOnFire) // If still no fire and ingredient is present
        {
            StartFire();
        }
    }

    IEnumerator BurnIngredient()
    {
        yield return new WaitForSeconds(burnTime);
        if (isOnFire && currentIngredient != null)
        {
            Destroy(currentIngredient);
            hasIngredient = false;
            HideTimer(); // Hide the timer when the ingredient is destroyed
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Salami") && currentIngredient == null)
        {
            Debug.Log("Salami added to the frying pan and started cooking.");
        }
        else if (other.CompareTag("Salami") && currentIngredient != null)
        {
            Debug.Log("Frying pan is already in use. Cannot add another Salami.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Stop cooking if the ingredient is removed from the pan
        if (other.CompareTag("Salami") && currentIngredient == other.gameObject)
        {
            StopCooking();
        }
    }

    public void StopCooking()
    {
        if (isCooking)
        {
            isCooking = false;
            audioSource.Stop();
            if (cookingCoroutine != null)
            {
                StopCoroutine(cookingCoroutine);
                cookingCoroutine = null;
            }

            if (burnCoroutine != null)
            {
                StopCoroutine(burnCoroutine);
                burnCoroutine = null;
            }

            if (GameManager.instance.iscookup)
            {
                cookingTime = originalCookingTime;
            }

            HideTimer(); // Hide the timer when cooking stops
            if (steamEffect != null)
            {
                steamEffect.SetActive(false); // Stop steam effect when cooking stops
            }
            Debug.Log("Cooking stopped.");
        }
    }

    public void GrabbedIngredient()
    {
        StopCooking(); // Stop the cooking or burning process
        isOnFire = false; // Ensure fire is turned off
        fireEffect.SetActive(false); // Hide the fire effect if it was visible
    }

    // UI Timer Functions
    void UpdateTimerBar(float time)
    {
        float fillAmount = Mathf.Clamp01(time / (isOnFire ? fireStartTime : cookingTime));
        fgImage.fillAmount = fillAmount; // Update the foreground image to reflect the timer progress
    }

    void HideTimer()
    {
        fgImage.fillAmount = 0f; // Hide the timer by setting the fill to zero
        bgImage.gameObject.SetActive(false); // Hide the background bar
        bgImage.color = Color.white; // Reset the background color to white
    }

    void ShowTimer()
    {
        fgImage.fillAmount = 1f; // Start with the timer bar full
        bgImage.gameObject.SetActive(true); // Show the background bar
        bgImage.color = Color.white; // Ensure the initial color is white
    }
}
