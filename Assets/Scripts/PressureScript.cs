using UnityEngine;

public class PressureScript : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] MovementScript ms;

    [Header("Pressure")]
    [SerializeField] float drainAmount;

    [Header("SpeedCurve")]
    [SerializeField] float accelerationTime;
    [SerializeField] float brakeTime;

    [Header("Debug")]
    public bool isDraining;
    public bool diffKey;
    public float pressure;
    public float allDrainTime;
    public float nowDrainTime;
    public float forceMultiplier;
    void Start()
    {
        isDraining = false;
        pressure = 0;
    }

    void Update()
    {
        if (!isDraining)
        {
            if(diffKey){
                if (Input.GetKeyDown("q"))
                {
                    pressure += 1f;
                    diffKey = !diffKey;
                }
            }
            if(!diffKey){
                if (Input.GetKeyDown("e"))
                {
                    pressure += 1f;
                    diffKey = !diffKey;
                }
            }
            if (Input.GetKeyDown("w") && pressure>0)
            {
                StartDraining();
            }
        }

        if (isDraining)
        {
            Drain();
            
        }
    }
    void StartDraining()
    {
        isDraining = true;
        allDrainTime = pressure / drainAmount;
        nowDrainTime = 0f;
    }

    void Drain()
    {
        nowDrainTime += Time.deltaTime;

        float t = nowDrainTime / allDrainTime;

        forceMultiplier = EvaluateMovementCurve(t);

        ms.SetMoveMultiplier(forceMultiplier);

        ms.Move();

        if (nowDrainTime >= allDrainTime)
        {
            isDraining = false;
            pressure = 0f;
        }
    }

    float EvaluateMovementCurve(float t)
    {
        if (t < brakeTime / allDrainTime)
        {
            float accelT = t / (brakeTime / allDrainTime);
            return Mathf.SmoothStep(0f, 5f, accelT);
        }

        if (t > 1f - (brakeTime / allDrainTime))
        {
            float decelT = (t - (1f - (brakeTime / allDrainTime))) / (brakeTime / allDrainTime);
            return Mathf.SmoothStep(5f, 0f, decelT);
        }

        return 5f;
    }
}
