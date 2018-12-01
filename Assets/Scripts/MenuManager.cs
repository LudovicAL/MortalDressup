using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    readonly string strMenu = "MenuPanel";
    readonly string strJoin = "Panel PlayerList";
    readonly string strStart = "StartPanel";
    readonly string strChoosePlayer = "ChoosePlayerPanel";
    readonly string strDressup = "DressUpPanel";
    readonly string strScriptBucket = "ScriptBucket";

    bool isFirstMenu;

    private GameStatesManager gameStatesManager;
    private StaticData.AvailableGameStates gameState;

    enum menuPanels {
        JOIN,
        START,
        CHOOSE,
        DRESSUP
    }

    GameObject rootGo;
    GameObject menuPanel;
    GameObject joinPanel;
    GameObject startPanel;
    GameObject choosePlayerPanel;
    GameObject dressupPanel;

    menuPanels currentMenu;

    // Use this for initialization
    void Start() {
        gameStatesManager = GameObject.Find(strScriptBucket).GetComponent<GameStatesManager>();
        gameStatesManager.MenuGameState.AddListener(OnMenu);
        gameStatesManager.StartingGameState.AddListener(OnStarting);
        gameStatesManager.PlayingGameState.AddListener(OnPlaying);
        gameStatesManager.PausedGameState.AddListener(OnPausing);
        SetState(gameStatesManager.gameState);

        rootGo = this.gameObject;
        menuPanel = getMenuObject(rootGo, strMenu);
        joinPanel = getMenuObject(menuPanel, strJoin);
        startPanel = getMenuObject(menuPanel, strStart);
        choosePlayerPanel = getMenuObject(menuPanel, strChoosePlayer);
        dressupPanel = getMenuObject(menuPanel, strDressup);
    }

    public void onFirstMenu() {
        isFirstMenu = false;
        menuPanel.SetActive(true);
        showMenuPanel(menuPanels.JOIN);
    }

    public void onEndGame() {
        menuPanel.SetActive(true);
        showMenuPanel(menuPanels.CHOOSE);
    }

    public void nextMenu() {
        switch (currentMenu) {
            case menuPanels.JOIN:
                showMenuPanel(menuPanels.START);
                break;
            case menuPanels.START:
                showMenuPanel(menuPanels.CHOOSE);
                break;
            case menuPanels.CHOOSE:
                showMenuPanel(menuPanels.DRESSUP);
                break;
            case menuPanels.DRESSUP:
                Close();
                break;
        }
    }

    public void previousMenu() {
        switch (currentMenu) {
            case menuPanels.JOIN:
                showMenuPanel(menuPanels.JOIN);
                break;
            case menuPanels.START:
                showMenuPanel(menuPanels.JOIN);
                break;
            case menuPanels.CHOOSE:
                showMenuPanel(menuPanels.START);
                break;
            case menuPanels.DRESSUP:
                showMenuPanel(menuPanels.CHOOSE);
                break;
        }

    }

    void showMenuPanel(menuPanels panel) {
        switch (panel) {
            case menuPanels.JOIN:
                joinPanel.SetActive(true);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                break;
            case menuPanels.START:
                joinPanel.SetActive(false);
                startPanel.SetActive(true);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                break;
            case menuPanels.CHOOSE:
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(true);
                dressupPanel.SetActive(false);
                break;
            case menuPanels.DRESSUP:
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(true);
                break;
        }
    }

    GameObject getMenuObject(GameObject go, string strToFind) {
        return go.transform.Find(strToFind).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if (gameState == StaticData.AvailableGameStates.Menu) {
            if (button_A_WasPressed()) {
                nextMenu();
            }
            if (button_B_WasPressed()) {
                previousMenu();
            }
        }
            
    }

    //Listener functions a defined for every GameState
    protected void OnMenu() {
        SetState(StaticData.AvailableGameStates.Menu);
        if(isFirstMenu) {
            onFirstMenu();
        } else {
            onEndGame();
        }
    }

    protected void OnStarting() {
        SetState(StaticData.AvailableGameStates.Starting);
    }

    protected void OnPlaying() {
        SetState(StaticData.AvailableGameStates.Playing);
    }

    protected void OnPausing() {
        SetState(StaticData.AvailableGameStates.Paused);
    }

    private void SetState(StaticData.AvailableGameStates state) {
        gameState = state;
    }

    //Use this function to request a game state change from the GameStateManager
    private void RequestGameStateChange(StaticData.AvailableGameStates state) {
        gameStatesManager.ChangeGameState(state);
    }

    int nbOfControllerMax = 4;
    string[] buttonsA = { "C1buttonA", "C2buttonA", "C3buttonA", "C4buttonA" };
    string[] buttonsB = { "C1buttonB", "C2buttonB", "C3buttonB", "C4buttonB" };


    bool button_A_WasPressed() {
        return checkIfButtonWasPressed(buttonsA);
    }

    bool button_B_WasPressed() {
        return checkIfButtonWasPressed(buttonsB);
    }

    bool checkIfButtonWasPressed(string[] buttons) {
        bool buttonWasPressed = false;
        for (int i = 0; i < nbOfControllerMax; i++) {
            if (Input.GetButton(buttons[i]) && !buttonWasPressed) {
                buttonWasPressed = true;
            }
        }
        return buttonWasPressed;
    }

    void Close() {
        menuPanel.SetActive(false);
    }
}
