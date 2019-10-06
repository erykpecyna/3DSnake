using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScipt : MonoBehaviour 
{
    public Text scoreText;

    public void Start()
    {
        //Fetches score from DataSCript
        int score = DataScript.Score;
        scoreUpdate(score);
    }

    //If the player chooses any of these buttons these run and set the DataScript variable
    //multiplier to the appropriate size and load the game scene
    public void Small () 
    {
        DataScript.Multiplier = 1;
        SceneManager.LoadScene(0);
    }
    public void Medium()
    {
        DataScript.Multiplier = 3;
        SceneManager.LoadScene(0);
    }
    public void Large()
    {
        DataScript.Multiplier = 5;
        SceneManager.LoadScene(0);
    }

    //Converts the scoreText Object on the string to the latest player score
    public void scoreUpdate(int score)
    {
        if (scoreText)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

}
