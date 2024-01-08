using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + offset.x, offset.y, target.transform.position.z + offset.z);
    }
}
