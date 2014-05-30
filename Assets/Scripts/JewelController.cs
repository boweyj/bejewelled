using UnityEngine;
using System.Collections;

public class JewelController : MonoBehaviour {

	public GameObject Controller;
	public GameObject highlight;
	public GUIText scoreText;
	public bool isSelected;
	public ParticleSystem explodePS;

	public float xPos;
	public float yPos;
	private Vector2 centre;
	private int x, y, dx, dy;  // varbiables to check adjacency
	private int boardWidth = 8;

	private bool isLineStrike;
	private bool isExploding;

	public Texture explosionTexture;
	public Texture lineStrikeTexture;

	private bool isDragging;
	private int count;
	private float totalSlope;
	private Vector2 mouseClick;


	void Start()
	{
		isLineStrike = false;
		isExploding = false;
		Controller = GameObject.Find ("GameController");
		isSelected = false;
		isDragging = false;
		xPos = gameObject.transform.position.x;
		centre = gameObject.transform.position;
	}

	void Update()
	{
//		float newX, newY;
//		newY = transform.position.y;
//		if(isDragging)
//		{
//			newY = yPos;
//		}
//		else
//		{
//			centre.y = transform.position.y;
//		}
//		newX = xPos;
//
//		Vector2 newPos = new Vector2 (newX, centre.y);
//
//		transform.position = newPos;
		if(transform.position.y >9.5f)
		{
			Destroy (GetComponent<Swap>().collider);
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
			//int row = (int)transform.position.y;

			if(gameObject != null && GetComponent<Swap>().collider != null)
			{
				Controller.GetComponent<SpawnPieces> ().SpawnJewelAtColumn (col, Random.Range(0, 6));
				GameObject.FindWithTag ("GameController").GetComponent<GUIController>().score += 10;
				Destroy (GetComponent<Swap>().collider);
				Destroy (gameObject);
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
	public void swap(GameObject obj1, GameObject obj2)
	{
//		float smooth = 0.0f;
//
//		for(int i=0; i<boardWidth; i++)
//		{
//			for(int j=0; j<boardWidth; j++)
//			{
//				GameObject.FindWithTag("GameController").GetComponent<SpawnPieces>().squares[j][i].GetComponent<SquareController>().jewel.rigidbody2D.gravityScale = 0;
//			}
//		}
//
//		obj1.collider2D.enabled = false;
//		obj2.collider2D.enabled = false;
//
//		Vector2 obj1_start = obj1.transform.position;
//		Vector2 obj2_start = obj2.transform.position;
//
//		while(smooth < 1.0f)
//		{
//			smooth += Time.deltaTime;
//			obj1.transform.position = Vector2.Lerp (obj1.transform.position, obj2_start, smooth);
//			obj2.transform.position = Vector2.Lerp (obj2.transform.position, obj1_start, smooth);
//
//		}
//		obj1.collider2D.enabled = true;
//		obj2.collider2D.enabled = true;
//
//		for(int i=0; i<boardWidth; i++)
//		{
//			for(int j=0; j<boardWidth; j++)
//			{
//				GameObject.FindWithTag("GameController").GetComponent<SpawnPieces>().squares[j][i].GetComponent<SquareController>().jewel.rigidbody2D.gravityScale = 1;
//			}
//		}
//
//		Vector2 temp = obj1.GetComponent<Swap> ().collider.transform.position;
//		obj1.GetComponent<Swap>().collider.transform.position = obj2.GetComponent<Swap>().collider.transform.position;
//		obj1.GetComponent<Swap>().collider.GetComponent<Collider>().xPos = obj2.GetComponent<Swap>().collider.transform.position.x;
//
//		obj2.GetComponent<Swap>().collider.transform.position = temp;
//		obj2.GetComponent<Swap> ().collider.GetComponent<Collider>().xPos = temp.x;
////		Vector2 temp = new Vector2 (obj1.transform.position.x, obj1.transform.position.y);
////		obj1.transform.position = new Vector2 (obj2.transform.position.x, obj2.transform.position.y);
////		obj2.transform.position = temp;
//
//		obj1.gameObject.GetComponent<JewelController> ().xPos = obj1.GetComponent<Swap>().collider.transform.position.x;
//		obj2.gameObject.GetComponent<JewelController> ().xPos = obj2.GetComponent<Swap>().collider.transform.position.x;
//		//obj1.GetComponent<JewelController> ().centre = obj2.GetComponent<JewelController> ().centre;
//		//obj2.GetComponent<JewelController> ().centre = obj1.GetComponent<JewelController> ().centre;

		StartCoroutine (GetComponent<Swap> ().AnimateSwap (obj1, obj2));

	}

	// start drag gesture, set as currently selected
	void OnMouseDown()
	{

		if(Controller.GetComponent<GameController>().isEnabled)
		{
			mouseClick = Camera.main.ScreenToWorldPoint (Input.mousePosition);


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
						Controller.GetComponent<GameController>().selected = null;
					}
				}
				else
				{
					Controller.GetComponent<GameController>().selected.GetComponent<JewelController> ().isSelected = false;
					Controller.GetComponent<GameController>().selected = null;
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
	

	void OnMouseDrag()
	{
		int numSamples = 5;
		Vector3 mouseWorld = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if(count < numSamples)
		{
			//Debug.Log(mouseWorld.y + " " + y + " " + mouseWorld.x + " " + x);
			float slope = (mouseWorld.y - mouseClick.y)/(mouseWorld.x -mouseClick.x);
			totalSlope += slope;
			count++;
			//Debug.Log (count + " " + slope);
		}

		else if(!isDragging)
		{
			isDragging = true;
			float avSlope = totalSlope/numSamples;

			GameObject homeSquare = null;
			GameObject newSquare = null;
			
			if(Controller.GetComponent<SpawnPieces> ().squares[x][y].GetComponent<SquareController>().jewel != null)
			{
				homeSquare = Controller.GetComponent<SpawnPieces> ().squares[x][y].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
			}
			
			Debug.Log ("The slope is: " + avSlope);
			if(float.IsNaN (avSlope) || (avSlope < .2f && avSlope > -.2f))
			{

//				Debug.Log ("small");
//				Debug.Log (mouseWorld.x + " " + x);
				if(mouseWorld.x > x)
				{
					if((x+1)<boardWidth && Controller.GetComponent<SpawnPieces>().squares[x+1][y].GetComponent<SquareController>().jewel != null)
					{
						newSquare = Controller.GetComponent<SpawnPieces>().squares[x+1][y].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
					}
				}
				else if(mouseWorld.x < x)
				{
					if((x-1)<boardWidth && Controller.GetComponent<SpawnPieces>().squares[x+1][y].GetComponent<SquareController>().jewel != null)
					{
						newSquare = Controller.GetComponent<SpawnPieces>().squares[x-1][y].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
					}
				}
			}
			else if(float.IsInfinity (avSlope) || float.IsNegativeInfinity (avSlope))
			{

				if(float.IsInfinity(avSlope) || (avSlope > 3.0f) || (avSlope < -3.0f))

			//	Debug.Log ("BIG");
//				Debug.Log (mouseWorld.y + " " + y);
				if(mouseWorld.y > y)
				{
					if(y+1 > boardWidth && Controller.GetComponent<SpawnPieces>().squares[x][y+1].GetComponent<SquareController>().jewel != null)
					{
						newSquare = Controller.GetComponent<SpawnPieces>().squares[x][y+1].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
					}
				}
				else if(mouseWorld.y < y)
				{
					if(y-1 > 0 && Controller.GetComponent<SpawnPieces>().squares[x][y-1].GetComponent<SquareController>().jewel != null)
					{
						newSquare = Controller.GetComponent<SpawnPieces>().squares[x][y-1].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
					}
				}
			}
			
			if(homeSquare != null && newSquare != null && isAdjacent(homeSquare, newSquare))
			{
				if(Controller.GetComponent<MatchingAI>().VerifySwap(x, y, (int)mouseWorld.x, (int)mouseWorld.y))
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


	// Finish drag gesture, swap accordingly
	void OnMouseUp()
	{

		isDragging = false;
		count = 0;
		totalSlope = 0.0f;
//		isDragging = false;
//		xPos = centre.x;
//		yPos = centre.y;
//		if(Controller.GetComponent<GameController>().isEnabled)
//		{
//			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//
//			dx = (int)pos.x;
//			dy = (int)pos.y;
//
//			if(dx < boardWidth && dy < boardWidth)
//			{
//				GameObject homeSquare = null;
//				GameObject newSquare = null;
//				if(Controller.GetComponent<SpawnPieces> ().squares[x][y].GetComponent<SquareController>().jewel != null && 
//				   Controller.GetComponent<SpawnPieces> ().squares[dx][dy].GetComponent<SquareController> ().jewel)
//				{
//					homeSquare = Controller.GetComponent<SpawnPieces> ().squares[x][y].GetComponent<SquareController>().jewel.GetComponent<Collider>().jewel;
//					newSquare = Controller.GetComponent<SpawnPieces> ().squares[dx][dy].GetComponent<SquareController> ().jewel.GetComponent<Collider>().jewel;
//				}
//
//				if(homeSquare != null && newSquare != null && isAdjacent (newSquare, homeSquare))
//				{
//					if(Controller.GetComponent<MatchingAI>().VerifySwap(x, y, dx, dy))
//					{
//					//	Debug.Log (x + " " + y + " " + dx + " " + dy);
//
//						swap (homeSquare, newSquare);
//					}
//
//					Controller.GetComponent<GameController>().selected = null;
//					isSelected = !isSelected;
//					GameObject highlight = GameObject.Find ("Highlight");
//					highlight.transform.position = new Vector2(-.5f, -.5f);
//				}
//			}
//		}
	}
}
