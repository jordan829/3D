3DUI Integrated Project

5-11-16:
I redid some of the XML stuff to make it more flexible. The menu items are no longer in a gameobject tree,
they are all independent of each other. The parent to child relationships are stored in a dictionary in ParentToChild.cs,
mapping the name of each parent gameobject to a list of the names of its children gameobjects:
	
	Dictionary<string, List<string>> parentToChild : parent.gameObject.name -> child.gameObject.name
	ParentToChild ptc = GameObject.Find("Hierarchy").GetComponent<ParentToChild>()

	So for example, parentToChild[top] = <"Main Menu", "Settings", "Power"> (Kevin you can use this dictionary to access the menu items
	and place them wherever you want on your sphere). Right now they are just placed in random positions so we can see them.

I also assigned a layer to each menu item (using DFS starting at the top node). This will make it easier to dispay the hierarchy when the
user tries to interact with it. This is attached to each SelectionBehavior script:
	
	GameObject.FindGameObjectsWithTag("MenuItem").GetComponent<SelectionBehavior>.Layer

You can use these helper methods to print out the parsed xml / data structures:

	ParentToChild.printDictionary()
	XMLReader.printLayers()

- Payam