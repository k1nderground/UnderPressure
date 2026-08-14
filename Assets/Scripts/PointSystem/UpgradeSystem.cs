using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] private NewMovementScript movementScript;
    [SerializeField] private PointSystem pointSystem;

    [Header("Upgrades Values")]
    [SerializeField] private float pushForceUpgrade = 5f;    // Сила одного толчка
    [SerializeField] private float maxSpeedUpgrade = 8f;     // Максимальная скорость
    [SerializeField] private float jumpForceUpgrade = 10f;   // Высота (сила) прыжка

    [Header("Prices")]
    [SerializeField] private int pushPrice = 10;
    [SerializeField] private int maxSpeedPrice = 15;
    [SerializeField] private int jumpPrice = 20;

    [Header("Limits")]
    [SerializeField] private int pushAmount;
    [SerializeField] private int maxSpeedAmount;
    [SerializeField] private int jumpAmount;

    [SerializeField] private int pushLimit = 20;
    [SerializeField] private int maxSpeedLimit = 15;
    [SerializeField] private int jumpLimit = 10;

    [Header("Text (Amount)")]
    [SerializeField] private TMP_Text pushText;
    [SerializeField] private TMP_Text maxSpeedText;
    [SerializeField] private TMP_Text jumpText;

    [Header("Text (Prices)")]
    [SerializeField] private TMP_Text pushPriceText;
    [SerializeField] private TMP_Text maxSpeedPriceText;
    [SerializeField] private TMP_Text jumpPriceText;

    private void Awake()
    {
        LoadUpgrades();
    }

    private void Start()
    {
        UpdateText();
        ApplyUpgrades();
    }

    public void LoadUpgrades()
    {
        pushForceUpgrade = PlayerPrefs.GetFloat("pushForceUpgrade", 5f);
        maxSpeedUpgrade = PlayerPrefs.GetFloat("maxSpeedUpgrade", 8f);
        jumpForceUpgrade = PlayerPrefs.GetFloat("jumpForceUpgrade", 10f);

        pushAmount = PlayerPrefs.GetInt("pushAmount", 0);
        maxSpeedAmount = PlayerPrefs.GetInt("maxSpeedAmount", 0);
        jumpAmount = PlayerPrefs.GetInt("jumpAmount", 0);

        pushPrice = PlayerPrefs.GetInt("pushPrice", 10);
        maxSpeedPrice = PlayerPrefs.GetInt("maxSpeedPrice", 15);
        jumpPrice = PlayerPrefs.GetInt("jumpPrice", 20);
    }

    public void SaveUpgrades()
    {
        PlayerPrefs.SetInt("pushAmount", pushAmount);
        PlayerPrefs.SetInt("maxSpeedAmount", maxSpeedAmount);
        PlayerPrefs.SetInt("jumpAmount", jumpAmount);

        PlayerPrefs.SetFloat("pushForceUpgrade", pushForceUpgrade);
        PlayerPrefs.SetFloat("maxSpeedUpgrade", maxSpeedUpgrade);
        PlayerPrefs.SetFloat("jumpForceUpgrade", jumpForceUpgrade);

        PlayerPrefs.SetInt("pushPrice", pushPrice);
        PlayerPrefs.SetInt("maxSpeedPrice", maxSpeedPrice);
        PlayerPrefs.SetInt("jumpPrice", jumpPrice);
    }

    // --- Методы прокачки ---

    public void UpgradePushForce()
    {
        if (pointSystem != null && pointSystem.getPoints() >= pushPrice && pushAmount < pushLimit)
        {
            pushForceUpgrade += 0.5f; // Прирост силы толчка
            pushAmount++;
            pointSystem.Withdrawl(pushPrice);
            pushPrice += 5 * pushAmount;
            
            OnUpgradeChanged();
        }
    }

    public void UpgradeMaxSpeed()
    {
        if (pointSystem != null && pointSystem.getPoints() >= maxSpeedPrice && maxSpeedAmount < maxSpeedLimit)
        {
            maxSpeedUpgrade += 1f; // Прирост максимальной скорости
            maxSpeedAmount++;
            pointSystem.Withdrawl(maxSpeedPrice);
            maxSpeedPrice += 10 * maxSpeedAmount;
            
            OnUpgradeChanged();
        }
    }

    public void UpgradeJumpForce()
    {
        if (pointSystem != null && pointSystem.getPoints() >= jumpPrice && jumpAmount < jumpLimit)
        {
            jumpForceUpgrade += 0.8f; // Прирост силы прыжка
            jumpAmount++;
            pointSystem.Withdrawl(jumpPrice);
            jumpPrice += 15 * jumpAmount;
            
            OnUpgradeChanged();
        }
    }

    private void OnUpgradeChanged()
    {
        if (pointSystem != null) pointSystem.UpdatePoints();
        SaveUpgrades();
        ApplyUpgrades();
        UpdateText();
    }

    public void ApplyUpgrades()
    {
        if (movementScript != null)
        {
            movementScript.SetPushForce(pushForceUpgrade);
            movementScript.SetMaxSpeed(maxSpeedUpgrade);
            movementScript.SetJumpForce(jumpForceUpgrade);
        }
    }

    public void UpdateText()
    {
        if (pushText != null) pushText.text = pushAmount + "/" + pushLimit;
        if (maxSpeedText != null) maxSpeedText.text = maxSpeedAmount + "/" + maxSpeedLimit;
        if (jumpText != null) jumpText.text = jumpAmount + "/" + jumpLimit;

        if (pushPriceText != null) pushPriceText.text = pushPrice + " Монет";
        if (maxSpeedPriceText != null) maxSpeedPriceText.text = maxSpeedPrice + " Монет";
        if (jumpPriceText != null) jumpPriceText.text = jumpPrice + " Монет";
    }
}