using UnityEngine;
using System.Collections;
using System;

public class MatchingAI : MonoBehaviour {


	private string[][] boardState;
	private int boardWidth;

	void Start()
	{
		boardWidth = GetComponent<GameController> ().boardWidth;
		boardState = new string[boardWidth][];
		for(int i=0; i<boardWidth; i++)
		{
			boardState[i] = new string[boardWidth];
		}
	}

	// Checks for potential matches and returns x,y coords for first one found
	public Vector2 CheckForPotentialMatches()
	{
		// copy current board state
		// check if there is a current match existing
		CopyBoardState ();

		for(int i=0; i<boardWidth; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				if(DoesMatchExistAtPoint(i, j))
				{
					Vector2 retVec = new Vector2(i, j);
					return retVec;
				}
			}
		}
		return new Vector2 (-1, -1);
	}

	// checks that swapped pieces result in a match.  returns true if match found, false otherwise
	public bool VerifySwap(int x, int y, int dx, int dy)
	{
		CopyBoardState ();

		// swap the two pieces on the boardstate
		string temp = boardState [x] [y];
		boardState [x] [y] = boardState [dx] [dy];
		boardState [dx] [dy] = temp;

		bool firstMatch = DoesMatchExistAtPoint (x, y);
		bool secondMatch = DoesMatchExistAtPoint (dx, dy);

		return (firstMatch || secondMatch);
	}

	// searches board looking for a match, returns true if match found, false otherwise
	public bool DoesMatchExist()
	{
		CopyBoardState ();
		// search board looking for match
		for(int i=0; i<boardWidth; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				if((i+1) < boardWidth && (i+2) < boardWidth)
				{
					if(boardState[i][j].Equals (boardState[i+1][j]) && boardState[i][j].Equals (boardState[i+1][j]))
				   	{
						return true;
					}          
				}
				else if((j+1) < boardWidth && (j+2) < boardWidth)
				{
					if(boardState[j][i].Equals (boardState[i][j+1]) && boardState[i][j].Equals (boardState[i][j+2]))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Checks the board for matches involving piece at coordinates (x,y)
	bool DoesMatchExistAtPoint(int x, int y)
	{
		int numVertical = 1;
		int numHorizontal = 1;

		if((x+1) < boardWidth &&  boardState[x][y].Equals(boardState[x+1][y]))
		{
			if((x+2) < boardWidth && boardState[x][y].Equals(boardState[x+2][y]))
			{
				return true;
			}
			numHorizontal++;
		}
		if((x-1) >= 0 && boardState[x][y].Equals(boardState[x-1][y]))
		{
			if((x-2) >= 0 && boardState[x][y].Equals (boardState[x-2][y]))
			{
				return true;
			}
			numHorizontal++;
		}
		if((y+1) < boardWidth && boardState[x][y].Equals (boardState[x][y+1]))
		{
			if((y+2) < boardWidth && boardState[x][y].Equals (boardState[x][y+2]))
			{
				return true;
			}
			numVertical++;
		}
		if((y-1) >= 0 && boardState[x][y].Equals(boardState[x][y-1]))
		{
			if((y-2) >= 0 && boardState[x][y].Equals (boardState[x][y-2]))
			{
				return true;
			}
			numVertical++;
		}
		if(numHorizontal >= 3 || numVertical >= 3)
		{
			return true;
		}
		return false;
	}

	// copies current board state to variable 'boardState'
	void CopyBoardState()
	{
		for(int i=0; i<boardWidth; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				if(gameObject.GetComponent<GameController>().squares[i][j].GetComponent<SquareController>().jewel != null)
				{

					boardState[i][j] = gameObject.GetComponent<GameController>().squares[i][j].GetComponent<SquareController>().jewel.tag;
				}
			}
		}
	}
}
