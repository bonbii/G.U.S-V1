// CameraRail.cs
using UnityEngine;
using Unity.Cinemachine;

public class CameraRail : MonoBehaviour
{
    [Header("Rail Settings")]
    public Transform railStart;
    public Transform railEnd;
    public float cameraDistance = 10f;
    public float height = 5f;
    public float lookDownAngle = 15f;
    
    [Header("Smoothing")]
    public float followSpeed = 5f;
    public float lookaheadDistance = 2f;
    
    private Transform player;
    private Vector3 targetPosition;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void LateUpdate()
    {
        if (player == null) return;
        
        // Project player position onto rail
        Vector3 playerPos = player.position;
        float t = Mathf.InverseLerp(railStart.position.x, railEnd.position.x, playerPos.x);
        t = Mathf.Clamp01(t);
        
        // Calculate camera position along rail
        Vector3 railPos = Vector3.Lerp(railStart.position, railEnd.position, t);
        targetPosition = railPos + new Vector3(0, height, -cameraDistance);
        
        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        
        // Look at player with slight offset
        Vector3 lookTarget = playerPos + Vector3.up * 1.5f + Vector3.right * lookaheadDistance;
        transform.rotation = Quaternion.Euler(lookDownAngle, 0, 0);
    }
    
    void OnDrawGizmos()
    {
        if (railStart == null || railEnd == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(railStart.position, railEnd.position);
        Gizmos.DrawWireSphere(railStart.position, 0.5f);
        Gizmos.DrawWireSphere(railEnd.position, 0.5f);
    }
}