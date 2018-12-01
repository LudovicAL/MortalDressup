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
	public float xSpeed;
	public float maxSpeed;
	public bool isJumping;
	public Rigidbody2D rigidBody2D;

	Character(Controller controller) {
	}

	// Use this for initialization
	void Start () {
		this.healthPoint = 100;
		this.controller = new Controller("C1");
		this.movementForceHorizontal = 15f;
		this.movementForceVertical = 1.5f;
		this.isJumping = false;
		this.maxSpeed = 50f;
		rigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		// Horizontal movement
		xSpeed = Input.GetAxis(controller.lHorizontal) * movementForceHorizontal;

		// Vertical movement # jump
		if (Input.GetButton(controller.buttonA) && !isJumping) {
			Debug.Log(rigidBody2D.velocity);
			rigidBody2D.AddForce(transform.up * movementForceVertical, ForceMode2D.Impulse);
		}

		Vector2 tempVect = new Vector2(xSpeed, 0);
		if (rigidBody2D.velocity.magnitude > maxSpeed) {
			rigidBody2D.velocity = rigidBody2D.velocity.normalized * maxSpeed;
		} else {
			rigidBody2D.AddForce(tempVect * Time.fixedDeltaTime, ForceMode2D.Impulse);	
		}
	}
}
