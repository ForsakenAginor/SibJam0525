using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float pitch = 2f;
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 15f;
    [SerializeField] private float rotateSpeed = 150f;
    private float currentZoom = 10f;
    private float currentRotate = 0f;
    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        currentRotate -= Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
    }

    private void LateUpdate()//идет сразу за Update()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.up, currentRotate);
    }


}
