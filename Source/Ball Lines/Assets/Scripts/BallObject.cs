using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : ScriptableObject
{
    int usedWidth;
    int usedHeight;

    GameObject usedBall = null;

    public int UsedWidth { get => usedWidth; set => usedWidth = value; }
    public int UsedHeight { get => usedHeight; set => usedHeight = value; }
    public GameObject UsedBall { get => usedBall; set => usedBall = value; }

    public BallObject(int width, int height)
    {
        UsedWidth = width;
        UsedHeight = height;
    }

}
