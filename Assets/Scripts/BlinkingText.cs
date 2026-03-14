using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float blinkSpeed = 1f;
    public float min = 0.025f;
    public float max = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f; // Oscillates between 0 and 1
        if (alpha < min) alpha = min; 
        if (alpha > max) alpha = max; // Keep it between min and max for better visibility
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}
