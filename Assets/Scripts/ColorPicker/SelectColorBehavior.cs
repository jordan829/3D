using UnityEngine;
using System.Collections;

public class SelectColorBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DoAction() {
		// set the parent's (colorBox) "currColor" field to this
		transform.parent.gameObject.GetComponent<ColorPickerBehavior>().currColor = gameObject;
	}
}
