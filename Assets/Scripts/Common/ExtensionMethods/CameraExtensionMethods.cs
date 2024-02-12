using UnityEngine;

public static class CameraExtensionMethods
{
    public static Vector3 CameraBottomLeftLocalSpace(this Camera camera, float objectPositionZ)
    {
        if (camera == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the screen bounds from screen to world space and we convert the screen bounds from world space to local space
        return camera.CameraPositionOnLocalSpace(new Vector3(0, 0, objectPositionZ));
    }

    public static Vector3 CameraTopRightLocalSpace(this Camera camera, float objectPositionZ)
    {
        if (camera == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the screen bounds from screen to world space and we convert the screen bounds from world space to local space
        return camera.CameraPositionOnLocalSpace(new Vector3(Screen.width, Screen.height, objectPositionZ));
    }

    public static Vector3 CameraPositionOnLocalSpace(this Camera camera, Vector3 position)
    {
        if (camera == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the position from screen to world space
        Vector3 worldPoint = camera.ScreenToWorldPoint(position);
        //we convert the position from world space to local space which we want to move
        return camera.transform.InverseTransformPoint(worldPoint);
    }
}