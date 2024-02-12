using UnityEngine;

public static class RendererExtensionMethods
{
    public static Vector3 GetRendererBoundsSize(this Renderer renderer)
    {
        if(renderer == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the bounds
        Bounds objectToClampRendererBounds = renderer.bounds;
        //we return the size
        return objectToClampRendererBounds.size;
    }
}