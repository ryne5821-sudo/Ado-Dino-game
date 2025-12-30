using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private MeshRenderer floorMeshRenderer;
    
    [HideInInspector] public bool gameStarted = false;
    [Header("Speed")]
    public float startingSpeed = 0.5f;
    public float speedIncreasePerSecond = 0.1f;
    public float currentScoreIncreaseSpeed = 2f;
    [Header("Text")]
    public TextMeshProUGUI scoreText;
    
    private int highScore = 0;
    private float currentScore = 0;
    
    
    private float currentSpeed;
    
    private void Awake()
    {
        Instance = this;

        currentSpeed = startingSpeed;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
        }
        
        UpdateScoreUI();
    }

    private void Update()
    {
        if (gameStarted)
        {
            currentSpeed += speedIncreasePerSecond * Time.deltaTime;
            
            floorMeshRenderer.material.mainTextureOffset += new Vector2(currentSpeed * Time.deltaTime, 0);

            currentScore += Time.deltaTime * currentSpeed * currentScoreIncreaseSpeed;

            if (Mathf.RoundToInt(currentScore) > highScore)
            {
                highScore = Mathf.RoundToInt(currentScore);
            }
            
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        scoreText.SetText($"Hi {highScore.ToString("D5")}  {Mathf.RoundToInt(currentScore).ToString("D5")}");
    }
}
