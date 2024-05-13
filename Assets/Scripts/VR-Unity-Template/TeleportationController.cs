using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationController : MonoBehaviour
{
    //Used to determine current active state of the Teleportation System
    static private bool _teleportIsActive = false;

    //Creates an enum that will determine if we're using the right or left controller
    public enum ControllerType
    {
        RightHand,
        LeftHand
    }

    //Stores the target controller from the editor
    public ControllerType targetController;

    //References our Input Actions that we are using
    public InputActionAsset inputAction;

    //References the rayInteractor to be enabled/disabled later
    public XRRayInteractor rayInteractor;

    //References the Teleportation Provider so we can use it to teleport the Player in the event of a succesful teleport call
    public TeleportationProvider teleportationProvider;


    //Will reference the Thumbstick Input Action when the scene starts up
    private InputAction _thumbstickInputAction;

    //Stores Action for Teleport Mode Activate
    private InputAction _teleportActivate;

    //Stores Action for Teleport Mode Cancel
    private InputAction _teleportCancel;

    // Teleport layers
    [SerializeField] private LayerMask _teleportLayers;

    // Obstacle layers
    [SerializeField] private LayerMask _obstacleLayers;

    void Start()
    {
        //We don't want the rayInteractor to on unless we're using the forward press on the thumbstick so we deactivate it here
        rayInteractor.enabled = false;

        //This will find the Action Map of our target controller for Teleport Mode Activate.
        //It will enable it and then subscribe itself to our OnTeleportActivate function
        _teleportActivate = inputAction.FindActionMap($"XRI {targetController} Locomotion").FindAction("Teleport Mode Activate");
        _teleportActivate.Enable();
        _teleportActivate.performed += OnTeleportActivate;

        //This will find the Action Map of our target controller for Teleport Mode Cancel.
        //It will enable it and then subscribe itself to our OnTeleportCancel function
        _teleportCancel = inputAction.FindActionMap($"XRI {targetController} Locomotion").FindAction("Teleport Mode Cancel");
        _teleportCancel.Enable();
        _teleportCancel.performed += OnTeleportCancel;


        //We grab this reference so we can use it to tell if the thumbstick is still being pressed
        _thumbstickInputAction = inputAction.FindActionMap($"XRI {targetController} Locomotion").FindAction("Move");
        _thumbstickInputAction.Enable();
    }

    private void OnDestroy()
    {
        _teleportActivate.performed -= OnTeleportActivate;
        _teleportCancel.performed -= OnTeleportCancel;
    }
    //We use the Update function to check for when a teleportation event has occured. 
    //The checks needed to ensure a succesful teleport event are
    //-Teleporting is currently active
    //-The ray currently the active one
    //-The Thumbstick isn't being pressed
    //-The rayInteractor is hitting a valid target
    //If those pass, we make a teleportation request to the Teleport Provider
    void Update()
    {
        if (!_teleportIsActive)
        {
            return;
        }
        if (!rayInteractor.enabled)
        {
            return;
        }
        if (_thumbstickInputAction.IsPressed())
        {
            return;
        }

        // Perform raycast
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit raycastHit))
        {
            // Check if the ray hits an object with a "Teleport" layer
            if ((_teleportLayers.value & (1 << raycastHit.collider.gameObject.layer)) != 0)
            {
                TeleportRequest teleportRequest = new TeleportRequest()
                {
                    destinationPosition = raycastHit.point,
                };

                teleportationProvider.QueueTeleportRequest(teleportRequest);

                rayInteractor.enabled = false;
                _teleportIsActive = false;
            }
            // Check if the ray hits an object with a "Obstacle" layer
            else if ((_obstacleLayers.value & (1 << raycastHit.collider.gameObject.layer)) != 0)
            {
                rayInteractor.enabled = false;
                _teleportIsActive = false;
            }
        }
    }

    //This is called when our Teleport Mode Activated action map is triggered
    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        if (!_teleportIsActive)
        {
            rayInteractor.enabled = true;
            _teleportIsActive = true;
        }

    }

    //This is called when our Teleport Mode Cancel action map is triggered
    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        if (_teleportIsActive && rayInteractor.enabled == true)
        {
            rayInteractor.enabled = false;
            _teleportIsActive = false;
        }

    }
}