using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class matchmanager : MonoBehaviour
{
    private int livesP1 = 3, livesP2 = 3;
    private InputManager2 inputManager;
    public GameObject player1, player2;
    //To control UI
    private UIController uiController;
    [SerializeField]
    [Header("Timers are divided by 100 to accomodate for time slow down")]
    float resetTimer;
    [SerializeField]
    float endResetTimer;
    [SerializeField]
    [Header("Enable to cause death on wall collision")]
    bool wallCollisionDeath;
    //Getter for wall collision var
    public bool deathOnWallCollision() { return wallCollisionDeath; }
    //Impact limit velocity to disable a hand or foot
    [Header("Used to check if the impact velocity is strong enough to disable a third level spot")]
    [SerializeField]
    float impactMinVelocityX;
    //Getter
    public float getMinVelocityX() { return impactMinVelocityX; }
    [SerializeField]
    float impactMinVelocityY;
    //Gettter
    public float getMinVelocityY() { return impactMinVelocityY; }

    private void Start()
    {
        //Get UI controller
        uiController = transform.parent.GetChild(0).GetComponent<UIController>();
        //Initial score display
        uiController.UpdateText(livesP1, livesP2);
        //Get input manager
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager2>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartReset();
        }
    }

    public void Incrementscore(GameObject hitter)
    {
        if (hitter.transform.root.gameObject == player1)
        {
            livesP2--;
        }
        else if (hitter.transform.root.gameObject == player2)
        {
            livesP1--;
        }
        transform.parent.GetChild(0).GetComponent<UIController>().UpdateText(livesP1, livesP2);
    }

    public void StartReset()
    {
        if (livesP1 != 0 || livesP2 != 0)
            StartCoroutine(ResetPositions(resetTimer));
        else
            ResetPositions(endResetTimer);
    }

    //Wait before reseting positions to show the blow of the hit
    public IEnumerator ResetPositions(float resetTime)
    {
        print("CALLED");
        //To accomodate for time slowing
        resetTime = resetTimer / 100;
        yield return new WaitForSeconds(resetTime);
        //Reset the positions
        foreach (resetter res in player1.GetComponentsInChildren<resetter>())
        {
            res.ResetTransform();
        }
        foreach (resetter res in player2.GetComponentsInChildren<resetter>())
        {
            res.ResetTransform();
        }

        //Reset hit after reset
        GameObject[] damagePoints = GameObject.FindGameObjectsWithTag("Head");
        foreach(GameObject g in damagePoints)
        {
            g.GetComponent<damagepoint>().ResetHit();
        }

        //If game is over, show UI
        //Pass in the name of winner, depending on lives
        if (livesP1 <= 0)
            uiController.ShowWinnerText("Player 2");
        else if (livesP2 <= 0)
            uiController.ShowWinnerText("Player 1");
        //If the game is still being played, allow selection again
        else
            inputManager.canSelect = true;
    }
}
