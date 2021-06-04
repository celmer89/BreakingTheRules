using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameLogic gameLogic;
    public Text score;
    public Text trafficLevel;
    public Text busted;

    public Text currentCamTxt;
    public Button unlockCamBtn;

    void Start()
    {
        
    }

    public void UnlockEye()
    {
        if (gameLogic.Score > gameLogic.GetCamUnlockCost())
        {
            gameLogic.UnlockCamera();
        }
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Money: " + gameLogic.Score + "$";
        trafficLevel.text = "Traffic Level: " + gameLogic.CurrentLevel;

        if(gameLogic.Busted)
        {
            busted.enabled = true;
        }
        else
        {
            busted.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameLogic.PrevCam();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameLogic.NextCam();
        }


        if (gameLogic.GetUnlockedCams() == gameLogic.GetTotalCams())
        {
            unlockCamBtn.gameObject.SetActive(false);
        }
        else
        {
            if (gameLogic.Score > gameLogic.GetCamUnlockCost())
            {
                unlockCamBtn.interactable = true;
            }
            else
            {
                unlockCamBtn.interactable = false;
            }
        }

        unlockCamBtn.GetComponentInChildren<Text>().text =
            System.String.Format("Unlock Eye\n${0}", gameLogic.GetCamUnlockCost());

        currentCamTxt.text = 
            System.String.Format("Eye: {0}/{1}", gameLogic.GetActiveCam() + 1, gameLogic.GetUnlockedCams());

    }
}
