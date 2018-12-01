using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	// Variable du personnage
	public int healthPoint;

	// Variable de mouvement
	public Controller controller;
	public float movementForceHorizontal;
	public float movementForceVertical;
	public float maxSpeed;

	private Rigidbody2D rigidBody2D;
    private bool isJumping;
    private float xSpeed;
    
    Collider2D headCollider;
    Collider2D leftCollider;
    Collider2D rightCollider;
    Collider2D legCollider;
    
    Collider2D topBox;
    Collider2D leftBox;
    Collider2D rightBox;
    Collider2D bottomBox;
    
	Character(Controller controller) {        
	}

	// Use this for initialization
	void Start () {
        headCollider = this.transform.Find("Colliders").gameObject.transform.Find("Head collider").gameObject.GetComponent<Collider2D>();
        leftCollider = this.transform.Find("Colliders").gameObject.transform.Find("Left collider").gameObject.GetComponent<Collider2D>();
        rightCollider = this.transform.Find("Colliders").gameObject.transform.Find("Right collider").gameObject.GetComponent<Collider2D>();
        legCollider = this.transform.Find("Colliders").gameObject.transform.Find("Leg collider").gameObject.GetComponent<Collider2D>();
        
        topBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Top box").gameObject.GetComponent<Collider2D>();
        leftBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Left box").gameObject.GetComponent<Collider2D>();
        rightBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Right box").gameObject.GetComponent<Collider2D>();
        bottomBox = this.transform.Find("Hit boxes").gameObject.transform.Find("Bottom box").gameObject.GetComponent<Collider2D>();
        
		controller = new Controller("C2");
		isJumping = false;
		rigidBody2D = this.GetComponent<Rigidbody2D>();
        
	}
	
	// Update is called once per frame
	void Update () {
		// Horizontal movement
		xSpeed = Input.GetAxis(controller.lHorizontal) * movementForceHorizontal;
        // Vertical movement # jump
		if (Input.GetButtonDown(controller.buttonB) && !isJumping) {
            isJumping = true;
			rigidBody2D.AddForce(transform.up * movementForceVertical, ForceMode2D.Impulse);
		}

		Vector2 tempVect = new Vector2(xSpeed, 0);
		if (rigidBody2D.velocity.magnitude > maxSpeed) {
			rigidBody2D.velocity = rigidBody2D.velocity.normalized * maxSpeed;
		} else {
			rigidBody2D.AddForce(tempVect * Time.fixedDeltaTime, ForceMode2D.Impulse);	
		}
	}
    
    public void setIsJumping(bool val) {
        isJumping = val;
    }
}
