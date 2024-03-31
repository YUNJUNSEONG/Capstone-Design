using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UItoCamera : MonoBehaviour
{
    private GameObject mainCameraObject;
    public Transform cameraTransform;
    void Start()
    {
        mainCameraObject = GameObject.Find("Main Camera");
    }
    
    void Update()
    {
        cameraTransform = mainCameraObject.transform;
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }
}
