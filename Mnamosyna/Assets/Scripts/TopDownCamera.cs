using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float rotationSpeed = 5f;

    private Vector3 _cameraPosition;
    private bool _isMouseDown = false;

    void Start()
    {
        var transform1 = transform;
        var position = target.position;
        transform1.position = position + offset;
        _cameraPosition = transform1.position - position;
        
        Quaternion camTurnAngle = Quaternion.AngleAxis(45, Vector3.up);
        _cameraPosition = camTurnAngle * _cameraPosition;
    }

    void LateUpdate()
    {
        /*
        if (Input.GetMouseButtonDown(2))
        {
            _isMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            _isMouseDown = false;
        }

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        if (_isMouseDown)
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(mouseX, Vector3.up);
            _cameraPosition = camTurnAngle * _cameraPosition;
        }
        */

        var position = target.position;
        transform.position = position + _cameraPosition;
        transform.LookAt(position);
    }
}
