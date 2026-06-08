using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicator : MonoBehaviour
{
    public GameObject indicatorVisual;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool IsActive { get; private set; }
    public Pose CurrentPose { get; private set; }

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeManager = FindObjectOfType<ARPlaneManager>();
        indicatorVisual.SetActive(false);
    }

    void Update()
    {
        if (raycastManager == null) return;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        bool hit = raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

        IsActive = hit;
        indicatorVisual.SetActive(hit);

        if (hit)
        {
            CurrentPose = hits[0].pose;
            // Keep indicator flat on horizontal plane
            transform.position = CurrentPose.position;
            transform.rotation = Quaternion.identity;
        }
    }

    public void Hide()
    {
        IsActive = false;
        indicatorVisual.SetActive(false);
        enabled = false;
    }

    public void SetAllPlanesActive(bool active)
    {
        if (planeManager == null) return;
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(active);
    }
}
