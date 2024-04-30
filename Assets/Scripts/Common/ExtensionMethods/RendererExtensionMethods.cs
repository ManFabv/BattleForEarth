using UnityEngine;
using UnityEngine.UI;

public static class RendererExtensionMethods
{
    public static Vector3 GetRendererBoundsSizeInWorldUnits(this Renderer renderer, Canvas parentCanvas)
    {
        if(renderer == null || parentCanvas == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the bounds
        Bounds objectToClampRendererBounds = renderer.bounds;
        //we return the size
        return objectToClampRendererBounds.size / parentCanvas.scaleFactor;
    }

    public static Vector3 GetImageBoundsSizeInWorldUnits(this Image image, CanvasScaler parentCanvasScaler)
    {
        if (image == null || image.sprite == null || parentCanvasScaler == null) return Vector3.zero;  //we return zero if the object is null
        //we calculate the bounds
        Bounds objectToClampRendererBounds = image.sprite.bounds;
        //we return the size
        return (image.rectTransform.sizeDelta * image.pixelsPerUnitMultiplier) / parentCanvasScaler.referencePixelsPerUnit;
    }

    public static Vector3 GetImageHalfBoundsSizeInWorldUnits(this Image image, CanvasScaler parentCanvasScaler)
    {
        return GetImageBoundsSizeInWorldUnits(image, parentCanvasScaler) / 2;
    }
}