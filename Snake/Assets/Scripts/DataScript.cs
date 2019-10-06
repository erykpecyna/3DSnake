public static class DataScript {

    //Multiplier is the relative size of the map and
    //score is the score, these variables can be accessed from
    //any object in the game
    private static float multiplier;
    private static int score;

    public static float Multiplier
    {
        get
        {
            return multiplier;
        }
        set
        {
            multiplier = value;
        }
    }

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
}
