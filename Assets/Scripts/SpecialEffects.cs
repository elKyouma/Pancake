using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialEffects : MonoBehaviour
{

    private static SpecialEffects instance;
    public static SpecialEffects Instance { get { return instance; } }

    CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeLength;
    private float shakeTimer;
    private float shakeStrength;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("2 Special Effect Objects");

        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ScreenShake(float time, float strength)
    {
        shakeLength = time;
        shakeTimer = time;
        shakeStrength = strength;
    }

    private void Update()
    {
        if(shakeTimer > 0f)
        {
            float strength = Mathf.Lerp(shakeStrength, 0, 1f - shakeTimer/shakeLength);
            noise.m_AmplitudeGain = strength;

            shakeTimer -= Time.deltaTime;
        }
        else 
            noise.m_AmplitudeGain = 0f;

    }
}
