using System;
using UnityEngine;

[Serializable]
public struct CameraCheckpoint
{
    public Camera camera;
    public Checkpoint checkpoint;
}

public class AutomaticCameraSystem : MonoBehaviour
{
    public CheckpointManager kartToFollow;
    public CameraCheckpoint[] cameraCheckpoints;

    private void Start()
    {
        setCameraActive(0);
        kartToFollow.reachedCheckpoint += OnReachedCheckpoint;
    }

    // Checkpoint에 도달하면 할일
    // 현재 도착한 Checkpoint를 CameraCheckpoint 쌍에서 찾아 연결된 카메라 활성화
    private void OnReachedCheckpoint(Checkpoint checkpoint)
    {
        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            if (cameraCheckpoint.checkpoint == checkpoint)
            {
                DeactivateAllCamera();
                cameraCheckpoint.camera.gameObject.SetActive(true);
            }
        }
    }

    // 해당 카메라만 홀성화
    public void setCameraActive(int index)
    {
        DeactivateAllCamera();
        cameraCheckpoints[index].camera.gameObject.SetActive(true);
    }

    // 모든 카메라 비활성화
    private void DeactivateAllCamera()
    {
        foreach (CameraCheckpoint cameraCheckpoint in cameraCheckpoints)
        {
            cameraCheckpoint.camera.gameObject.SetActive(false);
        }
    }
}
