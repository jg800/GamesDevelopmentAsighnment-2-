using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Use this for initialization
    public Camera maincam;
    public Rigidbody rb;
    public int speed;
    public int jump;
	public int jumpCount = 0;
	public bool allowMove = true;
	public Vector3 startPosition;
	public int deathHeight;
	float x;
	float z;

	void Start() {
        rb = GetComponent<Rigidbody>();
		startPosition = this.transform.position;
	}

    void FixedUpdate() {
		if (allowMove)
		{
			if (Input.GetKey("a")) {
				rb.AddForce(maincam.transform.right.x * -speed, 0, maincam.transform.right.z * -speed);
			}
			if (Input.GetKey("d")) {
				rb.AddForce(maincam.transform.right.x * speed, 0, maincam.transform.right.z * speed);
			}
			if (Input.GetKey("s")) {
				rb.AddForce(maincam.transform.forward.x * -speed, 0, maincam.transform.forward.z * -speed);
			}
			if (Input.GetKey("w")) {
				rb.AddForce(maincam.transform.forward.x * speed, 0, maincam.transform.forward.z * speed);
			}
			if (Input.GetButtonDown("Jump") && jumpCount == 0) {
				rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
				jumpCount = 1;
			}
		}
	}

    void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Ground")
		{
			jumpCount = 0;
		}
	}

	public bool checkIfFallen() {
		bool answer = false;
		if (this.transform.position.y < deathHeight) {
			answer = true;
		}
		return answer;
	}

	public void letFreeze(bool decision) {
		allowMove = !(decision);
		// If unfreezing the camera then must be reseting player, thus reset position
		if (!decision) {
			resetPlayer();
		}
	}

	private void resetPlayer() {
		this.transform.position = startPosition;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	public void setCoulour(int currentPlayer) {
		if (currentPlayer == 1) {
			this.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
		} else if (currentPlayer == 2) {
			this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		} else if (currentPlayer == 3) {
			this.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
		} else if (currentPlayer == 4) {
			this.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
		}
		
	}
}