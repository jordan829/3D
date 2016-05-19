using UnityEngine;
using System.Collections;

public class TextBehavior : MonoBehaviour
{
	void Start ()
	{
	
	}

	void Update ()
	{
		GetComponent<TextMesh> ().text = transform.parent.GetComponent<SelectionBehavior>().orig.name;
	}
}
