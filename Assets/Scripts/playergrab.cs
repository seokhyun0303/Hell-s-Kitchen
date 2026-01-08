

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playergrab : MonoBehaviour
{

    public GameObject cuttingBoardArrow1; 
    public GameObject cuttingBoardArrow2;
    public GameObject fryingPanArrow1;
    public GameObject fryingPanArrow2;

    private Animator animator;
    private bool grabbed;
    private int isgrab = 0;
    GameObject spawnedObject = null;
    public GameObject grabPoint;
    public LightController lightController;
    public int isFish = 0;
    public int isCarrot = 0;
    public int isPepper = 0;
    public int isCucumber = 0;
    public int isNori = 0;
    public int isRice = 0;
    public int isSalami = 0;
    public int isCookedSalami = 0;
    public int isExtinguisher = 0;
    public int isChoppedPepper = 0;
    public int isChoppedCucumber = 0;
    public int isChoppedFish = 0;
    public int isChoppedCarrot = 0;

    public int isKimbap = 0;
    public GameObject knifePrefab;
    GameObject knife = null;
    private bool isChopped = false;

    public AudioClip choppingSound;
    public AudioClip fryingSound;
    public AudioClip grabSound;
    public AudioClip discardSound;
    public AudioClip extinguishSound;
    private AudioSource audioSource;

    // Reference to the chopping board (for placing ingredients)
    public Chopping choppingScript;
    private Chopping choppingInstance;
    private playermovement playerMovement;

    private bool isButtonGrabPressed = false;
    private bool isButtonChopPressed = false;

    void Start()
    {
        lightController = GameObject.FindWithTag("Lever").GetComponent<LightController>();
        animator = GetComponent<Animator>();
        grabbed = false;
        choppingInstance = GameObject.FindObjectOfType<Chopping>();
        playerMovement = GetComponent<playermovement>();
        GameObject audioObject = GameObject.Find("AudioSourceObject");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found on the object.");
            }
        }
        else
        {
            Debug.LogError("Object with the specified name not found.");
        }
        HideAllArrows();

    }

    void Update()
    {
        animator.SetBool("holdingItem", grabbed);
        if (Input.GetKey(KeyCode.E)||isButtonChopPressed)
        {
            if (playerMovement != null)
                playerMovement.canMove = false;
        }
        else
        {
            if (playerMovement != null)
                playerMovement.canMove = true;
        }

        if (isgrab == 1)
        {
            UpdateArrowVisibility();
        }
        else
        {
            HideAllArrows();
        }
    }

    private void UpdateArrowVisibility()
    {
        if (isFish == 1 || isCarrot == 1 || isPepper == 1 || isCucumber == 1)
        {
            // Show arrows above the cutting boards
            if (cuttingBoardArrow1 != null) cuttingBoardArrow1.SetActive(true);
            if (cuttingBoardArrow2 != null) cuttingBoardArrow2.SetActive(true);

            // Hide the frying pan arrow
            if (fryingPanArrow1 != null) fryingPanArrow1.SetActive(false);
            if (fryingPanArrow2 != null) fryingPanArrow2.SetActive(false);
        }
        else if (isSalami == 1)
        {
            // Show the frying pan arrow
            if (fryingPanArrow1 != null) fryingPanArrow1.SetActive(true);
            if (fryingPanArrow2 != null) fryingPanArrow2.SetActive(true);

            // Hide cutting board arrows
            if (cuttingBoardArrow1 != null) cuttingBoardArrow1.SetActive(false);
            if (cuttingBoardArrow2 != null) cuttingBoardArrow2.SetActive(false);
        }
        else
        {
            // Hide all arrows if no relevant ingredient is held
            HideAllArrows();
        }
    }

    private void HideAllArrows()
    {
        if (cuttingBoardArrow1 != null) cuttingBoardArrow1.SetActive(false);
        if (cuttingBoardArrow2 != null) cuttingBoardArrow2.SetActive(false);
        if (fryingPanArrow1 != null) fryingPanArrow1.SetActive(false);
        if (fryingPanArrow2 != null) fryingPanArrow2.SetActive(false);
    }
    public void OnButtonGrabPressed()
    {
        isButtonGrabPressed = true;
    }

    public void OnButtonGrabReleased()
    {
        isButtonGrabPressed = false;
    }

    public void OnButtonChopPressed()
    {
        isButtonChopPressed = true;
    }

    public void OnButtonChopbReleased()
    {
        isButtonChopPressed = false;
    }

    void OnTriggerStay(Collider other)
    {
        if(isgrab == 0 && (Input.GetKey(KeyCode.Space) || isButtonGrabPressed) && (choppingInstance == null || !choppingInstance.isChopping))
        {
            TryGrabItem(other);
        }
        else if (isgrab == 1 && (Input.GetKey(KeyCode.Space) || isButtonGrabPressed))
        {
            // Handle placing the object on various stations
            if (other.CompareTag("ConveyorBelt"))
            {
                PlaceOnConveyorBelt(other);
            }
            else if (other.CompareTag("cutting"))
            {
                PlaceOnCuttingBoard(other);
            }
            else if (other.CompareTag("Pan"))
            {
                PlaceOnPan(other);
                ExtinguishFire(other);
            }
            else if (other.CompareTag("Trashcan"))
            {
                DiscardItem();
            }
            else if (other.CompareTag("Pan")) // Check for fire to extinguish
            {
                //ExtinguishFire(other);
            }
            else if (other.CompareTag("Plate"))
            {
                PlaceOnPlate(other);
            }
        }
    }





    private void PlaceOnPlate(Collider plate)
    {
        Plate plateScript = plate.GetComponent<Plate>();

        if (plateScript == null || plateScript.hasKimbap)
        {
            Debug.LogWarning("Kimbap already on plate. Cannot place more items.");
            return;
        }

        Transform placePoint = null;
        Transform salamiPlacePoint = plate.transform.Find("PlatePlacePointCookedSalami");

        if (spawnedObject.CompareTag("Nori") && !plateScript.hasNori)
        {
            placePoint = plate.transform.Find("PlatePlacePoint");
            PlaceItemOnPlate(placePoint, plateScript, "hasNori", "Nori placed on the plate.");
        }
        else if (spawnedObject.CompareTag("Rice") && !plateScript.hasRice)
        {
            placePoint = plate.transform.Find("PlatePlacePointRice");
            PlaceItemOnPlate(placePoint, plateScript, "hasRice", "Rice placed on the plate.");
        }
        else if (spawnedObject.CompareTag("ChoppedFish") && !plateScript.hasChoppedFish)
        {
            placePoint = plate.transform.Find("PlatePlacePointMain");
            PlaceItemOnPlate(placePoint, plateScript, "hasChoppedFish", "ChoppedFish placed on the plate.");
        }
        else if (spawnedObject.CompareTag("CookedSalami") && !plateScript.hasCookedSalami)
        {
            // Place the cooked salami at a specific point
            if (salamiPlacePoint != null)
            {
                Collider coll = spawnedObject.GetComponent<Collider>();
                coll.enabled = false; 
                spawnedObject.transform.position = salamiPlacePoint.position;
                spawnedObject.transform.rotation = salamiPlacePoint.rotation; // Set the desired rotation
                spawnedObject.transform.parent = plate.transform;

                plateScript.hasCookedSalami = true;
                audioSource.PlayOneShot(grabSound);
                StartCoroutine(CooldownAfterPlacement()); // Start cooldown to prevent immediate grab

                Debug.Log("CookedSalami placed on the plate.");
            }
            else
            {
                Debug.LogWarning("Salami place point not found.");
            }
        }
        else if ((spawnedObject.CompareTag("ChoppedPepper") || spawnedObject.CompareTag("ChoppedCucumber") || spawnedObject.CompareTag("ChoppedCarrot")))
        {
            if (spawnedObject.CompareTag("ChoppedPepper") && !plateScript.hasChoppedPepper)
            {
                placePoint = plate.transform.Find("PlatePlacePointVeg");
                PlaceItemOnPlate(placePoint, plateScript, "hasChoppedPepper", "ChoppedPepper placed on the plate.");
            }
            else if (spawnedObject.CompareTag("ChoppedCucumber") && !plateScript.hasChoppedCucumber)
            {
                placePoint = plate.transform.Find("PlatePlacePointVeg");
                PlaceItemOnPlate(placePoint, plateScript, "hasChoppedCucumber", "ChoppedCucumber placed on the plate.");
            }
            else if (spawnedObject.CompareTag("ChoppedCarrot") && !plateScript.hasChoppedCarrot)
            {
                placePoint = plate.transform.Find("PlatePlacePointVeg");
                PlaceItemOnPlate(placePoint, plateScript, "hasChoppedCarrot", "ChoppedCarrot placed on the plate.");
            }
            else
            {
                Debug.LogWarning("Vegetable is already on the plate, cannot place the same vegetable again.");
            }
        }
        else
        {
            Debug.LogWarning("Ingredient is already on the plate, cannot place again.");
        }
    }

    // Method to place item on a plate with consistent logic
    private void PlaceItemOnPlate(Transform placePoint, Plate plateScript, string plateProperty, string logMessage)
    {
        if (placePoint != null && spawnedObject != null)
        {
            Collider coll = spawnedObject.GetComponent<Collider>();
            coll.enabled = false;
            spawnedObject.transform.position = placePoint.position;
            spawnedObject.transform.rotation = placePoint.rotation;
            spawnedObject.transform.parent = plateScript.transform;

            // Use reflection to set the boolean value for the plate property
            typeof(Plate).GetField(plateProperty).SetValue(plateScript, true);

            audioSource.PlayOneShot(grabSound);
            StartCoroutine(CooldownAfterPlacement()); // Start cooldown to prevent immediate grab

            Debug.Log(logMessage);
        }
        else
        {
            Debug.LogWarning("placePoint not found on the plate or no item to place.");
        }
    }

    // Cooldown to prevent immediate grabbing after placing an item
    private IEnumerator CooldownAfterPlacement()
    {
        ResetGrabState();
        yield return new WaitForSeconds(0.5f); // Add a delay of 0.5 seconds
    }

    private void TryGrabItem(Collider other)
    {
        string prefabName = "";

        bool isChoppedFromBoard = false; // Flag to check if item is a chopped ingredient from chopping board
        bool isFromPan = false;
        if (other.CompareTag("Fish")) { prefabName = "Fish"; isFish = 1; }
        else if (other.CompareTag("Carrot")) { prefabName = "Carrot"; isCarrot = 1; }
        else if (other.CompareTag("Pepper")) { prefabName = "Pepper"; isPepper = 1; }
        else if (other.CompareTag("Cucumber")) { prefabName = "Cucumber"; isCucumber = 1; }
        else if (other.CompareTag("Nori")) { prefabName = "Nori_001"; isNori = 1; }
        else if (other.CompareTag("Rice")) { prefabName = "Rice_001"; isRice = 1; }
        else if (other.CompareTag("Salami")) { prefabName = "Salami"; isSalami = 1; }
        else if (other.CompareTag("Extinguisher")) { prefabName = "Extinguisher"; isExtinguisher = 1; }
        else if (other.CompareTag("CookedSalami")) { prefabName = "CookedSalami"; isCookedSalami = 1; isFromPan = true; }


        if (other.CompareTag("ChoppedPepper")) { prefabName = "ChoppedPepper"; isChoppedPepper = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedCucumber")) { prefabName = "ChoppedCucumber"; isChoppedCucumber = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedFish")) { prefabName = "ChoppedFish"; isChoppedFish = 1; isChoppedFromBoard = true; isChopped = true; }
        else if (other.CompareTag("ChoppedCarrot")) { prefabName = "ChoppedCarrot"; isChoppedCarrot = 1; isChoppedFromBoard = true; isChopped = true; }


        if (other.CompareTag("Kimbap1")||other.CompareTag("Kimbap2")||other.CompareTag("Kimbap3")||other.CompareTag("Kimbap4")||other.CompareTag("Kimbap5") || other.CompareTag("Kimbap6"))
        {
            prefabName = "KimbapPlate"; isKimbap = 1;
        }

        if((other.CompareTag("ChoppedPepper")||
            other.CompareTag("ChoppedCucumber") ||
            other.CompareTag("ChoppedFish") ||
            other.CompareTag("ChoppedCarrot")||
            other.CompareTag("CookedSalami")) &&(other.CompareTag("Plate")))
        {
            Debug.Log("No No NO grab chop plz");
            return;
        }

        if (!string.IsNullOrEmpty(prefabName))
        {
           

            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab != null)
            {
                // Destroy the original object on the chopping board
                if (isChoppedFromBoard)
                {
                    isfoodinhere cuttingScript = other.GetComponentInParent<isfoodinhere>();
                    if (cuttingScript != null)
                    {
                        cuttingScript.ishere = false; // ���� ���¸� ����������� ǥ��
                    }
                    Destroy(other.gameObject);
                    Debug.Log("Destroyed chopped ingredient on chopping board: " + prefabName);
                }
                if (isFromPan)
                {
                    FireController fireController = other.GetComponentInParent<FireController>();
                    if (fireController != null)
                    {
                        fireController.GrabbedIngredient();
                    }

                    isfoodinhere panScript = other.GetComponentInParent<isfoodinhere>();
                    if (panScript != null)
                    {
                        panScript.ishere = false;
                    }
                    Destroy(other.gameObject);
                    Debug.Log("Destroyed cooked ingredient on pan: " + prefabName);
                }
                if (isKimbap==1)
                {
                    if (other.CompareTag("Kimbap1") || other.CompareTag("Kimbap2") || other.CompareTag("Kimbap3") || other.CompareTag("Kimbap4") || other.CompareTag("Kimbap5") || other.CompareTag("Kimbap6"))
                    {
                        //Destroy(other.gameObject);
                        Plate plateScript = other.GetComponentInParent<Plate>();
                        if (plateScript != null)
                        {

                            plateScript.hasNori = false;
                            plateScript.hasRice = false;
                            plateScript.hasChoppedFish = false;
                            plateScript.hasCookedSalami = false;
                            plateScript.hasChoppedPepper = false;
                            plateScript.hasChoppedCucumber = false;
                            plateScript.hasChoppedCarrot = false;
                            plateScript.hasKimbap = false;
                        }
                        prefab.tag = other.gameObject.tag;
                        Destroy(other.gameObject);
                    }
                
                }

                // Instantiate a new object in the player's hand
                spawnedObject = Instantiate(prefab, grabPoint.transform.position, grabPoint.transform.rotation);
                spawnedObject.transform.parent = grabPoint.transform;
                if (prefabName == "Extinguisher")
                {
                    spawnedObject.transform.localPosition = new Vector3(0.0f, 0.0f, -1.25f);
                    spawnedObject.transform.Rotate(90, 0, 0);
                }
                else if (prefabName == "KimbapPlate")
                {
                    spawnedObject.transform.Rotate(90, 0, 0);
                }
            
                isgrab = 1;
                grabbed = true;
                audioSource.PlayOneShot(grabSound);
                Debug.Log("Grabbed chopped item: " + prefabName);
            }
        }

        if (other.CompareTag("Lever"))
        {
            lightController.SetSkyboxLighting();
        }

        
    }

    private void ExtinguishFire(Collider other)
    {
        FireController fireControllerExtinguish = other.GetComponent<FireController>();

        if (fireControllerExtinguish != null && isExtinguisher == 1 && fireControllerExtinguish.isOnFire)
        {
            fireControllerExtinguish.ExtinguishFire();
            Destroy(spawnedObject); // Remove the extinguisher from hand
            isExtinguisher = 0; // Reset extinguisher state
            isgrab = 0;
            grabbed = false;
            spawnedObject = null;
            isfoodinhere panScript = other.GetComponentInParent<isfoodinhere>();
            if (panScript != null)
            {
                panScript.ishere = false;
            }
            //Destroy(other.gameObject);
            audioSource.PlayOneShot(extinguishSound);
            Debug.Log("Fire extinguished!");
        }
    }


    private void PlaceOnConveyorBelt(Collider other)
    {
        Transform placePoint = other.transform.Find("placePoint");
        if (placePoint != null && spawnedObject != null)
        {
            // Place the dish at the conveyor belt's place point
            spawnedObject.transform.position = placePoint.position;
            spawnedObject.transform.rotation = placePoint.rotation;
            spawnedObject.transform.parent = other.transform;

            ConveyorBelt conveyorScript = other.GetComponent<ConveyorBelt>();
            if (conveyorScript != null)
            {
                conveyorScript.onBelt.Add(spawnedObject);
            }

            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Enable physics for conveyor movement
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

            // Call GameManager to verify the correctness of the dish
            GameManager.instance.VerifyDish(spawnedObject);
            
            audioSource.PlayOneShot(grabSound);
            // Reset grab state
            ResetGrabState();
            Debug.Log("Object placed on conveyor belt at placePoint.");
        }
    }


    private void PlaceOnCuttingBoard(Collider other)
    {
        // Check for valid components on the cutting board
        Chopping choppingScript = other.GetComponent<Chopping>();
        isfoodinhere cuttingScript = other.GetComponent<isfoodinhere>();

        if (cuttingScript != null && choppingScript != null)
        {

            if (isChopped || isSalami == 1 || isNori == 1 || isRice==1 || isCookedSalami==1 || isExtinguisher ==1)
            {
                Debug.Log("Processed ingredients cannot be placed on the cutting board.");
                return;
            }
            // Only place if the cutting board is available and the player is holding an ingredient
            if (!cuttingScript.ishere && spawnedObject != null)
            {
                // Detach the object from the player's hand
                spawnedObject.transform.SetParent(choppingScript.cookpoint);

                // Place the object on the board at the cookpoint
                spawnedObject.transform.position = choppingScript.cookpoint.position;
                spawnedObject.transform.rotation = choppingScript.cookpoint.rotation;

                // Mark the board as occupied and reset player grab state
                cuttingScript.ishere = true;

                // Release the reference to the held object and reset states
                spawnedObject = null;

                audioSource.PlayOneShot(grabSound);
                ResetGrabState();

                Debug.Log("Ingredient placed on cutting board and detached from player.");
            }
        }
    }

    private void PlaceOnPan(Collider other)
    {
        isfoodinhere panScript = other.GetComponent<isfoodinhere>();

        if (panScript != null && !panScript.ishere && isSalami == 1)
        {
            Transform cookpointTransform = other.transform.Find("cookpoint");
            if (cookpointTransform != null)
            {
                spawnedObject.transform.position = cookpointTransform.position;
                spawnedObject.transform.rotation = cookpointTransform.rotation;
                spawnedObject.transform.SetParent(cookpointTransform);

                FireController fireController = other.GetComponent<FireController>();
                if (fireController != null)
                {
                    fireController.currentIngredient = spawnedObject;
                    fireController.hasIngredient = true;
                    fireController.StartCooking();
                    Debug.Log("Cooking started for the salami.");
                }

                panScript.ishere = true;

                audioSource.PlayOneShot(grabSound);
                ResetGrabState();
                Debug.Log("Salami placed on the pan at cookpoint.");
            }
        }
    }

    private void DiscardItem()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            audioSource.PlayOneShot(discardSound);
            ResetGrabState();
            Debug.Log("Item discarded.");
        }
    }

    private void ResetGrabState()
    {
        isgrab = 0;
        grabbed = false;
        isFish = 0;
        isCarrot = 0;
        isPepper = 0;
        isCucumber = 0;
        isNori = 0;
        isRice = 0;
        isSalami = 0;
        isExtinguisher = 0;
        isChoppedPepper = 0;
        isChoppedCucumber = 0;
        isChoppedFish = 0;
        isChoppedCarrot = 0;
        isCookedSalami = 0;
        isKimbap = 0;
        isChopped = false;
        spawnedObject = null; // Clear the reference to the held object
        HideAllArrows();

    }

}

