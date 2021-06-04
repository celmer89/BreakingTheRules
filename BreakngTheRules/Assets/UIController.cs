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

    void Start()
    {
        
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
    }
}
