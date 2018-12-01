using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    private Image foreground;
    private bool updateHealth;
    public float waitTime = 30.0f;
    public int playerHealth;

    // Use this for initialization
    void Start () {
        foreground = this.gameObject.GetComponent<Image>();
        updateHealth = true;
        playerHealth = 100;
	}

    // Update is called once per frame
    void Update() {

        if (updateHealth == true)
        {
            //Reduce fill amount over 30 seconds
            //foreground.fillAmount -= 1.0f / waitTime * Time.deltaTime;

            foreground.fillAmount = 1.0f * playerHealth/100;
        }

    }
}
