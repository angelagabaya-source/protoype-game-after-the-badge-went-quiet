using UnityEngine;

public class ObjectGlow : MonoBehaviour
{
    [Header("Glow Settings")]
    public Color glowColor = Color.white;
    public float minIntensity = 0f;
    public float maxIntensity = 2f;
    public float pulseSpeed = 1.5f;

    private Light lightComp;

    void Start()
    {
        lightComp = GetComponent<Light>();
        if (lightComp != null)
        {
            lightComp.color = glowColor;
        }
    }

    void Update()
    {
        if (lightComp == null) return;

        // Uses a Sine wave to create a smooth looping pulse
        // (Mathf.Sin + 1) / 2 converts the wave from -1 to 1 into a 0 to 1 range
        float lerp = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        lightComp.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp);
    }
}