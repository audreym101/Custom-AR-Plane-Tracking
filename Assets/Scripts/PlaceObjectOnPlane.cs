using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    public ColorSelector colorSelector;
    public PlacementIndicator placementIndicator;
    public ToggleSpawnedObject toggleController;

    public bool HasSpawned { get; private set; }

    private ARRaycastManager raycastManager;
    private PlaneVisibilityController planeVisibility;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        planeVisibility = FindObjectOfType<PlaneVisibilityController>();
        Debug.Log("PlaceObjectOnPlane START — objectToPlace: " + (objectToPlace != null ? objectToPlace.name : "NULL"));
        Debug.Log("PlaceObjectOnPlane START — raycastManager: " + (raycastManager != null ? "FOUND" : "NULL"));
    }

    void Update()
    {
        if (HasSpawned) return;
        if (Input.touchCount != 1) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        Debug.Log("Touch detected at: " + touch.position);

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            Debug.Log("Touch blocked by UI");
            return;
        }

        if (raycastManager == null)
        {
            Debug.Log("raycastManager is NULL");
            return;
        }

        bool hitPlane = raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon);
        Debug.Log("Raycast hit plane: " + hitPlane);

        if (!hitPlane) return;

        HasSpawned = true;
        enabled = false;
        Vector3 spawnPos = hits[0].pose.position + new Vector3(0, 0.15f, 0);
        Debug.Log("Spawning at: " + spawnPos);

        GameObject spawned = Instantiate(objectToPlace, spawnPos, Quaternion.identity);
        spawned.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        spawned.AddComponent<ObjectGestureController>();

        Debug.Log("Spawned: " + spawned.name + " at " + spawned.transform.position);

        if (colorSelector != null)
            colorSelector.SetTarget(spawned);

        if (toggleController != null)
            toggleController.SetTarget(spawned);

        if (placementIndicator != null)
            placementIndicator.Hide();

        if (planeVisibility != null)
            planeVisibility.HideAllPlanes();
    }
}
