using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    private Image foreground;
    public Character character;

    // Use this for initialization
    void Start () {
        foreground = this.gameObject.GetComponent<Image>();
	}

    // Update is called once per frame
    void Update() {
        foreground.fillAmount = character.healthPoint/100.0f;
    }
}
