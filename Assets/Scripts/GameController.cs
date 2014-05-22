using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	// Jewel Prefab Objects
	public GameObject jewel_1;
	public GameObject jewel_2;
	public GameObject jewel_3;
	public GameObject jewel_4;
	public GameObject jewel_5;
	public GameObject jewel_6;
	public GameObject jewel_7;
	public GameObject square;
	public GameObject selected;
	public GameObject timeBar;
	public GameObject hintHighlight;
	public GameObject avatarSpawn;
	
	public GUIText scoreText;
	public GUIText timerText;
	public GUIText gameOverText;
	public GUIText restartText;
	public GUIText noMatchesText;

	public GameObject[][] squares;
	
	public int boardWidth;
	public int score;
	public int timeLimit;
	public int hintRate;

	public float nextHint;

	public bool isEnabled;

	private float spawnRate;
	private float nextSpawn;
	private float timeRemaining;

	private GameObject[] jewels;
	private Stack<int> spawnStack;



	// Called at the start of game; initializes board and variables
	void Start () 
	{
		// Initializations
		timeRemaining = timeLimit;
		selected = null;
		isEnabled = true;
		spawnRate = .1f;

		spawnStack = new Stack<int> ();

		gameOverText.enabled = false;
		noMatchesText.enabled = false;
		restartText.enabled = false;
		scoreText.text = "Score: " + score;
		timerText.text = "Time Remaining: " + (int)timeRemaining;


		// Initialize Board
		jewels = new GameObject[boardWidth];
		squares = new GameObject[boardWidth][];

		for(int i=0; i<boardWidth; i++)
		{
			squares[i] = new GameObject[boardWidth];
			for(int j=0; j<boardWidth; j++)
			{
				squares[i][j] = Instantiate (square, new Vector2(i+.5f, j+.5f), Quaternion.identity) as GameObject;
			}
		}

		jewels [0] = jewel_1;
		jewels [1] = jewel_2;
		jewels [2] = jewel_3;
		jewels [3] = jewel_4;
		jewels [4] = jewel_5;
		jewels [5] = jewel_6;
		jewels [6] = jewel_7;
 
		SpawnBoard ();
	}

	// Create board and spawn initial pieces
	void SpawnBoard()
	{
		for(int i=0; i<boardWidth; i++)
		{
			SpawnRow ();
		}
	}

	// Spawn a single jewel at col
	public void SpawnJewelAtColumn(int col)
	{
		spawnStack.Push (col);
	}
	
	// Spawns an entire row of pieces
	void SpawnRow()
	{
		for(int i=0; i<boardWidth; i++)
		{
			SpawnJewelAtColumn(i);
		}
	}
	
	// Called every frame
	void Update () 
	{
		// If spawn stack is not empty, spawn new pieces...
		if (spawnStack.Count > 0) {
			if (nextSpawn <= Time.time) {
					nextSpawn = Time.time + spawnRate;
					SpawnFromStack ();
			}
		}
		// ... else start looking for matches
		else if (Time.time > 8.0f)
		{
			if(GetComponent<MatchingAI>().DoesMatchExist())
			{
				spawnRate = 0.5f;
				GetComponent<FindMatches>().GetMatches ();

				if(nextHint <= Time.time && Time.time > 10.0f)
				{
					nextHint = Time.time + hintRate;
					Vector2 hint = GetComponent<MatchingAI>().CheckForPotentialMatches();
					if(hint.x != -1 && hint.y != -1)
					{
						//DisplayHint (hint);
					}
				}
			}
			else
			{
				//GameOver ();
			//	noMatchesText.enabled = true;
			}
		}

		scoreText.text = "Score: " + score.ToString ("n0");

		UpdateTimer ();
	}


	void DisplayHint(Vector2 hint)
	{
		Vector2 coords = new Vector2 (hint.x + .5f, hint.y + .5f);
		GameObject square = Instantiate (hintHighlight, coords, Quaternion.identity) as GameObject;
		square.renderer.material.color = Color.white;
		Destroy (square, 0.5f);
	}
	// Updates timer information and sets the appropriate gui elements
	void UpdateTimer()
	{
		timeRemaining = (float)timeLimit - Time.timeSinceLevelLoad;

		if(timeRemaining <= 0)
		{
			GameOver();

		}
		float timePercentage = (float)timeRemaining / (float)timeLimit;

		if(timePercentage < .5 && timePercentage > .2)
		{
			timeBar.renderer.material.color = Color.yellow;
		}
		else if(timePercentage < .2)
		{
			timeBar.renderer.material.color = Color.red;
		}

		timeBar.transform.localScale = new Vector3 (timeBar.transform.localScale.x, (5.5f * timePercentage), 1);

		int minutes = (int)timeRemaining / 60;
		int seconds = (int)timeRemaining % 60;

		if(seconds < 10)
		{
			timerText.text = minutes + ":0" + seconds;
		}
		else 
		{
			timerText.text = minutes + ":" + seconds;
		}
	}
	// pops element off of spawn stack and adds it to game
	void SpawnFromStack()
	{
		float col = (float)spawnStack.Pop () + 0.5f;
		Vector3 spawnLoc = new Vector3(col, 9.0f, -0.5f);
		Instantiate (jewels[Random.Range(0, 7)], spawnLoc, Quaternion.identity);
	}

	// Disable gameplay and display game over gui elements
	void GameOver()
	{ 
		gameOverText.enabled = true;
		restartText.enabled = true;
		isEnabled = false;

		timeRemaining = 0;
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			Application.LoadLevel (1);
		}
	}
}
