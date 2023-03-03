using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider teleProvider;
    private InputAction _thumbstick;
    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        // only show the line when pushing the stick
        // cancel it by pressing the grip button 
        rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;
        
        _thumbstick = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
        _thumbstick.Enable();
        

    }

    // Update is called once per frame
    void Update()
    {
        // if teleportation ray is not active
        if (!_isActive) return; 
        
        // if thumbstic is still pushed forward, don't teleport yet
        if (_thumbstick.triggered) return; 

        // return a raycast hit variable
        bool raycastHit = rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
        // if we didn't hit anything
        if (!raycastHit){
            rayInteractor.enabled = false;
            _isActive = false;
            return;
        }
        // else it does hit something
        TeleportRequest request = new TeleportRequest(){
            destinationPosition = hit.point, // TODO: change this maybe
            // destinationRotation = ,
            // matchOrientation = ,
            // requestTime = ,
        };

        // check to see if it's valid teleportation request
        // if so, it will teleport us
        teleProvider.QueueTeleportRequest(request);
        rayInteractor.enabled = false; // TODO: Since this is added later, check if needed
    }

    private void OnTeleportActivate(InputAction.CallbackContext context){
        rayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context){
        rayInteractor.enabled = false;
        _isActive = false;
    }
}
