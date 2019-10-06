using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AIMenuScript : MonoBehaviour
{
    public Text scoreText;

    public void Start()
    {

    }

    //If the player chooses any of these buttons these run and set the DataScript variable
    //multiplier to the appropriate size and load the game scene
    public void Small()
    {
        DataScript.Multiplier = 1;
        SceneManager.LoadScene(3);
    }
    public void Medium()
    {
        DataScript.Multiplier = 3;
        SceneManager.LoadScene(3);
    }
    public void Large()
    {
        DataScript.Multiplier = 5;
        SceneManager.LoadScene(3);
    }
}
