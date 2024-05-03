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
        //1. 안개 에셋이 카메라 이벤트 동안 켜졌다 꺼지기
        //2. destination 값받아서 일직선으로 움직였다가 다시 돌아오기
        //3. 되돌아오면 _isCameraEventEnded 값 넘겨주기
        //Q1. 시작 카메라 각도를 변경하고 싶은데, -> 초기화 x,y,z 값으로 진행했다가 다시 else진입시 main camera setting값 받아올 수 있는지? (tmp저장했다가)
        if(_pos_v < 10)
            _pos_v += Time.deltaTime;

        transform.position = new Vector3();
    }
}
