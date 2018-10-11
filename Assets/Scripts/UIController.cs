using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    //Button for advancing the gameplay
    private Button advanceBtn;
    private Text advanceTxt;

    const string p1turn = "Player 1 turn";
    const string p2turn = "Player 2 turn";
    const string executeTurn = "Execute";

    // Use this for initialization
    void Start()
    {
        //Get UI objects and store in vars
        GetUIObjects();

        //Set initial text for advanceBtn;
        ChangeTurn(1);
    }

    void GetUIObjects()
    {
        //Get the advance button components
        advanceBtn = GameObject.Find("Advance_Btn").GetComponent<Button>();
        advanceTxt = advanceBtn.gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    //Change text depending on status
    public void ChangeTurn(int id)
    {
        switch (id)
        {
            case 1:
                advanceTxt.text = p1turn;
                break;
            case 2:
                advanceTxt.text = p2turn;
                break;
            case 3:
                advanceTxt.text = executeTurn;
                break;
        }
    }

    //Execute button, attached to the advance play button
    public void ExecuteMove()
    {
        if (GetComponent<InputManager2>().gamePhase == InputManager2.GamePhases.Execute)
        {
            GetComponent<InputManager2>().ResetTimeSpeed();         
            GetComponent<InputManager2>().selectedBodyParts.Clear();
            GetComponent<InputManager2>().canSelect = true;
        }
    }
}
