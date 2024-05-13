using System.Collections;
using System.Collections.Generic;
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

    // Layers from objects that can be grabbed
    [SerializeField] private LayerMask _defaultObjectLayer;

    // Layers that will replace the objects that can be grabbed layers
    [SerializeField] private LayerMask _grabbedObjectLayer;

    private void Start()
    {
        // In case an object is grabbed
        interactor.selectEntered.AddListener((SelectEnterEventArgs args) => 
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the grabbed object

            // If the grabbedObj layer is inside defaultObjectLayer, then we change it to its corresponding grabbedObjectLayer layer
            if((_defaultObjectLayer.value & (1 << grabbedObj.layer)) != 0)
            {
                ChangeLayerToObjAndChildren(grabbedObj, FindCorrespondingLayer(grabbedObj.layer, _grabbedObjectLayer));    // We change the layer
            }
        });

        // In case an object is ungrabbed
        interactor.selectExited.AddListener((SelectExitEventArgs args) =>
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the ungrabbed object

            // If the grabbedObj layer is inside grabbedObjectLayer, then we change it to its corresponding defaultObjectLayer layer
            if ((_grabbedObjectLayer.value & (1 << grabbedObj.layer)) != 0)
            {
                ChangeLayerToObjAndChildren(grabbedObj, FindCorrespondingLayer(grabbedObj.layer, _defaultObjectLayer));    // We change the layer
            }
        });
    }

    // Method to find the corresponding layer from the given LayerMask
    private int FindCorrespondingLayer(int currentLayer, LayerMask layerMask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((layerMask.value & (1 << i)) != 0 && currentLayer != i)
            {
                return i;
            }
        }
        return currentLayer; // Return current layer if corresponding layer not found
    }

    private void ChangeLayerToObjAndChildren(GameObject obj, int inputLayer)
    {
        foreach (Transform child in obj.transform) {
            child.gameObject.layer = inputLayer;   // We change the layer

            // If the child has children, recursively change their layers
            if (child.childCount > 0)
            {
                ChangeLayerToObjAndChildren(child.gameObject, inputLayer);
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
