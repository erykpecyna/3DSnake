Snake Game
The basis of this game are two lists. One that contains positions occupied by the snake (snakeBlocks) and one that contains
positions not occupied by the snake (freeBlocks). FreeBlocks is initially populated with all of the positions within the map
and from there, freeBlocks will be removed base upon the position of the apple and the position of the snake. Anything outside of
the boundary wall was not included in these lists.
Additionally, there are five directional vectors that are defined at the beginning of Snakescript that will be used to allow the
snake to correctly rotate and move based on key press. Boolean variables were used to signify which button had been pressed instead
of actually rotating the snake in Update(). Rotating the snake in Update() would allow the user to rotate the snake in multiple
directions before the next translation, which goes against the rules of snake. The final if statement in Update() was included
such that the snake would immediately turn a certain direction and move even if the move function had not been called at that exact
movement in Start(). This essentially makes the snake movement more smooth because the snake will be able to turn on key down, not
only every 0.5 seconds. The doneMoving variable makes it such that Move() cannot be called while there is already a Move() function
running. Essentially, while the snake head is in the middle of rotating and translating, there cannot be another call to Move()
while the first Move() is running.
The Move() function is designed such that a new snake cube is created at the current location of the snake head. When the snake
head translates, the newly made snake cube will be in the position of the snake head previously. If no food is eaten as determined
by the checkFood() function, the first snake cube in the list (end of the tail) is deleted. If there is food, the tail is not
deleted and therefore, when the head moves, there will be an increase in the length of the snake by one block. This design makes
the function faster because only one block is moving (head) and one block or no block is being deleted.
Another important part of SnakeScript was the function getRoundedPosition, which returned integer numbers instead of doubles for
the position of GameObjects. This was vital to the functioning of the game because sometimes transform.position (function that
returns the position of the snake head), would return an inaccurate double (0.99999999998) for the position one. This made the
snake run into the wall when it wasn’t actually running into it.
The rest of MapScript and MenuScript are relatively self-explanatory and not extraordinarily complicated in any way. MenuScript
allows the user to select map size, changes the scene, and sends a map size variable to MapScript. MapScript changes the map size
according to the map size selected by the user and generates the map.
Food is generated through the use of two functions. GetFreeBlock() randomly selects a position within the map that is not occupied
by using freeBlocks as the guide. This free position is then inputted into generateFood() in the MapScript script. This function
simply generates the Apple at the position fed into it. The reason we separated the functions into different scripts was to make
parallel coding easier. Other functions like updateArray() from MapScript could be put into SnakeScript, but again it was easier
to code parallely this way.
AI Snake
For the AI we stripped out the controls. We decided it would best to move the snake head by simply translating it along the x, y,
and z axes instead of rotating it and moving it forward along the local z axis as we did in the player controlled snake.
 This was because the camera view was of the entire map rather than than the snake head like in the game.
ChooseDir() is called to find a direction that will move the snake towards the apple. It will randomize which direction it takes
to get closer to the apple and will take that direction if that direction is valid (no snake blocks and no map). If there is no
valid direction ChooseDir() will be called again. Only if there is no direction that the snake can take towards the apple, will
the loop exit (after 16 times). With a random number generator, there is always a chance that we didn’t pick the direction that
was valid and moves towards the apple. We calculated that chance that this would happen and it would be 0.22% (with 16 calls of
ChooseDir()). Even if this occurred, the snake would just take a random direction that was valid, so there is no real issue. If
there is no direction that the snake can take towards the apple without dying, it will go into the loop with chooseRandDir().
This find a direction (again randomly) that is valid for movement. The purpose of these random movements is to create less traps
for the snake to fall into. We can remove the random selections and make the snake movement “smoother” such that we could
definitively prove this claim statistically rather than “by inspection” (this is a quick one line change). However, this
statistical analysis is out of the scope of the project. Also, the randomized movements look cooler so…
The only other major design difference between AI and game modes is the camera angle manipulation required in AI CameraScript.
This was a very simply script that manipulated camera view based on the size of the map.
Our highest goal was creating a neural network in which the snake would learn how to play the game. Too many obstacles came in
front of us reaching this goal including the difficulty of importing machine learning packages into Unity. However, the framework
for creating a neural network is already in place because of the creation of a an array that stores every position inside the the
boundary with a corresponding digit. 0 means that there is nothing in that position, 1 means that a snake part is in that position,
and 2 means that there is a fruit in that position.

