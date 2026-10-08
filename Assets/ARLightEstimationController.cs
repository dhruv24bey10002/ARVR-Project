using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARLightEstimationController : MonoBehaviour
{
    public ARCameraManager arCameraManager;
    private Light directionalLight;

    void Awake()
    {
        directionalLight = GetComponent<Light>();
    }

    void OnEnable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        if (arCameraManager != null)
            arCameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        var lightEstimation = args.lightEstimation;

        if (lightEstimation.averageBrightness.HasValue)
        {
            directionalLight.intensity = lightEstimation.averageBrightness.Value;
        }

        if (lightEstimation.averageColorTemperature.HasValue)
        {
            directionalLight.useColorTemperature = true;
            directionalLight.colorTemperature = lightEstimation.averageColorTemperature.Value;
        }

        if (lightEstimation.colorCorrection.HasValue)
        {
            directionalLight.color = lightEstimation.colorCorrection.Value;
        }
    }
}