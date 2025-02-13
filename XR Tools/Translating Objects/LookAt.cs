using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;

    // Toggles for each axis
    public bool allowRotateX = true;
    public bool allowRotateY = true;
    public bool allowRotateZ = true;

    // Track rotation per axis
    private float xRotation;
    private float yRotation;
    private float zRotation;

    void Start()
    {
        // Initialize with current local rotation
        Vector3 startEuler = transform.eulerAngles;
        xRotation = startEuler.x;
        yRotation = startEuler.y;
        zRotation = startEuler.z;
    }

    void Update()
    {
        if (target == null) return;

        // Direction to target
        Vector3 direction = target.position - transform.position;

        // Desired rotation in Euler
        Quaternion targetRot = Quaternion.LookRotation(direction);
        Vector3 targetEuler = targetRot.eulerAngles;

        // Update only the allowed axes
        if (allowRotateX) xRotation = targetEuler.x;
        if (allowRotateY) yRotation = targetEuler.y;
        if (allowRotateZ) zRotation = targetEuler.z;

        // Apply the new rotation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }
}
