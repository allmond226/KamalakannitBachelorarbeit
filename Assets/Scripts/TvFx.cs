using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TvFx : MonoBehaviour
{
    public Light2D tvLight;
    public float minIntensity = 0.6f;
    public float maxIntensity = 1.1f;
    public float flickerSpeed = 2f;
    public Color colorA = new Color(0.6f, 0.7f, 1f);
    public Color colorB = new Color(1f, 0.8f, 0.6f);
    public float colorSpeed = 0.5f;
    Vector3 startPosition;

    float seed;
    // Start is called before the first frame update
    void Start()
    {
        seed = Random.value * 100f;
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float slow = Mathf.PerlinNoise(seed, Time.time * flickerSpeed * 1.5f);
        float fast = Mathf.PerlinNoise(seed + 5, Time.time * flickerSpeed * 8f) * 0.15f;
        tvLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, slow + fast);
        float colorNoise = Mathf.PerlinNoise(seed + 10, Time.time * colorSpeed);
        tvLight.color = Color.Lerp(colorA, colorB, colorNoise);
        float move = Mathf.PerlinNoise(seed + 20, Time.time * 0.5f);
        transform.localPosition = startPosition + new Vector3(move * 0.05f, 0, 0);
    }
}
