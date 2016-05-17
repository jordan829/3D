using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionBehavior : MonoBehaviour
{
	public bool activated;
	public int layer;
    XMLReader xmlr;

	void Start ()
	{
		layer = 0;
		activated = false;
        xmlr = GameObject.Find("ReadXML").GetComponent<XMLReader>();
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
		set { Debug.Log ("Set " + this.gameObject.name + " to: " + value); layer = value; }
	}

	public void incrLevel(GameObject parent)
    {
		if (parent.transform.name == "Top")
            layer = 1;
        else
			layer = parent.GetComponent<SelectionBehavior>().Layer + 1;
    }

	public void checkOtherLayers()
	{
		foreach (KeyValuePair<GameObject, int> kvp in GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap)
		{
			GameObject g = kvp.Key;
			int i = kvp.Value;

			if (i > GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap[this.gameObject])
				g.SetActive (false);
		}
	}

    public void SmartFlip()
    {
        NextLayersOff();
        checkOtherLayers();
        activateNextLevel();
    }

    public void NextLayersOff()
    {
        GameObject[] menus = GameObject.FindGameObjectsWithTag("MenuItem");

        foreach(GameObject m in menus)
        {
            int curLayer = m.GetComponent<SelectionBehavior>().Layer;
            Debug.Log(m.name + ", " + curLayer);
            if(curLayer > this.layer)
            {
                m.SetActive(false);

                for(int i = 0; i < m.transform.childCount; i++)
                {
                    m.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

	public void Flip()
	{
        foreach(GameObject m in GameObject.FindGameObjectsWithTag("MenuItem"))
        {
            if(m.GetComponent<SelectionBehavior>().layer == this.layer)
            {
                m.GetComponent<SelectionBehavior>().deactivateNextLevel();
            }
        }

		if (activated)
			Deselect ();
		else
			Select ();
	}

	public void Select()
	{
		checkOtherLayers ();
		activateNextLevel ();
	}

	public void Deselect()
	{
		deactivateNextLevel ();
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

	void deactivateNextLevel()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			if(!transform.GetChild(i).gameObject.name.Contains("Text"))
				transform.GetChild (i).gameObject.SetActive (false);
		}

		activated = false;
	}

	void deactivateCurrentLevel()
	{
		for (int i = 0; i < transform.parent.transform.childCount; i++)
			transform.parent.transform.GetChild (i).gameObject.SetActive (false);
		activated = false;
	}
}