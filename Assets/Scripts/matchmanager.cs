using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class matchmanager : MonoBehaviour
{
    public int score1 = 0, score2 = 0;
    public GameObject player1, player2;
    public Text scoreText1, scoreText2;

    public void Incrementscore(GameObject hitter)
    {
        if (hitter.transform.root.gameObject == player1)
        {
            score1++;
            scoreText1.text = score1.ToString();
        }
        else if (hitter.transform.root.gameObject == player2)
        {
            score2++;
            scoreText2.text = score2.ToString();
        }
        foreach (resetter res in player1.GetComponentsInChildren<resetter>())
        {
            res.ResetTransform();
        }
        foreach (resetter res in player2.GetComponentsInChildren<resetter>())
        {
            res.ResetTransform();
        }
    }
}
