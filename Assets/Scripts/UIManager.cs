using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText; // TextMeshPro �ؽ�Ʈ ����
    public TextMeshProUGUI moneyText;
    public Image orderDisplay; // �ֹ��� ǥ���� Image ����
    public Sprite[] orderSprites; // 6���� �ֹ� �̹��� �迭

    public static UIManager instance;

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����Ѵٸ� ���� ������Ʈ�� �ı�
        }
    }

    private void Start()
    {
        UpdateOrderDisplay(); // �ʱ� �ֹ� ǥ�� ������Ʈ
        UpdateMoneyDisplay();
    }

    private void Update()
    {
        if (GameManager.instance != null)
        {
            // GameManager�� ���� �ð��� ������ �ؽ�Ʈ�� ǥ��
            float remainingTime = GameManager.instance.GetRemainingTime();
            timeText.text = timeText.text = $"{remainingTime:00}";
            UpdateMoneyDisplay();
        }
    }

    public void UpdateOrderDisplay()
    {
        if (GameManager.instance != null)
        {
            // GameManager���� ���� �ֹ��� �����ɴϴ�.
            int orderIndex = (int)GameManager.instance.currentOrder;

            // �ֹ��� �´� �̹����� orderDisplay�� ����
            if (orderIndex >= 0 && orderIndex < orderSprites.Length)
            {
                orderDisplay.sprite = orderSprites[orderIndex];
            }
        }
    }

    public void UpdateMoneyDisplay()
    {
        if (GameManager.instance != null)
        {
            // GameManager���� currentmoney ���� ������ �ؽ�Ʈ�� ����
            moneyText.text = $"Money: {GameManager.instance.GetCurrentMoney()}";
        }
    }
}
