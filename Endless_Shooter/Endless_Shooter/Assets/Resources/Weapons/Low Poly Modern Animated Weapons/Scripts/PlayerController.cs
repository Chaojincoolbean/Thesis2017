using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public KeyCode		Forward = 	KeyCode.Z;
	public KeyCode 		Backward = 	KeyCode.S;
	public KeyCode		Left = 		KeyCode.Q;
	public KeyCode		Right =		KeyCode.D;
	public KeyCode		Jump =		KeyCode.Space;
	public KeyCode 		Sprint = 	KeyCode.LeftShift;
	
	public Camera		PlayerCam;
	public float 		mouseSensitivity = 5.0f;
	public float 		verticalRotation = 0;
	public float 		upDownRange = 60.0f;
	public float		speed = 5F;
	public float		jumpSpeed = 5F;
	public float		sprintMultiplier = 6F;
	public float 		gravity = 10F;
	
	private Animator 	PlayerAnimator;
	private float		moveH;
	private float		moveV;
	private Vector3		moveDirection = Vector3.zero;
	
	
	void Start () {
		Screen.lockCursor = true;
	}

	void Update () {

		//Rotation
		
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, rotLeftRight, 0);

		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		PlayerCam.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
		
		//Movments
		
		float Sprintspeed = speed;
		float Horizontal = 0F;
		float Vertical = 0F;
		

		
		if (Input.GetKey(Sprint)) {
			Sprintspeed = speed * sprintMultiplier;
		}
		
		if (Input.GetKey(Forward)) {
			Horizontal++;
		}
		
		if (Input.GetKey(Backward)) {
			Horizontal--;
		}
		
		if (Input.GetKey(Left)) {
			Vertical--;
		}
		
		if (Input.GetKey(Right)) {
			Vertical++;
		}
		

		

		
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Vertical, 0, Horizontal);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= Sprintspeed;
			if (Input.GetKeyDown (Jump))
				moveDirection.y = jumpSpeed;

		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
				
	}
}
