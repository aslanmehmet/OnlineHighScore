using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScores : MonoBehaviour {
    public Text[] highScoreText;
    HighScores highScoreManager;


    void Start () {
        for (int i = 0; i < highScoreText.Length; i++)
        {
            highScoreText[i].text = i + 1 + ".";
        }

        highScoreManager = GetComponent<HighScores>();

        StartCoroutine("RefreshHighScores");
	}	

    public void OnHighScoreDownloaded(HighScores.HighScore[] highScoreList)
    {
        for (int i = 0; i < highScoreText.Length; i++)
        {
            if (highScoreList.Length > i)
            {
                highScoreText[i].text += highScoreList[i].username + " - " + highScoreList[i].score; 
            }
        }
    }

    IEnumerator RefreshHighScores()
    {
        while (true)
        {
            highScoreManager.DowloadHighScores();
            yield return new WaitForSeconds(30);
        }
    }
}
