using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    readonly string strJoin = "Panel PlayerList";
    readonly string strMenuPanel = "MenuPanel";
    readonly string strStart = "StartPanel";
    readonly string strChoosePlayer = "ChoosePlayerPanel";
    readonly string strDressup = "DressUpPanel";
    readonly string strCountDown = "CountDownPanel";
    readonly string strGame = "GamePanel";
    readonly string strEnding = "EndingPanel";
    readonly string strScriptBucket = "ScriptBucket";
    readonly string strCountDownText = "CountDownText";
    readonly string strEndingText = "EndingText";

    private GameStatesManager gameStatesManager;
    private StaticData.AvailableGameStates gameState;
    private GameWatcher gameWatcher;
    
    public enum menuPanels {
        JOIN,
        START,
        CHOOSE,
        DRESSUP,
        COUNTDOWN,
        GAME,
        ENDING
    }

    GameObject menuPanel;
    GameObject joinPanel;
    GameObject startPanel;
    GameObject choosePlayerPanel;
    GameObject dressupPanel;
    GameObject countDownPanel;
    GameObject gamePanel;
    GameObject endingPanel;
    Text countDownText;
    Text endingText;

    public static menuPanels currentMenu;

    // Use this for initialization
    void Start() {
        gameWatcher = GameObject.Find(strScriptBucket).GetComponent<GameWatcher>();
        gameStatesManager = GameObject.Find(strScriptBucket).GetComponent<GameStatesManager>();
        gameStatesManager.MenuGameState.AddListener(OnMenu);
        gameStatesManager.StartingGameState.AddListener(OnStarting);
        gameStatesManager.PlayingGameState.AddListener(OnPlaying);
        gameStatesManager.PausedGameState.AddListener(OnPausing);
        gameStatesManager.EndingGameState.AddListener(OnEnding);
        SetState(gameStatesManager.gameState);

        menuPanel = this.gameObject.transform.Find(strMenuPanel).gameObject;
        joinPanel = getMenuObject(strJoin);
        startPanel = getMenuObject(strStart);
        choosePlayerPanel = getMenuObject(strChoosePlayer);
        dressupPanel = getMenuObject(strDressup);
        countDownPanel = getMenuObject(strCountDown);
        countDownText = countDownPanel.transform.Find(strCountDownText).gameObject.GetComponent<Text>();
        gamePanel = getMenuObject(strGame);
        endingPanel = getMenuObject(strEnding);
        endingText = endingPanel.transform.Find(strEndingText).gameObject.GetComponent<Text>();
        
        showMenuPanel(menuPanels.JOIN);
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
                showMenuPanel(menuPanels.COUNTDOWN);
                break;
            case menuPanels.COUNTDOWN:
                showMenuPanel(menuPanels.GAME);
                break;
            case menuPanels.GAME:
                showMenuPanel(menuPanels.ENDING);
                break;
            case menuPanels.ENDING:
                showMenuPanel(menuPanels.JOIN);
                break;
        }
    }

    public void previousMenu() {
        switch (currentMenu) {
            case menuPanels.JOIN:
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
            case menuPanels.COUNTDOWN:
                //Do nothing here. We can't go backward during COUNTDOWN
                break;
            case menuPanels.GAME:
                //Do nothing here. We can't go backward during GAME
                break;
            case menuPanels.ENDING:
                //Do nothing here. We can't go backward during ENDING
                break;
        }
    }

    void showMenuPanel(menuPanels panel) {

        currentMenu = panel;

        switch (panel) {
            case menuPanels.JOIN:
                joinPanel.SetActive(true);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(false);
                endingPanel.SetActive(false);
                break;
            case menuPanels.START:
                joinPanel.SetActive(false);
                startPanel.SetActive(true);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(false);
                endingPanel.SetActive(false);
                break;
            case menuPanels.CHOOSE:
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(true);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(false);
                endingPanel.SetActive(false);
                break;
            case menuPanels.DRESSUP:
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(true);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(false);
                endingPanel.SetActive(false);
                break;
            case menuPanels.COUNTDOWN:
                RequestGameStateChange(StaticData.AvailableGameStates.Starting);
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(true);
                gamePanel.SetActive(false);
                endingPanel.SetActive(false);
                StartCoroutine(startCountDown());
                break;
            case menuPanels.GAME:
                RequestGameStateChange(StaticData.AvailableGameStates.Playing);
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(true);
                endingPanel.SetActive(false);
                break;
            case menuPanels.ENDING:
                RequestGameStateChange(StaticData.AvailableGameStates.Playing);
                joinPanel.SetActive(false);
                startPanel.SetActive(false);
                choosePlayerPanel.SetActive(false);
                dressupPanel.SetActive(false);
                countDownPanel.SetActive(false);
                gamePanel.SetActive(false);
                endingPanel.SetActive(true);
                break;
        }
    }

    GameObject getMenuObject(string strToFind) {
        return menuPanel.transform.Find(strToFind).gameObject;
    }

    private IEnumerator startCountDown() {
        for (int i = 3; i > 0; i--) {
			countDownText.text = i.ToString();    
			yield return new WaitForSeconds(0.7f);
		}
        nextMenu();
    }
    
    // Update is called once per frame
    void Update() {
        if (gameState == StaticData.AvailableGameStates.Menu) {
            if (buttonNextWasPressed()) {
                nextMenu();
            }
            if (buttonBackWasPressed()) {
                previousMenu();
            }
        }
            
    }

    //Listener functions a defined for every GameState
    protected void OnMenu() {
        SetState(StaticData.AvailableGameStates.Menu);
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
    
    protected void OnEnding() {
        SetState(StaticData.AvailableGameStates.Ending);
        nextMenu();
        if (gameWatcher.listOfAlivePlayers.Count > 0) {
            endingText.text = gameWatcher.listOfAlivePlayers[0].ToString() + " won... obviously";
        } else {
            endingText.text = "No constest bro!";
        }
        StartCoroutine(goToNextGameStateAfterWait());
    }

    private void SetState(StaticData.AvailableGameStates state) {
        gameState = state;
    }
    
    //Use this function to request a game state change from the GameStateManager
    private void RequestGameStateChange(StaticData.AvailableGameStates state) {
        gameStatesManager.ChangeGameState(state);
    }

    private IEnumerator goToNextGameStateAfterWait() {
        yield return new WaitForSeconds(3.0f);
        RequestGameStateChange(StaticData.AvailableGameStates.Menu);
        nextMenu();
    }
    
    int nbOfControllerMax = 4;
    string[] buttonsStart = { "C1buttonStart", "C2buttonStart", "C3buttonStart", "C4buttonStart" };
    string[] buttonsBack = { "C1buttonBack", "C2buttonBack", "C3buttonBack", "C4buttonBack" };
    string[] buttonsA = { "C1buttonA", "C2buttonA", "C3buttonA", "C4buttonA" };
    string[] buttonsB = { "C1buttonB", "C2buttonB", "C3buttonB", "C4buttonB" };


    bool buttonNextWasPressed() {
        return checkIfButtonWasPressed(buttonsStart);
    }

    bool buttonBackWasPressed() {
        return checkIfButtonWasPressed(buttonsBack);
    }

    bool checkIfButtonWasPressed(string[] buttons) {
        bool buttonWasPressed = false;
        for (int i = 0; i < nbOfControllerMax; i++) {
            if (Input.GetButtonDown(buttons[i]) && !buttonWasPressed) {
                buttonWasPressed = true;
            }
        }
        return buttonWasPressed;
    }
}
