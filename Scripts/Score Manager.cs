using UnityEngine;
using TMPro; // TextMeshPro kullanmak için

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI Reference")]
    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;//ScoreManager bileþeni instance a atandý
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // .text, TextMeshPro yazýsýný okumak veay deðiþtirmek için kullanýlýr.
        }
    }
}