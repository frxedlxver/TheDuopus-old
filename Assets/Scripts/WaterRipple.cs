using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRipple : MonoBehaviour
{
    private Material material;
    

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
    }

    void Update()
    {
        float time = Time.time; 
        float rippleAmount = 0.1f * time;
    
        material.SetFloat("_WaterTime", time);
        material.SetFloat("_RippleAmount", rippleAmount);
    }
}
