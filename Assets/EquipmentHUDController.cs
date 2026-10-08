using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentHUDController : MonoBehaviour
{
    [Header("UI Elements")]
    public Button explodeButton;
    public Button torchButton;
    public TextMeshProUGUI statusText;

    private Animator activeAnimator;
    private Light inspectionLight;
    private bool isExploded = false;
    private bool isTorchOn = false;

    void Start()
    {
        // Add click listeners to the buttons
        if (explodeButton != null) explodeButton.onClick.AddListener(ToggleExplodedView);
        if (torchButton != null) torchButton.onClick.AddListener(ToggleInspectionTorch);
    }

    // The Image Tracker script calls this when it spawns the prefab
    public void RegisterActiveEquipment(GameObject equipment)
    {
        activeAnimator = equipment.GetComponent<Animator>();
        inspectionLight = equipment.GetComponentInChildren<Light>();
        if (statusText != null) statusText.text = "Status: Equipment Connected";
    }

    void ToggleExplodedView()
    {
        if (activeAnimator == null) return;

        isExploded = !isExploded;
        activeAnimator.SetBool("isExploded", isExploded);
        if (statusText != null) statusText.text = isExploded ? "Mode: Exploded View" : "Mode: Assembled View";
    }

    void ToggleInspectionTorch()
    {
        if (inspectionLight == null) return;

        isTorchOn = !isTorchOn;
        // 800 intensity for a bright inspection light, 0 to turn it off
        inspectionLight.intensity = isTorchOn ? 800f : 0f;
    }
}