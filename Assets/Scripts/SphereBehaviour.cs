using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereBehaviour : MonoBehaviour {

	public Transform camera;
	public GameObject hierarchy;
	public float positionOffset;

	private Dictionary<GameObject, List<GameObject>> ptc;

	// Use this for initialization
	void Start () {
		positionOffset = -0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 shoulderOffset = new Vector3(0.0f, positionOffset, 0.0f);
		transform.position = camera.position + shoulderOffset;

	}

	/**
	 * Determines the 3D world position of a point on this given sphere (with its given radius).
	 * input: yawRadians - how far in horizontal radians (the yaw) is this from backward (-z i think)
	 * input: pitchRadians - how far in vertical radians (the pitch) is this from backwards
	 */
	Vector3 InnerSpherePosition(float yawDegrees, float pitchDegrees) {
		// use back because by default, objects put into this position will face you; can be changed
		Vector3 toReturn = Vector3.back * transform.localScale.z / 2.0f;
		toReturn = Quaternion.Euler (pitchDegrees, yawDegrees, 0.0f) * toReturn;
		return toReturn + transform.position;
	}


	/**
	 * Dynamically loads a menu using a menuItemFab given and input distances between sections
	 * input: parent - the hierarchical parent of the menuItems to be loaded
	 * input: horizLevel - used to keep track of what level of the hierarchy is currently being set up
	 * input: horizDegDist - the distance in degrees on the sphere between two levels of the hierarchy (left to right)
	 * input: vertDegDist - the distance in vertical degrees on the sphere that separate two menuItems
	 */
	public void LoadMenu(GameObject parent, int horizLevel, float horizDegDist, float vertDegDist) {
		ptc = hierarchy.GetComponent<ParentToChild> ().parentToChild;
		if (ptc != null) {
			List<GameObject> currList = null;
			currList = ptc [parent];
			if (currList == null || currList.Count == 0)
				return;
			
			int indexOffset = currList.Count / 2;
			float cubeSize = 0.08f;
			for (int i = 0; i < currList.Count; i++) {
				//Debug.Log ("Setting position of " + currList [i].name);
				currList [i].transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
				currList [i].transform.position = InnerSpherePosition (horizLevel*horizDegDist, vertDegDist * (indexOffset - i));
				currList [i].transform.parent = transform;
				currList [i].transform.LookAt (transform);
				LoadMenu (currList [i], horizLevel + 1, horizDegDist, vertDegDist);
			}
		}
		if (ptc == null)
			//Debug.Log ("ParentToChild not found");
		Debug.Log ("Finished setting menu positions");
	}
}
