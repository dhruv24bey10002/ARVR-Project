using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTargetHUDManager : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject equipmentPrefab;

    private readonly Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // When an image target is first detected
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateImageTarget(trackedImage);
        }

        // When the tracked target moves or updates orientation
        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateImageTarget(trackedImage);
        }

        // When the camera loses sight of the target
        foreach (var trackedImage in eventArgs.removed)
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }

    void UpdateImageTarget(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (!spawnedPrefabs.ContainsKey(imageName))
        {
            // Instantiate the 3D digital twin at the tracked image pose
            GameObject newObject = Instantiate(equipmentPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            newObject.transform.SetParent(trackedImage.transform);
            spawnedPrefabs.Add(imageName, newObject);
            FindObjectOfType<EquipmentHUDController>().RegisterActiveEquipment(newObject);
        }

        GameObject targetObject = spawnedPrefabs[imageName];

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            targetObject.SetActive(true);
            targetObject.transform.position = trackedImage.transform.position;
            targetObject.transform.rotation = trackedImage.transform.rotation;
        }
        else
        {
            targetObject.SetActive(false);
        }
    }
}