using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoCharacter : MonoBehaviour {

    Collider2D headCollider;
    Collider2D leftCollider;
    Collider2D rightCollider;
    Collider2D legCollider;
    
    Collider2D topBox;
    Collider2D leftBox;
    Collider2D rightBox;
    Collider2D bottomBox;

    bool isJumping;
    
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
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
