using UnityEngine;
using System.Collections;
public class MoveWidget : MonoBehaviour
{
    public GameObject controller_left;
    public GameObject controller_right;
    public static bool MoveOn = false;
    private float origDist;
    private bool translate;
    private Vector3 startPos;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MoveOn)
        {
            this.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        if(MoveOn && ViveControl.press && ViveControl.toChange != null && ViveControl.domCont != null)
        {
            startPos = ViveControl.domCont.transform.position;
        }


        if (MoveOn && ViveControl.hold && ViveControl.toChange != null)
        {
            Vector3 move = ViveControl.domCont.transform.position - startPos;
            ViveControl.toChange.transform.position += move;
            startPos = ViveControl.domCont.transform.position;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (ViveControl.press && col.gameObject.tag == "Controller")
        {
            MoveOn = !MoveOn;
        }
    }
}
