using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicsHand : MonoBehaviour
{
    [SerializeField] private Transform trackedTransform;
    [SerializeField] private Rigidbody body;
    [SerializeField] private XRDirectInteractor interactor; // added interactor

    [SerializeField] private float positionStrength = 20;
    [SerializeField] private float positionThreshold = 0.005f;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private float rotationStrength = 30;
    [SerializeField] private float rotationThreshold = 10f;

    //// Layers from objects that can be grabbed
    //[SerializeField] private LayerMask _defaultObjectLayer;

    //// Layers that will replace the objects that can be grabbed layers
    //[SerializeField] private LayerMask _grabbedObjectLayer;

    // List of layers from objects that can be grabbed
    [SerializeField]
    private string defaultObjectLayer = "Interactable";

    // List of layers that will replace the objects that can be grabbed layers
    [SerializeField]
    private string grabbedObjectLayer = "GrabbedInteractable";

    private void Start()
    {
        // In case an object is grabbed
        interactor.selectEntered.AddListener((SelectEnterEventArgs args) => 
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the grabbed object

            // If the grabbedObj layer is inside defaultObjectLayer, then we change it to its corresponding grabbedObjectLayer layer
            if (grabbedObj.layer == LayerMask.NameToLayer(defaultObjectLayer))
                ChangeLayerToObjAndChildren(grabbedObj, LayerMask.NameToLayer(grabbedObjectLayer), LayerMask.NameToLayer(defaultObjectLayer));    // We change the layer

            //if((_defaultObjectLayer.value & (1 << grabbedObj.layer)) != 0)
            //{
            //    int positionIdx = FindLayerPosition(_defaultObjectLayer, grabbedObj.layer);
            //    if(positionIdx>-1)
            //        ChangeLayerToObjAndChildren(grabbedObj, GetNthLayer(_grabbedObjectLayer, positionIdx));    // We change the layer
            //}
        });

        // In case an object is ungrabbed
        interactor.selectExited.AddListener((SelectExitEventArgs args) =>
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the ungrabbed object

            // If the grabbedObj layer is inside grabbedObjectLayer, then we change it to its corresponding defaultObjectLayer layer
            if (grabbedObj.layer == LayerMask.NameToLayer(grabbedObjectLayer))
                ChangeLayerToObjAndChildren(grabbedObj, LayerMask.NameToLayer(defaultObjectLayer), LayerMask.NameToLayer(grabbedObjectLayer));    // We change the layer

            //if ((_grabbedObjectLayer.value & (1 << grabbedObj.layer)) != 0)
            //{
            //    int positionIdx = FindLayerPosition(_grabbedObjectLayer, grabbedObj.layer);
            //    if (positionIdx > -1)
            //        ChangeLayerToObjAndChildren(grabbedObj, GetNthLayer(_defaultObjectLayer, positionIdx));    // We change the layer
            //}
        });
    }

    // Function to find the position of a layer within a layer mask
    private int FindLayerPosition(LayerMask layerMask, int layer)
    {
        int position = 0;
        int mask = layerMask.value;

        // Iterate over each bit in the layer mask
        while (mask > 0)
        {
            // Check if the current bit is set
            if ((mask & 1) != 0)
            {
                // If the layer at the current position matches the given layer,
                // return the current position
                if (LayerMask.LayerToName(position) == LayerMask.LayerToName(layer))
                {
                    return position;
                }
            }

            // Move to the next bit
            mask >>= 1;
            position++;
        }

        // If the layer is not found, return -1
        return -1;
    }


    // Get the nth layer from _obstacleLayers
    int GetNthLayer(LayerMask layerMask, int n)
    {
        int count = 0;
        int result = 0;
        int mask = layerMask.value;

        // Iterate over each bit until we reach the nth bit
        while (mask != 0 && count < n)
        {
            // Isolate the least significant 1 bit
            int leastSignificantBit = (mask & -mask);

            // Left shift to move to the next bit
            result = leastSignificantBit << 1;

            // Clear the least significant 1 bit
            mask &= ~leastSignificantBit;

            // Increment the count
            count++;
        }

        return result;
    }



    private void ChangeLayerToObjAndChildren(GameObject obj, int inputLayer, int previousLayer)
    {
        if (obj.layer == previousLayer)
            obj.layer = inputLayer;   // We change the layer

        foreach (Transform child in obj.transform) {
            if(child.gameObject.layer == previousLayer)
                child.gameObject.layer = inputLayer;   // We change the layer

            // If the child has children, recursively change their layers
            if (child.childCount > 0)
            {
                ChangeLayerToObjAndChildren(child.gameObject, inputLayer, previousLayer);
            }
        }
    }

    private void FixedUpdate()
    {
        // Rest of the script
        var distance = Vector3.Distance(trackedTransform.position, body.position);
        if (distance > maxDistance || distance < positionThreshold)
        {
            body.MovePosition(trackedTransform.position);
        }
        else
        {
            var vel = (trackedTransform.position - body.position).normalized * positionStrength * distance;
            body.velocity = vel;
        }

        float angleDistance = Quaternion.Angle(body.rotation, trackedTransform.rotation);
        if (angleDistance < rotationThreshold)
        {
            body.MoveRotation(trackedTransform.rotation);
        }
        else
        {
            float kp = (6f * rotationStrength) * (6f * rotationStrength) * 0.25f;
            float kd = 4.5f * rotationStrength;
            Vector3 x;
            float xMag;
            Quaternion q = trackedTransform.rotation * Quaternion.Inverse(transform.rotation);
            q.ToAngleAxis(out xMag, out x);
            x.Normalize();
            x *= Mathf.Deg2Rad;
            Vector3 pidv = kp * x * xMag - kd * body.angularVelocity;
            Quaternion rotInertia2World = body.inertiaTensorRotation * transform.rotation;
            pidv = Quaternion.Inverse(rotInertia2World) * pidv;
            pidv.Scale(body.inertiaTensor);
            pidv = rotInertia2World * pidv;
            body.AddTorque(pidv);
        }
    }
}
