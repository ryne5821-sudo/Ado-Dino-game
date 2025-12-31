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
    [Header("Speed")] public float startingSpeed = 0.5f;
    public float speedIncreasePerSecond = 0.1f;
    public float currentScoreIncreaseSpeed = 2f;
    [Header("Text")] public TextMeshProUGUI scoreText;
    
    [Header("Obstacle Spawn")] 
    public float timeDelayBetweenObstacle = 1f;
    [Space]
    public GameObject[] allGroundObstacles;
    public GameObject[] allFlyingObstacles;
    [Space]
    public Transform groundObstacleSpawnPoint;
    public Transform flyingObstacleSpawnPoint;
    
    private List<GameObject> allCurrentObstacles = new List<GameObject>();
    
    private float currentSpeed;
    
    private int highScore = 0;
    private float currentScore = 0;
    
    private float timeUntilNextObstacle = 1f;
    
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
            timeUntilNextObstacle -= Time.deltaTime;

            if (timeUntilNextObstacle <= 0f)
            {
                timeUntilNextObstacle = timeDelayBetweenObstacle;
                
                //Spawn new obstacle
                if(currentScore >= 50)
                {
                    //random spawn ground or air
                    if (UnityEngine.Random.value > 0.8F) //20 % Chance
                    {
                        //spawn air
                        GameObject newObstacle = Instantiate(allFlyingObstacles[UnityEngine.Random.Range(0, allFlyingObstacles.Length)], flyingObstacleSpawnPoint.position, Quaternion.identity);
                        allCurrentObstacles.Add(newObstacle);
                    }
                }
                else
                {
                    //spawn ground
                    GameObject newObstacle = Instantiate(allGroundObstacles[UnityEngine.Random.Range(0, allGroundObstacles.Length)], groundObstacleSpawnPoint.position, Quaternion.identity);
                    allCurrentObstacles.Add(newObstacle);
                }
            }
            else
            {
                //spawn ground
                GameObject newObstacle = Instantiate(allGroundObstacles[UnityEngine.Random.Range(0, allGroundObstacles.Length)], groundObstacleSpawnPoint.position, Quaternion.identity);
                allCurrentObstacles.Add(newObstacle);
            }

            foreach (GameObject _obstacle in allCurrentObstacles)
            {
                _obstacle.transform.Translate(new Vector3(-currentSpeed * Time.deltaTime, 0, 0));
            }
            
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
