using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    [SerializeField] int Points;
    public static int NowPoints;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text nowPointsText;

    void Start()
    {
        loadPoints();
        NowPoints = 0;
        if (pointsText != null)
        {
            pointsText.text = "Монеты "+Points;
        }
    }
    void Update()
    {
        if(nowPointsText!=null){
        nowPointsText.text = "Монеты "+NowPoints;
        }
    }
    public static void addCoin()
    {
        NowPoints+= 1;
    }

    public void endGame()
    {
        Points += NowPoints;
        PlayerPrefs.SetInt("Points", Points);
        PlayerPrefs.SetInt("NowPoints", NowPoints);
    }

    public void loadPoints()
    {
        Points = PlayerPrefs.GetInt("Points", 0);
        NowPoints = PlayerPrefs.GetInt("NowPoints", 0);
        pointsText.text = "Монет "+Points;
    }

    public void ResetNowPoints()
    {
        NowPoints = 0;
        PlayerPrefs.SetInt("NowPoints", NowPoints);
    }

    public void Withdrawl(int Amount)
    {
        Points-=Amount;
    }

    public int getPoints()
    {
        return Points;
    }

    public void UpdatePoints()
    {
       pointsText.text = "Монет "+Points;
       PlayerPrefs.SetInt("Points", Points);
    }
}