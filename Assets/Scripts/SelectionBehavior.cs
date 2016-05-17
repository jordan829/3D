using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionBehavior : MonoBehaviour
{
	public bool activated;
	public int layer;
	XMLReader xmlr;
	Vector3 defaultScale;
	public Vector3 enlargedScale;

	void Start ()
	{
		layer = 0;
		activated = false;
        xmlr = GameObject.Find("ReadXML").GetComponent<XMLReader>();
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
		this.transform.localScale = enlargedScale;
		activateNextLevel ();
	}

	public void checkOtherLayers()
	{
		foreach (KeyValuePair<GameObject, int> kvp in GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap)
		{
			GameObject g = kvp.Key;
			int i = kvp.Value;

			if (i > GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap[this.gameObject])
				g.SetActive (false);

			if(GameObject.Find("Plane").GetComponent<ParentToChild>().parentToChild.ContainsKey(g))
			{
				if (!GameObject.Find ("Plane").GetComponent<ParentToChild> ().parentToChild [g].Contains(this.gameObject))
					g.transform.localScale = defaultScale;
			}
			//g.transform.localScale = defaultScale;
		}
	}

	void activateNextLevel()
	{
		GameObject g = GameObject.Find ("Plane");

		foreach (KeyValuePair<GameObject, List<GameObject>> kvp in GameObject.Find("Plane").GetComponent<ParentToChild>().parentToChild)
		{
			GameObject parent = kvp.Key;
			List<GameObject> children = kvp.Value;

			for(int i = 0; i < children.Count; i++)
			{
				Debug.Log(parent.name + " -> " + children[i].name);
			}            

		}

		for (int i= 0; i < g.GetComponent<ParentToChild>().parentToChild[this.gameObject].Count; i++)
			g.GetComponent<ParentToChild>().parentToChild[this.gameObject][i].SetActive (true);

		activated = true;
	}

	public void action(Vector3 controllerPos)
	{
		switch (this.gameObject.name)
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