using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
	public int informationToPass;
	// If true parsing num of players, if false parsing game time extension
	public bool parsingNumPlayers;
	
	public void myClick()
	{
		GameObject finder = GameObject.Find("PopUpImage");
		ParseInfo thing = finder.GetComponent<ParseInfo>();
		thing.parseInfo(informationToPass, parsingNumPlayers);
	}
}
