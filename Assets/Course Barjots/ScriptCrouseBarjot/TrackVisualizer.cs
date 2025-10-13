using UnityEngine;

public class TrackVisualizer : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public Color trackColor = Color.gray;

    void OnDrawGizmos()
    {
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = trackColor;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
            
            Gizmos.DrawSphere(startPoint.position, 0.2f);
            Gizmos.DrawSphere(endPoint.position, 0.2f);
        }
    }
}