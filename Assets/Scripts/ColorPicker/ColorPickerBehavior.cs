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
			colorPosition.transform.position = (new Vector3 (pos.r - transform.localScale.x, 
				pos.g - transform.localScale.y, 
				pos.b - transform.localScale.z)) + transform.position;
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
			controllerPos -= transform.position;

			// limit the position to be within the colorBox's limits (because trigger still happens outside)
			controllerPos.x = (controllerPos.x > transform.localScale.x) ?
				transform.localScale.x : (controllerPos.x < -1 * transform.localScale.x) ?
				-1 * transform.localScale.x : controllerPos.x;

			controllerPos.y = (controllerPos.y > transform.localScale.y) ?
				transform.localScale.y : (controllerPos.y < -1 * transform.localScale.y) ?
				-1 * transform.localScale.y : controllerPos.y;

			controllerPos.z = (controllerPos.z > transform.localScale.z) ?
				transform.localScale.z : (controllerPos.z < -1 * transform.localScale.z) ?
				-1 * transform.localScale.z : controllerPos.z;

			controllerPos += new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);
			// controllerPos is now each component between 0 and 1 inclusive

			// set currColor (which is a display of color)'s color to be the (x,y,z)
			currColor.GetComponent<Renderer>().material.color = new Color(controllerPos.x, controllerPos.y, controllerPos.z);
		}
	}
}
