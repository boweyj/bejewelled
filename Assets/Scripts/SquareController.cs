using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

	public GameObject jewel;		// jewel currently associated with this square's position

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag.Equals ("Collider"))
		{
			jewel = null;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.tag.Equals ("Collider"))
		{
			jewel = other.gameObject;

		}
	}
}
