using UnityEngine;

public static class TransformExtensionMethods
{
    public static void SmoothLookAt(this Transform objectToRotateToLookAt, Transform target, Vector3 up, float smoothSpeed)
    {
        if (objectToRotateToLookAt == null || target == null) return; //we don't do anything if the object is null
        //we get the forward direction where we want to look at (direction vector resulting of the target minus the source object)
        Vector3 lookForwardDirection = (target.position - objectToRotateToLookAt.position).normalized;
        //we calculate the rotation to face the forward vector (the destination rotation)
        Quaternion targetLookRotation = Quaternion.LookRotation(lookForwardDirection, up);
        //we make an interpolation between our source object (objectToRotateToLookAt) and the lookRotation
        Quaternion smoothRotationToTarget = Quaternion.Lerp(objectToRotateToLookAt.rotation, targetLookRotation, Time.deltaTime * smoothSpeed);
        //we assign our "partial" rotation
        objectToRotateToLookAt.rotation = smoothRotationToTarget;
    }

    public static void SmoothTranslate(this Transform objectToTranslate, float verticalInput, float horizontalInput, Vector3 up, Vector3 right, float deltaTime)
    {
        if (objectToTranslate == null) return; //we don't do anything if the object is null
        //we make our directions to point in the relative up directions
        Vector3 verticalRelativeInputMovement = verticalInput * up * deltaTime;
        //we make our directions to point in the relative right directions
        Vector3 horizontalRelativeInputMovement = horizontalInput * right * deltaTime;
        //we combine directions for horizontal and vertical and translate the object
        objectToTranslate.Translate(verticalRelativeInputMovement + horizontalRelativeInputMovement, Space.World);
    }

    public static void ClampTranslationInsideCameraBounds(this Transform objectToClamp, Camera camera)
    {
        if (objectToClamp == null || camera == null) return; //we don't do anything if the object is null
        // TODO: move this calculations inside its own class inheriting from Unity Camera and adding those properties
        // Those should only be recalculated when the player changes resolution (we can make a callback OnResolutionChange to be
        // triggered when the player changes resolution from pause menu)
        // TODO: calculations breaks when the player moves the character to the sides or up/down
        //we calculate the boundaries of the camera view
        float cameraWidth = Screen.width / camera.fieldOfView / 2.0f;
        float cameraHeigth = cameraWidth / camera.aspect;
        //we have to apply an offset to align to the player's view
        float offset = cameraHeigth / 2.0f;
        //we move the object to the correct local position clamping it's value to be inside of the camera view
        Vector3 localPosition = new Vector3
        {
            x = Mathf.Clamp(objectToClamp.localPosition.x, -cameraWidth, cameraWidth),
            y = Mathf.Clamp(objectToClamp.localPosition.y, -cameraHeigth + offset, cameraHeigth + offset),
            z = objectToClamp.localPosition.z
        };
        //we assign the local position
        objectToClamp.localPosition = localPosition;
    }

    public static void ClampTranslationInsideBounds(this Transform objectToClamp, Vector2 limits)
    {
        if (objectToClamp == null) return; //we don't do anything if the object is null
        //we clamp the local position inside given limit values
        objectToClamp.ClampTranslationInsideBounds(limits.x, limits.y);
    }

    public static void ClampTranslationInsideBounds(this Transform objectToClamp, float limitX, float limitY)
    {
        if (objectToClamp == null) return; //we don't do anything if the object is null

        //we clamp the local position inside given limit values
        Vector3 localPosition = new Vector3
        {
            x = Mathf.Clamp(objectToClamp.localPosition.x, -limitX, limitX),
            y = Mathf.Clamp(objectToClamp.localPosition.y, -limitY, limitY),
            z = objectToClamp.localPosition.z
        };
        //we assign the local position
        objectToClamp.localPosition = localPosition;
    }
}
