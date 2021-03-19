using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class NumberCube : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public MeshRenderer meshRenderer;
    public int value;
    public int pow;
    public int maxValue;
    public int colorIndex;
    public GameObject negative;
    public GameObject positive;
    public GameManager gm;

    public void SetValue(int minValue, int maxValue, GameManager gm)
    {
        this.gm = gm;
        (value, pow) = RandomValue(minValue, maxValue);
        valueText.text = value.ToString();
        colorIndex = (pow - 1) % gm.colors.Count;
        meshRenderer.material.color = this.gm.colors[colorIndex >= 0 ? colorIndex : 0];
    }

    protected (int, int) RandomValue(int minValue, int maxValue)
    {
        int result = 0;
        int pow = Random.Range(minValue, maxValue);
        result = (int)Math.Pow(2, pow);
        
        return (result, pow);
    }
}
