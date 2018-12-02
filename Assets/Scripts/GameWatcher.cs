using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWatcher : MonoBehaviour {

    private GameStatesManager gameStatesManager;	//Refers to the GameStateManager
	private StaticData.AvailableGameStates gameState;	//Mimics the GameStateManager's gameState variable at all time

    public List<GameObject> listOfAlivePlayers;
    public List<GameObject> listOfDeadPlayers;
    
	// Use this for initialization
	void Start () {
        listOfAlivePlayers = new List<GameObject>();
        listOfDeadPlayers = new List<GameObject>();
        gameStatesManager = GameObject.Find ("ScriptBucket").GetComponent<GameStatesManager>();
		gameStatesManager.MenuGameState.AddListener(OnMenu);
		gameStatesManager.StartingGameState.AddListener(OnStarting);
		gameStatesManager.PlayingGameState.AddListener(OnPlaying);
		gameStatesManager.PausedGameState.AddListener(OnPausing);
        gameStatesManager.EndingGameState.AddListener(OnEnding);
		SetState (gameStatesManager.gameState);
	}
	
	// Update is called once per frame
	void Update () {

	}
    
    public void addPlayer(GameObject newGo) {
        listOfAlivePlayers.Add(newGo);
    }
    
    public void killPlayer(GameObject deadGO) {
        listOfAlivePlayers.Remove(deadGO);
        listOfDeadPlayers.Add(deadGO);
        checkForGameEnd();
	}

    void checkForGameEnd() {
        Debug.Log("Check for game end: " + listOfAlivePlayers.Count); 
        if (listOfAlivePlayers.Count < 2){
            Debug.Log("Game ended");
            StartCoroutine(goToNextGameState());
        }
    }
    
    private IEnumerator goToNextGameState() {
        yield return new WaitForSeconds(1.5f);
        RequestGameStateChange(StaticData.AvailableGameStates.Ending);
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

	}

	protected void OnPausing() {
		SetState (StaticData.AvailableGameStates.Paused);
	}
    
    protected void OnEnding() {
		SetState (StaticData.AvailableGameStates.Ending);
	}

	private void SetState(StaticData.AvailableGameStates state) {
		gameState = state;
	}

	//Use this function to request a game state change from the GameStateManager
	private void RequestGameStateChange(StaticData.AvailableGameStates state) {
		gameStatesManager.ChangeGameState (state);
	}
}
