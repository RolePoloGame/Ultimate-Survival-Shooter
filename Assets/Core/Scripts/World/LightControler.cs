using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightControler : MonoBehaviour
{
    public bool IsActive = true;
    public float BlinkTime = 5.0f;
    [Range(0.1f, 0.0001f)]
    public float SwitchInterval = 0.01f;

    [SerializeField]
    private float HighIntensity = 4.0f;
    [SerializeField]
    private float LowIntensity = 0.0f;

    private void Awake()
    {
        if (!IsActive) return;
        StartBlink();
        UpdateColor();
    }

    private void UpdateColor()
    {
        GetLight().color = Color;
    }

    private Light m_light;

    public Color Color
    {
        get => color;
        set
        {
            color = value;
            GetLight().color = color;
        }
    }
    [SerializeField]
    [ColorUsage(false)]
    private Color color = Color.white;
    public void SetIntensity(float high, float low)
    {
        SetHigh(high);
        SetLow(low);
    }
    public void SetHigh(float High) => HighIntensity = High;
    public void SetLow(float Low) => LowIntensity = Low;

    private Light GetLight()
    {
        if (m_light == null)
            m_light = GetComponent<Light>();
        return m_light;
    }
    [Button]
    public void StartBlink()
    {
        StopAllCoroutines();
        StartCoroutine(Blink(BlinkTime));
    }

    private IEnumerator Blink(float blinkTime)
    {
        float halfBlinkTime = blinkTime / 2f;

        float timer = 0.0f;
        while (true)
        {
            float timeFraction = timer / halfBlinkTime;
            if (timer > halfBlinkTime)
                timeFraction = 1.0f - (timeFraction - 1.0f);
            float intensityValue = Mathf.Lerp(LowIntensity, HighIntensity, timeFraction);
            GetLight().intensity = intensityValue;
            if (timer > blinkTime)
                timer = 0.0f;
            yield return new WaitForSeconds(SwitchInterval);
            timer += Time.deltaTime;
        }
    }
}
