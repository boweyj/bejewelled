using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	// Jewel Prefab Objects

	public GameObject selected;
	public GameObject hintHighlight;
	public GameObject avatarSpawn;


	public bool isEnabled;

	public static int roundNum = 1;

	private bool roundFourPaused;

	//private Leaderboard leaderboard;


	// Called at the start of game; initializes board and variables
	void Start () 
	{
		// Initializations
		selected = null;
		isEnabled = true;
		roundFourPaused = false;
		Time.timeScale = 1;
		gameObject.AddComponent<Leaderboard> ();

		gameObject.GetComponent<SpawnPieces> ().SpawnBoard ();
	}

	// Called every frame
	void Update () 
	{
		// If there are no pieces to spawn, check for matches
		if(!gameObject.GetComponent<SpawnPieces> ().CheckSpawn ())
		{
			if (Time.time > 9.0f)
			{
				GetComponent<SpawnPieces>().spawnRate = 0.3f;
				gameObject.GetComponent<FindMatches>().CheckMatches ();
			}
		}
		GetComponent<GUIController>().UpdateGUI ();

		// Rounds 1-4 are training
		if(roundNum <= 4 && GetComponent<GUIController>().timeRemaining <= 0)
		{
			// If timer is down to 0, end round and spawn next one
			if(roundNum == 1 || roundNum == 3 || roundNum == 4)
			{
				GetComponent<GUIController>().EndRound();
			}
			else if(roundNum == 2)
			{
				StartCoroutine("ScheduleWait");
				if(Input.GetKeyDown (KeyCode.Space))
				{
					GetComponent<GUIController>().EndRound();
				}
			}
		}
		else if(roundNum <=13 && GetComponent<GUIController>().timeRemaining <= 0)
		{
			// normal gameplay
			if(roundNum % 3 == 0)
			{
				//Debug.Log ("Pairwise Comparison");
				// if gameplay is over, generate pairwise comparison
			}
			GetComponent<Leaderboard>().DisplayLeaderboard ();
			Time.timeScale = 0;
			if(Input.GetKeyDown(KeyCode.Space))
			{
				GetComponent<GUIController>().EndRound();
			}
			Debug.Log ("Generate Leaderboards");
			Debug.Log ("End Round" + GameController.roundNum);
			// generate leaderboards
			// increment roundNum and restart for next round
		}
		else if(roundNum == 4 && GetComponent<GUIController>().timeRemaining <= GetComponent<GUIController>().timeLimit/2)
		{
			if(!roundFourPaused)
			{
				roundFourPaused = true;
				isEnabled = false;
				StartCoroutine ("ScheduleWait");
				if(Input.GetKeyDown (KeyCode.Space))
				{
					Time.timeScale = 1;
					isEnabled = true;
				}
			}
		}
		else if(GetComponent<GUIController>().timeRemaining <= 0)
		{
			// end study
		}
	}


	public IEnumerator ScheduleWait()
	{
		yield return StartCoroutine(GetComponent<GUIController>().WaitToContinue());
	}


}
