using UnityEngine;
using System.Collections;

public class ColorPickerBehavior : MonoBehaviour {

	public GameObject colorPosition;
	public bool allowAction;
	public GameObject currColor;
	public GameObject cam;

	// Use this for initialization
	void Start () {
		allowAction = false;
		currColor = null;
	}
	
	// Update is called once per frame
	void Update () {
		// update colorposition's position
		if (currColor == null) {
			colorPosition.SetActive (false);
		} 
		else {
			colorPosition.SetActive (true);
			Color pos = currColor.GetComponent<Renderer> ().material.color;
			colorPosition.transform.localPosition = (new Vector3 (pos.r - 0.5f, 
				pos.g - 0.5f, 
				pos.b - 0.5f));
		}

		// if we're not active, just reset a little
		if (!gameObject.activeSelf) {
			currColor = null;
		}
	}

	void OnTriggerStay(Collider other) {
		// if controller enters the colorBox
		if (other.gameObject.name.Equals ("Controller (left)") ||
		    other.gameObject.name.Equals ("Controller (right)")) {
			// allow action to be available
			allowAction = true;
		}
			
	}

	void OnTriggerExit(Collider other) {
		allowAction = false;
	}

	public void DoAction(Vector3 controllerPos) {
		if (allowAction && currColor != null) {
			// make the controller position relative to our colorBox's position
			controllerPos = transform.InverseTransformPoint(controllerPos);

			if (controllerPos.x < 0.5f && controllerPos.x > -0.5f &&
			   controllerPos.y < 0.5f && controllerPos.y > -0.5f &&
			   controllerPos.z < 0.5f && controllerPos.z > -0.5f) {
				// controllerPos is now each component between 0 and 1 inclusive

				// set currColor (which is a display of color)'s color to be the (x,y,z)
				currColor.GetComponent<Renderer> ().material.color = new Color (controllerPos.x + 0.5f, controllerPos.y + 0.5f, controllerPos.z + 0.5f);
			}
		}
	}
}
