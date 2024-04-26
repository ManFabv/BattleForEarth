using KBCore.Refs;
using UnityEngine;

public class AlwaysFaceCamera : ValidatedMonoBehaviour
{
    [SerializeField] private Camera _facingCamera;

    [HideInInspector, SerializeField, Self] Transform _cachedTransform;

    private void Awake()
    {
        if(_facingCamera == null)
        {
            _facingCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        _cachedTransform.LookAt(_facingCamera.transform.position);
    }
}
