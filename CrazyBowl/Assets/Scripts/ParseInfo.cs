using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParseInfo : MonoBehaviour
{
	private static int numPlayers;
	private static int addGameTime;

	// id thetype is true parse thrugh info must be num of players else is the time extension
	public void parseInfo(int info, bool theType) {
		if (info != -1) {
			if (theType) {
				numPlayers = info;
				SceneManager.LoadScene("DifficultyMenu");
			} else {
				addGameTime = info;
				SceneManager.LoadScene("Level1");
			}
		} else {
			SceneManager.LoadScene("StartMenu");
		}
	}

	public int getPlayers() {
		return numPlayers;
	}

	public int getExtensionTime()
	{
		return addGameTime;
	}
}