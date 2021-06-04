using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Texture2D cursorIdleTexture;
    public Texture2D cursorHitTexture;
    public int Score = 50;
    public int ScorePerCar = 10;
    public int ScoreForMistake = 50;
    private AudioSource audioSource;
    public AudioClip wrong;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetBaseCursor();
        audioSource = GetComponent<AudioSource>();
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
        
    }

    void Update()
    {
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

    public static void GameOver()
    {
            // TODO
    }

    private CarController m_LastHitCar = null;
}
