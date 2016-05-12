using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParentToChild : MonoBehaviour
{
	public Dictionary<GameObject, List<GameObject>> parentToChild;

	void Start ()
    {
        parentToChild = new Dictionary<GameObject, List<GameObject>>();
	}
	
	void Update ()
    {
	
	}

    public void AddPath(GameObject parent, GameObject child)
    {
		if (parentToChild == null)
			parentToChild = new Dictionary<GameObject, List<GameObject>>();
		
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

    public void assignLayers(GameObject root)
    {
        if (parentToChild.ContainsKey(root))
        {
            List<GameObject> rootList = parentToChild[root];

            for (int i = 0; i < rootList.Count; i++)
            {
                GameObject curChild = rootList[i];
                //GameObject curChild = GameObject.Find(curChildName);
                curChild.GetComponent<SelectionBehavior>().incrLevel(root);
                assignLayers(curChild);
            }
        }
    }
}
