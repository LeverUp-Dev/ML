using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checkpoint 컴포넌트를 가지고 있는자식들을 리스트에 삽입
public class Checkpoints : MonoBehaviour
{
    public List<Checkpoint> checkPoints;
    
    private void Awake()
    {
        checkPoints = new List<Checkpoint>(GetComponentsInChildren<Checkpoint>());
    }
}
