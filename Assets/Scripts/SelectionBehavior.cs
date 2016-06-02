using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class SelectionBehavior : MonoBehaviour
{
	public bool activated;
	public int layer;
	Vector3 defaultScale;
	public Vector3 enlargedScale;
	public GameObject orig;
	private GameObject Sphere;
	private GameObject Belt;
	private GameObject rightH;

	bool MenuOnOff = true;
	float origDist;

	public bool resizeOn = false;
	bool scale;
	GameObject controller_left, controller_right;

	public bool moveOn = false;
	private bool translate;
	private Vector3 startPos;

	Material defaultMat;
	public Material highlightMat;

	bool isLeaf;

	void Start ()
	{
		defaultMat = this.GetComponent<Renderer> ().material;
		isLeaf = false;
		rightH = GameObject.Find ("Controller (right)");
		Sphere = GameObject.Find ("Sphere");
		layer = 0;
		activated = false;
		defaultScale = this.transform.localScale;
		enlargedScale = 1.2f * defaultScale;
		controller_left = GameObject.Find ("Controller (left)");
		controller_right = GameObject.Find ("Controller (right)");
	}

	void Update ()
	{

		if (orig.name == "Resize") {
			if (resizeOn) {
				this.GetComponent<Renderer> ().material = GameObject.Find ("GreenColor").transform.GetChild(1).GetComponent<Renderer>().material;
			} else {
				this.GetComponent<Renderer> ().material = GameObject.Find ("BlueColor").transform.GetChild(1).GetComponent<Renderer>().material;

				scale = false;
			}

			if (resizeOn && ViveControl.press && ViveControl.toChange != null) {
				origDist = Vector3.Distance (controller_left.transform.position, controller_right.transform.position);
				scale = true;
			}

			if (scale && ViveControl.hold && ViveControl.toChange != null) {
				float curDist = Vector3.Distance (controller_left.transform.position, controller_right.transform.position);
				//if distance increase, up scale. if it decreases reduce scale
				ViveControl.toChange.transform.localScale *= 1.0f + (curDist - origDist);
				origDist = curDist;
			}
		}
		if (orig.name == "Move") {
			if (moveOn) {
				this.GetComponent<Renderer> ().material = GameObject.Find ("GreenColor").transform.GetChild(1).GetComponent<Renderer>().material;
			} else {
				this.GetComponent<Renderer> ().material = GameObject.Find ("BlueColor").transform.GetChild(1).GetComponent<Renderer>().material;
			}
			if (moveOn && ViveControl.press && ViveControl.toChange != null && ViveControl.domCont != null) {
				startPos = ViveControl.domCont.transform.position;
			}
		

			if (moveOn && ViveControl.hold && ViveControl.toChange != null) {
				Vector3 move = ViveControl.domCont.transform.position - startPos;
				ViveControl.toChange.transform.position += move;
				startPos = ViveControl.domCont.transform.position;
			}
		}

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

	public bool Leaf
	{
		get { return isLeaf; }
		set { isLeaf = value; }
	}
		
	public void Select()
	{
		checkOtherLayers ();
		//NOTE: This makes the object super small when it is applied to a shortcut on the toolbelt
		//this.transform.localScale = enlargedScale;
		//defaultMat = this.GetComponent<Renderer>().material;
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
				if (!GameObject.Find ("Plane").GetComponent<ParentToChild> ().parentToChild [g].Contains (orig))
				{
					g.transform.localScale = defaultScale;
					g.GetComponent<Renderer> ().material = defaultMat; 
				}
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
			//stack.Peek ().GetComponent<SelectionBehavior> ().checkOtherLayers ();
			GameObject blackColor = GameObject.Find ("BlackColor");
			Material blackColorMat = blackColor.transform.GetChild (1).GetComponent<Renderer> ().material;
			stack.Peek().GetComponent<Renderer> ().material = blackColorMat;
			stack.Pop ().GetComponent<SelectionBehavior> ().activateNextLevel ();
		}
	}

	public void action(Vector3 controllerPos)
	{
		//Sphere.gameObject.SetActive (true);

		switch (orig.name)
		{
			case "Shut Down":
				UnityEditor.EditorApplication.isPlaying = false;
				break;
			case "Color Palette":
				GameObject.Find ("ColorPicker").GetComponent<ColorPickerPosition>().toggle();
				break;
			case "Move":
				moveOn = !moveOn;
				if (this.gameObject.name == "Move" && GameObject.Find ("Movecopy") != null)
				{
					GameObject g = GameObject.Find ("Movecopy");
					g.GetComponent<SelectionBehavior> ().moveOn = moveOn;
				}

				else if(this.gameObject.name == "Movecopy")
				{
					orig.GetComponent<SelectionBehavior> ().moveOn = moveOn;
				}
				break;
			case "Resize":
				resizeOn = !resizeOn;
				if (this.gameObject.name == "Resize" && GameObject.Find ("Resizecopy") != null)
				{
					GameObject g = GameObject.Find ("Resizecopy");
					g.GetComponent<SelectionBehavior> ().resizeOn = resizeOn;
				}

				else if(this.gameObject.name == "Resizecopy")
				{
					orig.GetComponent<SelectionBehavior> ().resizeOn = resizeOn;
				}
				break;
			case "MaxMinMenu":
				MenuOnOff = !MenuOnOff;
				Sphere.gameObject.SetActive (MenuOnOff);
				orig.transform.LookAt ( GameObject.Find ("Camera (head)").transform.position);
				break;
			case "ResizeBelt":
				Belt = GameObject.Find ("Belt");
				resetBelt();
				break;
			case "Time":
				GameObject.Find ("Clock").transform.position = GameObject.Find ("Camera (head)").gameObject.transform.position + new Vector3 (-0.25f, -0.25f, -0.5f);
				GameObject.Find ("Clock").transform.LookAt (GameObject.Find ("Camera (head)").transform);
				GameObject.Find ("Clock").transform.Rotate (new Vector3 (0, 180, 0));
				break;
			case "Restart":
				SceneManager.LoadScene (0);
				break;
			default:
			
				break;
		}
	}
	public void turnOnMainMenu()
	{
		Sphere.gameObject.SetActive (true);

	}

	public void resetBelt()
	{
		Belt.transform.position = Vector3.zero;

		for (int i = 0; i < Belt.transform.childCount; i++)
		{
			Belt.transform.GetChild (i).transform.localPosition = Vector3.zero;
		}
		rightH.transform.gameObject.GetComponent<InstantiateBeltLength> ().enabled = true;
		rightH.transform.gameObject.GetComponent<InstantiateBeltLength> ().startMess.SetActive (true);
		Belt.transform.gameObject.GetComponent<menuMove> ().offset = Vector3.zero;
		Belt.SetActive (false);
	}

	public void HoverOn()
	{
		this.transform.localScale = enlargedScale;
	}

	public void HoverOff()
	{
		this.transform.localScale = defaultScale;
	}

	public void HoverDrag()
	{
		this.transform.localScale = this.transform.parent.localScale * 1.2f;
	}

	/*public void incrLevel(GameObject parent)
    {
		if (parent.transform.name == "Top")
            layer = 1;
        else
			layer = parent.GetComponent<SelectionBehavior>().Layer + 1;
    }*/
}