using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roads;
    public List<int> roadPercents;
    public GameObject[] levelObjects;
    public List<int> percents;
    private GameManager _gm;
    private Vector3 _lastPos;

    void Start()
    {
        _gm = GetComponent<GameManager>();
        LevelStart(new Vector3(0f, 0f, 0f));
    }
    
    public void StartPlay()
    {
        StartGenerateWithMovement(2f, _gm.playerController.playerMovement);
    }

    public void StopGenerate()
    {
        CancelInvoke(nameof(InitRoad));
    }

    public void StartGenerateWithMovement(float startTime, PlayerMovement playerMovement)
    {
        float repeatTime = playerMovement.maxSpeed / playerMovement.forwardSpeed * 21f;
        InvokeRepeating(nameof(InitRoad), startTime, repeatTime);
    }
    
    private void LevelStart(Vector3 position)
    {
        Instantiate(roads[0], position, Quaternion.identity);
        position.z += 20f;
        Instantiate(roads[0], position, Quaternion.identity);
        position.z += 20f;
        Instantiate(roads[0], position, Quaternion.identity);
        position.z += 20f;
        _lastPos = position;
        InitRoad();
        InitRoad();
        InitRoad();
    }

    private void InitRoad()
    {
        Vector3 position = _lastPos;
        
        for (int i = 0; i < 3; i++)
        {
            int randomValueRoad = RandomPercentValue(roadPercents);
            Instantiate(roads[randomValueRoad], position, Quaternion.identity);
            float localObjectPositions = -9f;
            
            if(randomValueRoad == 0)
                for (int j = 0; j < 2; j++)
                {
                    //This fucking code set value for number cubes  
                    int randomValueLevelObject = RandomPercentValue(percents);

                    if (randomValueLevelObject == 0)
                    {
                        GameObject localCube = Instantiate(levelObjects[randomValueLevelObject],
                            position + new Vector3(Random.Range(-2f, 2f), 1f,
                                localObjectPositions += Random.Range(3f, 7f)),
                            Quaternion.identity);

                        //TODO gm player value
                        //_gm.currentPow >= 3 ? _gm.currentPow - 2 : 1
                        localCube.GetComponent<NumberCube>().SetValue(_gm.currentPow >= 3 ? _gm.currentPow - 2 : 1,
                            _gm.currentPow + 2, _gm);
                    }
                    else if (randomValueLevelObject == 1)
                    {
                        float coinPosX = Random.Range(-2f, 2f);
                        levelObjects[randomValueLevelObject].GetComponent<Coin>().gm = _gm;
                        levelObjects[randomValueLevelObject].GetComponent<Coin>().targetTransform =
                            _gm.playerController.transform;
                        Instantiate(levelObjects[randomValueLevelObject],
                            position + new Vector3(coinPosX, 1f, localObjectPositions += Random.Range(4f, 7f)),
                            Quaternion.identity);
                        for (int k = 0; k < 3; k++)
                        {
                            Instantiate(levelObjects[randomValueLevelObject],
                                position + new Vector3(coinPosX, 1f, localObjectPositions += 1.5f),
                                Quaternion.identity);
                        }

                        localObjectPositions += 3f;
                        j++;
                    }
                    else if (randomValueLevelObject == 2)
                    {
                        levelObjects[randomValueLevelObject].GetComponent<ButtonHolder>().gm = _gm;
                        Instantiate(levelObjects[randomValueLevelObject],
                            position + new Vector3(Random.Range(-2f, 2f), 0.4f,
                                localObjectPositions += Random.Range(4f, 7f)),
                            Quaternion.identity);
                        localObjectPositions += 5f;
                        j++;
                    }
                    else if (randomValueLevelObject == 3)
                    {
                        float posX = Random.Range(-1f, 1f);
                        Instantiate(levelObjects[randomValueLevelObject],
                            position + new Vector3(posX, 1f,
                                localObjectPositions += Random.Range(4f, 7f)),
                            Quaternion.identity);
                        levelObjects[1].GetComponent<Coin>().gm = _gm;
                        levelObjects[1].GetComponent<Coin>().targetTransform =
                            _gm.playerController.transform;
                        Instantiate(levelObjects[1],
                            position + new Vector3(posX + 1.5f, 1f, localObjectPositions),
                            Quaternion.identity);
                        Instantiate(levelObjects[1],
                            position + new Vector3(posX - 1.5f, 1f, localObjectPositions),
                            Quaternion.identity);
                    }
                }

            position.z += 20;
        }

        _lastPos = position;
    }

    private int RandomPercentValue(IEnumerable<int> percents)
    {
        float random = Random.Range(0, 1f);
        List<float> hmm = new List<float>();
        float localValue = 0f;
        foreach (var percent in percents)
        { 
            localValue += (float)percent / 100;
            hmm.Add(localValue);
        }

        for (int i = 0; i < hmm.Count; i++)
        {
            if (HasPoint(random, i == 0 ? 0 : hmm[i - 1], hmm[i]))
            {
                return i;
            }
        }

        return 0;
    }

    private bool HasPoint (float x, float from, float to) {
        return x >= from && x <= to;
    }
}

/*else if (randomValueLevelObject == 4)
                {
                    float lavaRoadPosX = -2.8f;
                    Instantiate(levelObjects[randomValueLevelObject], 
                        position + new Vector3(lavaRoadPosX, 0.5f, localObjectPositions += Random.Range(15f, 18f)),
                        Quaternion.identity);
                    position.z += 20;
                    Instantiate(road, position, Quaternion.identity);
                    localObjectPositions += 10f;
                    j++;
                }*/