using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;//projedeki tek GameManager oldugu icin singleton dedik
    public int best;//en iyi skor
    public int score;//suanki skor
    public int currentStage = 0;//suanki stage 
    public int tStage = 1;//text icin stage 
    

    private void Awake()
    {  
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        //en yuksek skoru kaydetme
        best = PlayerPrefs.GetInt("Highscore");
    }

    public void NextLevel()
    {
        //stage sayisini arttir topu sifirla yeni stage yukle
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
        tStage++;
    }

    public void RestartLevel()
    { //top olum parcasina carparsa seviyeyi sifirlama
        Debug.Log("Restarting Level");
        // Show Adds Advertisement.Show();
        singleton.score = 0;
        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score > best)
        {
            PlayerPrefs.SetInt("Highscore", score);
            best = score;
        }
    }


}
