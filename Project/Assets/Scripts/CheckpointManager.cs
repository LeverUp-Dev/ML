using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public float MaxTimeToReachNextCheckpoint = 30f;
    public float TimeLeft = 30f;
    
    public KartAgent kartAgent;
    public Checkpoint nextCheckPointToReach;
    
    private int CurrentCheckpointIndex;
    private List<Checkpoint> Checkpoints;

    // 체크포인트에 도달했을 때 발생하는 이벤트 선언
    public event Action<Checkpoint> reachedCheckpoint; 

    void Start()
    {
        Checkpoints = FindObjectOfType<Checkpoints>().checkPoints;
        ResetCheckpoints();
    }

    public void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        TimeLeft = MaxTimeToReachNextCheckpoint;
        
        SetNextCheckpoint();
    }

    private void Update()
    {
        // 30초내에 Checkpoint에 도달 못하면 -1 보상 후 에피소드 종료
        TimeLeft -= Time.deltaTime;
        if (TimeLeft < 0)
        {
            kartAgent.AddReward(-1);
            kartAgent.EndEpisode();
        }
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;        
 
        reachedCheckpoint?.Invoke(checkpoint);// reachedCheckpoint 이벤트 발생
        CurrentCheckpointIndex++;

        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            // Goal 도착: +0.5 보상후 에피소드 종료
            kartAgent.AddReward(0.5f);
            kartAgent.EndEpisode();
        }
        else
        {
            // Checkpoint 도착: +0.5/Checkpoints.Count 보상 
            kartAgent.AddReward(0.5f / Checkpoints.Count);
            SetNextCheckpoint();
        }
    }

    private void SetNextCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            TimeLeft = MaxTimeToReachNextCheckpoint;
            nextCheckPointToReach = Checkpoints[CurrentCheckpointIndex];            
        }
    }
}
