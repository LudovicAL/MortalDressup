using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    private Image foreground;
    public GameObject go;
    private Character character;

    // Use this for initialization
    void Start () {
        foreground = this.gameObject.GetComponent<Image>();
        character = go.GetComponent<Character>();
	}

    // Update is called once per frame
    void Update() {
        foreground.fillAmount = character.healthPoint/100.0f;
    }
}
