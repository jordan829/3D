using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	MeshRenderer my_renderer;
	private Material oldMat;
	public Material selMat;

	void Start()
	{
		//Set menu offset and save current material into a variable  
		my_renderer = GetComponent<MeshRenderer>();
		oldMat = my_renderer.material;

	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnTriggerEnter(Collider collide)
	{
		//Controller is hovering
		if(collide.gameObject.tag == "Controller")
		{
			my_renderer.material = selMat;

		}
	}

	void OnTriggerExit(Collider collide)
	{
		if (collide.gameObject.tag == "Controller")
		{
			//Controller exists hover
			my_renderer.material = oldMat;
		}
	}
}
