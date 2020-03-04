using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	
	public static SoundManager instance;

	public GameObject soundOffButton;
	public GameObject soundOnButton;

	public AudioSource gameMusic;

	private bool playSound = true;

	void Awake(){
		// if there is no instance set it to this object
		if (instance == null) {
			instance = this;
			// If there is an instance and it is not this then distroy this
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad(this);
	}

	public void TurnOnSound(){
		soundOffButton.SetActive (false);
		soundOnButton.SetActive (true);
		playSound = true;
		gameMusic.Play ();
	}


	public void TurnOffSound(){
		soundOffButton.SetActive (true);
		soundOnButton.SetActive (false);
		playSound = false;
		gameMusic.Stop ();
	}

	public bool IsSoundOn(){
		return playSound;
	}
}
