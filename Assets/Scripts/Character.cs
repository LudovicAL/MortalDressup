using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private GameStatesManager gameStatesManager;	//Refers to the GameStateManager
	private StaticData.AvailableGameStates gameState;	//Mimics the GameStateManager's gameState variable at all time
    private GameWatcher gameWatcher;
    
	// Variable du personnage
	public int healthPoint;
	public int id;

	// Variable de mouvement
	public Controller controller;
	public float movementForceHorizontal;
	public float movementForceVertical;
	public float maxSpeed;

	private Rigidbody2D rigidBody2D;
    private bool isJumping;
    private float xSpeed;
    private bool alive;
        
    Collider2D topBox;
    Collider2D leftBox;
    Collider2D rightBox;
    Collider2D bottomBox;

    SoundManager sm;
    SpriteManager srm;
    float jumpTimer;
    
	Character(Controller controller) {        
	}

	// Use this for initialization
	void Start () {
        gameStatesManager = GameObject.Find ("ScriptBucket").GetComponent<GameStatesManager>();
		gameStatesManager.MenuGameState.AddListener(OnMenu);
		gameStatesManager.StartingGameState.AddListener(OnStarting);
		gameStatesManager.PlayingGameState.AddListener(OnPlaying);
		gameStatesManager.PausedGameState.AddListener(OnPausing);
        gameStatesManager.PausedGameState.AddListener(OnEnding);
		SetState (gameStatesManager.gameState);
                
        topBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Top box").gameObject.GetComponent<Collider2D>();
        leftBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Left box").gameObject.GetComponent<Collider2D>();
        rightBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Right box").gameObject.GetComponent<Collider2D>();
        bottomBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Bottom box").gameObject.GetComponent<Collider2D>();
        
		controller = new Controller("C" + id);
		isJumping = false;
		rigidBody2D = this.GetComponent<Rigidbody2D>();
		sm = GameObject.Find("ScriptBucket").GetComponent<SoundManager>();
		srm = GameObject.Find("ScriptBucket").GetComponent<SpriteManager>();
        
        gameWatcher = GameObject.Find("ScriptBucket").GetComponent<GameWatcher>();
        
        alive = true;
        
        jumpTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameState == StaticData.AvailableGameStates.Playing && alive) {
            if (isJumping) {
                if (jumpTimer > 0.0f) {
                    jumpTimer -= 1.0f * Time.deltaTime;
                }
                if (jumpTimer <= 0.0f) {
                    setIsJumping(false);
                }
            }
            // Horizontal movement
            xSpeed = Input.GetAxis(controller.lHorizontal) * movementForceHorizontal;
            // Vertical movement # jump
            if (Input.GetButtonDown(controller.buttonB) && !isJumping) {
                setIsJumping(true);
                sm.playJumpSound();
                jumpTimer = 3.0f;
                rigidBody2D.AddForce(transform.up * movementForceVertical, ForceMode2D.Impulse);
            }

            Vector2 tempVect = new Vector2(xSpeed, 0);
            if (rigidBody2D.velocity.magnitude > maxSpeed) {
                rigidBody2D.velocity = rigidBody2D.velocity.normalized * maxSpeed;
            } else {
                rigidBody2D.AddForce(tempVect * Time.deltaTime, ForceMode2D.Impulse);	
            }

            // Attaque 'A'
            if (Input.GetButtonDown(controller.buttonA)) {
                float xAxis = Input.GetAxis(controller.lHorizontal);
                float yAxis = Input.GetAxis(controller.lVertical);
                Collider2D[] collisions = new Collider2D[20];
                string direction;
                if (Mathf.Abs(xAxis) >= Mathf.Abs(yAxis)) {
                    if (xAxis >= 0) {
                        rightBox.OverlapCollider(new ContactFilter2D(), collisions);
                        direction = "right";
                    } else {
                        leftBox.OverlapCollider(new ContactFilter2D(), collisions);
                        direction = "left";
                    }
                } else {
                    if (yAxis >= 0) {
                        topBox.OverlapCollider(new ContactFilter2D(), collisions);
                        direction = "top";
                    } else {
                        bottomBox.OverlapCollider(new ContactFilter2D(), collisions);
                        direction = "bottom";
                    }	
                }			
                if (collisions[0] != null) {
                    handleCollisions(collisions, direction);
                }
            }
        }
	}

	private void handleCollisions(Collider2D[] collisions, string direction) {
		List<int> collided = new List<int>();
		foreach (Collider2D item in collisions) {
            if (item == null) {
				break;
			}
			if (item.tag != "CharCollider") {
				continue;
			}
			GameObject characterObject = item.transform.parent.gameObject.transform.parent.gameObject;
			Character character = characterObject.GetComponent<Character>();
			if (this.id != character.id && !collided.Contains(character.id)) {
				collided.Add(character.id);
				character.GetComponent<Character>().receiveDamage(10, direction);
				sm.playRandomPunchSound();
				srm.drawRandomFightingSprite(this.transform.position.x, this.transform.position.y);
			}
		}
	}
    
    public void setIsJumping(bool val) {
        isJumping = val;
    }

    public void receiveDamage(int damage, string direction) {
        if (alive) {
            this.healthPoint -= damage;
            if (this.healthPoint <= 0) {
            	rigidBody2D.constraints = RigidbodyConstraints2D.None;
                if (direction == "right") {
                    rigidBody2D.AddTorque(20, ForceMode2D.Impulse);
                } else {
                    rigidBody2D.AddTorque(-20, ForceMode2D.Impulse);
                }
                alive = false;
                gameWatcher.killPlayer(this.gameObject);
                sm.playDeathSound();
            }
        }
    	// Knock in direction
    	int xDir = direction == "right" ? 1 : -1;
        rigidBody2D.AddForce(transform.right * 5 * xDir, ForceMode2D.Impulse);
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
