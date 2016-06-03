using UnityEngine;
using System.Collections;

public class TestColorControllerScript : MonoBehaviour {
	
	private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
	private SteamVR_TrackedObject trackedObj;

	bool pressed;
	bool holding;
	float timer;

	// Use this for initialization
	void Start () {
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		pressed = false;
		holding = false;
		timer = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (controller == null)
		{
			Debug.Log("Controller not initialized");
			return;
		}

		if (controller.GetPressDown (trigger)) 
		{
			pressed = true;
			timer = Time.time;
		} 
		else if (controller.GetPress (trigger)) 
		{
			if (timer > 0 && Time.time - timer > 0.5f) {
				holding = true;
			}
		} 
		else 
		{
			timer = -1;
			holding = false;
			pressed = false;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.layer == 10 && pressed) {
			if (other.gameObject.name.Equals ("ColorPicker")) {
				other.gameObject.GetComponent<ColorPickerBehavior> ().DoAction (transform.position);
			} 
			else {
				
				other.gameObject.GetComponent<SelectColorBehavior> ().DoAction ();
			}
		}
	}
}
