using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    [SerializeField] public static int score;
    [SerializeField] public static int HighScore;
    [SerializeField] private float startPos;
    [SerializeField] private float nowPos;
    [SerializeField] GameObject Player;
    [SerializeField] TMP_Text ScoreText;

    void Start(){
        
        startPos = Mathf.Abs(transform.position.z);
        score = 0;
        ScoreText.text = "Очки: 0";
    }

    void Update(){
        if (Player.transform.position.z < transform.position.z){
            transform.position = new Vector3(0, 0, Player.transform.position.z);
            nowPos = Mathf.Abs(transform.position.z);

            score = (int)(Mathf.Abs(startPos - nowPos));

            updateText();
        }
    }

    void updateText(){
        ScoreText.text = "Очки: "+score;
    }
 }
