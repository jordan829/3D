using UnityEngine;
using System.Collections;

public class Knob : MonoBehaviour {

    public GameObject valueText;
    public GameObject knobHand;
    private bool on = false;
    public float min = float.NegativeInfinity;
    public float max = float.PositiveInfinity;
    public static float value = 7;
    private Quaternion origRotCont;
    private Quaternion origRot;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (on && ViveControl.press)
        {
            origRotCont = ViveControl.domCont.transform.rotation;
            origRot = transform.rotation;     
        }
        if (on && ViveControl.hold)
        {
            Quaternion angleDelta = (ViveControl.domCont.transform.rotation * Quaternion.Inverse(origRotCont));         
            angleDelta = Quaternion.Euler(0, angleDelta.eulerAngles.z, 0);
            //transform.rotation = (origRot * angleDelta);
            transform.RotateAround(transform.position, transform.up,1);

        }

        float t = Vector3.Angle(transform.up,knobHand.transform.position);
        //Debug.Log(knobHand.transform.position);
        //Debug.Log(t);
        value = (1 - t) * min + t * max;
        valueText.GetComponent<TextMesh>().text = t.ToString();
        

	}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Controller")
        {
            on = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        on = false;
    }
}
