using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 충돌 오브젝트에 CheckpointManager가 있으면(Checkpoint면) CheckPointReached 함수 호출
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CheckpointManager>() != null)
        {
            other.GetComponent<CheckpointManager>().CheckPointReached(this);
        }
    }
}
