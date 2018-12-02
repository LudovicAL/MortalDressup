using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetTrigger : MonoBehaviour {

    Character character;

	// Use this for initialization
	void Start () {
		character = this.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	}
    
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Floor") {
            Debug.Log("GameObject1 collided with " + col.name);
            character.setIsJumping(false);
        }
    }
}
