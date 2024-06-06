using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove: MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    public GameObject destination;
    //private bool _isCameraEventEnded = false;
    //private float _pos_v = 0f;
    // Update is called once per frame
    private Quaternion initialRotation; //초기 카메라 셋팅값 저장 하기 위해

    void Start()
    {
        //초기 카메라 셋팅값 저장하기 위해 / destination tag 찾기 실행
        initialRotation = transform.rotation;
        destination = GameObject.FindGameObjectWithTag("destination");
    }    

    
    void Update()
    {
        //if (_isCameraEventEnded)
            //CameraEvent();
        //else
            //초기 카메라 셋팅 값 가져오기
            transform.rotation = initialRotation;
            transform.position = new Vector3(target.transform.position.x + offset.x, offset.y, target.transform.position.z + offset.z);
    }
    /*
    void CameraEvent()
    {
        //1. 안개 에셋이 카메라 이벤트 동안 켜졌다 꺼지기
        //2. destination 값받아서 일직선으로 움직였다가 다시 돌아오기
        if(_pos_v < 10)
        {
             _pos_v += Time.deltaTime;
        //transform.position = new Vector3();
        }
        //else에서 _isCameraEventEnded 값 넘겨주기
        else
        {
            _isCameraEventEnded = true;
            _pos_v = 0f;
        }
    }*/
}
