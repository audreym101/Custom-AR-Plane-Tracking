using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>
/// Attach to any GameObject in the AR scene.
/// Hides plane visuals immediately when detected — shows only the custom plane prefab mesh briefly.
/// After object is placed, hides all planes completely.
/// </summary>
public class PlaneVisibilityController : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private bool hideAll = false;

    void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager != null)
            planeManager.trackablesChanged.AddListener(OnPlanesChanged);
    }

    void OnDestroy()
    {
        if (planeManager != null)
            planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
    }

    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach (var plane in args.added)
            plane.gameObject.SetActive(false);
    }

    public void HideAllPlanes()
    {
        if (planeManager == null) return;
        planeManager.trackablesChanged.RemoveListener(OnPlanesChanged);
        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(false);
    }
}
