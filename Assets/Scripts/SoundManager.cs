using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private GameStatesManager gameStatesManager;	//Refers to the GameStateManager
	private StaticData.AvailableGameStates gameState;	//Mimics the GameStateManager's gameState variable at all time

    public AudioSource audioSource1;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource audioSource2;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             
    public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.
    public AudioClip[] punchSounds;
    public AudioClip themeSong;
    public AudioClip buttonSound;
    public AudioClip deathSound;
    public AudioClip jumpSound;

    public void playRandomPunchSound() {
        int randomIndex = Random.Range(0, punchSounds.Length);
        audioSource2.PlayOneShot(punchSounds[randomIndex]);
    }

    public void playJumpSound() {
        audioSource2.clip = jumpSound;
        audioSource2.Play();
    }
    
    public void playDeathSound() {
        audioSource2.clip = deathSound;
        audioSource2.Play();
    }
    
    public void playButtonSound() {
        audioSource2.clip = buttonSound;
        audioSource2.Play();
    }
    
    public void playThemeSong() {
        audioSource1.loop = false;
        audioSource1.clip = themeSong;
        audioSource1.Play();
    }
    
    public void stopThemeSong() {
        audioSource1.Stop();
    }

    void Start () {
        gameStatesManager = GameObject.Find("ScriptBucket").GetComponent<GameStatesManager>();
        gameStatesManager.MenuGameState.AddListener(OnMenu);
        gameStatesManager.StartingGameState.AddListener(OnStarting);
        gameStatesManager.PlayingGameState.AddListener(OnPlaying);
        gameStatesManager.PausedGameState.AddListener(OnPausing);
        gameStatesManager.EndingGameState.AddListener(OnEnding);
        SetState(gameStatesManager.gameState);
        
        punchSounds = loadPunchSounds();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    AudioClip[] loadPunchSounds() {

        AudioClip[] clips = new AudioClip[12];
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

    void Awake() {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);
    }


    //Used to play single sound clips.
    public void PlaySingle(AudioClip clip) {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        audioSource2.clip = clip;

        //Play the clip.
        audioSource2.Play();
    }


    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    void RandomizeSfx(params AudioClip[] clips) {
        //Generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //Choose a random pitch to play back our clip at between our high and low pitch ranges.
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        //Set the pitch of the audio source to the randomly chosen pitch.
        audioSource2.pitch = randomPitch;

        //Set the clip to the clip at our randomly chosen index.
        audioSource2.clip = clips[randomIndex];

        //Play the clip.
        audioSource2.Play();
    }
    
    //Listener functions a defined for every GameState
	protected void OnMenu() {
		SetState (StaticData.AvailableGameStates.Menu);
	}

	protected void OnStarting() {
		SetState (StaticData.AvailableGameStates.Starting);

	}

	protected void OnPlaying() {
		SetState (StaticData.AvailableGameStates.Playing);
        playThemeSong();
	}

	protected void OnPausing() {
		SetState (StaticData.AvailableGameStates.Paused);
	}
    
    protected void OnEnding() {
		SetState (StaticData.AvailableGameStates.Ending);
        stopThemeSong();
	}

	private void SetState(StaticData.AvailableGameStates state) {
		gameState = state;
	}

	//Use this function to request a game state change from the GameStateManager
	private void RequestGameStateChange(StaticData.AvailableGameStates state) {
		gameStatesManager.ChangeGameState (state);
	}
}