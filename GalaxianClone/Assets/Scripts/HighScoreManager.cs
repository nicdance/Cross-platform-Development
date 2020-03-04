using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public GameObject[] Positions;
    public GameObject[] RecordHolders;
    public GameObject[] HighScoreScores;
    //public GameObject[] HighScoreLevels;
    public TextMeshProUGUI scoresText;

    private string secretKey = "qpwo85cjfh281kc91"; // Edit this value and make sure it's the same as the one stored on the server
    string addScoreURL = "https://chaosshard.com.au/Games/HighScore/GalaxianClone/addscore.php?"; //be sure to add a ? to your url
    string highscoreURL = "https://chaosshard.com.au/Games/HighScore/GalaxianClone/display.php";

    public List<string> recordHolders;
    public List<int[]> highScores;

    public bool globalHighScores;
    public bool changeHighScores;

    public int currentStart;

    // Use this for initialization
    void Start()
    {
        currentStart = 0;
        StartCoroutine("GetScores");
        globalHighScores = false;
        changeHighScores = true;

        recordHolders = new List<string>();
        highScores = new List<int[]>();
    }

    private void Update()
    {
        if (globalHighScores == false && changeHighScores == true)
        {
            Debug.Log("Populate Local ");
            PopulateScore(HighScores.instance.recordHolders, HighScores.instance.highScores);
            changeHighScores = false;
        }
        else if (globalHighScores == true && changeHighScores == true)
        {
            Debug.Log("Populate Global");
            PopulateScore(recordHolders, highScores);
            changeHighScores = false;
        }
    }



    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    public IEnumerator GetScores()
    {
        //gameObject.guiText.text = "Loading Scores";
        WWW hs_get = new WWW(highscoreURL);
        yield return hs_get;

        if (hs_get.error != null)
        {
            print("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            //    gameObject.guiText.text = hs_get.text; // this is a GUIText that will display the scores in game.

            Debug.Log("Get Success");
            string result = hs_get.text;
            Debug.Log("details in result - " + result);
            string[] resultElements = result.Split(',');
            Debug.Log("Sice of ResultElements " + resultElements.Length);

            for (int i = 0; i < (resultElements.Length - 1); i = i + 2)
            {
                LoadHighScores(resultElements[i], int.Parse(resultElements[i + 1]));
               // Debug.Log(resultElements[i] + " - " + int.Parse(resultElements[i + 1]) + " - " + int.Parse(resultElements[i + 2]));
            }

        }
        //        PopulateScore();
    }



    void PopulateScore(List<string> _names, List<int[]> _highScores)
    {

        if (currentStart < _highScores.Count)
        {
            if (_highScores.Count <= 5)
            {
                for (int count = 0; count < 5; count++)
                {
                    Positions[count].GetComponent<Text>().text = "";
                    RecordHolders[count].GetComponent<Text>().text = "";
                    HighScoreScores[count].GetComponent<Text>().text = "";
                 //   HighScoreLevels[count].GetComponent<Text>().text = "";

                }
            }
            else
            {
                for (int count = 0; count < _highScores.Count; count++)
                {

                    if (count < 5)
                    {
                        Positions[count].GetComponent<Text>().text = "";
                        RecordHolders[count].GetComponent<Text>().text = "";
                        HighScoreScores[count].GetComponent<Text>().text = "";
                   //     HighScoreLevels[count].GetComponent<Text>().text = "";
                    }
                }
            }


            for (int count = 0; count < _highScores.Count; count++)
            {
                if ((count + currentStart) < (currentStart + 5) && (count + currentStart) < _highScores.Count)
                {
                    Positions[count].GetComponent<Text>().text = (count + currentStart + 1).ToString();
                    RecordHolders[count].GetComponent<Text>().text = _names[(count + currentStart)].ToString();
                    HighScoreScores[count].GetComponent<Text>().text = _highScores[(count + currentStart)][0].ToString();
                  //  HighScoreLevels[count].GetComponent<Text>().text = _highScores[(count + currentStart)][1].ToString();
                }

            }
        }

    }

    public void SetScores()
    {
        StartCoroutine("GetScores");
    }

    public void LoadHighScores(string _name, int _score)
    {
        recordHolders.Add(_name);
        highScores.Add(new int[2]);
        highScores[highScores.Count - 1][0] = _score;
       // highScores[highScores.Count - 1][1] = _level;

    }

    public void ActivateGlobalScores()
    {
        Debug.Log("ActivateGlobalScores");
        scoresText.text = "Global High Scores";
        globalHighScores = true;
        changeHighScores = true;
        currentStart = 0;
    }

    public void ActivateLocalScores()
    {
        Debug.Log("ActivateLocalScores");
        scoresText.text = "Local High Scores";
        globalHighScores = false;
        changeHighScores = true;
        currentStart = 0;
    }

    public void nextFiveScores()
    {
        currentStart = currentStart + 5;
        if (globalHighScores == false && currentStart > HighScores.instance.recordHolders.Count)
        {
            currentStart = currentStart - 5;
        }
        else if (globalHighScores == true && currentStart > recordHolders.Count)
        {
            currentStart = currentStart - 5;
        }
        changeHighScores = true;
    }

    public void previousFiveScores()
    {
        currentStart = currentStart - 5;
        if (currentStart < 0)
        {
            currentStart = currentStart + 5;
        }
        changeHighScores = true;
    }

}
