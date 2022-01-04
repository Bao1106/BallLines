using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallClicker : MonoBehaviour
{
    private bool isDragging;
    private Vector2 offSet, originalPos, currentPos;

    public BallManager ballManager;
    public static BallClicker instance;

    public int widthLimit, heightLimit;

    private void OnMouseDown()
    {
        ballManager.PickUp();
        isDragging = true;
        originalPos = transform.position;
        offSet = GetMousePos() - (Vector2)transform.position;
        
    }

    private void OnMouseUp()
    {
        moveToNewPos();

        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);
        calculateForHori(x, y);
        calculateForVerti(x, y);
        calculateForDiag(x, y);
        calculateForDiagReverse(x, y);

        ballManager.GetComponent<BallManager>().ClickGenerateBall(); 
    }

    void moveToNewPos()
    {
        int newX = Mathf.RoundToInt(currentPos.x);
        int newY = Mathf.RoundToInt(currentPos.y);

        int oldX = Mathf.RoundToInt(originalPos.x);
        int oldY = Mathf.RoundToInt(originalPos.y);

        isDragging = false;

        if (newX >= widthLimit || newY >= heightLimit || newX < 0 || newY < 0)
        {
            transform.position = originalPos;
            return;
        }

        var oldPos = ballManager.getBallObjectAtLocation(oldX, oldY);
        var newPos = ballManager.getBallObjectAtLocation(newX, newY);
        if (newPos.UsedBall != null)
        {
            transform.position = originalPos;
            return;
        }
        transform.position = new Vector2(newX, newY);
        newPos.UsedBall = oldPos.UsedBall;
        oldPos.UsedBall = null;
    }

    void Awake()
    {
        originalPos = transform.position;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDragging)
            return;

        var mousePosition = GetMousePos();

        transform.position = mousePosition - offSet;
        currentPos = transform.position;
    }

    Vector2 GetMousePos()
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void calculateForHori(int x, int y)
    {
        string str = " ";
        List<BallObject> lst = ballManager.getBallHorizontal(y);
        ballManager.getMatch(x, y);
     
    }

    public void calculateForVerti(int x, int y)
    {
        string str = " ";
        List<BallObject> lst = ballManager.getBallVertical(x);
        ballManager.getMatch(x, y);
    }

    public void calculateForDiag(int x, int y)
    {
        string str = "";
        List<BallObject> lst = ballManager.getBallDiagonal(x, y);

        ballManager.getMatch(x, y);
    }

    public void calculateForDiagReverse(int x, int y)
    {
        string str = "";
        List<BallObject> lst = ballManager.getReverseDiagonal(x, y);

        ballManager.getMatch(x, y);
    }
}
