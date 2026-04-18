using UnityEngine;
using UnityEngine.UI;

public class PressureScript : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] MovementScript ms;

    [Header("Pressure")]
    [SerializeField] float drainAmount;
    [SerializeField] float maxPressure = 10f;
    [SerializeField] ParticleSystem Smoke;

    [Header("KeyCodes")]
    [SerializeField] KeyCode firstKey = KeyCode.Q;
    [SerializeField] KeyCode secondKey = KeyCode.E;
    [SerializeField] KeyCode startKey = KeyCode.W;

    [Header("SpeedCurve")]
    [SerializeField] float accelerationTime;
    [SerializeField] float brakeTime;

    [Header("AirPump")]
    [SerializeField] Sprite[] Sprites;
    [SerializeField] Image AirPump;
    [SerializeField] Button QButton;
    [SerializeField] Button EButton;

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
        diffKey = true;

        EButton.gameObject.SetActive(false);
        QButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isDraining)
        {
            if(pressure<maxPressure){
            if(diffKey){
                if (Input.GetKeyDown(firstKey))
                {
                    QButton.gameObject.SetActive(false);
                    EButton.gameObject.SetActive(true);
                    AirPump.sprite = Sprites[1];

                    pressure += 0.2f;
                    diffKey = !diffKey;
                }
            }
            if(!diffKey){
                if (Input.GetKeyDown(secondKey))
                {
                    QButton.gameObject.SetActive(true);
                    EButton.gameObject.SetActive(false);
                    AirPump.sprite = Sprites[0];

                    pressure += 0.2f;
                    diffKey = !diffKey;
                }
            }
            }
            if (Input.GetKeyDown(startKey) && pressure>0)
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
        Smoke.Play();
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

        if (nowDrainTime >= allDrainTime)
        {
            Smoke.Stop();
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
            float decelT = (t - (5f - (brakeTime / allDrainTime))) / (brakeTime / allDrainTime);
            return Mathf.SmoothStep(5f, 0f, decelT);
        }

        return 5f;
    }

    public void QButtonPress()
    {
        if(pressure<maxPressure){
            if(diffKey){
                if (Input.GetKeyDown(firstKey))
                {
                    QButton.gameObject.SetActive(false);
                    EButton.gameObject.SetActive(true);
                    AirPump.sprite = Sprites[1];

                    pressure += 0.2f;
                    diffKey = !diffKey;
                }
            }
        }
    }

    public void EButtonPress()
    {
        if(pressure<maxPressure){
            if(!diffKey){
                if (Input.GetKeyDown(secondKey))
                {
                    QButton.gameObject.SetActive(true);
                    EButton.gameObject.SetActive(false);
                    AirPump.sprite = Sprites[0];

                    pressure += 0.2f;
                    diffKey = !diffKey;
                }
            }
        }
    }
}
