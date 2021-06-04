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
    public GameObject gameOver;

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

        if(gameLogic.gameOver)
        {
            gameOver.SetActive(true);
            return;
        }
        gameOver.SetActive(false);

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

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            gameLogic.PrevCam();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
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
            System.String.Format("Unlock Next Eye\n${0}", gameLogic.GetCamUnlockCost());

        currentCamTxt.text = 
            System.String.Format("Eye: {0}/{1} {2}", gameLogic.GetActiveCam() + 1, gameLogic.GetUnlockedCams(), gameLogic.GetActiveCamDesc());

    }
}
