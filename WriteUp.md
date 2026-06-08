# AR Plane Tracking – Project Write-Up
**Author:** Audrey

---

## Plane Type
This project detects **horizontal planes** (e.g., floors, tables, desks).

---

## Implementation Overview

### Welcome UI
The welcome screen uses a `SceneLoader` script to navigate into the AR scene. It includes:
- A **Start button** to launch the AR experience
- A **Toggle** (via `InstructionToggle.cs`) that shows/hides an instruction panel — this fulfills the "non-button interactive element" requirement

### Custom Plane Tracker
The default AR Foundation plane visualizer is replaced with a custom prefab (`CustomPlanePrefab`). The plane mesh uses a custom material (`PlaneMaterial`) with a texture that displays the student's full name. The tracker only appears when AR Foundation detects a horizontal plane.

### Tap-to-Place
`PlaceObjectOnPlane.cs` handles spawning. On the first valid single tap over a detected plane:
1. An AR raycast confirms a hit on `TrackableType.PlaneWithinPolygon`
2. The object is instantiated once at the hit pose
3. `ARPlaneManager` is disabled to stop detecting new planes
4. A boolean flag (`hasSpawned`) prevents any further spawns

UI taps are filtered using `EventSystem.IsPointerOverGameObject` to avoid accidental placement when pressing UI buttons.

### Color Selector UI
A panel with 5 color buttons (Red, Blue, Green, White, Yellow) is displayed in the AR scene. The buttons are disabled until an object is placed. Once placed, `ColorSelector.SetTarget()` is called, enabling the buttons and allowing real-time material color changes on the spawned object.

### Object Manipulation
`ObjectGestureController.cs` (added to the spawned object at runtime) handles:
- **Pinch to scale** — tracks distance between two touches; scale is clamped between 0.05× and 5× to prevent degenerate values
- **Two-finger twist to rotate** — computes angle between the two touch points each frame and applies the delta as a Y-axis world rotation

### Plane Tracking
AR Foundation's `ARPlaneManager` and `ARRaycastManager` handle plane detection and hit testing respectively. The detection mode is set to **horizontal planes only**, targeting surfaces like floors and tables. Plane detection is stopped after object placement for performance and to prevent the scene from being cluttered with plane visualizers.
