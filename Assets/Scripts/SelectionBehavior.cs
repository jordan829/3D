using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionBehavior : MonoBehaviour
{
	public bool activated;
	public int layer;
	Vector3 defaultScale;
	public Vector3 enlargedScale;

	public GameObject orig;

	void Start ()
	{
		layer = 0;
		activated = false;
		defaultScale = this.transform.localScale;
		enlargedScale = 1.25f * defaultScale;
	}

	void Update ()
	{
		
	}

	public bool Activated
	{
		get { return activated; }
	}

	public int Layer
	{
		get { return layer; }
		set { layer = value; }
	}
		
	public void Select()
	{
		checkOtherLayers ();
		//NOTE: This makes the object super small when it is applied to a shortcut on the toolbelt
		//this.transform.localScale = enlargedScale;
		activateNextLevel ();
		activatePreviousLevels();
	}

	public void checkOtherLayers()
	{
		
		foreach (KeyValuePair<GameObject, int> kvp in GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap)
		{
			GameObject g = kvp.Key;
			int i = kvp.Value;

			if (i > GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap[orig])
				g.SetActive (false);

			if(GameObject.Find("Plane").GetComponent<ParentToChild>().parentToChild.ContainsKey(g))
			{
				if (!GameObject.Find ("Plane").GetComponent<ParentToChild> ().parentToChild [g].Contains(orig))
					g.transform.localScale = defaultScale;
			}
			g.transform.localScale = defaultScale;
		}
	}

	void activateNextLevel()
	{
		GameObject g = GameObject.Find ("Plane");

		for (int i= 0; i < g.GetComponent<ParentToChild>().parentToChild[orig].Count; i++)
			g.GetComponent<ParentToChild>().parentToChild[orig][i].SetActive (true);

		activated = true;
	}

	void activatePreviousLevels()
	{
		Stack<GameObject> stack = new Stack<GameObject>();
		stack.Push (orig);

		// Create stack of parent objects leading up to "Top". Then go through each object, starting from "Top,
		// and activate the next layer. This ensures that all layers will be activated if a shortcut is created 
		// for an item that is deep in the meny hierarchy
		while (stack.Peek() != GameObject.Find ("Top")) 
		{
			// Loop through all menu items to find the parent of the current
			//Note: This is pretty brute force. If performance ever takes a hit, this should be redone to be more efficient
			foreach (KeyValuePair<GameObject, List<GameObject>> kvp in GameObject.Find ("Plane").GetComponent<ParentToChild> ().parentToChild) 
			{
				GameObject g = kvp.Key;
				List<GameObject> children = kvp.Value;	
				foreach(GameObject c in children)
				{
					// Push parent to stack
					if (stack.Peek () == c)
						stack.Push (g);
				}
			}
		}

		// Pop top since it does not have SelectionBehavior
		stack.Pop ();

		// Perform "selection" on each object leading up to the current menu item being selected
		while (stack.Count != 0) 
		{
			stack.Peek ().GetComponent<SelectionBehavior> ().checkOtherLayers ();
			stack.Pop ().GetComponent<SelectionBehavior> ().activateNextLevel ();
		}
	}

	public void action(Vector3 controllerPos)
	{
		switch (orig.name)
		{
			case "Shut Down":
				UnityEditor.EditorApplication.isPlaying = false;
				break;
			case "Color Palette":
				GameObject.Find ("ColorPicker").transform.position = controllerPos + new Vector3 (0.15f, 0.3f, 0.2f);
				break;
			default:
				break;
		}
	}


	/*public void incrLevel(GameObject parent)
    {
		if (parent.transform.name == "Top")
            layer = 1;
        else
			layer = parent.GetComponent<SelectionBehavior>().Layer + 1;
    }*/
}