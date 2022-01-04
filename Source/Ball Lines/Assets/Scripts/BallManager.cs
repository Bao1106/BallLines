using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class BallManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> waitingBall;

    Queue<GameObject> waitingQueue; //Queue contain balls type wait to put on board

    public GameObject[] ballType; // Array contain type of ball
    public BallObject[,] usedPos; //Array contain ball position

    public TextMeshProUGUI scoreText, highScoreText;
    public AudioSource pickUpSound, dropDownSound, earnPoint, backgroundTheme;

    public int heightLimit;
    public int widthLimit;

    private int score = 0;

    public static void PrintValues(IEnumerable myCollection)
    {
        string str = "";
        foreach (Object obj in myCollection)
            str += obj.name + " ";

    }

    // Start is called before the first frame update
    void Start()
    {
        TimeController.instance.BeginTimer();
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 000).ToString();

        usedPos = new BallObject[widthLimit, heightLimit];
        addIndexToBallArray();
        waitingBall = new List<GameObject>();
        displayWaitingBall();

        waitingQueue = new Queue<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            addBallToQueue();
        }
        PrintValues(waitingQueue);

        for (int i = 0; i < 5; i++) 
        {
            GenerateBall();
        }
        UpdateWaitingBall();
    }

    void displayWaitingBall()
    {
        for(int i = 0; i < 3; i++)
        {
            var emptyBall = Instantiate(ballType[0], new Vector2(7 + i, 7.5f), Quaternion.identity);
            var ballClicker = emptyBall.GetComponent<BallClicker>();
            Destroy(ballClicker);
            waitingBall.Add(emptyBall);
        }
    }

    public void UpdateWaitingBall()
    {
        var lst = waitingQueue.ToList();
        for (int i = 0; i < 3; i++)
        {
            var a = waitingBall[i].GetComponent<SpriteRenderer>();
            var b = lst[i].GetComponent<SpriteRenderer>();
            a.color = b.color;
            
        }
        
    }

    void addBallToQueue()
    {
        waitingQueue.Enqueue(ballType[Random.Range(0, ballType.Length)]);
    }

    void addIndexToBallArray()
    {
        for(int i = 0; i < widthLimit; i++)
        {
            for(int j = 0; j < heightLimit; j++)
            {
                usedPos[i, j] = new BallObject(i,j);                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateBall()
    {
        //GameObject checkPos = Array.Find(usedPos, element => element == null);
        //Choose random position to place the ball

        var validPoses = (from BallObject valid in usedPos
                       where valid.UsedBall == null
                       select valid).ToList();

        if (validPoses.Count <= 0)
        {
            FindObjectOfType<GameManager>().GameOver();
            StopMusic();            
            return;
        }

        var pick = Random.Range(0, validPoses.Count); // Pick random a number from 0 to the number of empty Pos
        var pickPosition = validPoses[pick]; // Get value in list

        //Check position valid or not        
        Vector2 temp = new Vector2(pickPosition.UsedWidth, pickPosition.UsedHeight);                     
        GameObject ball = Instantiate(waitingQueue.Dequeue(), temp, Quaternion.identity); // Clone that sprite
        ball.transform.parent = this.transform;
        ball.name = ball.name.Replace("(Clone)", " ");

        var script = ball.GetComponent<BallClicker>();
        script.heightLimit = heightLimit;
        script.widthLimit = widthLimit;
        script.ballManager = this;


        usedPos[pickPosition.UsedWidth, pickPosition.UsedHeight].UsedBall = ball; // Link ball to that position
        addBallToQueue();
        PrintValues(waitingQueue);
    }

    public void ClickGenerateBall()
    {
        for (int i = 0; i < 3; i++)
        {
            GenerateBall();
        }
        UpdateWaitingBall();
        dropDownSound.Play();
    }
    public void PickUp()
    {
        pickUpSound.Play();
    }

    void StopMusic()
    {
        pickUpSound.volume = 0f;
        dropDownSound.volume = 0f;
        backgroundTheme.volume = 0.3f;
    }

    public BallObject getBallObjectAtLocation(int x, int y)
    {
        return usedPos[x, y];
    }

    public void TestGetBallHorizontal(int y)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();

        for (int i = 0; i < widthLimit; i++)
        {
            if (usedPos[i, y].UsedBall != null)
                str += usedPos[i, y].UsedBall.GetComponent<SpriteRenderer>().name + "";
            else
                str += "null ";
        }

        Debug.Log("Horizontal: " + str);
        //return lst;
    }

    public void TestGetBallVertical(int x)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();

        for (int j = 0; j < heightLimit; j++)
        {
            if (usedPos[x, j].UsedBall != null)
                str += usedPos[x, j].UsedBall.GetComponent<SpriteRenderer>().name + "";
            else
                str += "null ";
        }

        Debug.Log("Vertical: " + str);
    }

    public List<BallObject> getBallHorizontal(int y)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();
        for (int i = 0; i < widthLimit; i++)
        {
            lst.Add(usedPos[i, y]);
        }

        return lst;
    }

    public List<BallObject> getBallVertical(int x)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();
        for (int j = 0; j < heightLimit; j++)
        {
            lst.Add(usedPos[x, j]);
        }

        return lst;
    }

    public List<BallObject> getBallDiagonal(int x, int y)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();

        var min = Mathf.Min(x, y);
        var beginX = x - min;
        var beginY = y - min;

        while(beginX < widthLimit && beginY < heightLimit)
        {
            lst.Add(usedPos[beginX, beginY]);
            beginX++;
            beginY++;
        }

        return lst;
    }

    public List<BallObject> getReverseDiagonal(int x, int y)
    {
        string str = "";
        List<BallObject> lst = new List<BallObject>();

        var tempX = x;
        var tempY = y;

        while (tempX < widthLimit && tempY < heightLimit && tempX >= 0 && tempY >= 0) 
        {
            lst.Add(usedPos[tempX, tempY]);
            tempX--;
            tempY++;

        }

        lst.Reverse();
        x++; y--;
        while (x < widthLimit && y < heightLimit && x >= 0 && y >= 0)
        {
            lst.Add(usedPos[x, y]);
            x++;
            y--;

        }
        return lst;
    }

    public void getMatch(int x, int y)
    {
        var count = 0;
        count += calculateList(getBallHorizontal(y), x ,y);
        count += calculateList(getBallVertical(x), x, y);
        count += calculateList(getBallDiagonal(x, y), x, y);
        count += calculateList(getReverseDiagonal(x, y), x, y);

        if(count > 0)
        {
            DeleteBall(x, y);
            dropDownSound.Stop();
            earnPoint.Play();           
        }
    }

    public int calculateList(List<BallObject> ball, int x, int y)
    {
        var sampleColor = ball[0].UsedBall ? ball[0].UsedBall.name : "";
        var countBall = 1;
        var lstReturn = new List<BallObject>();
        var ind = new List<int>();
        var val = new List<int>();
        ind.Add(0);

        for (int i = 1; i < ball.Count; i++)
        {
            if (ball[i].UsedBall != null && sampleColor == ball[i].UsedBall.name)
            {
                countBall += 1;
            }
            else
            {
                ind.Add(i);
                val.Add(countBall);
                sampleColor = ball[i].UsedBall ? ball[i].UsedBall.name : "";
                countBall = 1;
            }
        }
        val.Add(countBall);
        countBall = 0;
        for(int i = 0; i < ind.Count; i++)
        {
            if(val[i] == 5)
            {
                DeleteListBalls(ball.GetRange(ind[i], val[i]), x, y);
                countBall += val[i];
                score += 100;
                scoreText.text = score.ToString(); 
            }

            if (val[i] >= 6)
            {
                DeleteListBalls(ball.GetRange(ind[i], val[i]), x, y);
                countBall += val[i];
                score += 300;
                scoreText.text = score.ToString();
            }

            if (score > PlayerPrefs.GetInt("HighScore", 000)) 
            {
                PlayerPrefs.SetInt("HighScore", score);
                highScoreText.text = score.ToString();
            }          
        }
        return countBall;

        //var str1 = "Index ";
        //var str2 = "Value ";

        //for (int i = 0; i < ind.Count; i++)
        //{
        //    str1 += " - " + ind[i];
        //    str2 += " - " + val[i];
        //}

        //Debug.Log(str1);
        //Debug.Log(str2);
    }

    public void DeleteListBalls(List<BallObject> balls, int x = -1, int y = -1)
    {
        foreach(BallObject ball in balls)
        {
            if(ball.UsedWidth != x || ball.UsedHeight != y)
            {
                if(ball.UsedBall != null)
                {
                    Destroy(ball.UsedBall);
                    ball.UsedBall = null;
                }
            }
        }
    }

    public void DeleteBall(int x, int y)
    {
        Destroy(usedPos[x, y].UsedBall);
        usedPos[x, y].UsedBall = null;
    }
}
