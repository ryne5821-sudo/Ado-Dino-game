using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[SerializeField] private MeshRenderer floorMeshRenderer;

	[HideInInspector] public bool gameStarted = false;
	[HideInInspector] public bool gameEnded = false;

	[Header("Speed")]
	public float startingSpeed = 0.5f;
	public float speedIncreasePerSecond = 0.1f;
	public float currentScoreIncreaseSpeed = 2f;

	[Header("UI")] public TextMeshProUGUI scoreText;
	public GameObject gameEndScreen;

	[Header("Obstacle Spawn")] public float minTimeDelayBetweenObstacle = 1f;
	public float maxTimeDelayBetweenObstacle = 5;
	public float obstacleSpeedMultiplier = 3f;
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
	//private bool debugging = false;

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

	public void ShowGameEndScreen()
	{
		gameEndScreen.SetActive(true);
	}

	private void Update()
	{
		//if (!debugging)
		//{
		//	debugging = true;
		//	System.Diagnostics.Debugger.Break();
		//}
		

		//Debug.Log("In update!");
		if (gameStarted && !gameEnded)
		{
			timeUntilNextObstacle -= Time.deltaTime * currentSpeed;
			Debug.Log(string.Format("timeUntilNextObstacle before: {0}", timeUntilNextObstacle));
			Debug.Log(string.Format("Delta time is: {0}", Time.deltaTime));
			Debug.Log(string.Format("timeUntilNextObstacle after: {0}", timeUntilNextObstacle));

			if (timeUntilNextObstacle <= 0.0)
			{
				timeUntilNextObstacle = UnityEngine.Random.Range(minTimeDelayBetweenObstacle, maxTimeDelayBetweenObstacle);

				//Spawn new obstacle
				if(currentScore >= 50)
				{
					//random spawn ground or air
					if (UnityEngine.Random.value > 0.5F) //50 % Chance
					{
						//spawn air
						GameObject newObstacle = Instantiate(allFlyingObstacles[UnityEngine.Random.Range(0, allFlyingObstacles.Length)], flyingObstacleSpawnPoint.position, Quaternion.identity);
						allCurrentObstacles.Add(newObstacle);
					}
					else
					{
						//spawn cactus
						Debug.Log("Spawning a cactus!!");
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
			}

			foreach (GameObject _obstacle in allCurrentObstacles)
			{
				_obstacle.transform.Translate(new Vector3(-currentSpeed * Time.deltaTime * obstacleSpeedMultiplier, 0, 0));
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

	public void resetScene()
	{
		Debug.Log("Pressed Reset");
		SceneManager.LoadScene(0);
	}

	private void UpdateScoreUI()
	{
		scoreText.SetText($"Hi {highScore.ToString("D5")}  {Mathf.RoundToInt(currentScore).ToString("D5")}");
	}
}