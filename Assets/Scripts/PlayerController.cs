using System;
using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using TMPro;
using TMPro.Examples;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI countText;
    public MeshRenderer meshRenderer;
    public GameManager gm;
    private bool _changed;
    private Color _changedColor;
    public PlayerMovement playerMovement;
    public float maxDistance;
    public LineRenderer lineRenderer;
    public LayerMask layerMask;
    private NumberCube _numberCube;
    public GameObject finishEffect;
    public GameObject magnetEffect;
    public GameObject upgradeSoundObj;
    public GameObject coinSoundObj;
    public GameObject levelWinSoundObj;
    public GameObject magnetSoundObj;
    public P3dPaintDecal paintDecal;
    
    private void Start()
    {
        gm.currentPow = 1;
        count = (int)Math.Pow(2,gm.currentPow);
        gm.currentColorIndex = (gm.currentPow - 1) % gm.colors.Count;
        meshRenderer.material.color = gm.colors[gm.currentColorIndex];
        paintDecal.Color = meshRenderer.material.color;
        countText.text = count.ToString();
    }

    private void Update()
    {
       // SearchCubes();
        if (_changed)
        {
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, gm.colors[gm.currentColorIndex], 1 * Time.deltaTime);
            if (meshRenderer.material.color == gm.colors[gm.currentColorIndex])
            {
                _changed = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!gm.isStarted)
            return;
        
        if (other.gameObject.layer == 9)
        {
            if (other.gameObject.GetComponent<NumberCube>().value == count)
            {
                UpgradePlayer();
            }
            else
            {
                if(PlayerPrefs.GetInt("IsVibration") != 0)
                    Handheld.Vibrate();

                if (gm.IsInfinityScene())
                {
                    gm.SetInfinityLoseUI(playerMovement);
                    return;
                }
                
                if (gm.currentPow == 1)
                {
                    gm.SetLose(playerMovement);
                    return;
                }
                
                DowngradePlayer();
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 10)
        {
            gm.coins++;
            gm.uiManager.levelCoinsText.text = gm.coins.ToString();
            gm.uiManager.CreateCoinUI(playerMovement.boxTransform);
            CreateSoundByObject(coinSoundObj);
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.layer == 11)
        {
            other.gameObject.GetComponent<BtnCube>().MergeCubes();
        }

        if (other.gameObject.layer == 14)
        {
            gm.isMagnetting = true;
            gm.magnettingTimeTicks = DateTime.Now.Ticks;
            magnetEffect?.SetActive(true);
            CreateSoundByObject(magnetSoundObj);
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 15)
        {
            gm.SetInfinityLoseOnLavaUI(playerMovement);
        }
    }

    private void CreateSoundByObject(GameObject sound)
    {
        if (PlayerPrefs.GetInt("Audio") != 0)
        {
            GameObject soundObj = GameObject.Instantiate(sound);
            soundObj.transform.SetParent(Camera.main.transform);
            soundObj.GetComponent<AudioSource>().Play();
            Destroy(soundObj, 1f);
        }
    }

    public void UpgradePlayer()
    {
        count *= 2;
        gm.currentPow++;
        gm.currentColorIndex = (gm.currentPow - 1) % gm.colors.Count;
        _changed = true;
        paintDecal.Color = gm.colors[gm.currentColorIndex];
        countText.text = count.ToString();
        
        if (gm.currentPow == gm.needPow && gm.IsLevelScene())
        {
            CreateSoundByObject(levelWinSoundObj);
            gm.SetWin(playerMovement);
            playerMovement.transform.localPosition = new Vector3(playerMovement.transform.localPosition.x, 1f, playerMovement.transform.localPosition.z);
            playerMovement.transform.localRotation = Quaternion.identity;
            meshRenderer.material.color = gm.colors[gm.currentColorIndex];
            return;
        }
        CreateSoundByObject(upgradeSoundObj);
        playerMovement.Jump();
        if (gm.IsInfinityScene())
        {
            gm.levelGenerator.StopGenerate();
            gm.levelGenerator.StartGenerateWithMovement(0f, playerMovement);

            if(!playerMovement.IsMaxSpeed(playerMovement.maxSpeed - 1))
                playerMovement.forwardSpeed += 5f;
        }
    }
    
    public void DowngradePlayer()
    {
        count /= 2;
        gm.currentPow--;
        gm.currentColorIndex = (gm.currentPow - 1) % gm.colors.Count;
        paintDecal.Color = gm.colors[gm.currentColorIndex];
        playerMovement.Obstacle();
        _changed = true;
        countText.text = count.ToString();
    }
    
    private void SearchCubes()
    {
        if (!gm.isStarted)
            return;
        
        RaycastHit hit;
        bool searchWall = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,maxDistance, layerMask);
        if (!searchWall)
        {
            lineRenderer.enabled = false;
            if (_numberCube != null)
            {
                _numberCube.positive.SetActive(false);
                _numberCube.negative.SetActive(false);
                _numberCube = null;
            }
            return;
        }

        lineRenderer.enabled = true;
        _numberCube = hit.collider.gameObject.GetComponent<NumberCube>();

        _numberCube.positive.SetActive(_numberCube.pow == gm.currentPow);
        _numberCube.negative.SetActive(_numberCube.pow != gm.currentPow);
        
        lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y -0.4f, transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(hit.point.x, transform.position.y -0.4f, hit.point.z));
    }

    public void SetFinish()
    {
        Instantiate(finishEffect, transform.position + new Vector3(2f, -0.2f, 0f), Quaternion.Euler(-90f, 0f, 0f));
        Instantiate(finishEffect, transform.position + new Vector3(-2f, -0.2f, 0f), Quaternion.Euler(-90f, 0f, 0f));
        Instantiate(finishEffect, transform.position + new Vector3(0f, -0.2f, 1f), Quaternion.Euler(-90f, 0f, 0f));
    }
}
