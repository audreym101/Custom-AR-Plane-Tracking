using UnityEngine;
using TMPro;

public class ARHudInstructions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hudText;
    [SerializeField] private PlacementIndicator placementIndicator;
    [SerializeField] private PlaceObjectOnPlane placer;

    private string lastText;

    void Update()
    {
        if (hudText == null) return;

        string next;
        if (placer.HasSpawned)
            next = "Drag to move \u2022 Pinch to scale \u2022 Twist to rotate";
        else if (placementIndicator.IsActive)
            next = "Tap to place object";
        else
            next = "Point camera at a flat surface...";

        if (next == lastText) return;
        lastText = next;
        hudText.text = next;

        if (placer.HasSpawned)
            enabled = false;
    }
}
