using UnityEngine;
using System.Collections;

public class CopyToMenu : MonoBehaviour {


    bool isCopy;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		this.gameObject.transform.LookAt(GameObject.Find("Sphere").transform);
        if (this.transform.tag == "Copy")
            isCopy = true;
    }

    void OnTriggerStay(Collider collide)
    {
		Physics.IgnoreLayerCollision (8,9,true);
        Debug.Log("moo");

        if (isCopy && !ViveControl.hold && collide.gameObject.tag == "Belt")
        {
            this.gameObject.transform.position = collide.gameObject.transform.position;
            this.gameObject.transform.SetParent(collide.gameObject.transform);
            //this.gameObject.transform.LookAt(GameObject.Find("[CameraRig]").transform);
            this.gameObject.transform.tag = "MenuItem";
			this.gameObject.layer = 0;
			isCopy = false;
        }

        else if (isCopy && !ViveControl.hold && collide.gameObject.tag != "Belt")
        {
            Destroy(gameObject);
        }
    }
}
