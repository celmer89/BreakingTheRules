using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public Texture2D cursorIdleTexture;
    public Texture2D cursorHitTexture;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetBaseCursor();
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
                if (hit_object.CompareTag("Car"))
                {
                    SetHitCursor();
                }
                else
                {
                    SetBaseCursor();
                }
            }
            else
            {
                SetBaseCursor();
            }
        }
    }
}
