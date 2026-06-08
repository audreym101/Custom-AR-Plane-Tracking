using UnityEngine;
using TMPro;

public class ScreenDebug : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    private string log = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        log = message + "\n" + log;
        // Keep only last 6 lines
        string[] lines = log.Split('\n');
        if (lines.Length > 6)
            log = string.Join("\n", lines, 0, 6);
        if (debugText != null)
            debugText.text = log;
    }
}
