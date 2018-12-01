using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    float remainingTime;
    int a;
    string counter;

    void Start()
    {
        remainingTime = 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        remainingTime -= Time.deltaTime;

        a = (int)remainingTime;
        counter = a.ToString();

        print(counter);

        if (remainingTime < 0)
        {
            print("0");
        }
    }
}
