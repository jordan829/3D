using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSetup : MonoBehaviour
{
	ParentToChild ptc;
	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	public void deactivateAll()
	{
		Dictionary<GameObject, List<GameObject>>.KeyCollection keyColl = ParentToChild.parentToChild.Keys;
		foreach (GameObject g in keyColl) {
			Debug.Log (g.name);
			if(g.name != "Top")
				g.SetActive (false);
		}

	}

	public void setUp()
	{
		
		// Deactivate all levels of menu
		/*for (int i = 0; i < transform.childCount; i++) 
		{
			deactivateAll (transform.GetChild (i).transform);

			// Reactivate first level
			transform.GetChild (i).gameObject.SetActive (true);
		}*/
		deactivateAll ();
		for(int i = 0; i < ParentToChild.parentToChild[GameObject.Find("Top")].Count; i ++){
			//Debug.Log (ParentToChild.parentToChild [GameObject.Find("Top")] [i]);
			(ParentToChild.parentToChild [GameObject.Find("Top")] [i]).SetActive (true);
		}
}
}
