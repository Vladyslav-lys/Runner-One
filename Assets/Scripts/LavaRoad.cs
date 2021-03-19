using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LavaRoad : MonoBehaviour
{
    private int[] _triggeredLayers;

    private void Start()
    {
        _triggeredLayers = new []{14, 9, 10, 11, 16};
    }

    private void OnTriggerStay(Collider other)
    {
        if (_triggeredLayers.Contains(other.gameObject.layer))
        {
            Destroy(other.gameObject);
        }
    }
}
