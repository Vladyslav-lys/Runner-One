using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCube : MonoBehaviour
{
    public List<MergeCube> cubes;
    public ButtonHolder bh;
    
    private void OnEnable()
    {
        cubes[0].SetValue(bh.gm.currentPow, bh.gm.currentPow, bh.gm);
        cubes[1].gm = bh.gm;
        cubes[1].value = cubes[0].value;
        cubes[1].pow = cubes[0].pow;
        cubes[1].valueText.text = cubes[0].valueText.text;
        cubes[1].meshRenderer.material.color = cubes[0].meshRenderer.material.color;
    }

    public void MergeCubes()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            if(cubes[i])
                cubes[i].enabled = true;
        }
    }
}
