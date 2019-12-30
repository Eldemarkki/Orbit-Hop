using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Pole> poles;
    [SerializeField] private float minimumPoleSpacing = 3f;
    [SerializeField] private float maximumPoleSpacing = 6f;
    [SerializeField] private GameObject polePrefab;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerController player;

    [SerializeField, ColorUsage(false, true)] private Color passedPoleColor;
    [SerializeField, ColorUsage(false, true)] private Color nextPoleColor;

    [SerializeField] private AudioClip scoreSoundEffect;
    [SerializeField] private AudioClip gameOverSoundEffect;
    [SerializeField] private AudioSource soundEffectSource;

    private Camera cam;

    private int score;

    private Pole currentPole;
    private Pole nextPole;

    public int Score { get => score; set => SetScore(value); }

    public Pole FirstPole { get; private set; }

    private void SetScore(int value)
    {
        score = value;
        scoreText.text = score.ToString();
    }

    private void Awake()
    {
        cam = Camera.main;

        int poleCount = Mathf.CeilToInt(cam.orthographicSize * cam.aspect / minimumPoleSpacing);
        poles = new List<Pole>();

        CreatePole();
        currentPole = poles[Score];
        currentPole.SetColor(passedPoleColor);
        currentPole.transform.position = new Vector2(0, currentPole.transform.position.y);

        FirstPole = currentPole;

        for (int i = 0; i < poleCount; i++)
        {
            CreatePole();
        }

        nextPole = poles[Score + 1];
        nextPole.SetColor(nextPoleColor);

        cameraController.UpdateCenter(currentPole.transform.position, nextPole.transform.position);
    }

    private void CreatePole()
    {
        float lastX = GetLastX();

        float newX = lastX + UnityEngine.Random.Range(minimumPoleSpacing, maximumPoleSpacing);
        float newY = -UnityEngine.Random.Range(cam.orthographicSize * 1.75f, cam.orthographicSize) * 0.5f;

        Pole newPole = Instantiate(polePrefab, new Vector2(newX, newY), Quaternion.identity).GetComponent<Pole>();
        newPole.name = "Pole_" + poles.Count;
        poles.Add(newPole);
    }

    public void OnPlayerDied()
    {
        soundEffectSource.PlayOneShot(gameOverSoundEffect);
        Restart();
    }

    private void Restart()
    {
        player.transform.position = Vector2.zero;
        Score = 0;
        foreach (var pole in poles)
        {
            if(pole) Destroy(pole.gameObject);
        }

        player.Restart();

        Awake();
    }

    private float GetLastX()
    {
        float lastX = 0;
        for (int i = poles.Count - 1; i >= 0; i++)
        {
            Pole pole = poles[i];
            if (pole != null)
            {
                lastX = pole.transform.position.x;
                break;
            }
        }

        return lastX;
    }

    public bool OnPoleReached(Pole pole)
    {
        pole.StartDisappearing();

        if (pole == nextPole)
        {
            Score++;

            currentPole = poles[Score];
            currentPole.SetColor(passedPoleColor);

            nextPole = poles[Score + 1];
            nextPole.SetColor(nextPoleColor);

            cameraController.UpdateCenter(currentPole.transform.position, nextPole.transform.position);

            soundEffectSource.PlayOneShot(scoreSoundEffect);

            CreatePole();

            return true;
        }

        return false;
    }

    public bool IsFirstPole(Pole pole)
    {
        return pole == poles[0];
    }
}
