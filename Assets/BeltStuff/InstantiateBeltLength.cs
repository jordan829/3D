using UnityEngine;
using System.Collections;

public class InstantiateBeltLength : MonoBehaviour {
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
	private SteamVR_TrackedObject trackedObj;
	public GameObject camera;
	public GameObject Belt;
	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (controller == null)
		{
			Debug.Log("Controller not initialized");
			return;
		}
		if (controller.GetPressDown(triggerButton))
		{
			Belt.transform.position = transform.position;
			float angle = (float)360.0 / (float)Belt.transform.childCount;
			for(int i = 1; i < Belt.transform.childCount; i++)
			{
				float diff = (Belt.transform.GetChild (i).transform.position - new Vector3(camera.transform.position.x, Belt.transform.GetChild (i).transform.position.y, camera.transform.position.z)).magnitude;
				Belt.transform.GetChild (i).transform.position = new Vector3(camera.transform.position.x, Belt.transform.GetChild (i).transform.position.y, camera.transform.position.z);
				Belt.transform.GetChild (i).transform.Rotate(Vector3.up * angle * (float)(i+1), Space.Self);
				Belt.transform.GetChild (i).transform.position += Belt.transform.GetChild (i).transform.forward * diff;
			}
			Belt.transform.gameObject.SetActive (true);
			Belt.transform.gameObject.GetComponent<menuMove> ().offset = Belt.transform.position;

			// Remove starting message
			GameObject.Find ("StartMessage").SetActive (false);

			transform.gameObject.GetComponent<InstantiateBeltLength> ().enabled = false;
		}

	}
}
