3DUI Integrated Project

5-11-16:
I redid some of the XML stuff to make it more flexible. The menu items are no longer in a gameobject tree,
they are all independent of each other. The parent to child relationships are stored in a dictionary in ParentToChild.cs,
mapping each parent gameobject to a list of its children gameobjects:
	
	Dictionary<GameObject, List<GameObject>> parentToChild : parent.gameObject -> child.gameObject
	ParentToChild ptc = GameObject.Find("Plane").GetComponent<ParentToChild>()

	So for example, parentToChild[GameObject.Find("Top")] = <(GameObject) Main Menu, (GameObject) Settings, (GameObject) Power> 
    (Kevin you can use this dictionary to access the menu items and place them wherever you want on your sphere). Right now they 
    are just placed in random positions so we can see them.

I also assigned a layer to each menu item (using DFS starting at the top node). This will make it easier to dispay the hierarchy when the
user tries to interact with it. This is attached to each SelectionBehavior script:
	
	GameObject.FindGameObjectsWithTag("MenuItem").GetComponent<SelectionBehavior>.Layer

You can use these helper methods to print out the parsed xml / data structures:

	ParentToChild.printDictionary()
	XMLReader.printLayers()

- Payam

Update (Jordan / Jonathan): Dictionary was changed to contain references to gameobjects instead of strings. This was done to take care of the 
issue of not being able to find a gameobject using GameObject.Find("Name") for a gameobject that has been set inactive. Additionally, 
ParentToChild.cs has been deactivated on "Hierarchy" and placed on the "Plane" gameobject. There was an issue with the dictionary 
getting cleared after XMLReader.Start() when attached to "Hierarchy". If someone can figure out why the dictionary is not persistent 
when attached to "Hierarchy", feel free to revert this change.