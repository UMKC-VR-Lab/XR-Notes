using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HandMenuPoint : MonoBehaviour
{
    public float updatesPerSecond = 30f;
    public Transform handTransform, headTransform;
    public Vector3 positionOffsetFromHand;
    public bool tiltAlongAxisZ = true;
    public bool tiltAlongAxisX = true;
    public bool trackHand = true;
    public bool aimAtFace = true;
    public bool smoothUpdates = true;
    public float smoothingSpeed = 5f;
    private float updateInterval;

    public Vector3 FocusPosition { get; private set; }
    public Quaternion FocusRotation { get; private set; }
    private Coroutine updateRoutine;
    private bool updateTransform = true;

    private void OnEnable()
    {
        if(updateTransform)
        {
            StartUpdating();
        }
    }

    private void OnDisable()
    {
        StopUpdating();
    }

    public void StartUpdating()
    {
        updateTransform = true;
        updateRoutine = StartCoroutine(UpdateFocusPoint());
        updateInterval = 1f / updatesPerSecond;
    }

    public void StopUpdating()
    {
        if(updateTransform)
        {
            updateTransform = false;
            if(updateRoutine != null)
                StopCoroutine(updateRoutine);
        }
    }

    private IEnumerator UpdateFocusPoint()
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand Transform is null on " + gameObject.name);
            trackHand = false;
            yield break;
        }

        // Calculate target focus position
        Vector3 targetPosition = handTransform.position + Vector3.up * (positionOffsetFromHand.y);

        // Calculate target focus rotation
        Quaternion targetRotation = quaternion.identity;
        if(aimAtFace) {
            targetRotation = Quaternion.LookRotation(handTransform.position - headTransform.position);
        }
        else
        {
            Vector3 eulerAngles = Vector3.zero;
            eulerAngles.y = handTransform.eulerAngles.y;
            if (tiltAlongAxisZ) eulerAngles.z = handTransform.eulerAngles.z;
            if (tiltAlongAxisX) eulerAngles.x = handTransform.eulerAngles.x;
            targetRotation = Quaternion.Euler(eulerAngles);
        }

        // Apply smoothing if enabled
        if (smoothUpdates)
        {
            FocusPosition = Vector3.Lerp(FocusPosition, targetPosition, smoothingSpeed * Time.deltaTime);
            FocusRotation = Quaternion.Lerp(FocusRotation, targetRotation, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            FocusPosition = targetPosition;
            FocusRotation = targetRotation;
        }

        // Update transform
        transform.position = FocusPosition;
        transform.rotation = FocusRotation;

        yield return new WaitForSeconds(updateInterval);

        if(updateTransform)
        {
            StartCoroutine(UpdateFocusPoint());
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the focus point in the Scene view
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(FocusPosition, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(FocusPosition, FocusPosition + FocusRotation * Vector3.forward * 0.5f);

        // Draw a line perpendicular to the forward direction to visualize rotation more thoroughly
        Gizmos.color = Color.red;
        Gizmos.DrawLine(FocusPosition, FocusPosition + FocusRotation * Vector3.right * 0.5f);
    }
}
