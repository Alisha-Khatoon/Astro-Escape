using System.Collections;
using UnityEngine;
using Cinemachine;

public class cameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachinePerlin;
    private float shakeIntensity = 1f;
    private float shakeDuration = 0.2f;
    private int shakeCount = 4; // Number of times to shake
    private float shakeInterval = 2f; // Interval between shakes
    private float initialDelay = 1f; // Delay before the first shake
    private float shakeTimer = 0f;
    private int currentShakeCount = 0;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachinePerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        // Start the shake sequence after a delay (e.g., 3 seconds)
        StartCoroutine(StartShakeSequence());
    }

    
    
    IEnumerator StartShakeSequence()
    {
        yield return new WaitForSeconds(initialDelay);
        StartCoroutine(ShakeCameraRepeatedly());
    }

    IEnumerator ShakeCameraRepeatedly()
    {
        while (currentShakeCount < shakeCount)
        {
            ShakeCamera();
            yield return new WaitForSeconds(shakeDuration + shakeInterval);
            currentShakeCount++;
        }
    }

    public void ShakeCamera()
    {
        cinemachinePerlin.m_AmplitudeGain = shakeIntensity;
        shakeTimer = shakeDuration;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                StopShake();
            }
        }
    }

    void StopShake()
    {
        cinemachinePerlin.m_AmplitudeGain = 0f;
    }
}
