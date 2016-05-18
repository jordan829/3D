using UnityEngine;
using System.Collections;
public class ResizeWidget : MonoBehaviour {
    private GameObject controller_left;
    private GameObject controller_right;
    public static bool ResizeOn = false;
    private float origDist;
    private bool scale;
	// Use this for initialization
	void Start () {
        controller_left = GameObject.Find("Controller (left)");
        controller_right = GameObject.Find("Controller (right)");
    }
	
	// Update is called once per frame
	void Update () {
        if (ResizeOn)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.red;
            scale = false;
        }

        if(ResizeOn && ViveControl.press && ViveControl.toChange != null)
        {
            origDist = Vector3.Distance(controller_left.transform.position, controller_right.transform.position);
            scale = true;
        }

        if (scale && ViveControl.hold && ViveControl.toChange != null)
        {
            float curDist = Vector3.Distance(controller_left.transform.position, controller_right.transform.position);
            //if distance increase, up scale. if it decreases reduce scale
            ViveControl.toChange.transform.localScale *= 1.0f + (curDist - origDist);
            origDist = curDist;
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (ViveControl.press && col.gameObject.tag == "Controller")
        {
            ResizeOn = !ResizeOn;
        }
    }
}
