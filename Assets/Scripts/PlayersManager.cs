﻿//This script checks for player trying to enter or quit the game.
//Pressing the A button get's a player to join the game.
//Pressing the B button get's a player to quit the game.
//A simple UI let the users know who has joined the game already.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayersManager : MonoBehaviour {

    private GameStatesManager gameStatesManager;	//Refers to the GameStateManager
	private StaticData.AvailableGameStates gameState;	//Mimics the GameStateManager's gameState variable at all time
    public float spawnDistance;
    public GameObject characterPrefab;
	public GameObject panelPlayerJoinedPrefab;
	public GameObject panelPlayerList;
	public GameObject panelJoinGameInvite;
	public GameObject healthBarPrefab;
	public GameObject gamePanel;
	public int maxNumPlayers;	//Maximum is 11
	public List<Player> listOfPlayers {get; private set;}
	private List<Controller> listOfAvailableContollers;
	private List<int> listOfAvailableIds;
	private SpriteManager srm;
    private GameWatcher gameWatcher;
    
	void Start () {
        gameStatesManager = GameObject.Find ("ScriptBucket").GetComponent<GameStatesManager>();
		gameStatesManager.MenuGameState.AddListener(OnMenu);
		gameStatesManager.StartingGameState.AddListener(OnStarting);
		gameStatesManager.PlayingGameState.AddListener(OnPlaying);
		gameStatesManager.PausedGameState.AddListener(OnPausing);
        gameStatesManager.EndingGameState.AddListener(OnEnding);
		SetState (gameStatesManager.gameState);
        
		listOfAvailableIds = new List<int> ();
		for (int id = 1, max = Mathf.Min(maxNumPlayers, 11); id <= max; id++) {
			listOfAvailableIds.Add (id);
		}
        gameWatcher = GameObject.Find("ScriptBucket").GetComponent<GameWatcher>();
		listOfPlayers = new List<Player> ();
		listOfAvailableContollers = new List<Controller>() {
			new Controller("C1"),	//Controller 1
			new Controller("C2"),	//Controller 2
			new Controller("C3"),	//Controller 3
			new Controller("C4"),	//Controller 4
			new Controller("C5"),	//Controller 5
			new Controller("C6"),	//Controller 6
			new Controller("C7"),	//Controller 7
			new Controller("C8"),	//Controller 8
			new Controller("C9"),	//Controller 9
			new Controller("C10"),	//Controller 10
			new Controller("C11")	//Controller 11
		};
		srm = GameObject.Find("ScriptBucket").GetComponent<SpriteManager>();
	}

	void Update () {
        if (gameState == StaticData.AvailableGameStates.Menu) {
            if(MenuManager.currentMenu == MenuManager.menuPanels.JOIN) {
                foreach (Controller controller in listOfAvailableContollers) {
                    if (Input.GetButtonDown(controller.buttonA)) {
                        AddPlayer(controller);
                        break;
                    }
                }
                foreach (Player player in listOfPlayers) {
                    if (Input.GetButtonDown(player.controller.buttonB)) {
                        RemovePlayer(player);
                        break;
                    }
                }
            }
        }
	}

	//Adds a player to the game
	private void AddPlayer(Controller controller) {
		if (listOfAvailableIds.Count > 0) {
			GameObject panelPlayerJoined = GameObject.Instantiate(panelPlayerJoinedPrefab, panelPlayerList.transform);
			panelPlayerJoined.GetComponent<RectTransform> ().localScale = Vector3.one;
			panelPlayerJoined.transform.Find ("Text").GetComponent<Text>().text = "Player " + listOfAvailableIds[0].ToString() + " joined the game!";
            Player newPlayer = new Player (listOfAvailableIds[0], controller, panelPlayerJoined);
			listOfPlayers.Add (newPlayer);
			listOfAvailableIds.RemoveAt(0);
			listOfAvailableContollers.Remove (controller);
			if (listOfAvailableIds.Count == 0) {
				panelJoinGameInvite.SetActive(false);
			}
			Canvas.ForceUpdateCanvases ();
            GameObject characterGO = Instantiate(characterPrefab, new Vector2(spawnDistance * newPlayer.id, 0.0f), Quaternion.identity);
            characterGO.GetComponent<SpriteRenderer>().sprite = srm.getRandomCharacter();
            GameObject healthBarGO = Instantiate(healthBarPrefab);
            gamePanel.SetActive(true);
            healthBarGO.transform.SetParent(gamePanel.transform);
            Healthbar hb = healthBarGO.transform.Find("Foreground").GetComponent<Healthbar>();
            hb.character = characterGO.GetComponent<Character>();
            gamePanel.SetActive(false);
            newPlayer.healthBarGO = healthBarGO;
            newPlayer.characterGO = characterGO;
            gameWatcher.addPlayer(characterGO);
            try {
                characterGO.GetComponent<Character>().id = Int32.Parse(newPlayer.controller.name.Substring(1));
            } catch (FormatException e) {
                Console.WriteLine(e.Message);
            }
		}
	}

	private void reset() {
        while (listOfPlayers.Count > 0) {
			RemovePlayer(listOfPlayers[0]);
		}
	}

	//Removes a player from the game
	private void RemovePlayer(Player player) {
		listOfAvailableContollers.Add (player.controller);
		listOfAvailableIds.Add (player.id);
		listOfAvailableIds.Sort ();
		Destroy (player.panelPlayerJoined);
        Destroy (player.characterGO);
        Destroy (player.healthBarGO);
		listOfPlayers.Remove (player);
		panelJoinGameInvite.SetActive(true);
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
        reset();
	}

	private void SetState(StaticData.AvailableGameStates state) {
		gameState = state;
	}

	//Use this function to request a game state change from the GameStateManager
	private void RequestGameStateChange(StaticData.AvailableGameStates state) {
		gameStatesManager.ChangeGameState (state);
	}
}
