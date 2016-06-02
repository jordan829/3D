using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParentToChild : MonoBehaviour
{
	public Dictionary<GameObject, List<GameObject>> parentToChild = new Dictionary<GameObject, List<GameObject>>();

	void Start ()
    {
	}
	
	void Update ()
    {
		if (Input.GetKey (KeyCode.Alpha4))
			PrintLeaves ();
	}

    public void AddPath(GameObject parent, GameObject child)
    {
        if (parentToChild.ContainsKey(parent))
        {
			List<GameObject> list = parentToChild[parent];

            if (list.Contains(child) == false)
            {
                list.Add(child);
            }
        }

        else
        {
            List<GameObject> list = new List<GameObject>();
            list.Add(child);
            parentToChild.Add(parent, list);

        }

		if(!parentToChild.ContainsKey(child)){
			List<GameObject> list1 = new List<GameObject>();
			parentToChild.Add (child, list1);
		}
    }

    public void PrintDictionary()
    {
        foreach (KeyValuePair<GameObject, List<GameObject>> kvp in parentToChild)
        {
            GameObject parent = kvp.Key;
            List<GameObject> children = kvp.Value;
            
            for(int i = 0; i < children.Count; i++)
            {
                Debug.Log(parent + " -> " + children[i]);
            }            

        }
    }

	public void PrintLeaves()
	{
		Debug.Log ("Leaves:");

		foreach (KeyValuePair<GameObject, List<GameObject>> kvp in parentToChild)
		{
			GameObject parent = kvp.Key;

			if (parent.GetComponent<SelectionBehavior> ().Leaf == true)
				Debug.Log (parent.name);
		}
	}

	public void CheckLeaves()
	{
		foreach (KeyValuePair<GameObject, List<GameObject>> kvp in parentToChild)
		{
			GameObject parent = kvp.Key;
			List<GameObject> children = kvp.Value;

			if(children.Count == 0)
				parent.GetComponent<SelectionBehavior>().Leaf = true;
		}

		List<GameObject> menus = new List<GameObject> ();
		List<GameObject> texts = new List<GameObject>();

		foreach (KeyValuePair<GameObject, int> kvp in GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap)
		{
			GameObject g = kvp.Key;
			bool leaf = g.GetComponent<SelectionBehavior> ().Leaf;

			for (int i = 0; i < g.transform.childCount; i++)
			{
				GameObject child = g.transform.GetChild (i).gameObject;

				//if (this.gameObject.name == "WhiteColor")
				//	child.GetComponent<TextMesh>().color = Color.black;
				//else
				//	child.GetComponent<TextMesh>().color = Color.white;

				if(leaf)
					child.GetComponent<TextMesh>().color = Color.red;
			}
		}  
	}

    /*public void assignLayers(GameObject root)
    {
        if (parentToChild.ContainsKey(root))
        {
            List<GameObject> rootList = parentToChild[root];

            for (int i = 0; i < rootList.Count; i++)
            {
                GameObject curChild = rootList[i];
                curChild.GetComponent<SelectionBehavior>().incrLevel(root);
                assignLayers(curChild);
            }
        }
    }*/
}