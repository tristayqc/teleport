using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using VarjoExample;

public class TeleportToStartPosition : MonoBehaviour
{
    [SerializeField] ExperimentManager experimentManager;
    [SerializeField] Transform startPosition_Route1;
    [SerializeField] Transform startPosition_Route2;
    [SerializeField] GameObject xrRig;
    [SerializeField] Transform mainCamera;
    [SerializeField] Timer timer;
    [SerializeField] AudioSource StartNavigationAudio;
    [SerializeField] ExitExploration exitRoute1;
    [SerializeField] ExitExploration exitRoute2;

    [SerializeField] DataManager dataManager;

    bool hasStarted;
    bool hasEnded;
    bool buttonDown;
    Controller controller;

    string Path;
    string FileName;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller>();
        hasStarted = false;
        buttonDown = false;

        Path = dataManager.folderPath;
        FileName = dataManager.fileName;
        RecordData.SaveData(Path, FileName,
             "Start Time: " + ","+ DateTime.Now.ToString()
                       + '\n');
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
            if(!hasStarted)
            {
                if(experimentManager.routeType == RouteType.Route1)
                    xrRig.transform.position = startPosition_Route1.position;
                else if (experimentManager.routeType == RouteType.Route2)
                    xrRig.transform.position = startPosition_Route2.position;
                hasStarted = true;
                experimentManager.ActiveBodyFixedCue(hasStarted);
                // timer.SetTimerOff();

                StartNavigationAudio.PlayDelayed(2);


                RecordData.SaveData(Path, FileName,
                        "Exploration Start Time: " + "," + DateTime.Now.ToString()
                        + '\n');

                RecordData.SaveData(Path, FileName,
                        "Time" + "," 
                        + "HeadRotationAngles_x" + ","
                        + "HeadRotationAngles_y" + ","
                        + "HeadRotationAngles_z" + ","
                        + '\n');
            }
            buttonDown = false;
        }
        if (!hasEnded && (exitRoute1.exploreHasEnded() || exitRoute2.exploreHasEnded()))
        {
            hasEnded = true;
            RecordData.SaveData(Path, FileName,
                        "Exploration End Time: " + "," + DateTime.Now.ToString()
                        + '\n');
        }
    }

    void FixedUpdate()
    {
        // while we haven't reached destination
        if (hasStarted && !hasEnded)
        {
            RecordData.SaveData(Path, FileName,
                        DateTime.Now.ToString() + ","
                        + mainCamera.eulerAngles.x + ","
                        + mainCamera.eulerAngles.y + ","
                        + mainCamera.eulerAngles.z
                        + '\n');
        }
    }

    public bool gameHasStarted()
    {
        return hasStarted;
    }
}
