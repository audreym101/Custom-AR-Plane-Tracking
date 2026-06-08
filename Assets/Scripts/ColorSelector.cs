using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSelector : MonoBehaviour
{
    [SerializeField] private Button[] colorButtons;
    [SerializeField] private TextMeshProUGUI statusText; // optional — assign in Inspector

    private GameObject targetObject;

    void Start()
    {
        SetButtonsInteractable(false);
        if (statusText != null)
            statusText.text = "Place an object to enable colors";
    }

    // Called by PlaceObjectOnPlane after spawning
    public void SetTarget(GameObject obj)
    {
        targetObject = obj;
        SetButtonsInteractable(true);
        if (statusText != null)
            statusText.text = "Select a color:";
    }

    public void SetColor(Color color)
    {
        if (targetObject == null) return;
        foreach (var r in targetObject.GetComponentsInChildren<Renderer>())
            r.material.color = color;
    }

    public void SetRed()    => SetColor(Color.red);
    public void SetBlue()   => SetColor(Color.blue);
    public void SetGreen()  => SetColor(Color.green);
    public void SetWhite()  => SetColor(Color.white);
    public void SetYellow() => SetColor(Color.yellow);

    private void SetButtonsInteractable(bool state)
    {
        foreach (var btn in colorButtons)
            if (btn != null) btn.interactable = state;
    }
}
