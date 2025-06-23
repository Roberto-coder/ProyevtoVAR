using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float distanceInFront = 1.8f;
    public float rotationSpeed = 10f;

    void Update()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceInFront;
        Vector3 directionToTarget = targetPosition - transform.position;
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
