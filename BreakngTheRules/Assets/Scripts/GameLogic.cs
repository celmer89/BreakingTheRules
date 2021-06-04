using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Texture2D cursorIdleTexture;
    public Texture2D cursorHitTexture;
    public int Score = 100;
    public int CurrentLevel = 0;
    public int ScorePerCar = 10;
    public int ScoreForMistake = 50;
    private AudioSource audioSource;
    public AudioClip wrong;
    public List<float> LevelThresholds = new List<float>();
    public bool Busted = false;

    public List<GameObject> allCams = new List<GameObject>();
    int unlockedCams = 1;
    int activeCamIdx = 0;
    public int camUnlockBaseCost = 20;

    public int GetUnlockedCams()
    {
        return unlockedCams;
    }

    public int GetTotalCams()
    {
        return allCams.Count;
    }

    public int GetActiveCam()
    {
        return activeCamIdx;
    }

    public string GetActiveCamDesc()
    {
        switch (activeCamIdx)
        {
            case 0:
                return "Crossing A";
                break;
            case 1:
                return "Crossing B";
                break;

           case 2:
                return "Highway";
                break;

            case 3:
                return "Drone";
                break;
        }
        return "";
    }

    public int GetCamUnlockCost()
    {
        return unlockedCams * camUnlockBaseCost;
    }

    public void UnlockCamera()
    {
        if (unlockedCams < allCams.Count)
        {
            Score -= GetCamUnlockCost();
            unlockedCams++;
        }
    }

    public void PrevCam()
    {
        allCams[activeCamIdx].SetActive(false);

        if (activeCamIdx == 0)
        {
            activeCamIdx = unlockedCams - 1;
        }
        else
        {
            activeCamIdx--;
        }

        allCams[activeCamIdx].SetActive(true);
    }

    public void NextCam()
    {
        allCams[activeCamIdx].SetActive(false);

        if (activeCamIdx == unlockedCams - 1)
        {
            activeCamIdx = 0;
        }
        else
        {
            activeCamIdx++;
        }

        allCams[activeCamIdx].SetActive(true);
    }


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetBaseCursor();
        audioSource = GetComponent<AudioSource>();
        m_TrafficSystem = GameObject.FindGameObjectsWithTag("TrafficSystem")[0].GetComponent<TrafficSystem>();

        foreach (GameObject c in allCams)
        {
            c.SetActive(false);
        }

        allCams[activeCamIdx].SetActive(true);
    }

    private void SetBaseCursor()
    {
        Vector2 hotSpot = new Vector2(cursorIdleTexture.width / 2f, cursorIdleTexture.height / 2f);
        Cursor.SetCursor(cursorIdleTexture, hotSpot, CursorMode.Auto);
    }

    private void SetHitCursor()
    {
        Vector2 hotSpot = new Vector2(cursorHitTexture.width / 2f, cursorHitTexture.height / 2f);
        Cursor.SetCursor(cursorHitTexture, hotSpot, CursorMode.Auto);
    }

    void Start()
    {
        m_TrafficSystem.SetChallengeLevel(CurrentLevel);
    }

    void Update()
    {
        m_TimeInGame += Time.deltaTime;

        CheckLevelProgress();

        if (Busted)
        {
            m_CurrentTimeBusted += Time.deltaTime;
            if(m_CurrentTimeBusted > m_TimeToDisableBusted)
            {
                Busted = false;
                m_CurrentTimeBusted = 0;
            }
        }
       
        if (Camera.main)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hit_object = hit.transform.gameObject;
                if (hit_object.CompareTag("Car") && !hit.collider.isTrigger)
                {
                    SetHitCursor();
                    m_LastHitCar = hit_object.GetComponent<CarController>();
                    m_LastHitCar.SetHighlight(true);

                    if (Input.GetMouseButtonDown(0))
                    {
                        if (m_LastHitCar.GetIsRoadhog())
                        {
                            m_LastHitCar.Busted();
                            Score += ScorePerCar;
                            Busted = true;
                            m_CurrentTimeBusted = 0;
                        }
                        else
                        {
                            Score -= ScoreForMistake;
                            audioSource.PlayOneShot(wrong);

                            if (Score < 0)
                            {
                                GameOver();
                            }
                        }
                    }
                }
                else
                {
                    SetBaseCursor();
                    if(m_LastHitCar)
                    {
                        m_LastHitCar.SetHighlight(false);
                    }
                }
            }
            else
            {
                SetBaseCursor();
            }
        }
    }


    private void CheckLevelProgress()
    {
        if(CurrentLevel < LevelThresholds.Count && m_TimeInGame > LevelThresholds[CurrentLevel])
        {
            CurrentLevel++;
            m_TrafficSystem.SetChallengeLevel(CurrentLevel);
        }
    }

    public void GameOver()
    {
        // TODO
    }

    private CarController m_LastHitCar = null;
    private float m_TimeInGame = 0f;
    private TrafficSystem m_TrafficSystem;
    private float m_TimeToDisableBusted = 3;
    private float m_CurrentTimeBusted = 0;
}



