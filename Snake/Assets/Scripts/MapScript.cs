using System;
using UnityEngine;

public class MapScript : MonoBehaviour
{

    GameObject Food;
    GameObject Snake;

    // MapArray was meant to be an input vector for a neural network (our best case scenario)
    int[,,]MapArray;
    float multiplier;
    int size;

    // Use this for initialization
    void Start()
    {
        //Stores the Snake object for easier access to its attrributes and methods later
        Snake = GameObject.Find("SnakeHead");

        //Fetches the multiplier chosen by the player
        multiplier = DataScript.Multiplier;

        //Sets the size of the network input vector to the proper dimensions
        MapArray = new int[(int)multiplier * 11, (int)multiplier * 11, (int)multiplier * 11];

        //Sets the Grid Pattern on the walls to the proper amount for the size of the map
        Renderer[] GridShader = GetComponentsInChildren<Renderer>();
        foreach (Renderer Rend in GridShader)
            Rend.material.SetInt("_GridSize", (int)multiplier * 11);

        //Sets the range of the light source in the middle of the map to reach all sides of the map
        GetComponentInChildren<Light>().range = 8.35f * multiplier;
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

        //Sets the position of the map so the bottom left corner on the negative z axis is the origin (0,0,0)
        transform.localScale *= multiplier;

        //Set the Snakes position to the middle of the map
        Snake.GetComponent<SnakeScript>().setPosition(transform.position);

        //Instantiate a 3D array of the game map for use as an input vector later
        int condition = (int)(11 * multiplier);
        for (int x = 0; x < condition; x++)
            for (int y = 0; y < condition; y++)
                for (int z = 0; z < condition; z++)
                {
                    MapArray[x, y, z] = 0;
                }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateFood()
    {
        //Fetches a free block in the map from SnakeScript
        Vector3 pos = Snake.GetComponent<SnakeScript>().getFreeBlock();

        //Creates a new food object from the prefab made in Unity
        Food = Instantiate(Resources.Load("FP_apple"), pos, Quaternion.identity) as GameObject;

        //Sets the name of the object so it is findable outside this script
        Food.name = "Food";
    }
    
    //This allows any other object to update this scripts MapArray
    public void updateArray(Vector3 gato, int value)
    {
         int xPos = (int)Mathf.Round(gato.x);
         int yPos = (int)Mathf.Round(gato.y);
         int zPos = (int)Mathf.Round(gato.z);

        //try { MapArray[xPos, yPos, zPos] = value; }
        //catch (Exception e) { Debug.Log(e); }

    }
}
