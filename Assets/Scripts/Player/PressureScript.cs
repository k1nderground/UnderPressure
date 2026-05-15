using UnityEngine;
using UnityEngine.UI;
using YG;

public class PressureScript : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] MovementScript ms;
    [SerializeField] UpgradeSystem us;
    [SerializeField] SoundScript sound;

    [Header("Pressure")]
    [SerializeField] float drainAmount;
    [SerializeField] float maxPressure = 10f;
    [SerializeField] float pressureAmount = 0.2f;
    [SerializeField] float speed = 5f;
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

        if(!YG2.envir.isMobile && !YG2.envir.isTablet){
            EButton.gameObject.SetActive(false);
            QButton.gameObject.SetActive(true);
        }
        else{
            EButton.gameObject.SetActive(false);
            QButton.gameObject.SetActive(false);
        }
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
                    sound.Play(3);
                    pressure += pressureAmount;
                    diffKey = !diffKey;
                }
            }
            if(!diffKey){
                if (Input.GetKeyDown(secondKey))
                {
                    QButton.gameObject.SetActive(true);
                    EButton.gameObject.SetActive(false);
                    AirPump.sprite = Sprites[0];
                    sound.Play(4);

                    pressure += pressureAmount;
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
    public void StartDraining()
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
            return Mathf.SmoothStep(0f, speed, accelT);
        }

        if (t > speed - (brakeTime / allDrainTime))
        {
            float decelT = (t - (speed - (brakeTime / allDrainTime))) / (brakeTime / allDrainTime);
            return Mathf.SmoothStep(speed, 0f, decelT);
        }

        return speed;
    }

    public void QButtonPress()
    {
        if(pressure<maxPressure){
            if(diffKey){
                    QButton.gameObject.SetActive(false);
                    EButton.gameObject.SetActive(true);
                    AirPump.sprite = Sprites[1];

                    pressure += 0.2f;
                    diffKey = !diffKey;
            }
        }
    }

    public void EButtonPress()
    {
        if(pressure<maxPressure){
            if(!diffKey){
                    QButton.gameObject.SetActive(true);
                    EButton.gameObject.SetActive(false);
                    AirPump.sprite = Sprites[0];

                    pressure += 0.2f;
                    diffKey = !diffKey;
            }
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetPressureAmount(float pressureAmount)
    {
        this.pressureAmount = pressureAmount;
    }

    public void SetMaxPressure(float maxPressure)
    {
        this.maxPressure = maxPressure;
    }

    public void AddPressure()
    {
        if(pressure<maxPressure){
            pressure += pressureAmount;
        }
    }
}
