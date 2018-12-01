using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    public AudioClip[] punchSounds;

    void Start () {
        efxSource = GetComponent<AudioSource>();
        musicSource = GetComponent<AudioSource>();
        punchSounds = loadPunchSounds();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioClip[] loadPunchSounds() {

        AudioClip[] clips = new AudioClip[12];
        //Load an AudioClip (Assets/Resources/Audio/audioClip01.mp3)

        clips[0] = Resources.Load<AudioClip>("Audio/Punch1");
        clips[1] = Resources.Load<AudioClip>("Audio/punch2");
        clips[2] = Resources.Load<AudioClip>("Audio/punch3");
        clips[3] = Resources.Load<AudioClip>("Audio/punch4");
        clips[4] = Resources.Load<AudioClip>("Audio/punch5");
        clips[5] = Resources.Load<AudioClip>("Audio/punch6");
        clips[6] = Resources.Load<AudioClip>("Audio/punch7");
        clips[7] = Resources.Load<AudioClip>("Audio/punch8");
        clips[8] = Resources.Load<AudioClip>("Audio/punch9");
        clips[9] = Resources.Load<AudioClip>("Audio/punch10");
        clips[10] = Resources.Load<AudioClip>("Audio/punch11");
        clips[11] = Resources.Load<AudioClip>("Audio/Hadouken Sound effect (320  kbps)");

        return clips;
    }

    public void playRandomPunchSound() {

        int randomIndex = Random.Range(0, punchSounds.Length);

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = punchSounds[randomIndex];

        //Play the clip.
        efxSource.Play();
    }

    void Awake() {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip) {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        efxSource.clip = clip;

        //Play the clip.
        efxSource.Play();
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    void RandomizeSfx(params AudioClip[] clips) {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        efxSource.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        efxSource.clip = clips[randomIndex];

        //Play the clip.
        efxSource.Play();
    }
}