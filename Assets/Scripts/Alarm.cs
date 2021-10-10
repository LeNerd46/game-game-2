using UnityEngine;

public class Alarm : MonoBehaviour
{
    public Light alarm;

    public float fadeSpeed = 2;
    public float highIntensity = 2;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;

    public bool alarmOn;

    private float targetIntensity;

    void Awake()
    {   
        targetIntensity = highIntensity;
    }

    void Update()
    {
        if (alarmOn)
        {
            alarm.intensity = Mathf.Lerp(alarm.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        else
            alarm.intensity = Mathf.Lerp(alarm.intensity, 0f, fadeSpeed * Time.deltaTime);
    }

    void CheckTargetIntensity()
    {
        if(Mathf.Abs(targetIntensity - alarm.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
                targetIntensity = lowIntensity;
            else
                targetIntensity = highIntensity;
        }
    }
}