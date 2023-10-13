using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarjoExample;

public class TeleportToStartTraining : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject xrRig;
    [SerializeField] GameObject ScreenInstruction; 
    bool hasStarted;
    bool buttonDown;
    Controller controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
        buttonDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.primaryButton)
        {
            if (!buttonDown)
            {
                // Button is pressed
                buttonDown = true;
            }
            else
            {
                // Button is held down
            }
        }
        else if (!controller.primaryButton && buttonDown)
        {
            // Button is released
            Debug.Log("tRIGGER");
            if (!hasStarted)
            {
                xrRig.transform.position = startPosition.position;
                hasStarted = true;
                ScreenInstruction.SetActive(true);
            }
            buttonDown = false;
        }
    }

    public bool trainingHasStarted()
    {
        return hasStarted;
    }
}

