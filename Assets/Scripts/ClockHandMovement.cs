using UnityEngine;
using System.Collections;
using System;

public class ClockHandMovement : MonoBehaviour
{
	bool hourCalled;
	bool minuteCalled;
	bool secondCalled;
	int prevSecond;
	int prevMinute;
	Vector3 originalPos;
	Vector3 originalRot;
	float amountRotated;

	void Start ()
	{
		amountRotated = 0;
		originalPos = this.transform.position;
		originalRot = this.transform.localEulerAngles;
		prevSecond = 0;
		prevMinute = 0;
		hourCalled = false;
		minuteCalled = false;
		secondCalled = false;
	}

	void Update ()
	{
		if (GameObject.Find ("Clock").transform.parent != null)
		{
			secondCalled = false;
			return;
		}
		
		int second = System.DateTime.Now.Second;
		int minute = System.DateTime.Now.Minute;
		int hour = System.DateTime.Now.Hour % 12;
		Vector3 point = GameObject.Find ("Middle").transform.position;
	
		if (prevSecond != second)
			secondCalled = false;

		if (prevMinute != minute)
			minuteCalled = false;
		
		if (this.gameObject.name == "Hour" && !hourCalled)
		{
			this.gameObject.transform.RotateAround (point, Vector3.back, (hour * 30) + (minute/2));
			hourCalled = true;
		}

		if (this.gameObject.name == "Minute" && !minuteCalled)
		{
			prevMinute = minute;
			this.transform.localEulerAngles = originalRot;
			this.transform.position = originalPos;
			//this.gameObject.transform.RotateAround (point, Vector3.back, minute * 6);
			this.gameObject.transform.RotateAround (point, -GameObject.Find("Middle").transform.forward, minute * 6);
			minuteCalled = true;
		}
			
		if (this.gameObject.name == "Second" && !secondCalled)
		{
			prevSecond = second;
			this.transform.localEulerAngles = originalRot;
			this.transform.position = originalPos;
			//this.gameObject.transform.RotateAround (point, Vector3.back, second * 6);
			this.gameObject.transform.RotateAround (point, -GameObject.Find("Middle").transform.forward, -amountRotated);
			this.gameObject.transform.RotateAround (point, -GameObject.Find("Middle").transform.forward, second * 12);
			secondCalled = true;
			amountRotated = second * 6;
		}
	}

	public Vector3 Pos
	{
		get { return originalPos; }
		set { originalPos = value; }
	}

	public Vector3 Rot
	{
		get { return originalRot; }
		set { originalRot = value; }
	}

	public void updateOrientation()
	{
		this.originalPos = this.transform.position;
		this.originalRot = this.transform.localEulerAngles;
	}
}