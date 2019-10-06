using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMapScript : MonoBehaviour
{
    GameObject Food;
    float multiplier;

    // Use this for initialization
    void Start()
    {
        //Multiplier stores the size of the map along one axis relative to the small size of 11
        multiplier = DataScript.Multiplier;

        //Sets the Grid Pattern on the walls to the proper amount for the size of the map
        Renderer[] GridShader = GetComponentsInChildren<Renderer>();
        foreach (Renderer Rend in GridShader)
            Rend.material.SetInt("_GridSize", (int)multiplier * 11);

        //Sets the range of the light source in the middle of the map to reach all sides of the map
        GetComponentInChildren<Light>().range = 8.35f * multiplier;

        //Sets the position of the map so the bottom left corner on the negative z axis is the origin (0,0,0)
        switch ((int)multiplier)
        {
            case 1:
                transform.position = new Vector3(5, 5, 5);
                break;

            case 3:
                transform.position = new Vector3(16, 16, 16);
                break;

            case 5:
                transform.position = new Vector3(27, 27, 27);
                break;

            default:
                transform.position = new Vector3(5, 5, 5);
                break;

        }

        //Multiplies the size of the map by the chosen size
        transform.localScale *= multiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }


}
