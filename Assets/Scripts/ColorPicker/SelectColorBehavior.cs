using UnityEngine;
using System.Collections;

public class SelectColorBehavior : MonoBehaviour {

	public int radius = 10;
	public int densityCenter = 0;  // density between 0 and 255 (center of "triangle" in histogram)

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
