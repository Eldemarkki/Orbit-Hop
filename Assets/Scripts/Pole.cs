using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] private SpriteRenderer graphics;
    [SerializeField] private Transform fill;
    [SerializeField] private float disappearTime;

    private bool disappearing;
    private float timeLeft;

    void Update()
    {
        if (disappearing)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Vector3 scale = fill.localScale;
                scale.y = Mathf.Lerp(0, graphics.transform.localScale.y, (disappearTime - timeLeft) / disappearTime);
                fill.localScale = scale;
            }
        }
    }

    public void SetColor(Color color)
    {
        graphics.material.SetColor("_Emission", color);
    }

    public void StartDisappearing()
    {
        disappearing = true;
        timeLeft = disappearTime;
    }
}
