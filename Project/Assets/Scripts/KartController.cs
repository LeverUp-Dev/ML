using UnityEngine;

public class KartController : MonoBehaviour
{
    private SpawnPointManager spawnPointManager;

    public Transform kartModel;    
    public Rigidbody rb;

    float speed, currentSpeed;
    float rotate, currentRotate;

    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public Transform frontWheels;
    public Transform backWheels;
    public Transform steeringWheel;

    public void Awake()
    {
        spawnPointManager = FindObjectOfType<SpawnPointManager>();
    }

    public void ApplyAcceleration(float input)
    {
        speed = acceleration * input;
        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;
    }

    public void AnimateKart(float input)
    {
        kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (input * 15), kartModel.localEulerAngles.z), .2f);

        frontWheels.localEulerAngles = new Vector3(0, (input * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, rb.velocity.magnitude / 2);
        backWheels.localEulerAngles += new Vector3(0, 0, rb.velocity.magnitude / 2);

        steeringWheel.localEulerAngles = new Vector3(-25, 90, ((input * 45)));
    }

    public void Respawn()
    {
        Vector3 pos = spawnPointManager.SelectRandomSpawnpoint();
        rb.MovePosition(pos);
        transform.position = pos - new Vector3(0, 0.4f, 0); 
    }

    public void FixedUpdate()
    {        
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles,
            new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f); //회전 적용
        
        rb.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration); // 가속 적용        
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration); //Gravity 적용        
        transform.position = rb.transform.position - new Vector3(0, 0.4f, 0); // Collider를 따라 위치 설정
    }

    public void Steer(float steeringSignal)
    {
        int steerDirection = steeringSignal > 0 ? 1 : -1;
        float steeringStrength = Mathf.Abs(steeringSignal);

        rotate = (steering * steerDirection) * steeringStrength;
    }

}
