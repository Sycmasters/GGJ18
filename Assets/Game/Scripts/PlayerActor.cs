using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : MonoBehaviour 
{
	public int inputIndex = 0;
	public float moveSpeed = 3, runSpeed = 6;

	private float movementSpeed;
	private Vector3 movement;

	private Rigidbody rbody;

	// Use this for initialization
	void Start () 
	{
		rbody = GetComponent<Rigidbody> ();
	}

	private void Update ()
	{
		if (inputIndex >= 0) {
			movementSpeed = Input.GetKey ("joystick " + (inputIndex + 1) + " button 0") ? runSpeed : moveSpeed;
		} else {
			movementSpeed = Input.GetKey (KeyCode.LeftShift) ? runSpeed : moveSpeed;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float horizontal = inputIndex >= 0 ? Input.GetAxis ("Horizontal" + inputIndex) : Input.GetAxis ("Horizontal");
		float vertical = inputIndex >= 0 ? Input.GetAxis ("Vertical" + inputIndex) : Input.GetAxis ("Vertical");

		movement = new Vector3(horizontal * movementSpeed,
							   rbody.velocity.y,
							   vertical * movementSpeed);

		rbody.velocity = movement;
	}
}
