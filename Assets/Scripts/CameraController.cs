using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 center;

    private void Update()
    {
        Vector2 newPosition = Vector2.Lerp(transform.position, center, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(center, 0.2f);
    }

    public void UpdateCenter(Vector2 a, Vector2 b)
    {
        center = (a + b) / 2f;
    }
}
