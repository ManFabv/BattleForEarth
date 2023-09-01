using UnityEngine;

public static class TransformExtensionMethods
{
    public static void SmoothLookAt(this Transform objectToRotateToLookAt, Transform target, Vector3 up, float smoothSpeed)
    {
        //we get the forward direction where we want to look at (direction vector resulting of the target minus the source object)
        Vector3 lookForwardDirection = (target.position - objectToRotateToLookAt.position).normalized;
        //we calculate the rotation to face the forward vector (the destination rotation)
        Quaternion targetLookRotation = Quaternion.LookRotation(lookForwardDirection, up);
        //we make an interpolation between our source object (objectToRotateToLookAt) and the lookRotation
        Quaternion smoothRotationToTarget = Quaternion.Lerp(objectToRotateToLookAt.rotation, targetLookRotation, Time.deltaTime * smoothSpeed);
        //we asign our "partial" rotation
        objectToRotateToLookAt.rotation = smoothRotationToTarget;
    }

    public static void SmoothTranslate(this Transform objectToTranslate, float verticalInput, float horizontalInput, Vector3 up, Vector3 right, float deltaTime)
    {
        //we make our directions to point in the relative up directions
        Vector3 verticalRelativeInputMovement = verticalInput * up * deltaTime;
        //we make our directions to point in the relative right directions
        Vector3 horizontalRelativeInputMovement = horizontalInput * right * deltaTime;
        //we combine directions for horizontal and vertical and translate the object
        objectToTranslate.Translate(verticalRelativeInputMovement + horizontalRelativeInputMovement, Space.World);
    }
}
