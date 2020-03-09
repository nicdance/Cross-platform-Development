using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSlider;
    private Options options;

    // Start is called before the first frame update
    void Start()
    {
        options = FindObjectOfType<Options>();
        musicSlider.value = options.MusicLevel;
        soundSlider.value = options.SoundLevel;
    }

    public void UpdateSound() {
        options.SoundLevel = soundSlider.value;
    }


    public void UpdateMusic()
    {
        options.MusicLevel = musicSlider.value;
    }


    private void OnDestroy()
    {
        options.SaveSettings();
    }
}
