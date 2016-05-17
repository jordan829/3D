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
    }

}
