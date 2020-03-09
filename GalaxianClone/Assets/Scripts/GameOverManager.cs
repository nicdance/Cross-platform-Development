using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public TextMeshProUGUI pcScoresText;
    public TextMeshProUGUI mobileScoresText;
    public Text enterYourName;
    public MenuManager menuManager;

    public GameObject mobileUI;
    public GameObject pcUI;
    // Start is called before the first frame update
    void Start()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBGL
        pcUI.SetActive(true);
        mobileUI.SetActive(false);
        pcScoresText.text = HighScores.instance.currentScore + "";
#endif

#if UNITY_ANDROID
        mobileUI.SetActive(true);
        pcUI.SetActive(false);
        mobileScoresText.text = HighScores.instance.currentScore + "";
        HighScores.instance.currentName = "MobileUser";
#endif
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBGL
        int _maxLength = 12;
        string _name = enterYourName.text;
        if (_name.Length > _maxLength)
        {
            _name = _name.Substring(0, _maxLength);
            enterYourName.text = _name;
        }
        HighScores.instance.currentName = _name;
#endif

    }
    
    public void MainMenuButton()
    {
        HighScores.instance.CreateHighScore(HighScores.instance.highScores, HighScores.instance.currentScore);
        menuManager.MainMenuScene();
    }
}
