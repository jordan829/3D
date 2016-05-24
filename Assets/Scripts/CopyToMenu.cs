using UnityEngine;
using System.Collections;

public class CopyToMenu : MonoBehaviour {


    public bool isCopy;
	bool beltCollide = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		this.gameObject.transform.LookAt(GameObject.Find("Camera (head)").transform);
        if (this.transform.tag == "Copy")
            isCopy = true;

		if (isCopy && !ViveControl.hold && !beltCollide) {
			Destroy(gameObject);
		}
    }

    void OnTriggerStay(Collider collide)
    {
		Physics.IgnoreLayerCollision (8,9,true);

		// If belt item already contains a shortcut, this will ensure that a second shortcut is not placed but will
		// instead be destroyed
		if (isCopy && collide.gameObject.tag == "Belt" && collide.gameObject.transform.childCount == 0)
			beltCollide = true;

        if (isCopy && !ViveControl.hold && collide.gameObject.tag == "Belt")
        {
			this.gameObject.transform.position = collide.gameObject.transform.position;
			this.gameObject.transform.SetParent (collide.gameObject.transform);
			this.gameObject.transform.tag = "MenuItem";
			this.gameObject.layer = 0;
			//isCopy = false;
        }
    }
}
