using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using YG;

public class PointSystem : MonoBehaviour
{
    [SerializeField] int Points;
    [SerializeField] int highScore;
    public static int NowPoints;
    public static int NowScore;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] TMP_Text nowPointsText;
    [SerializeField] TMP_Text ScoreText;
    [SerializeField] TMP_Text HighScoreText;

    [SerializeField] AudioSource src;
    [SerializeField] AudioClip pickup;

    void Start()
    {
        loadPoints();

        if (SceneManager.GetActiveScene().name == "Game")
        {
            ResetNowPoints();
        }

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
        NowScore = ScoreScript.score;
        PlayerPrefs.SetInt("NowScore", NowScore);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(NowScore > highScore){
            highScore = NowScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            YG2.SetLeaderboard("highscore", highScore);
        }
    }

    public void loadPoints()
    {
        Points = PlayerPrefs.GetInt("Points", 0);
        NowPoints = PlayerPrefs.GetInt("NowPoints", 0);
        if(pointsText != null){
        pointsText.text = "Монет "+Points;
        }
        NowScore = PlayerPrefs.GetInt("NowScore", 0);
        if(ScoreText != null){
        ScoreText.text = "Счёт "+NowScore;
        }
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if(HighScoreText != null){
            HighScoreText.text = "Лучший счет: "+highScore;
        }
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

    public void AddPoints(int amount)
    {
        NowPoints+=amount;
        src.PlayOneShot(pickup, 1f);
    }
}