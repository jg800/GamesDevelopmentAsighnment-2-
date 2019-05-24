using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameInfo : MonoBehaviour
{
	private bool playing;
	private static int numPlayers;
	private static int[] Score;
	private int currentPlayer;
	private int currentLevel;
	private static int[] levelTimes;
	private float gameTime;
	private Text timeUI;
	private GameObject popText;
	private GameObject popImg;
	private Player thePlayer;
	private MyCamera theCamera;
	private List<GameObject> thePins = new List<GameObject>();
	private bool enter;
	private bool pause;
	private bool running;
	private bool endGame;
	

	void Start()
	{
		currentLevel = SceneManager.GetActiveScene().buildIndex - 1;
		popText = GameObject.Find("PopUpText");
		popImg = GameObject.Find("PopUpImage");
		if (currentLevel == 2)
		{
			playing = false;
			endGame = true;
			endGameScreen();
		} else {
			playing = true;
			endGame = false;
			pause = false;
			running = false;
			enter = false;
			
			GameObject finder = GameObject.Find("Timer");
			timeUI = finder.GetComponent<Text>();
			finder = GameObject.Find("Player");
			thePlayer = finder.GetComponent<Player>();
			
			finder = GameObject.Find("Main Camera");
			theCamera = finder.GetComponent<MyCamera>();
			Debug.Log("cl = " + currentLevel);
			if (currentLevel == 0)
			{
				currentPlayer = 0;
				numPlayers = popImg.GetComponent<ParseInfo>().getPlayers();
				Score = new int[numPlayers];
				thePlayer.letFreeze(true);
				theCamera.lockTheCamera(currentLevel, false);
				displayMessage("Welcome");
				int extensionTime = popImg.GetComponent<ParseInfo>().getPlayers();
				levelTimes = new int[] { 16, 22 };
				for (int i = 0; i < levelTimes.Length; i++)
				{
					levelTimes[i] += popImg.GetComponent<ParseInfo>().getExtensionTime();
				}
			} else {
				currentPlayer = 1;
				showText(false);
			}
			thePlayer.setCoulour((currentPlayer));
			int num = 1;
			bool y = true;
			while (y) {
				finder = GameObject.Find("Pin (" + num + ")");
				if (finder != null) {
					Debug.Log("found pin " + num);
					num++;
					thePins.Add(finder);
				} else {
					y = false;
				}
			}
			for (int i = 0; i < Score.Length; i++) {
				Debug.Log("scr(" + i + ") = " + Score[i]);
			}
			gameTime = levelTimes[currentLevel];
			Debug.Log("cp = " + currentLevel);
		}
	}

	private void hitMouse() {
		Debug.Log("click");
		if (enter == false) {
			enter = true;
			if (currentPlayer == numPlayers) {
				displayMessage("Player (" + (1) + ")");
			} else {
				displayMessage("Player (" + (currentPlayer + 1) + ")");
			}
		} else {
			enter = false;
			playing = true;
			showText(false);
			nextPlayer();
		}
	}

	void Update()
	{
		if (playing) {
			updateTimer();
			if (thePlayer.checkIfFallen()) {
				// if running then ball has left area after hitting trigger thus dont show gutter image
				if (!running) {
					playerEnd(false);
				}
			}
		} else if(!endGame){
			// enter this stament when we are watting for player click
			if (Input.GetMouseButtonDown(0)) {
				hitMouse();
			}
		}
	}

	private void resetLevel(){
		foreach (GameObject ob in thePins)
		{
			ob.GetComponent<Pin>().Reset();
		}
		theCamera.unlockTheCamera();
		thePlayer.letFreeze(false);
		thePlayer.setCoulour((currentPlayer));
		pause = false;
	}

	private void loadNextLevel() {
		currentLevel++;
		Debug.Log("Loading next scene" + currentLevel);
		SceneManager.LoadScene(currentLevel+1);
	}

	private void nextPlayer()
	{
		currentPlayer += 1;
		Debug.Log("cp = "+currentPlayer+" numPlayers = "+numPlayers);
		if (currentPlayer <= numPlayers) {
			resetLevel();
			gameTime = levelTimes[currentLevel];
		} else {
			loadNextLevel();
		}
	}

	private void updateTimer() {
		if (!pause) {
			gameTime -= Time.deltaTime;
			timeUI.text = (Mathf.Round(gameTime).ToString());
			if (!(gameTime > 0)) {
				playerEnd(false);
			}
		}
	}

	private void displayMessage(bool ended, int score = 0) {
		if (ended) {
			if (score == 0) {
				popText.GetComponent<Text>().text = "Thats a miss!";
			} else {
				if (score == thePins.Count) {
					popText.GetComponent<Text>().text = "Strike!";
				} else if (score == thePins.Count-1) {
					popText.GetComponent<Text>().text = "Spare";
				} else {
					popText.GetComponent<Text>().text = "You hit " + score;
				}
			}
		} else {
			if (gameTime <= 0) {
				popText.GetComponent<Text>().text = "And Thats Time!";
			} else {
				popText.GetComponent<Text>().text = "Gutter!";
			}
		}
		showText(true);
		playing = false;
	}

	private void displayMessage(string[] messages)
	{
		for (int i = 0; i < messages.Length; i++) {
			popText.GetComponent<Text>().text += messages[i];
		}
		showText(true);
	}

	private void displayMessage(string message)
	{
		popText.GetComponent<Text>().text = message;
		showText(true);
		playing = false;
	}

	private void showText(bool answer)
	{
		popText.SetActive(answer);
		popImg.SetActive(answer);
	}

	IEnumerator Wait()
	{
		running = true;
		pause = true;
		yield return new WaitForSeconds(5);
		int pins = checkPins();
		displayMessage(true, pins);
		running = false;
	}

	public void playerEnd(bool ended)
	{
		thePlayer.letFreeze(true);
		theCamera.lockTheCamera(currentLevel, ended);
		if (ended) {
			StartCoroutine(Wait());
		} else {
			int pins = checkPins();
			displayMessage(ended, pins);
		}
	}

	// Return number of pins fell over and add it to the score
	private int checkPins() {
		int fallenPins = 0;
		foreach (GameObject ob in thePins) {
			if (ob.GetComponent<Pin>().checkIfFallen()) {
				fallenPins++;
			}
		}
		Debug.Log(currentPlayer - 1);
		Score[currentPlayer-1] += fallenPins;
		Debug.Log("knocked pins = " + fallenPins);
		return fallenPins;
	}

	private void endGameScreen() {
		String[] printOut = new string[numPlayers];
		for (int i = 0; i < Score.Length; i++)
		{
			printOut[i] = ((i + 1) + ") Player (" + (i + 1) + ") =  " + Score[i] + "\n");
			Debug.Log(printOut[i]);
		}
		displayMessage(printOut);
	}
}

/* 
 * Goals
 * Make play game screen (enter num of players)
 * Make End screen
 * Make a course
 * Add materials/objects
 * 
 * Optional
 * Make Spring
 * Make Moving objects for player to traverse
 * Add time up object (updateTimer(int addTime))
 * 
 * Questions
 * How do u stop the player from flying.
 * How would you parse info from a scene into another.
 * Do i need more forces
 * Does it have to look pretty, eg can I use planes for objects
 */
