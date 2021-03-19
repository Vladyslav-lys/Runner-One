using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform boxTransform;
    public GameManager gm;
    private Camera _camera;
    private Vector3 _startPosition;
    private BoxCollider _collider;
    //private Vector3 _curPos;
    private Vector3 _offset;
    private float _offsetFactor;
    public UIManager uiManager;
    public P3dPaintDecal paintDecal;
    
    private void Start()
    {
        _camera = Camera.main;
        _offsetFactor = 2.5f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!gm.isStarted)
        {
            uiManager.InitStartCube();
            gm.StartPlay();
        }
        
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        
        _offset = _camera.ScreenToWorldPoint(mousePos);
        _offset.x = boxTransform.localPosition.x / _offsetFactor - _offset.x;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, offset.y, offset.z);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        
        Vector3 curPos = _camera.ScreenToWorldPoint(mousePos);
        curPos.x = curPos.x + _offset.x;
        
        if(!gm.isStarted)
            return;
        
        if (curPos.x > -1.2f && curPos.x < 1.2f)
        {
            boxTransform.localPosition = Vector3.MoveTowards(boxTransform.localPosition, 
                new Vector3(curPos.x * _offsetFactor, 0f ,0f), 55f * Time.deltaTime);
        }

        // if (curPos.x > -2f && curPos.x < 2f)
        // {
        //     _camera.transform.localPosition = Vector3.MoveTowards(_camera.transform.localPosition, 
        //         new Vector3(curPos.x, _camera.transform.localPosition.y, _camera.transform.localPosition.z), 
        //         Time.deltaTime);
        // }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
