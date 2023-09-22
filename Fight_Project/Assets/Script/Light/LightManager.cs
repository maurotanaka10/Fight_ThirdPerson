using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private Light _light;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    private void Awake()
    {
        _light = GetComponent<Light>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(MakeTheLightBlink), 1.0f, 1.0f);
    }

    private void MakeTheLightBlink()
    {
        StartCoroutine(BlinkTimer());
        _light.intensity = maxIntensity;
    }

    private IEnumerator BlinkTimer()
    {
        yield return new WaitForSeconds(0.5f);
        _light.intensity = minIntensity;
    }
}
