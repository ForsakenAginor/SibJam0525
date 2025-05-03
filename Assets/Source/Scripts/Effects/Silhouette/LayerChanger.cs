using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterialPrefab;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private LayerMask _mask;

    private Camera _mainCamera;
    private Material _defaultMaterial;
    private Material _transparentMaterial;

    private void Awake()
    {
        _defaultMaterial = _renderer.material;
        _defaultMaterial = new Material(_defaultMaterial);
        _transparentMaterial = new Material(_transparentMaterialPrefab);
        _renderer.material = _defaultMaterial;
        _mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 dir = (_collider.transform.position - _mainCamera.transform.position).normalized;

        if (Physics.Raycast(_mainCamera.transform.position, dir, out hit, _mask) && hit.collider == _collider)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            _renderer.material = _defaultMaterial;
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Silhouette");
            _renderer.material = _transparentMaterial;
        }
    }

}
