using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;       
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] float fixedY = 2f;     
    [SerializeField] float depth = -10f;     
    [SerializeField] Transform leftBound;    
    [SerializeField] Transform rightBound;   

    private float halfCamWidth;

    void Start()
    {
        
        Camera cam = Camera.main;
        halfCamWidth = cam.orthographicSize * cam.aspect;
    }

    void LateUpdate()
    {
        

        
        float targetX = player.position.x;

        
        targetX = Mathf.Clamp(
            targetX,
            leftBound.position.x + halfCamWidth,   
            rightBound.position.x - halfCamWidth 
        );

        Vector3 desiredPosition = new Vector3(
            targetX,
            fixedY,
            depth
        );

        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}