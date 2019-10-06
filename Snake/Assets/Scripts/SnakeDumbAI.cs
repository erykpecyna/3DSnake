using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SnakeDumbAI : MonoBehaviour {

    //Local Componenets and constants
    Vector3 dir;
    Vector3 lastPos;
    System.Random rand;

    //Arrays to be used for various functions
    List<GameObject> snakeBlocks;
    List<Vector3> freeBlocks;

    //Saving other Gameobjects for use elsewhere as well as 
    //variables saved in other Scripts
    float multiplier;
    GameObject Map, Food;

    //Local vaeriables for controlling snake and food handling
    int foodQueue, score;

    //DUMB AI VARIABLES
    int diffX, diffY, diffZ;


    void Start()
    {
        //Instantiates arrays
        freeBlocks = new List<Vector3>();
        snakeBlocks = new List<GameObject>();


        //Saves non-local variables and objects to a local reference
        multiplier = DataScript.Multiplier;

        //Sets bools used for controlling snake movement to an initial state of false
        foodQueue = 0;
        score = 0;
        dir = Vector3.forward;
        rand = new System.Random();

        //Instantiate a list that stores all the currently free blocks
        int condition = (int)(11 * multiplier);
        for (int x = 0; x < condition; x++)
            for (int y = 0; y < condition; y++)
                for (int z = 0; z < condition; z++)
                {
                    freeBlocks.Add(new Vector3(x, y, z));
                }
        freeBlocks.Remove(transform.position);

        //Generates food in a random position
        generateFood();

        //Sets up move to be called every "x" seconds, this third parameter can be changed to adjust the snake speed
        InvokeRepeating("Move", 0f, 0.025f);
    }
    private void Update()
    {
        

    }
    private void Move()
    {
        //Check if the snakes head is in the same place as the food
        checkFood();
        Vector3 FoodPos = Food.transform.position;

        //Seperate the differences between the snake and the head into differences among axes
        int[] diff = new int[3];
        diff[0] = (int)(getRoundedPosition().x - FoodPos.x);
        diff[1] = (int)(getRoundedPosition().y - FoodPos.y);
        diff[2] = (int)(getRoundedPosition().z - FoodPos.z);


        //Choose a random direction towards the food up to 16 times until one of the directions has a free block along it
        int x = 0;
        do
        {
            if (x > 15) break;
            chooseDir(diff);
            x++;
        } while (!freeBlocks.Contains(getRoundedPosition() + dir) && (getRoundedPosition() + dir != Food.transform.position));

        //If no free block has still been found then just choose any random direction if it has a free block
        if (!freeBlocks.Contains(getRoundedPosition() + dir) && (getRoundedPosition() + dir != Food.transform.position))
        {
            chooseRandDir();
            x++;
        }

        //FOOD HANDLING
        
        if (snakeBlocks.Count > 0 || foodQueue > 0)
        {
            
            //Creates new SnakeBlock where the head is then moves the head
            //If no food has been eaten, destroy the tail
            snakeBlocks.Add(Instantiate(Resources.Load("SnakeBody"), getRoundedPosition(), Quaternion.identity) as GameObject);
            if (foodQueue == 0)
            {
                freeBlocks.Add(snakeBlocks[0].transform.position);
                Destroy(snakeBlocks[0]);
                snakeBlocks.RemoveAt(0);
            }
            else
            {
                //If at least one food has been eaten, it will not delete a block for one iteration
                foodQueue--;
            }
        }
        else
        {
            //If there is only one block to the snake(the head) free that position before moving the head
            freeBlocks.Add(getRoundedPosition());
        }

        //MOVEMENT
        transform.Translate(dir);

        //STILL ALIVE CHECK
        checkAlive();

        //UPDATE FREE BLOCKS
        freeBlocks.Remove(getRoundedPosition());


    }

    private void chooseDir(int[] diff)
    {
        //Choose one of the three axes randomly
        System.Random rand = new System.Random();
        int choice = rand.Next(2);

        //Comment out the previous two lines and uncomment the line 142, the movement will be smoother along the axes and
        //less random
        //int choice = 0;

        //If the random axis does not have a free block, move to the next case, this is just for efficiency
        switch (choice)
        {
            case 0:
                if (diff[0] < 0)
                    dir = new Vector3(1, 0, 0);
                else if (diff[0] > 0)
                    dir = new Vector3(-1, 0, 0);
                else goto case 1;
                break;
            case 1:
                if (diff[1] < 0)
                    dir = new Vector3(0, 1, 0);
                else if (diff[1] > 0)
                    dir = new Vector3(0, -1, 0);
                else goto case 2;
                break;
            case 2:
                if (diff[2] < 0)
                    dir = new Vector3(0, 0, 1);
                else
                    dir = new Vector3(0, 0, -1);

                break;
        }
    }

    private void chooseRandDir()
    {
        //Make a list of the six possible direction vectors to make the randomizing process easier
        List<Vector3> dirs;
        dirs = new List<Vector3>();

        dirs.Add(new Vector3(1, 0, 0));
        dirs.Add(new Vector3(-1, 0, 0));
        dirs.Add(new Vector3(0, 1, 0));
        dirs.Add(new Vector3(0, -1, 0));
        dirs.Add(new Vector3(0, 0, 1));
        dirs.Add(new Vector3(0, 0, -1));

        int x = rand.Next(dirs.Count-1);

        //Choose random directions until all have been chosen or the direction is valid
        while (dirs.Count > 0)
        {
            if (freeBlocks.Contains(getRoundedPosition() + dirs[x]) || getRoundedPosition() + dirs[x] == Food.transform.position)
            {
                dir = dirs[x];
                break;
            }
            dirs.RemoveAt(x);

            if (dirs.Count>0)
                x = rand.Next(dirs.Count - 1);
        }
    }

    //Checks if food has been eaten and makes new food if it has.
    private void checkFood()
    {
        if (getRoundedPosition() == Food.GetComponent<Transform>().position)
        {
            foodQueue += 1;
            score++;
            Destroy(Food);
            generateFood();
        }

    }

    //Checks if snake is still within bounds of the map and not intersecting itself
    private void checkAlive()
    {
        if (!freeBlocks.Contains(getRoundedPosition()))
        {
            if (!(getRoundedPosition() == Food.transform.position))
            {
                DataScript.Score = score;
                SceneManager.LoadScene(2);
            }
        }


    }

    public void setPosition(Vector3 pos) { transform.position = pos; }

    public void generateFood()
    {
        //Load food at a random free block and set the name to AIFood to make it findable outside of the object
        Vector3 pos = getFreeBlock();
        Food = Instantiate(Resources.Load("FP_apple"), pos, Quaternion.identity) as GameObject;
        Food.name = "AIFood";
    }

    public Vector3 getFreeBlock()
    {
        //chooses a random freeblock from the list and return it
        int ind = rand.Next(freeBlocks.Count);
        Vector3 foodPos = freeBlocks[ind];
        freeBlocks.RemoveAt(ind);
        return foodPos;
    }

    //Round the x y and z positions and return them
    private Vector3 getRoundedPosition()
    {
        return new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }


}
