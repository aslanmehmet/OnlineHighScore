using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour {

    const string privateCode = "NMG43HU5_UKn23T_rNfYUA3mQmAJHI6EW-fzaJwMrfzg";
    const string publicCode = "597dda42b0b05c1ad44ed6e5";
    const string webURL = "http://dreamlo.com/lb/";
    public HighScore[] highScoresList;
    static HighScores instance;
    DisplayHighScores highScoreDisplay;

    private void Awake()
    {
        instance = this;
        highScoreDisplay = GetComponent<DisplayHighScores>();
    }
    
    //Yeni skor ekleme
    public static void AddNewHighScore(string username, int score)
    {
        instance.StartCoroutine(instance.UploadHighScore(username, score));
    }

    //Skoru Yukleme
    IEnumerator UploadHighScore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Succesful");
            DowloadHighScores();
        }
        else
        {
            print("Error uploading: " + www.error);
        } 
    }

    public void DowloadHighScores()
    {
        StartCoroutine("DownloadHighScoreFromDatabase");
    }

    //Veri tabanindan skorlari indir
    IEnumerator DownloadHighScoreFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighScores(www.text);
            highScoreDisplay.OnHighScoreDownloaded(highScoresList);
        }
        else
        {
            print("Error Dowloading: " + www.error);
        }
    }

    //Skoru ayristirma(parse) 
    void FormatHighScores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highScoresList = new HighScore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highScoresList[i] = new HighScore(username, score);
            print(highScoresList[i].username + ": " + highScoresList[i].score);
        }
    }

    public struct HighScore
    {
        public string username;
        public int score;

        public HighScore(string _username, int _score)
        {
            username = _username;
            score = _score;
        }
    }
}
