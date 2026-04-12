using UnityEngine;
using TMPro;

public class PointSystem : MonoBehaviour
{
    [SerializeField] int Points;
    static int NowPoints;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text nowPointsText;

    void Start()
    {
        loadPoints();
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
        NowPoints = 0;
        PlayerPrefs.SetInt("Points", Points);
    }

    public void loadPoints()
    {
        Points = PlayerPrefs.GetInt("Points", 0);
        pointsText.text = "Монет "+Points;
    }
}