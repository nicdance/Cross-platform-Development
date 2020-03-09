using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManager : MonoBehaviour
{
 //   private static AudioPlayerManager instance = null;
    public AudioSource audio;
    private Options options;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //        return;
    //    }
    //    if (instance == this) return;
    //    Destroy(gameObject);
    //}

    void Start()
    {
        audio = GetComponent<AudioSource>();
        options = FindObjectOfType<Options>();
        audio.volume = options.MusicLevel;
        audio.Play();
    }
}
