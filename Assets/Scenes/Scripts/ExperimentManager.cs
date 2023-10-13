using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InfoType
{
    WithoutLandmark,
    WithLandmark
}
public enum RouteType
{
    Route1,
    Route2
}
public enum CueType
{
    ScreenFixed,
    WorldFixed
}
public enum TeleportDistance
{
    Short,
    Long
}

public class ExperimentManager : MonoBehaviour
{
    // public static ExperimentManager Instance; //singleton https://learn.unity.com/tutorial/implement-data-persistence-between-scenes#
    public RouteType routeType;
    public TeleportDistance teleportDistance;
    public CueType cueType;
    public InfoType infoType;

    [SerializeField] GameObject Instruction_Route1;
    [SerializeField] GameObject Instruction_Route2;

    [SerializeField] GameObject BodyFixedCue_Route1;
    [SerializeField] GameObject BodyFixedCue_Route2;

    [SerializeField] GameObject WorldARCues_Direction_Route1;
    [SerializeField] GameObject WorldARCues_Direction_Route2;

    [SerializeField] GameObject ScreenARCues_Direction_Route1;
    [SerializeField] GameObject ScreenARCues_Direction_Route2;

    [SerializeField] GameObject WorldARCues_Landmark_Route1;
    [SerializeField] GameObject WorldARCues_Landmark_Route2;

    [SerializeField] GameObject ScreenARCues_Landmark_Route1;
    [SerializeField] GameObject ScreenARCues_Landmark_Route2;

    /// <summary>
    /// This field sets the go-straignt cue for WorldFixed condition
    /// </summary>
    [SerializeField] GameObject WorldARCues_Straight_Route1;
    [SerializeField] GameObject WorldARCues_Straight_Route2;

    [SerializeField] TeleportRay teleportRayScript;


    void Awake()
    {
        Instruction_Route1.SetActive(false);
        Instruction_Route2.SetActive(false);

        BodyFixedCue_Route1.SetActive(false);
        BodyFixedCue_Route2.SetActive(false);

        WorldARCues_Direction_Route1.SetActive(false);
        WorldARCues_Direction_Route2.SetActive(false);
        ScreenARCues_Direction_Route1.SetActive(false);
        ScreenARCues_Direction_Route2.SetActive(false);
        WorldARCues_Landmark_Route1.SetActive(false);
        WorldARCues_Landmark_Route2.SetActive(false);
        ScreenARCues_Landmark_Route1.SetActive(false);
        ScreenARCues_Landmark_Route2.SetActive(false);

        WorldARCues_Straight_Route1.SetActive(false);
        WorldARCues_Straight_Route2.SetActive(false);
    }
    
    void Start()
    {
        if (teleportDistance == TeleportDistance.Short)
        {
            teleportRayScript.distance = 5;
            teleportRayScript.initialSpeed = 6;
        }
        else if (teleportDistance == TeleportDistance.Long)
        {
            teleportRayScript.distance = 15;
            teleportRayScript.initialSpeed = 13;
        }

            //If Route1, teleport to Route1 (after reading the instructions)
            // Active cues on Route1 
            if (routeType == RouteType.Route1)
        {
            Instruction_Route1.SetActive(true);
            if (cueType == CueType.ScreenFixed)
            {
                ScreenARCues_Direction_Route1.SetActive(true);
                //BodyFixedCue_Route1.SetActive(true);
                if (infoType == InfoType.WithLandmark)
                {
                    ScreenARCues_Landmark_Route1.SetActive(true);
                }
            }
            else if (cueType == CueType.WorldFixed)
            {
                WorldARCues_Direction_Route1.SetActive(true);
                WorldARCues_Straight_Route1.SetActive(true);
                if (infoType == InfoType.WithLandmark)
                {
                    WorldARCues_Landmark_Route1.SetActive(true);
                }
            }
        }

        //else if Route2, teleport to Route2
        // Active cues on Route1 
        else if (routeType == RouteType.Route2)
        {
            Instruction_Route2.SetActive(true);
            if (cueType == CueType.ScreenFixed)
            {
                ScreenARCues_Direction_Route2.SetActive(true);
                // BodyFixedCue_Route2.SetActive(true);
                if (infoType == InfoType.WithLandmark)
                {
                    ScreenARCues_Landmark_Route2.SetActive(true);
                }
            }
            else if (cueType == CueType.WorldFixed)
            {
                WorldARCues_Direction_Route2.SetActive(true);
                WorldARCues_Straight_Route2.SetActive(true);
                if (infoType == InfoType.WithLandmark)
                {
                    WorldARCues_Landmark_Route2.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //call in class - TeleportToStartPosition
    public void ActiveBodyFixedCue(bool hasStarted)
    {
        //if (cueType == CueType.ScreenFixed)
        //{
            if (routeType == RouteType.Route1)
            {
                BodyFixedCue_Route1.SetActive(true);
            }
            else if (routeType == RouteType.Route2)
            {
                BodyFixedCue_Route2.SetActive(true);
            }
        //}
    }
}
