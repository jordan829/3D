using UnityEngine;
using System.Collections;

public class menuMove : MonoBehaviour {

    public GameObject camera;
    public Vector3 offset;


    void Start()
    {
        //Set menu offset and save current material into a variable  
		offset = -camera.transform.position + transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.transform.position + offset;

		//work on this
		/*float x = this.transform.localEulerAngles.x;
		float y = camera.transform.localEulerAngles.y;
		float z = this.transform.localEulerAngles.z;
		this.transform.localEulerAngles = new Vector3 (x, y, z);*/
    }

}
