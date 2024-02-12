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
        //we take the position of the object to screen space
        float distance = camera.WorldToScreenPoint(objectToClamp.position).z;
        //we take the local position of the camera to apply the correct offset to the object
        Vector2 cameraLocalPosition = camera.transform.localPosition;
        //we convert the screen bounds to local space which we want to move
        Vector3 bottomLeftLocal = camera.CameraBottomLeftLocalSpace(distance);
        Vector3 topRightLocal = camera.CameraTopRightLocalSpace(distance);
        //we get the renderer of the object if it exists
        Renderer objectToClampRenderer = objectToClamp.GetComponentInChildren<Renderer>();
        //we calculate the bounds size if it exists
        Vector3 objectToClampRendererBoundsSize = objectToClampRenderer.GetRendererBoundsSize();
        //we save the local position of the object
        Vector3 newLocalPosition = objectToClamp.localPosition;
        //we calculate the limits taking into account the camera position and the size of the object to clamp
        float minX = bottomLeftLocal.x + cameraLocalPosition.x + objectToClampRendererBoundsSize.x;
        float maxX = topRightLocal.x + cameraLocalPosition.x - objectToClampRendererBoundsSize.x;
        float minY = bottomLeftLocal.y + cameraLocalPosition.y - objectToClampRendererBoundsSize.y;
        float maxY = topRightLocal.y + cameraLocalPosition.y - objectToClampRendererBoundsSize.y;
        //we are going to calculate the new local position but clamped to be inside screen bounds
        newLocalPosition.x = Mathf.Clamp(newLocalPosition.x, minX, maxX);
        newLocalPosition.y = Mathf.Clamp(newLocalPosition.y, minY, maxY);
        //we update the local position
        objectToClamp.localPosition = newLocalPosition;
    }

    public static void ClampTranslationInsideBounds(this Transform objectToClamp, Camera camera, Vector2 limits)
    {
        if (objectToClamp == null) return; //we don't do anything if the object is null
        //we clamp the local position inside given limit values
        objectToClamp.ClampTranslationInsideBounds(camera, limits.x, limits.y);
    }

    public static void ClampTranslationInsideBounds(this Transform objectToClamp, Camera camera, float limitX, float limitY)
    {
        if (objectToClamp == null) return; //we don't do anything if the object is null
        //we take the local position of the camera to apply the correct offset to the object
        Vector2 cameraLocalPosition = camera.transform.localPosition;
        //we clamp the local position inside given limit values
        Vector3 localPosition = new Vector3
        {
            x = Mathf.Clamp(objectToClamp.localPosition.x, -limitX + cameraLocalPosition.x, limitX - cameraLocalPosition.x),
            y = Mathf.Clamp(objectToClamp.localPosition.y, -limitY - cameraLocalPosition.y, limitY - cameraLocalPosition.y),
            z = objectToClamp.localPosition.z
        };
        //we assign the local position
        objectToClamp.localPosition = localPosition;
    }
}
