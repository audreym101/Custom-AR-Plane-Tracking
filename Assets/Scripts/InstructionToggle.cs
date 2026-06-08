using UnityEngine;

public class InstructionToggle : MonoBehaviour
{
    public GameObject instructionPanel;

    public void ToggleInstructions(bool value)
    {
        Debug.Log("Toggle changed: " + value);
        instructionPanel.SetActive(value);
    }
}
