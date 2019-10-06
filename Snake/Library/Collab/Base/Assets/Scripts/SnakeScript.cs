using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeScript : MonoBehaviour
{
    //Local Componenets and constants
    Vector3 dir = Vector3.forward;
    Vector3 up = Vector3.left * 90;
    Vector3 down = Vector3.right * 90;
    Vector3 left = Vector3.down * 90;
    Vector3 right = Vector3.up * 90;

    //Arrays to be used for various functions
    List<GameObject> snakeBlocks;
    List<Vector3> freeBlocks;

    //Saving other Gameobjects for use elsewhere as well as 
    //variables saved in other Scripts
    float multiplier;
    GameObject Map, Food;

    //Local vaeriables for controlling snake and food handling
    bool upb, downb, rightb, leftb, doneMoving;
    int foodQueue, score;

    void Start()
    {
        //Instantiates arrays
        freeBlocks = new List<Vector3>();
        snakeBlocks = new List<GameObject>();

        //Saves non-local variables and objects to a local reference
        Map = GameObject.Find("Map");
        Food = GameObject.Find("Food");
        multiplier = DataScript.Multiplier;

        //Sets bools used for controlling snake movement to an initial state of false
        upb = false;
        downb = false;
        rightb = false;
        leftb = false;
        doneMoving = true;
        foodQueue = 0;
        score = 0;
        
        //Instantiate a list that stores all the currently free blocks
        int condition = (int)(11 * multiplier);
        for (int x = 0; x < condition; x++)
            for (int y = 0; y < condition; y++)
                for (int z = 0; z < condition; z++)
                {
                    freeBlocks.Add(new Vector3(x, y, z));
                }

        freeBlocks.Remove(transform.position);

        Map.GetComponent<MapScript>().generateFood();

        //Update 3D Array in Map that might be used if we get to neural network
        Map.GetComponent<MapScript>().updateArray(getRoundedPosition(), 1);

        //Begin the movement repetition
        InvokeRepeating("Move", 1f, 0.5f);
    }
    private void Update()
    {
        //Checks for key presses and ensures only one movement is made per movement cycle
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            upb = false;
            downb = false;
            rightb = true;
            leftb = false;
        }
            
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            upb = false;
            downb = true;
            rightb = false;
            leftb = false;
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            upb = false;
            downb = false;
            rightb = false;
            leftb = true;
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upb = true;
            downb = false;
            rightb = false;
            leftb = false;
        }

        if((upb||downb||rightb||leftb)&&doneMoving)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0f, 0.5f);
        }

        
    }
    private void Move()
    {
        doneMoving = false;

        //FOOD HANDLING
        checkFood();
        if (snakeBlocks.Count > 0 || foodQueue > 0)
        {
            Vector3 lastPos;
            //Creates new SnakeBlock where the head is then moves the head
            //If no food has been eaten, destroy the tail
            snakeBlocks.Add(Instantiate(Resources.Load("SnakeBody"), getRoundedPosition(), Quaternion.identity) as GameObject);
            if (foodQueue == 0)
            {
                lastPos = snakeBlocks[0].transform.position;
                Map.GetComponent<MapScript>().updateArray(getRoundedPosition(), 0);
                Destroy(snakeBlocks[0]);
                snakeBlocks.RemoveAt(0);
            }
            else
            {
                foodQueue--;
                lastPos = getRoundedPosition();
            }
            
            freeBlocks.Add(lastPos);
        }
        else
        {
            freeBlocks.Add(getRoundedPosition());
            Map.GetComponent<MapScript>().updateArray(getRoundedPosition(), 0);
        }

        //MOVEMENT
        if (upb)
        { 
            transform.Rotate(up);
            upb = false;
        }
        else if (downb)
        {
            transform.Rotate(down);
            downb = false;
        }
        else if (leftb)
        {
            transform.Rotate(left);
            leftb = false;
        }
        else if (rightb)
        {
            transform.Rotate(right);
            rightb = false;
        }
        transform.Translate(dir);

        //STILL ALIVE CHECK
        checkAlive();


        //UPDATE 3D ARRAY
        if (SceneManager.GetActiveScene().buildIndex == 0)
            Map.GetComponent<MapScript>().updateArray(getRoundedPosition(), 1);

        //UPDATE FREE BLOCKS
        freeBlocks.Remove(getRoundedPosition());

        doneMoving = true;

    }

    //Checks if food has been eaten and makes new food if it has.
    private void checkFood()
    {
        if (getRoundedPosition() == GameObject.Find("Food").GetComponent<Transform>().position)
        {
            foodQueue += 1;
            score++;
            Map.GetComponent<MapScript>().updateArray(getRoundedPosition(), 1);
            Destroy(GameObject.Find("Food"));
            Map.GetComponent<MapScript>().generateFood();
            Food = GameObject.Find("Food");
            Map.GetComponent<MapScript>().updateArray(Food.transform.position, 2);
        }

    }

    //Checks if snake is still within bounds of the map and not intersecting itself
    private void checkAlive()
    {
        if(!freeBlocks.Contains(getRoundedPosition()))
        {
            if (!(getRoundedPosition() == GameObject.Find("Food").GetComponent<Transform>().position))
            {
                DataScript.Score = score;
                SceneManager.LoadScene(1);
            }
        }
            
        
    }

    public void setPosition(Vector3 pos) { transform.position = pos; }

    public Vector3 getFreeBlock()
    {
        System.Random rand = new System.Random();
        int ind = rand.Next(freeBlocks.Count);
        Vector3 foodPos = freeBlocks[ind];
        freeBlocks.RemoveAt(ind);
        return foodPos;
    }
    private Vector3 getRoundedPosition()
    {
        return new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }

}


