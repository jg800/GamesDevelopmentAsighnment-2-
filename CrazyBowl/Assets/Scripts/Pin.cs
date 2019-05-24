using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
	// Start is called before the first frame update
	public Vector3 myStartLoaction;
	public Vector3 myStartRotation;
	public Rigidbody rb;
    void Start()
    {
		myStartLoaction = this.transform.position;
		myStartRotation = this.transform.eulerAngles;
		rb = GetComponent<Rigidbody>();
	}
	public void Reset() {
		this.transform.position = myStartLoaction;
		this.transform.eulerAngles = myStartRotation;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	public bool checkIfFallen() {
		bool answer = false;
		int xrot = Mathf.FloorToInt(this.transform.rotation.eulerAngles.x > 180 ? 360 - this.transform.rotation.eulerAngles.x : this.transform.rotation.eulerAngles.x);
		int zrot = Mathf.FloorToInt(this.transform.rotation.eulerAngles.z > 180 ? 360 - this.transform.rotation.eulerAngles.z : this.transform.rotation.eulerAngles.z);
		// If the x rotaion or z roation is over 35 degrees or if the y position is smaller than its stating location
		if (xrot > 35 || zrot > 35 || this.transform.position.y < (myStartLoaction.y - (this.transform.lossyScale.y / 2))) {
			answer = true;
		}
		return answer;
	}
}
