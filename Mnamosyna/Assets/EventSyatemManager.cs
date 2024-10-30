using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSyatemManager : MonoBehaviour
{
    public static EventSyatemManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ� ����
            return;
        }

    }
}
