using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //The display of lives
    private Text scoreText1, scoreText2;
    //Button for advancing the gameplay
    private Button advanceBtn;
    private Text advanceTxt;
    //EndGame Text 
    private Text winnerText;
    private GameObject endGameParent;

    const string p1turn = "Player 1 turn";
    const string p2turn = "Player 2 turn";
    const string executeTurn = "Execute";

    // Use this for initialization
    void Awake()
    {
        //Get UI objects and store in vars
        GetUIObjects();

        //Set initial text for advanceBtn;
        ChangeTurn(1);

        //Hide the end game text at start
        endGameParent.SetActive(false);
    }

    void GetUIObjects()
    {
        //Get the advance button components
        advanceBtn = GameObject.Find("Advance_Btn").GetComponent<Button>();
        advanceTxt = advanceBtn.gameObject.transform.GetChild(0).GetComponent<Text>();
        //Get text displays
        scoreText1 = GameObject.Find("ScoreLeft_Txt").GetComponent<Text>();
        scoreText2 = GameObject.Find("ScoreRight_Txt").GetComponent<Text>();
        //End Game objects
        endGameParent = GameObject.Find("EndGame_Parent");
        winnerText = GameObject.Find("EndGame_Text").GetComponent<Text>();
    }

    public void UpdateText(int livesP1, int livesP2)
    {
        scoreText1.text = livesP1.ToString();
        scoreText2.text = livesP2.ToString();
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

    //Show winner text
    public void ShowWinnerText(string winner)
    {
        winnerText.text = "Winner is " + winner;
        endGameParent.SetActive(true);
    }
}
