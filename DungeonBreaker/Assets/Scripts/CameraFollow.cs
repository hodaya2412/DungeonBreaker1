using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;       // השחקנית
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] float fixedY = 2f;      // גובה קבוע
    [SerializeField] float depth = -10f;     // עומק מצלמה
    [SerializeField] Transform leftBound;    // גבול שמאל
    [SerializeField] Transform rightBound;   // גבול ימין

    private float halfCamWidth;

    void Start()
    {
        // מחשבים חצי רוחב של המצלמה לפי ה־Orthographic Size והיחס
        Camera cam = Camera.main;
        halfCamWidth = cam.orthographicSize * cam.aspect;
    }

    void LateUpdate()
    {
        

        // מיקום יעד לפי השחקנית
        float targetX = player.position.x;

        // מגבילים את X שלא יעבור את הגבולות
        targetX = Mathf.Clamp(
            targetX,
            leftBound.position.x + halfCamWidth,   // שמאל
            rightBound.position.x - halfCamWidth  // ימין
        );

        Vector3 desiredPosition = new Vector3(
            targetX,
            fixedY,
            depth
        );

        // תנועה חלקה
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}