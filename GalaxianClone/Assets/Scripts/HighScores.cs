using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]

public class HighScores : MonoBehaviour
{
    public List<string> recordHolders;
    //public int[][] highScores;
    public List<int[]> highScores;
    public int currentScore = 0, currentLevel = 0;
    public string currentName;
    public static HighScores instance;
    public int currentHighScoreIndex = 0;
    public bool highScoreExists = false;
    public HighScoreManager highScoreManager;

	private string secretKey = "qpwo85cjfh281kc91"; // Edit this value and make sure it's the same as the one stored on the server
	public string addScoreURL = "https://www.chaosshard.com.au/Games/HighScore/GalaxianClone/addscore.php?"; //be sure to add a ? to your url

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        LoadHighScores();
        
        
    }
    public void CreateHighScores()
    {
        //highScores = new int[5][];
        highScores = new List<int[]>();
        for (int count = 0; count < highScores.Count; count++)
        {
            highScores[count] = new int[2];
            highScores[count][0] = 0;
            highScores[count][1] = 0;
        }
        
    }

    public List<int[]> GetHighScores()
    {
        return highScores;
    }

    public void SetHighScores(List<int[]> _highScores)
    {
        highScores = _highScores;
    }

    public void CreateHighScore(List<int[]> _highScores, int _score)
    {
        int[] tempScore = new int[2];



		StartCoroutine (PostScores(currentName,_score));
        for (int count = 0; count < _highScores.Count; count++)
        {
            if (_score > _highScores[count][0])
            {
                string tempName;
                tempName = recordHolders[count];
                tempScore[0] = _highScores[count][0];
                tempScore[1] = _highScores[count][1];
                recordHolders[count] = currentName;

                _highScores[count][0] = _score;

				currentName = tempName;
                _score = tempScore[0];
            }
            HighScores.instance.highScores[count][0] = _highScores[count][0];
            HighScores.instance.highScores[count][1] = _highScores[count][1];            
        }
        highScores.Add(new int[2]);
        recordHolders.Add(currentName);
        highScores[highScores.Count - 1][0] = _score;
        SaveHighScore();
    }

	// remember to use StartCoroutine when calling this function!
	IEnumerator PostScores(string name, int score)
	{
		//This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.

		// If name is blank set to Anonymous
		if(name == ""){name = "Anonymous";}

		string hash = Md5Sum(name + score + secretKey);

		string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
		Debug.Log (post_url);

		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done

		if (hs_post.error != null)
		{
			Debug.Log("There was an error posting the high score: " + hs_post.error);
		}
	}


    public void SaveHighScore()
    {
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/highscore.save", false);
        for (int count = 0; count < HighScores.instance.highScores.Count; count++)
        {
            writer.WriteLine(HighScores.instance.recordHolders[count]);
            writer.WriteLine(HighScores.instance.highScores[count][0]);
            writer.WriteLine(HighScores.instance.highScores[count][1]);
        }
        writer.Close();
    }


    public void LoadHighScores()
    {
        //Debug.Log(Application.persistentDataPath);
        highScores = new List<int[]>();
        recordHolders = new List<string>();
        if (File.Exists(Application.persistentDataPath + "/highscore.save"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/highscore.save", true);
			Debug.Log (Application.persistentDataPath + "/highscore.save");
            string nextLine;
            while((nextLine = reader.ReadLine()) != null)
            {
                recordHolders.Add(nextLine);
                int line;
                highScores.Add(new int[2]);
                int.TryParse(reader.ReadLine(), out line);
                highScores[highScores.Count-1][0] = line;
                int.TryParse(reader.ReadLine(), out line);
                highScores[highScores.Count-1][1] = line;
            }
            reader.Close();
        }
        else
        {
            CreateHighScore(HighScores.instance.highScores, HighScores.instance.currentScore);
            SaveHighScore();
        }
       

    }
    public int GetScore()
    {
        return HighScores.instance.currentScore;
    }

    public int GetLevel()
    {
        return HighScores.instance.currentLevel;
    }

    public void PrintHighScores()
    {
        for (int count = 0; count < HighScores.instance.highScores.Count; count++)
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log("HighScore" + count + ": " + HighScores.instance.highScores[count][0] + " " + HighScores.instance.highScores[count][1]);

        }
    }

	public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

    public void PayPalDonate()
    {
        Application.OpenURL("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=EMMKM86XPUE34");
    }
}
