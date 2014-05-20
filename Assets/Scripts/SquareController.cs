using UnityEngine;
using System.Collections;

public class SquareController : MonoBehaviour {

	public GameObject jewel;		// jewel currently associated with this square's position

	void OnTriggerExit2D(Collider2D other)
	{
		jewel = null;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		jewel = other.gameObject;
	}
}
