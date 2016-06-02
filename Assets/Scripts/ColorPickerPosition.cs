using UnityEngine;
using System.Collections;

public class ColorPickerPosition : MonoBehaviour
{
	public bool inView;
	Vector3 defaultPos;

	void Start ()
	{
		defaultPos = this.transform.position;
		inView = false;
	}

	void Update ()
	{
	
	}

	public void toggle()
	{
		if (inView)
		{
			inView = false;
			this.transform.position = defaultPos;
		}

		else
		{
			inView = true;
			GameObject.Find ("ColorPicker").transform.position = GameObject.Find ("Camera (head)").gameObject.transform.position + new Vector3 (0f, 0f, -0.5f);
			GameObject.Find ("ColorPicker").transform.LookAt (GameObject.Find ("Camera (head)").transform);
			GameObject.Find ("ColorPicker").transform.Rotate (new Vector3 (0, 180, 0));
		}
	}
}
