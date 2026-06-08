using UnityEngine;

public class ObjectGestureController : MonoBehaviour
{
    private float previousPinchDistance;
    private Vector2 previousT0Pos;
    private Vector2 previousT1Pos;
    private bool isDragging;
    private Camera arCamera;

    private const float MinScale = 0.05f;
    private const float MaxScale = 3f;
    private const float DragSpeed = 0.003f;
    private const float MaxRotationPerFrame = 45f;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1)
            HandleDrag();
        else if (Input.touchCount == 2)
            HandlePinchAndRotate();
        else
            isDragging = false;
    }

    void HandleDrag()
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            isDragging = Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform;
        }

        if (!isDragging) return;

        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.deltaPosition;
            // Clamp delta to prevent overflow from extreme values
            delta = Vector2.ClampMagnitude(delta, 100f);
            transform.position += new Vector3(delta.x, 0, delta.y) * DragSpeed;
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            isDragging = false;
    }

    void HandlePinchAndRotate()
    {
        isDragging = false;
        Touch t0 = Input.GetTouch(0);
        Touch t1 = Input.GetTouch(1);

        float currentDist = Vector2.Distance(t0.position, t1.position);

        if (t1.phase == TouchPhase.Began)
        {
            previousPinchDistance = currentDist;
            previousT0Pos = t0.position;
            previousT1Pos = t1.position;
            return;
        }

        // Scale — safe ratio clamped per frame
        if (previousPinchDistance > 0.001f)
        {
            float ratio = Mathf.Clamp(currentDist / previousPinchDistance, 0.8f, 1.2f);
            float newScale = Mathf.Clamp(transform.localScale.x * ratio, MinScale, MaxScale);
            transform.localScale = Vector3.one * newScale;
        }
        previousPinchDistance = currentDist;

        // Rotate — compute angle from previous positions, no accumulation
        float prevAngle = Mathf.Atan2(
            previousT1Pos.y - previousT0Pos.y,
            previousT1Pos.x - previousT0Pos.x) * Mathf.Rad2Deg;

        float currAngle = Mathf.Atan2(
            t1.position.y - t0.position.y,
            t1.position.x - t0.position.x) * Mathf.Rad2Deg;

        float rotDelta = Mathf.Clamp(Mathf.DeltaAngle(prevAngle, currAngle), -MaxRotationPerFrame, MaxRotationPerFrame);
        transform.Rotate(Vector3.up, -rotDelta, Space.World);

        float maxCoord = Mathf.Max(Screen.width, Screen.height);
        previousT0Pos = Vector2.ClampMagnitude(t0.position, maxCoord);
        previousT1Pos = Vector2.ClampMagnitude(t1.position, maxCoord);
    }
}
