using UnityEngine;
using System.Collections;

public class ViveControl : MonoBehaviour
{
	private Valve.VR.EVRButtonId appMenu = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId axis0 = Valve.VR.EVRButtonId.k_EButton_Axis0;
	private Valve.VR.EVRButtonId axis1 = Valve.VR.EVRButtonId.k_EButton_Axis1;
	private Valve.VR.EVRButtonId axis2 = Valve.VR.EVRButtonId.k_EButton_Axis2;
	private Valve.VR.EVRButtonId axis3 = Valve.VR.EVRButtonId.k_EButton_Axis3;
	private Valve.VR.EVRButtonId axis4 = Valve.VR.EVRButtonId.k_EButton_Axis4;
    private Valve.VR.EVRButtonId grip = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId touchpad = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    bool rayHitMenu;

    void Start ()
    {
        rayHitMenu = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        LineRenderer laser = this.gameObject.GetComponent<LineRenderer>();
        laser.SetWidth(0f, 0f);
		laser.SetColors (Color.blue, Color.green);
    }
	
	void Update ()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }

		if (controller.GetPress (grip))
		{
			
		}

        if (controller.GetPress (trigger))
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
                        //hit.transform.gameObject.GetComponent<SelectionBehavior>().Flip();
                        rayHitMenu = true;
                    }
                }

                else
                    rayHitMenu = false;

                if (hit.transform.tag == "Color")
                {
					Debug.Log ("hit color");
                    shrinkAll();
                    hit.transform.gameObject.GetComponent<ColorProperty>().Enlarge();
                }
			}

            else
            {
                shrinkAll();
                rayHitMenu = false;
            }
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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.tag == "MenuItem")
                {
                    
                }

                if (hit.transform.tag == "Color")
                {
                    hit.transform.GetComponent<ColorProperty>().ChangeMenuColors();
                }
            }
        }
    }

	void OnTriggerStay(Collider collide)
	{
		if (collide.transform.tag == "MenuItem") 
		{
			if (controller.GetPressDown (trigger)) 
			{
				collide.gameObject.GetComponent<SelectionBehavior>().Select();
			}
		}

		if (collide.transform.tag == "ColorHandle")
		{
			if (controller.GetPressDown (grip))
			{
				collide.transform.parent = transform;
			}

			if (controller.GetPressUp (grip))
			{
				collide.transform.parent = null;
			}
		}
	}

	void OnTriggerEnter(Collider collide)
	{
		if (collide.transform.tag == "MenuItem") 
			controller.TriggerHapticPulse (3000);	//NOTE: make stronger
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