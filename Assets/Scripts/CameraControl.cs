using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public float speed = 5.0f;

	public float minX = -360.0f;
	public float maxX = 360.0f;

	public float minY = -45.0f;
	public float maxY = 45.0f;

	public float sensX = 1.1f;
	public float sensY = 1.1f;

	float rotationY = 0.0f;
	float rotationX = 0.0f;

	// I halved these
	public float moveSpeed = 5f;
	public float turnSpeed = 25;

	public bool lookAtCenter = true;

	// Use this for initialization
	void Start ()
	{
		SetDefaultCamera ();
	}

	// Update is called once per frame
	void Update ()
	{
		// Shift right
		if(Input.GetKey(KeyCode.RightArrow))
		{
			//transform.position = new Vector3(speed * Time.deltaTime,0,0);
			transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z);
		}

		// Shift left
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z);
		}

		// Shift down
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, transform.position.z);
		}

		// Shift up
		if(Input.GetKey(KeyCode.UpArrow))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);		
		}

		// Shift out
		if(Input.GetKey(KeyCode.J))
		{
			transform.Translate (-Vector3.forward * moveSpeed * Time.deltaTime);
		}

		// Shift in
		if(Input.GetKey(KeyCode.K))
		{
			transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
		}

		// Rotate camera with mouse
		if (Input.GetMouseButton (0))
		{
			//RotateWithMouse ();
		}

		// Tilt right
		if (Input.GetKey (KeyCode.X))
		{
			transform.Rotate (Vector3.forward, turnSpeed * Time.deltaTime);
		}

		// Tilt left
		if (Input.GetKey (KeyCode.Z))
		{
			transform.Rotate (Vector3.forward, -turnSpeed * Time.deltaTime);	
		}

		// Tilt down
		if (Input.GetKey (KeyCode.S))
		{
			transform.Rotate (Vector3.right, turnSpeed * Time.deltaTime);	
		}

		// Tilt up
		if (Input.GetKey (KeyCode.W))
		{
			transform.Rotate (Vector3.right, -turnSpeed * Time.deltaTime);	
		}

		// Rotate left
		if (Input.GetKey (KeyCode.A))
		{
			transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
		}


		// Rotate right
		if (Input.GetKey (KeyCode.D))
		{
			transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
		}

		// Print camera location
		if (Input.GetKey (KeyCode.C))
		{
			Debug.Log ("Camera Position: " + "(" + transform.position.x + ", " + transform.position.y + ", " + transform.position.z + ")");
			Debug.Log ("Camera Rotation: " + "(" + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + ")");
			Debug.Log ("Local Euler: " + "(" + transform.localEulerAngles.x + ", " + transform.localEulerAngles.y + ", " + transform.localEulerAngles.z + ")");
		}

		// Reset
		if (Input.GetKeyDown (KeyCode.Space))
		{
			SetDefaultCamera ();
		}
	}

	void RotateWithMouse()
	{
		rotationX += Input.GetAxis ("Mouse X") * sensX * Time.deltaTime;
		rotationY += Input.GetAxis ("Mouse Y") * sensY * Time.deltaTime;
		rotationY = Mathf.Clamp (rotationY, minY, maxY);
		transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
		//transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x - rotationY, 
		//transform.localEulerAngles.y + rotationX,
		//transform.localEulerAngles.z);
	}

	void SetDefaultCamera()
	{
		//transform.position = new Vector3 (0, 0, 0);
		//transform.localEulerAngles = new Vector3 (0, 0, 0);
		//transform.localEulerAngles = new Vector3 (349.8f, 95.5f, 0.84f);
		//transform.position = new Vector3 (-26.1f, 31.7f, -9.7f);
		//transform.localEulerAngles = new Vector3 (47f, 84f, 0.339f);
		//transform.position = new Vector3 (-6.9f, 22.9f, -31.7f);
		//transform.localEulerAngles = new Vector3 (14f, 16f, 326f);
		//transform.position = new Vector3 (-2.3f, 26.7f, -37.57f);
		//transform.localEulerAngles = new Vector3 (13.5f, 1.5f, 0f);
	}
}