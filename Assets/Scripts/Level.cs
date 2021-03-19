using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameManager gm;
    public GameObject[] levelObjects;
    public List<int> percents;
    public int startPow;
    public int needPow;
    
    void Start()
    {
        gm.currentPow = startPow;
        gm.needPow = needPow;
        gm.levelGenerator.levelObjects = levelObjects;
        gm.levelGenerator.percents = percents;
    }
}