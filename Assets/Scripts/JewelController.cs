﻿using UnityEngine;
using System.Collections;

public class JewelController : MonoBehaviour {

	public GameObject Controller;
	public GameObject highlight;
	public GUIText scoreText;
	public bool isSelected;
	public ParticleSystem explodePS;

	public float xPos;
	private int x, y, dx, dy;  // varbiables to check adjacency
	private int boardWidth = 8;

	private bool isLineStrike;
	private bool isExploding;

	public Texture explosionTexture;
	public Texture lineStrikeTexture;

	void Start()
	{
		isLineStrike = false;
		isExploding = false;
		Controller = GameObject.Find ("GameController");
		isSelected = false;
		xPos = gameObject.transform.position.x;
	}

	void Update()
	{
		Vector2 pos = new Vector2 (xPos, gameObject.transform.position.y);
		gameObject.transform.position = pos;
		if(rigidbody2D.transform.position.y >9.5f)
		{
			Destroy(gameObject);
		}
	}

	// removes current jewel from the game and calls respawn at the same column
	public void RemoveJewel()
	{
		if(isLineStrike)
		{
			LineStrike();

		}
		else if(isExploding)
		{
			Explode();
		}
		else
		{
			// Respawn Jewel at column
			int col = (int)transform.position.x;

			if(gameObject != null)
			{
				audio.Play ();
				Destroy (gameObject, audio.clip.length);
				Controller.GetComponent<SpawnPieces> ().SpawnJewelAtColumn (col, Random.Range(0, 6));
				GameObject.FindWithTag ("GameController").GetComponent<GUIController>().score += 10;
			}
		}
	}

	// compares 2 game objects and returns true if they are in adjacent squares, false otherwise
	bool isAdjacent(GameObject second, GameObject selected)
	{
		if(second != null && selected != null)
		{
			int x, y, dx, dy;
			x = (int)selected.transform.position.x;
			y = (int)selected.transform.position.y;
			dx = (int)second.transform.position.x;
			dy = (int)second.transform.position.y;

			// Check if horizontal adjacent
			if(((dx == x+1) || (dx == x-1)) && (dy == y))
			{
				return true;
			}
			// Check if vertical adjacent
			else if(((dy == y+1) || (dy == y-1)) && (dx == x))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	public void MakeLineStrikeJewel()
	{
		isLineStrike = true;
		renderer.material.mainTexture = lineStrikeTexture;
	}

	public void MakeExplodingJewel()
	{
		isExploding = true;
		renderer.material.mainTexture = explosionTexture;
	}

	public void LineStrike()
	{
		isLineStrike = false;
		int row = (int)gameObject.transform.position.x;
		int column = (int)gameObject.transform.position.y;
		
		for(int i=0; i<boardWidth; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				if(i == row || j == column)
				{
					GameObject.FindWithTag ("GameController").GetComponent<FindMatches>().RemoveJewelAtCoords(i, j);
				}
			}
		}

	}

	public void Explode()
	{
		isExploding = false;
		int column = (int)gameObject.transform.position.x;
		int row = (int)gameObject.transform.position.y;
	
		for(int i=0; i<boardWidth; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				if(i>=column-1 && i<=column+1 && j>=row-1 && j<=row+1)
				{
					//GameObject temp = GetComponent<SpawnPieces>().squares[j][i];
					GameObject.FindWithTag("GameController").GetComponent<FindMatches>().RemoveJewelAtCoords(i, j);
				}
			}

		}

	}

	// swaps the location of 2 game objects
	void swap(GameObject obj1, GameObject obj2)
	{
		Vector2 temp = new Vector2 (obj1.transform.position.x, obj1.transform.position.y);
		obj1.transform.position = new Vector2 (obj2.transform.position.x, obj2.transform.position.y);
		obj2.transform.position = temp;
		obj1.gameObject.GetComponent<JewelController> ().xPos = obj1.transform.position.x;
		obj2.gameObject.GetComponent<JewelController> ().xPos = obj2.transform.position.x;
	}

	// start drag gesture, set as currently selected
	void OnMouseDown()
	{
		if(Controller.GetComponent<GameController>().isEnabled)
		{
			x = (int)gameObject.transform.position.x;
			y = (int)gameObject.transform.position.y;

			// Nothing is currently selected, set current jewel as selected
			if(Controller.GetComponent<GameController>().selected == null)
			{
				Controller.GetComponent<GameController>().selected = gameObject;
			}

			// Something is currently selected, but not this jewel
			// Check if jewel is neighbour to currently selected and perform swap, otherwise become selected
			else if(!isSelected)
			{
				if(isAdjacent(gameObject, Controller.GetComponent<GameController>().selected))
			    {
					int x2 = (int)Controller.GetComponent<GameController>().selected.transform.position.x;
					int y2 = (int)Controller.GetComponent<GameController>().selected.transform.position.y;

					if(Controller.GetComponent<MatchingAI>().VerifySwap(x, y, x2, y2))
					{
						swap(gameObject, Controller.GetComponent<GameController>().selected);
					}
					Controller.GetComponent<GameController>().selected = null;
				}
				else
				{
					Controller.GetComponent<GameController>().selected.GetComponent<JewelController> ().isSelected = false;
					Controller.GetComponent<GameController>().selected = gameObject;
				}
			}

			// Clicked jewel must be currently selected.
			// Set currently selected to null and deselect current jewel
			else
			{
				Controller.GetComponent<GameController>().selected = null;
			}

			isSelected = !isSelected;
			GameObject highlight = GameObject.Find ("Highlight");
			if(Controller.GetComponent<GameController>().selected != null)
			{
				highlight.transform.position = new Vector2(transform.position.x, transform.position.y);
			}
			else
			{
				highlight.transform.position = new Vector2(-.5f, -.5f);
			}
		}
	}

	// Finish drag gesture, swap accordingly
	void OnMouseUp()
	{
		if(Controller.GetComponent<GameController>().isEnabled)
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			dx = (int)pos.x;
			dy = (int)pos.y;

			if(dx < boardWidth && dy < boardWidth)
			{
				GameObject homeSquare = Controller.GetComponent<SpawnPieces> ().squares[x][y].GetComponent<SquareController>().jewel;
				GameObject newSquare = Controller.GetComponent<SpawnPieces> ().squares[dx][dy].GetComponent<SquareController> ().jewel;


				if(isAdjacent (newSquare, homeSquare))
				{
					if(Controller.GetComponent<MatchingAI>().VerifySwap(x, y, dx, dy))
					{
						swap (homeSquare, newSquare);
					}

					Controller.GetComponent<GameController>().selected = null;
					isSelected = !isSelected;
					GameObject highlight = GameObject.Find ("Highlight");
					highlight.transform.position = new Vector2(-.5f, -.5f);
				}
			}
		}
	}
}
