using UnityEngine;
using System.Collections;

public class ScoreTextScript : MonoBehaviour {


	public float fadeTime;

	// Use this for initialization
	void Start () {
		StartCoroutine(WaitAndDestroy ());
	}
	
	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds(fadeTime);
		Destroy (gameObject);
	}
}
