using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BoardInteraction : MonoBehaviour
{
    // Attributes
    [SerializeField] private LineRenderer lineRendererLeft;
    [SerializeField] private LineRenderer lineRendererRight;
    [SerializeField] private Transform controllerTransformLeft;
    [SerializeField] private Transform controllerTransformRight;
    [SerializeField] private LayerMask boardLayer; // Layer mask for the board
    [SerializeField] private float interactionDistance = 2.5f;

    //For button down
    private bool previousLeftTriggerState = false;
    private bool previousRightTriggerState = false;

    // Methods
    private void Update()
    {
        HandleRaycast(controllerTransformLeft, lineRendererLeft);
        HandleRaycast(controllerTransformRight, lineRendererRight);

        bool leftTrigger;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger))
        {
            // On Trigger Down
            if (leftTrigger && !previousLeftTriggerState)
            {
                OnTriggerDown(controllerTransformLeft);
            }
            // Hold trigger
            else if (leftTrigger)
            {
                OnTriggerHold(controllerTransformLeft);
            }
        }

        bool rightTrigger;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightTrigger))
        {
            // On Trigger Down
            if (rightTrigger && !previousRightTriggerState)
            {
                OnTriggerDown(controllerTransformRight);
            }
            // Hold trigger
            else if (rightTrigger)
            {
                OnTriggerHold(controllerTransformRight);
            }
        }

        previousLeftTriggerState = leftTrigger;
        previousRightTriggerState = rightTrigger;
    }

    private void HandleRaycast(Transform controllerTransform, LineRenderer lineRenderer)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance, boardLayer))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, controllerTransform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    // On Key Down
    private void OnTriggerDown(Transform controllerTransform)
    {
        TryInteractWithDropdown(controllerTransform);
        TryInteractWithButton(controllerTransform);
    }

    // On Key Hold
    private void OnTriggerHold(Transform controllerTransform)
    {
        TryInteractWithSlider(controllerTransform);
    }

    private void TryInteractWithButton(Transform controllerTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance))
        {
            // Check if the hit component is a button
            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                // Call the method to interact with the button
                button.onClick.Invoke();
            }
        }
    }

    private void TryInteractWithSlider(Transform controllerTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance))
        {
            // Check if the hit component is a Slider
            Slider slider = hit.collider.GetComponent<Slider>();
            if (slider != null)
            {
                // Calculate the hit point's position relative to the slider's transform
                Vector3 hitPointLocal = slider.transform.InverseTransformPoint(hit.point);

                // Calculate the normalized value based on the hit point, considering the entire range of the slider
                float sliderWidth = slider.gameObject.GetComponent<RectTransform>().rect.width;
                float normalizedValue = Mathf.InverseLerp(-sliderWidth / 2, sliderWidth / 2, hitPointLocal.x);

                // Set the slider value considering the entire range
                slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, normalizedValue);
            }
        }
    }

    private void TryInteractWithDropdown(Transform controllerTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance))
        {
            // Check if the hit component is a Dropdown
            TMP_Dropdown dropdown = hit.collider.GetComponent<TMP_Dropdown>();
            if (dropdown != null)
            {
                // Check if the dropdown is already open
                bool wasExpanded = dropdown.IsExpanded;

                // If not open, simulate a click event on the dropdown to open it
                if (!wasExpanded)
                {
                    dropdown.Show();
                }
                else
                {
                    // Calculate the hit point's position relative to the dropdown's transform
                    Vector3 hitPointLocal = dropdown.transform.InverseTransformPoint(hit.point);

                    // Calculate the normalized value based on the hit point, considering the entire height of the dropdown
                    float dropdownHeight = dropdown.gameObject.GetComponent<RectTransform>().rect.height;
                    float normalizedValue = Mathf.InverseLerp(0, dropdownHeight, hitPointLocal.y);

                    // Calculate the item index based on the normalized value
                    int itemCount = dropdown.options.Count;
                    int itemIndex = Mathf.FloorToInt(normalizedValue * (itemCount - 1));

                    // Select the item from the dropdown
                    dropdown.value = itemIndex;

                    // Hide it after updating the value
                    dropdown.Hide();
                }
            }
        }
    }
}
