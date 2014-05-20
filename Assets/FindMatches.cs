using UnityEngine;
using System.Collections;

public class FindMatches : MonoBehaviour {

	int boardWidth;
	void Start()
	{
		boardWidth = GetComponent<GameController> ().boardWidth;
	}

	public bool GetMatches()
	{
		bool verticalCheck = CheckVertical ();
		bool horizontalCheck = CheckHorizontal ();
		return verticalCheck || horizontalCheck;
	}

	// Searches for vertical matches. returns true if match found, false otherwise
	bool CheckVertical()
	{
		bool matchFound = false;
		// Make update loop wait until check is completed

		// Looping through columns
		for(int i=0; i<boardWidth; i++)
		{
			// Looping through rows
			for(int j=0; j<boardWidth; j++)
			{
				// Checking if indices are within bounds
				if((i+1)<boardWidth && (i+2)<boardWidth)
				{
					// set jewels to variable
					GameObject jewel1 = GetComponent<GameController>().squares[j][i].GetComponent<SquareController>().jewel;
					GameObject jewel2 = GetComponent<GameController>().squares[j][i+1].GetComponent<SquareController>().jewel;
					GameObject jewel3 = GetComponent<GameController>().squares[j][i+2].GetComponent<SquareController>().jewel;
					
					// Checks if all three jewels match AND are not null
					if((jewel1 != null) && (jewel2 != null) && (jewel3 != null) &&  (jewel1.tag.Equals (jewel2.tag)) && (jewel1.tag.Equals (jewel3.tag)))
					{
						jewel1.renderer.material.color = Color.white;
						jewel1.renderer.material.mainTexture = null;
						
						jewel2.renderer.material.color = Color.white;
						jewel2.renderer.material.mainTexture = null;
						
						jewel3.renderer.material.color = Color.white;
						jewel3.renderer.material.mainTexture = null;
						
						if((i+3)<boardWidth)
						{
							GameObject jewel4 = GetComponent<GameController>().squares[j][i+3].GetComponent<SquareController>().jewel;
							if((jewel4 != null) && (jewel1.tag.Equals (jewel4.tag)))
							{
								jewel4.renderer.material.color = Color.white;
								jewel4.renderer.material.mainTexture = null;
								
								if((i+4)<boardWidth)
								{
									GameObject jewel5 = GetComponent<GameController>().squares[j][i+4].GetComponent<SquareController>().jewel;
									if((jewel5 != null) && (jewel1.tag.Equals (jewel5.tag)))
									{
										jewel5.renderer.material.color = Color.white;
										jewel5.renderer.material.mainTexture = null;
										jewel5.GetComponent<JewelController>().RemoveJewel ();
									}
								}
								jewel4.GetComponent<JewelController>().RemoveJewel ();
							}
						}
						jewel3.GetComponent<JewelController>().RemoveJewel ();
						jewel2.GetComponent<JewelController>().RemoveJewel ();
						jewel1.GetComponent<JewelController>().RemoveJewel ();

						i = boardWidth;
						j = boardWidth;
						matchFound = true;
						GetComponent<GameController>().nextHint = Time.time + GetComponent<GameController>().hintRate;
					}
				}
				
			}
		}
		return matchFound;
	}
	
	// Searches for vertical matches.  returns true if match found, false otherwise
	bool CheckHorizontal()
	{
		bool matchFound = false;
		// Looping through rows
		for (int i=0; i<boardWidth; i++) 
		{
			// Looping through columns
			for (int j=0; j<boardWidth; j++) 
			{
				// Checking if indices are within bounds
				if ((j+1)<boardWidth && (j+2)<boardWidth) 
				{
					// set jewels to variables
					GameObject jewel1 = GetComponent<GameController>().squares[j][i].GetComponent<SquareController> ().jewel;
					GameObject jewel2 = GetComponent<GameController>().squares[j+1][i].GetComponent<SquareController> ().jewel;
					GameObject jewel3 = GetComponent<GameController>().squares[j+2][i].GetComponent<SquareController> ().jewel;
					
					// Checks if all three jewels match and are not null
					if ((jewel1 != null) && (jewel2 != null) && (jewel3 != null) && (jewel1.tag.Equals (jewel2.tag)) && (jewel1.tag.Equals (jewel3.tag))) 
					{
						jewel1.renderer.material.color = Color.white;
						jewel1.renderer.material.mainTexture = null;
						
						jewel2.renderer.material.color = Color.white;
						jewel2.renderer.material.mainTexture = null;
						
						jewel3.renderer.material.color = Color.white;
						jewel3.renderer.material.mainTexture = null;
						
						if((j+3)<boardWidth)
						{
							GameObject jewel4 = GetComponent<GameController>().squares[j+3][i].GetComponent<SquareController>().jewel;
							if(jewel4 != null && (jewel1.tag.Equals (jewel4.tag)))
							{
								jewel4.renderer.material.color = Color.white;
								jewel4.renderer.material.mainTexture = null;
								
								if((j+4)<boardWidth)
								{
									GameObject jewel5 = GetComponent<GameController>().squares[j+4][i].GetComponent<SquareController>().jewel;
									if(jewel5 != null && (jewel1.tag.Equals (jewel5.tag)))
									{
										jewel5.renderer.material.color = Color.white;
										jewel5.renderer.material.mainTexture = null;

										jewel5.GetComponent<JewelController>().RemoveJewel ();
									}
								}
								jewel4.GetComponent<JewelController>().RemoveJewel ();
							}
						}
						
						jewel1.renderer.material.color = Color.white;
						jewel1.renderer.material.mainTexture = null;
						
						jewel2.renderer.material.color = Color.white;
						jewel2.renderer.material.mainTexture = null;
						
						jewel3.renderer.material.color = Color.white;
						jewel3.renderer.material.mainTexture = null;
						
						jewel1.GetComponent<JewelController> ().RemoveJewel ();
						jewel2.GetComponent<JewelController> ().RemoveJewel ();
						jewel3.GetComponent<JewelController> ().RemoveJewel ();
						j=boardWidth;
						i=boardWidth;
						matchFound = true;
						GetComponent<GameController>().nextHint = Time.time + GetComponent<GameController>().hintRate;
					}
				}
			}
		}
		return matchFound;
	}
}
