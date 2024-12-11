using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class KartAgent : Agent
{
    public CheckpointManager checkpointManager;
    private KartController kartController;

    public override void Initialize()
    {
        kartController = GetComponent<KartController>();
    }

    public override void OnEpisodeBegin()
    {
        // Checkpoint 리셋 & 카트들 Respawn
        checkpointManager.ResetCheckpoints();
        kartController.Respawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // nextCheckPoint의 방향벡터 관찰과 시간지연 페널티 보상
        Vector3 diff = checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f); // (x, y, z), -1 < value < +1
        AddReward(-0.001f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // 회전, 가속, 애니메이션에 대한 actions 적용
        var contActions = actions.ContinuousActions;
        var disActions = actions.DiscreteActions;

        kartController.Steer(contActions[0]);                        // 회전
        kartController.ApplyAcceleration(disActions[0]);    // 가속
        kartController.AnimateKart(contActions[0]);            // kart 애니메이션  
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 좌우 방향키와 스페이스바에 대한 입력 처리
        //contActionsOut[0] = Input.GetAxis("Horizontal");
        //discreteActionsOut[0] = 0;
        //if (Input.GetKey(KeyCode.Space)) {
        //    discreteActionsOut[0] = 1;
        //}
    }
}
