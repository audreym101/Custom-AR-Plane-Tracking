using UnityEngine;
using TMPro;

public class ToggleSpawnedObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonLabel;

    private GameObject spawnedObject;
    private bool isVisible = true;

    public void SetTarget(GameObject obj)
    {
        spawnedObject = obj;
        isVisible = true;
        UpdateLabel();
    }

    public void Toggle()
    {
        if (spawnedObject == null) return;
        isVisible = !isVisible;
        spawnedObject.SetActive(isVisible);
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (buttonLabel == null) return;
        buttonLabel.text = isVisible ? "Remove Object" : "Add Object";
    }
}
