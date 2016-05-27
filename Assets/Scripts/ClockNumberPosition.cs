using UnityEngine;
using System.Collections;
using System;

public class ClockNumberPosition : MonoBehaviour
{
	void Start ()
	{
		int hour = Int32.Parse(name) % 12;
		Quaternion origRot = transform.rotation;
		transform.RotateAround (GameObject.Find ("Middle").transform.position, Vector3.back, hour*30);
		transform.rotation = origRot;
	}

	void Update ()
	{
		
	}
}
