using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
	private GameInfo theGame;
	private void Start()
	{
		GameObject finder = GameObject.Find("EndPlatform");
		theGame = finder.GetComponent<GameInfo>();
	}

	void OnTriggerEnter(Collider col)
    {
		if (col.gameObject.name == "Player")
		{
			theGame.playerEnd(true);
		}
	}
}
