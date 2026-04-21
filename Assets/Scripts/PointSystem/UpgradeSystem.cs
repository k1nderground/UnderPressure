using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] MovementScript ms;
    [SerializeField] PressureScript ps;
    [SerializeField] PointSystem pointSystem;

    [Header("Upgrades")]
    [SerializeField] float pressureUpgrade = 10f;
    [SerializeField] float speedUpgrade = 5f;
    [SerializeField] float maxPressureUpgrade = 0.2f;

    [Header("Prices")]
    [SerializeField] int speedPrice;
    [SerializeField] int pressurePrice;
    [SerializeField] int maxPressurePrice;

    [Header("Limits")]
    [SerializeField] int pUAmount;
    [SerializeField] int mPUAmount;
    [SerializeField] int sUAmount;

    [SerializeField] int pULimit;
    [SerializeField] int mPULimit;
    [SerializeField] int sULimit;

    [Header("Text")]
    [SerializeField] TMP_Text mpText;
    [SerializeField] TMP_Text pText;
    [SerializeField] TMP_Text sText;

    void Start()
    {
        loadUpgrades();

        pULimit = 20;
        mPULimit = 30;
        sULimit = 10;

        if(ms != null && ps != null)
        {
            ApplyUpgrades();
        }
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
        }
    }

    public void ApplyUpgrades()
    {
        ps.SetMaxPressure(maxPressureUpgrade);
        ps.SetSpeed(speedUpgrade);
        ps.SetPressureAmount(pressureUpgrade);
    }

    public void UpdateText()
    {
        mpText.text = mPUAmount + "/" + mPULimit;
        pText.text = pUAmount + "/" + pULimit;
        sText.text = sUAmount + "/" + sULimit;
    }
    
    }
