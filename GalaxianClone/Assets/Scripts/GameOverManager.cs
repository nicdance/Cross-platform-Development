using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public TextMeshProUGUI scoresText;
    public Text enterYourName;
    public MenuManager menuManager;
    // Start is called before the first frame update
    void Start()
    {
        scoresText.text = HighScores.instance.currentScore+ "";
    }

    // Update is called once per frame
    void Update()
    {
        int _maxLength = 8;
        string _name = enterYourName.text;
        if (_name.Length > _maxLength)
        {
            _name = _name.Substring(0, _maxLength);
            enterYourName.text = _name;
        }
        HighScores.instance.currentName = _name;

    }
    
    public void MainMenuButton()
    {
        HighScores.instance.CreateHighScore(HighScores.instance.highScores, HighScores.instance.currentScore);
        menuManager.MainMenuScene();
    }
}
