using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VarjoExample;

public class TeleportRay : MonoBehaviour
{
    public LayerMask ControllerRayLayerMask;      // Collision layers
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform startPosition;
    [SerializeField]
    public Transform xrRig;

    [SerializeField] DataManager dataManager;

    [Header("Display Controls")]
    [Tooltip("Sets the timestep to calculate the projectile line, smaller the timestep, smoother the curve (more points)")]
    [SerializeField]
    [Range(0.001f, 0.005f)]
    private float timeBtwPoints = 0.001f;

    [Tooltip("Sets the initial speed to calculate the projectile line")]
    [SerializeField]
    [Range(5f, 15f)]
    public float initialSpeed = 6f;

    [Tooltip("Sets the teleporting distance")]
    [SerializeField]
    [Range(1, 20)]
    public float distance = 5; // the distance on xz-plane

    public float threshold;

    Controller controller;
    TeleportToStartPosition teleportToStartPositionScript;

    Vector3 teleportPos; // position one will be teleported to
    Vector3[] startingLineRendererPoints; // the array of points in the line renderer
    bool buttonDown;
    bool canTeleport;
    int teleportNum;

    string path;
    string fileName;
    string distLabel;
    int taskNum;
    bool headerTyped;

    // Start is called before the first frame update
    void Start()
    {
        headerTyped = false;
        controller = GetComponent<Controller>();
        teleportToStartPositionScript = GetComponent<TeleportToStartPosition>();
        teleportNum = 0;
        path = dataManager.folderPath;
        taskNum = dataManager.taskNum;
    }

    // Fixed update is used for physics
    void FixedUpdate()
    {
        // allow teleporting only when the experiment has started
        if (teleportToStartPositionScript.gameHasStarted())
        {
            if (!headerTyped)
            {
                writeHeader();
            }
            else
            {
                if (controller.triggerButton)
                {
                    buttonDown = true;
                    Vector3 startDirection = startPosition.forward;
                    Vector3 xzDirection = Vector3.ProjectOnPlane(startDirection, new Vector3(0, 1, 0)); // projection vector on xz plane
                    Vector3 v0_xz = initialSpeed * xzDirection; // initial velocity in xz direction
                    // only draw projectile when controller's angle is not too vertical
                    // (to avoid too many points on lineRenderer)
                    Debug.Log("xz mag: " + v0_xz.magnitude);
                    if (v0_xz.magnitude > threshold)
                    {
                        teleportPos = DrawProjectile(v0_xz);
                    } else
                    {
                        canTeleport = false;
                    }                  
                }
                // if button released and we can teleport (not hitting buildings)
                else if (!controller.triggerButton && buttonDown && canTeleport)
                {
                    lineRenderer.enabled = false;
                    xrRig.position = teleportPos; // we teleport
                    ++teleportNum;
                    RecordData.SaveData(path, fileName,
                            teleportNum + ","
                            + DateTime.Now.ToString() + ","
                            + teleportPos.x + ","
                            + teleportPos.y + ","
                            + teleportPos.z + '\n');
                    buttonDown = false; // reset
                }
            }
        }
    }

    // this function will be called only once right after exploratin started
    private void writeHeader()
    {
        if (distance == 5)
        {
            distLabel = "Short";
            threshold = 2.5f;
        }
        else if (distance == 15)
        {
            distLabel = "Long";
            threshold = 7f;
        }
        fileName = "Teleport" + distLabel + "_Trial_" + taskNum.ToString();
        RecordData.SaveData(path, fileName,
                        "Teleport Count" + ","
                        + "Teleport Time" + ","
                        + "TeleportPosition_x" + ","
                        + "TeleportPosition_y" + ","
                        + "TeleportPosition_z" + '\n');
        headerTyped = true;
    }

    private bool CheckCanTeleport()
    {
        canTeleport = false;
        if (lineRenderer)
        {
            for (int i = 0; i < startingLineRendererPoints.Length - 1; i++)
            {
                // true if there's collider intersecting the line segment
                if (Physics.Linecast(startingLineRendererPoints[i], startingLineRendererPoints[i + 1], ControllerRayLayerMask))
                {
                    return false; // hit buildings, can't teleport
                }
            }
             canTeleport = true; // went through the loop, safe to teleport
        }
        return canTeleport;
    }

    // return the teleport position
    // param is the intial velocity in xz direction
    private Vector3 DrawProjectile(Vector3 v0_xz)
    {
        lineRenderer.enabled = true;
        Vector3 startPos = startPosition.position;
        float totalTime = distance / v0_xz.magnitude; // total time it takes for projectile to have such a distance on xz plane
        Vector3 v0_y = new Vector3(0, (9.8f / 2f * totalTime * totalTime - startPos.y) / totalTime, 0); // vertical velocity
        Vector3 v0 = v0_xz + v0_y;
        //Debug.DrawRay(startPos, v0, Color.green);
        int numPoints = Mathf.CeilToInt(totalTime / timeBtwPoints) + 1;
        lineRenderer.positionCount = numPoints;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPosition(0, startPos); // start point
        int i = 0;
        Debug.Log("cur numPoints: " + numPoints);

        for (float time = 0; time < totalTime; time += timeBtwPoints)
        {
           ++i;
           Vector3 nextPt = startPos + time * v0;
           nextPt.y = startPos.y + v0.y * time - 9.8f / 2f * time * time;
           lineRenderer.SetPosition(i, nextPt);
        }    

        // store points of the line into array
        startingLineRendererPoints = new Vector3[numPoints];
        lineRenderer.GetPositions(startingLineRendererPoints);
        
        // red if invalid teleportation
        if (CheckCanTeleport())
        {
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
        else
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }

        Vector3 dist = lineRenderer.GetPosition(i) - new Vector3(startPos.x, 0, startPos.z);
        dist.y = 0; // to counter the floating point error
        Vector3 temp = xrRig.position + dist;
        Debug.Log("@ i= " + i + ": " + lineRenderer.GetPosition(i).ToString("f3")
            + "\t#points: " + numPoints
            + "\nteleportPos: " + temp.ToString("f3")
            + "\tdistance: " + dist.magnitude);
        return xrRig.position + dist;
    }

    public int getTeleportCount()
    {
        return teleportNum;
    }
}
