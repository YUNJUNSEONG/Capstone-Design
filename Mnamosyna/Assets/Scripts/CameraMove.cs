using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public GameObject destination;
    private bool _isCameraEventEnded = false;
    private float _pos_v = 0f;
    // Update is called once per frame
    void Update()
    {
        if (_isCameraEventEnded)
            CameraEvent();
        else
            transform.position = new Vector3(target.transform.position.x + offset.x, offset.y, target.transform.position.z + offset.z);
    }

    void CameraEvent()
    {
        if(_pos_v < 10)
            _pos_v += Time.deltaTime;

    }
}
