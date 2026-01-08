using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // ��� ���� ���� Ȯ�� ���� (public���� �����Ͽ� �ܺο��� ���� ����)
    public bool hasNori = false;
    public bool hasRice = false;

    public bool hasChoppedFish = false;
    public bool hasCookedSalami = false;
    public bool hasChoppedPepper = false;
    public bool hasChoppedCucumber = false;
    public bool hasChoppedCarrot = false;
    // Sparkle Effect
    public GameObject sparklePrefab; 
    public float sparkleDuration = 1f;

    // ������ ��� ������
    public GameObject kimbapPrefab;
    public bool hasKimbap = false;

    // ��� ��ġ ����Ʈ
    public Transform placePointNori;

    void Update()
    {
        if (!hasKimbap && CheckIngredientsReady())
        {
            CreateKimbap();
        }

    }

    // Plate ���� �ʱ�ȭ �Լ�
    public void ResetPlate()
    {
        hasNori = false;
        hasRice = false;
        hasChoppedFish = false;
        hasChoppedPepper = false;
        hasChoppedCucumber = false;
        hasChoppedCarrot = false;
        hasCookedSalami = false;
        hasKimbap = false;
    }

    // ��� ��ᰡ �غ�Ǿ����� Ȯ���ϴ� �Լ�
    public bool CheckIngredientsReady()
    {
        // �⺻ ����: ��� ���� �ְ�, �ּ� �ϳ��� ���� ���� �ּ� �ϳ��� ��ä�� �־�� ����� ���� �� ����
        bool hasMainIngredient = hasChoppedFish || hasCookedSalami;
        bool hasVegetable = hasChoppedPepper || hasChoppedCucumber || hasChoppedCarrot;

        return hasNori && hasRice && hasMainIngredient && hasVegetable;
    }

    // ��� ���� �Լ�
    public void CreateKimbap()
    {
        if (!CheckIngredientsReady())
        {
            Debug.LogWarning("All necessary ingredients are not present on the plate.");
            return;
        }

        // Plate ���� ��Ḹ �����ϰ�, ��ġ ����Ʈ�� ���ܵ�
        foreach (Transform child in transform)
        {
            if (child.name != "PlatePlacePoint" &&
                child.name != "PlatePlacePointRice" &&
                child.name != "PlatePlacePointMain" &&
                child.name != "PlatePlacePointVeg" &&
                child.name != "PlatePlacePointCookedSalami")
            {
                Destroy(child.gameObject);
            }
        }

        // ��� ������Ʈ ����
        GameObject kimbap = Instantiate(kimbapPrefab, placePointNori.position, placePointNori.rotation);

        // ��� �±� ���� (��� ���տ� ���� ���1 ~ ���6)
        kimbap.tag = DetermineKimbapTag();

        // Plate�� ���̵��� ����
        kimbap.transform.parent = transform;
        hasKimbap = true;

        TriggerSparkleEffect();

    }

    // ��� �±� ���� �Լ�
    private string DetermineKimbapTag()
    {
        // ���÷� �±׸� �����ϴ� ���� �ۼ� (��� ���տ� ���� �±� ����)
        if (hasCookedSalami && hasChoppedCucumber)
        {
            Debug.Log("iskimbap1");
            return "Kimbap1";
        }
        else if (hasCookedSalami && hasChoppedPepper)
        {
            Debug.Log("iskimbap2");
            return "Kimbap2";
        }
        else if (hasCookedSalami && hasChoppedCarrot)
        {
            Debug.Log("iskimbap3");
            return "Kimbap3";
        }
        else if (hasChoppedFish && hasChoppedCucumber)
        {
            Debug.Log("iskimbap4");
            return "Kimbap4";
        }
        else if (hasChoppedFish && hasChoppedPepper)
        {
            Debug.Log("iskimbap5");
            return "Kimbap5";
        }
        else if (hasChoppedFish && hasChoppedCarrot)
        {
            Debug.Log("iskimbap6");
            return "Kimbap6";
        }
        else
            return "Kimbap1"; // �⺻��
    }

    private void TriggerSparkleEffect()
    {
        if (sparklePrefab != null)
        {
            // Instantiate the sparkle effect at the plate position
            GameObject sparkle = Instantiate(sparklePrefab, placePointNori.position, Quaternion.identity);

            // Destroy the sparkle effect after a certain duration
            Destroy(sparkle, sparkleDuration);
        }
        else
        {
            Debug.LogWarning("No sparkle prefab assigned!");
        }
    }
}
