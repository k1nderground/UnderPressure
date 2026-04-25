using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] MovementScript ms;
    [SerializeField] PressureScript ps;
    [SerializeField] PointSystem pointSystem;

    [Header("Upgrades")]
    [SerializeField] float pressureUpgrade = 0.2f;
    [SerializeField] float speedUpgrade = 5f;
    [SerializeField] float maxPressureUpgrade = 10f;

    [Header("Prices")]
    [SerializeField] int speedPrice;
    [SerializeField] int pressurePrice;
    [SerializeField] int maxPressurePrice;

    [Header("Limits")]
    [SerializeField] int pUAmount;
    [SerializeField] int mPUAmount;
    [SerializeField] int sUAmount;

    [SerializeField] int pULimit = 20;
    [SerializeField] int mPULimit = 30;
    [SerializeField] int sULimit = 10;

    [Header("Text")]
    [SerializeField] TMP_Text mpText;
    [SerializeField] TMP_Text pText;
    [SerializeField] TMP_Text sText;

    [SerializeField] TMP_Text price1;
    [SerializeField] TMP_Text price2;
    [SerializeField] TMP_Text price3;

    void Awake()
    {
        loadUpgrades();
    }

    void Start()
    {
        UpdateText();
        ApplyUpgrades();
    }

    public void loadUpgrades()
    {
        pressureUpgrade = PlayerPrefs.GetFloat("pressureUpgrade", 0.2f);
        speedUpgrade = PlayerPrefs.GetFloat("speedUpgrade", 5f);
        maxPressureUpgrade = PlayerPrefs.GetFloat("maxPressureUpgrade", 10f);

        pUAmount = PlayerPrefs.GetInt("pUAmount", 0);
        mPUAmount = PlayerPrefs.GetInt("mPUAmount", 0);
        sUAmount = PlayerPrefs.GetInt("sUAmount", 0);

        speedPrice = PlayerPrefs.GetInt("speedPrice", 15);
        maxPressurePrice = PlayerPrefs.GetInt("maxPressurePrice", 5);
        pressurePrice = PlayerPrefs.GetInt("pressurePrice", 10);
    }

    public void saveUpgrades()
    {
        PlayerPrefs.SetInt("pUAmount", pUAmount);
        PlayerPrefs.SetInt("mPUAmount", mPUAmount);
        PlayerPrefs.SetInt("sUAmount", sUAmount);

        PlayerPrefs.SetFloat("pressureUpgrade", pressureUpgrade);
        PlayerPrefs.SetFloat("speedUpgrade", speedUpgrade);
        PlayerPrefs.SetFloat("maxPressureUpgrade", maxPressureUpgrade);

        PlayerPrefs.SetInt("speedPrice", speedPrice);
        PlayerPrefs.SetInt("maxPressurePrice", maxPressurePrice);
        PlayerPrefs.SetInt("pressurePrice", pressurePrice);
    }

    public void UpgradeMaxPressure()
    {
        if (pointSystem.getPoints() >= maxPressurePrice && mPUAmount<mPULimit)
        {
            maxPressureUpgrade += 1f;
            mPUAmount++;
            pointSystem.Withdrawl(maxPressurePrice);
            maxPressurePrice += 5*mPUAmount;
            pointSystem.UpdatePoints();
            saveUpgrades();
            UpdateText();
        }
    }

    public void UpgradePressure()
    {
        if (pointSystem.getPoints() >= pressurePrice && pUAmount<pULimit)
        {
            pressureUpgrade += 0.1f;
            pUAmount++;
            pointSystem.Withdrawl(pressurePrice);
            pressurePrice += 5*pUAmount;
            pointSystem.UpdatePoints();
            saveUpgrades();
            UpdateText();
        }
    }

    public void UpgradeSpeed()
    {
        if (pointSystem.getPoints() >= speedPrice && sUAmount<sULimit)
        {
            speedUpgrade += 0.25f;
            sUAmount++;
            pointSystem.Withdrawl(speedPrice);
            speedPrice += 5*sUAmount;
            pointSystem.UpdatePoints();
            saveUpgrades();
            UpdateText();
        }
    }

    public void ApplyUpgrades()
    {
        if (ps != null)
    {
        ps.SetMaxPressure(maxPressureUpgrade);
        ps.SetSpeed(speedUpgrade);
        ps.SetPressureAmount(pressureUpgrade);
    }
    }

    public void UpdateText()
    {
        if (mpText != null)
        mpText.text = mPUAmount + "/" + mPULimit;

    if (pText != null)
        pText.text = pUAmount + "/" + pULimit;

    if (sText != null)
        sText.text = sUAmount + "/" + sULimit;

        price1.text = speedPrice + " Монет";
        price2.text = pressurePrice + " Монет";
        price3.text = maxPressurePrice + " Монет";
    }
    
    }
