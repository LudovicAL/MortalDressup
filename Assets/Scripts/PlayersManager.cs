//This script checks for player trying to enter or quit the game.
//Pressing the A button get's a player to join the game.
//Pressing the B button get's a player to quit the game.
//A simple UI let the users know who has joined the game already.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayersManager : MonoBehaviour {

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
	SpriteManager srm;

	void Start () {
		listOfAvailableIds = new List<int> ();
		for (int id = 1, max = Mathf.Min(maxNumPlayers, 11); id <= max; id++) {
			listOfAvailableIds.Add (id);
		}
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
            GameObject characterGO = Instantiate(characterPrefab);
            characterGO.GetComponent<SpriteRenderer>().sprite = srm.getRandomCharacter();
            GameObject healthBarGO = Instantiate(healthBarPrefab);
            gamePanel.SetActive(true);
            healthBarGO.transform.SetParent(gamePanel.transform);
            Healthbar hb = healthBarGO.transform.Find("Foreground").GetComponent<Healthbar>();
            hb.character = characterGO.GetComponent<Character>();
            gamePanel.SetActive(false);

            newPlayer.healthBarGO = healthBarGO;
            newPlayer.characterGO = characterGO;
            try {
                characterGO.GetComponent<Character>().id = Int32.Parse(newPlayer.controller.name.Substring(1));
            } catch (FormatException e) {
                Console.WriteLine(e.Message);
            }
		}
	}

	private void reset() {
		foreach (Player curPlayer in listOfPlayers) {
			RemovePlayer(curPlayer);
		}
	}

	public void resetGame() {
		reset();
		// reset all character and shizzle
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
}
