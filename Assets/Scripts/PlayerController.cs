using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameManager gameManager;

    private Rigidbody2D rb;
    private Vector2 rotationPoint;
    private bool isRotating;
    private float angle;
    private float distance;
    private bool canRotate = true;

    private bool hasTouchedFirstPole = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canRotate)
        {
            rotationPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            distance = Vector2.Distance(rotationPoint, transform.position);
            angle = Mathf.Atan2(rotationPoint.x - transform.position.x, rotationPoint.y - transform.position.y) * Mathf.Rad2Deg + 90;

            isRotating = true;
            canRotate = false;
            rb.gravityScale = 0;

            if (!hasTouchedFirstPole)
            {
                gameManager.FirstPole.StartDisappearing();
                hasTouchedFirstPole = true;
            }
        }

        if (isRotating)
        {
            angle += rotationSpeed * Time.deltaTime;
            RotatePosition(rotationPoint, -angle);
        }

        if (Input.GetMouseButtonUp(0) && isRotating)
        {
            Vector2 offset = rotationPoint - (Vector2)transform.position;
            Vector2 velocity = new Vector2(-offset.y, offset.x);

            rb.velocity = velocity;
            rb.gravityScale = 1;
            isRotating = false;
        }

        if (transform.position.y < -20 && !isRotating)
        {
            gameManager.OnPlayerDied();
        }
    }

    private void RotatePosition(Vector2 rotationPoint, float angle)
    {
        Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * distance, Mathf.Sin(angle * Mathf.Deg2Rad) * distance);
        rb.MovePosition(rotationPoint + offset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PoleTop"))
        {
            Pole pole = other.transform.parent.GetComponent<Pole>();
            bool wasGoalPole = gameManager.OnPoleReached(pole);
            if (wasGoalPole)
            {
                canRotate = true;
            }
            else
            {
                if (gameManager.IsFirstPole(pole) && !hasTouchedFirstPole)
                {
                    hasTouchedFirstPole = true;
                }
                else
                {
                    gameManager.OnPlayerDied();
                }
            }
        }
    }
}