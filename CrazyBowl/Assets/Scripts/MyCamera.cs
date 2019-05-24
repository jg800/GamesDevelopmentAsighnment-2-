using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MyCamera : MonoBehaviour
{
	public Vector3[] endCamPos;
	public bool lockCamera;
	public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 3;
    public Vector2 dstMinMax = new Vector2(2, 6);
    public Vector2 pitchMinMax = new Vector2(-40, 85);
	public float scrollWheel;

	public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    private void Start()
    {
		endCamPos = new[] { new Vector3(5, 4, -5), new Vector3(-211, 12, 31) };
		if (lockCursor ){//&& SceneManager.GetActiveScene().buildIndex != 2) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

    private void LateUpdate()
    {
        scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0 && dstFromTarget > dstMinMax[0]){
            dstFromTarget -= 1;
        } else if (scrollWheel < 0 && dstFromTarget < dstMinMax[1]) {
            dstFromTarget += 1;
        }
		if (!lockCamera) {
			yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
			pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
			pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
			currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
			transform.eulerAngles = currentRotation;
			transform.position = target.position - transform.forward * dstFromTarget;
		} else {
			transform.LookAt(target);
		}
    }

	public void lockTheCamera(int levelNum, bool ended) {
		lockCamera = true;
		if (ended) {
			Debug.Log(levelNum);
			transform.position = endCamPos[levelNum];
		}
	}

	public void unlockTheCamera() {
		lockCamera = false;
	}

}