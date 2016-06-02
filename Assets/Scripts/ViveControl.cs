using UnityEngine;
using System.Collections;

public class ViveControl : MonoBehaviour
{
	//private Valve.VR.EVRButtonId appMenu = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	//private Valve.VR.EVRButtonId axis0 = Valve.VR.EVRButtonId.k_EButton_Axis0;
	//private Valve.VR.EVRButtonId axis1 = Valve.VR.EVRButtonId.k_EButton_Axis1;
	//private Valve.VR.EVRButtonId axis2 = Valve.VR.EVRButtonId.k_EButton_Axis2;
	//private Valve.VR.EVRButtonId axis3 = Valve.VR.EVRButtonId.k_EButton_Axis3;
	//private Valve.VR.EVRButtonId axis4 = Valve.VR.EVRButtonId.k_EButton_Axis4;
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    public static bool press = false;
    public static bool hold = false;

    public static GameObject toChange;
    public static GameObject domCont;

    static float timer;
    bool timerStop = false;
    bool copyMove = false;

    bool rayHitMenu;
	bool rayHitColor;

	float shrinker;
	bool shrinking;
	GameObject shrinkObject;

    void Start ()
    {
		shrinkObject = null;
		shrinker = 1.0f;
		shrinking = false;
        rayHitMenu = false;
		rayHitColor = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        LineRenderer laser = this.gameObject.GetComponent<LineRenderer>();
        laser.SetWidth(0f, 0f);
		laser.SetColors (Color.blue, Color.green);
    }
	
	void Update ()
    {
		if (shrinking)
		{
			shrinker -= 0.005f;
			shrinkObject.transform.localScale = shrinkObject.transform.localScale * shrinker;

			if (shrinker <= 0.1f)
			{
				shrinking = false;
				shrinker = 1.0f;
				Destroy (shrinkObject);
				shrinkObject = null;
			}
		}

        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

		// Release grabbable items
		if (controller.GetPressUp (grip))
		{
			GameObject.Find ("Second").GetComponent<ClockHandMovement> ().updateOrientation ();

			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Grabbable"))
			{
				g.transform.parent = null;
			}
		}

        if (controller.GetPressDown(trigger))
		{
            press = true;
            domCont = this.gameObject;
            timer = 1.0f;
        }

		// Render laser (for color picker)
		//else if (controller.GetPress (trigger) || (controller.GetPress(trigger)  && controller.GetPress(grip)))
		else if (controller.GetPress (trigger) && GameObject.Find("ColorPicker").GetComponent<ColorPickerPosition>().inView == true)
        {
            LineRenderer laser = GetComponent<LineRenderer>();
            Vector3[] laserPoints = new Vector3[2];
            laser.SetColors(Color.blue, Color.green);
            laserPoints[0] = transform.position;
            laserPoints[1] = transform.position + (transform.forward * 5);
            laser.SetPositions(laserPoints);
            laser.SetWidth(0.01f, 0.01f);

			RaycastHit hit;
			if (Physics.Raycast (transform.position, transform.forward, out hit))
			{
                if (hit.transform.tag == "MenuItem")
                {
                    if (!rayHitMenu)
                    {
                        rayHitMenu = true;
                    }
                }

                else
                    rayHitMenu = false;

                if (hit.transform.tag == "Color")
                {
					//controller.TriggerHapticPulse (5000);	//NOTE: make stronger
                    shrinkAll();
                    hit.transform.gameObject.GetComponent<ColorProperty>().Enlarge();
					hit.transform.GetComponent<ColorProperty>().ChangeMenuColors();

                }
			}

            else
            {
                shrinkAll();
                rayHitMenu = false;
            }

            press = false;
            hold = true;
        }

		else if (controller.GetPress (trigger) && GameObject.Find("ColorPicker").GetComponent<ColorPickerPosition>().inView == false)
		{
			press = false;
			hold = true;
		}

        else
        {
            LineRenderer laser = GetComponent<LineRenderer>();
            laser.SetWidth(0, 0);
            shrinkAll();
            rayHitMenu = false;
        }

        if (controller.GetPressUp(trigger))
        {
            hold = false;
            toChange = null;
            domCont = null;
            copyMove = false;
			timerStop = false;
        }
    }

	void OnTriggerStay(Collider collide)
	{
		if (hold && collide.transform.name.Contains ("copy") && collide.transform.tag == "MenuItem")
		{
			timer -= Time.deltaTime;

			if (timer < -1)
			{
				shrinking = true;
				shrinkObject = collide.gameObject;
				//Destroy (collide.gameObject);
			}
		}

		else if (hold && collide.transform.tag == "MenuItem" && !copyMove)
        {
            timer -= Time.deltaTime;
			if(timer < 0 && !timerStop && !collide.gameObject.GetComponent<CopyToMenu>().isCopy)
            {
				collide.GetComponent<SelectionBehavior> ().HoverOff ();
                GameObject copy = Instantiate(collide.gameObject);
                copy.transform.name = collide.gameObject.name + "copy";
				//NOTE: just needed to reset orig. shortcuts are now working properly
				copy.GetComponent<SelectionBehavior> ().orig = collide.gameObject.GetComponent<SelectionBehavior> ().orig;
                copy.transform.position = this.transform.position;
                copy.transform.SetParent(this.transform);
                
                timerStop = true;
                copyMove = true;

                copy.transform.tag = "Copy";
				copy.layer = 9;
            }
        }

        else if (collide.transform.tag == "MenuItem") 
		{
			if (controller.GetPressDown (trigger)) 
			{
				collide.gameObject.GetComponent<SelectionBehavior> ().turnOnMainMenu();

				if(collide.gameObject.GetComponent<SelectionBehavior>() != null)
				{
					collide.gameObject.GetComponent<SelectionBehavior>().Select();
				}
			}

			//if (collide.transform.localScale == collide.gameObject.GetComponent<SelectionBehavior>().enlargedScale && controller.GetTouchDown (touchpad))
			if (controller.GetTouchDown (touchpad))
			{
				collide.gameObject.GetComponent<SelectionBehavior>().action (controller.transform.pos);
			}
		}

		// Grab object
		else if (collide.transform.tag == "Grabbable")
		{
			if (controller.GetPressDown (grip))
			{
				collide.transform.parent = transform;
			}
		}
        else if(collide.gameObject.tag == "Object")
        {
            toChange = collide.gameObject;
        }
        
	}

	// Safe to remove
	/*void OnTriggerStayTest(Collider collide)
	{
		if (collide.transform.tag == "MenuItem") 
		{
			if (controller.GetPressDown (trigger)) 
			{
				//collide.gameObject.GetComponent<SelectionBehavior> ().turnOnMainMenu();
				if(collide.gameObject.GetComponent<SelectionBehavior>() !=null)
					collide.gameObject.GetComponent<SelectionBehavior>().Select();
			}

			else if (collide.transform.localScale == collide.gameObject.GetComponent<SelectionBehavior>().enlargedScale && controller.GetTouchDown (touchpad))
			{

				collide.gameObject.GetComponent<SelectionBehavior>().action (controller.transform.pos);
			}
		}

		else if (collide.transform.tag == "ColorHandle")
		{
			if (controller.GetPressDown (grip))
			{
				collide.transform.parent = transform;
			}
		}
	}*/


	void OnTriggerEnter(Collider collide)
	{
		if (collide.transform.tag == "MenuItem" || collide.transform.tag == "Belt") 
		{
			controller.TriggerHapticPulse (3000);	//NOTE: make stronger

			if (collide.transform.tag == "MenuItem")
			{
				collide.GetComponent<SelectionBehavior> ().HoverOn ();	
			}
		}
	}

	void OnTriggerExit(Collider collide)
	{
		if (collide.transform.tag == "MenuItem")
		{
			collide.GetComponent<SelectionBehavior> ().HoverOff ();	
		}
	}
		
    void shrinkAll()
    {
        GameObject[] colors = GameObject.FindGameObjectsWithTag("Color");

        foreach (GameObject c in colors)
        {
            c.GetComponent<ColorProperty>().Shrink();
        }
    }
}