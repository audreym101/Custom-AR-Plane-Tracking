using UnityEngine;
using TMPro;

public class ARHudInstructions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hudText;
    [SerializeField] private PlacementIndicator placementIndicator;
    [SerializeField] private PlaceObjectOnPlane placer;

    void Update()
    {
        if (hudText == null) return;

        if (placer.HasSpawned)
            hudText.text = "Drag to move • Pinch to scale • Twist to rotate";
        else if (placementIndicator.IsActive)
            hudText.text = "Tap to place object";
        else
            hudText.text = "Point camera at a flat surface...";
    }
}
