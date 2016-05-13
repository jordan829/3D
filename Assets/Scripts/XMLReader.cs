using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class XMLReader : MonoBehaviour
{
    string curElement;
    public GameObject top;
	public GameObject menuItem;
    Stack<string> layers;
    public string topTitle;
    public bool doneParsing;
    ParentToChild ptc;

    public class MenuItem
    {
        public string name;
        public Transform t;
        public MenuItem parent;
        public List<MenuItem> subFolders;

        public MenuItem()
        {
            name = "";
            t = null;
            subFolders = new List<MenuItem>();
        }

        public void addChild(string n, Transform prefab)
        {
            MenuItem toAdd = new MenuItem();
            toAdd.name = n;
            toAdd.t = Instantiate(prefab);
            subFolders.Add(toAdd);
        }
        
    }
       
	void Start ()
    {
        layers = new Stack<string>();
        TextAsset xmlText = Resources.Load("menu") as TextAsset;
        XmlTextReader reader = new XmlTextReader(new StringReader(xmlText.text));
        ptc = GameObject.Find("Plane").GetComponent<ParentToChild>();
        topTitle = "";
        doneParsing = false;
        curElement = "";
        parseXML(reader);
        randomPlace();
        //ptc.PrintDictionary();
		ptc.assignLayers(GameObject.Find(topTitle));
        //printLayers();
		//GameObject.Find("Top").GetComponent<MenuSetup>().setUp();
	}
	
	void Update ()
    {
		//GameObject.Find("Main Menu").GetComponent<SelectionBehavior>().Select();

	}

    void parseXML(XmlTextReader reader)
    {
        GameObject menu;

        while(reader.Read())
        {
            switch(reader.NodeType)
            {
                case XmlNodeType.Element:
                    curElement = reader.Name;
                    if (curElement == "menu")
                    {
                        menu = Instantiate(top, Vector3.zero, Quaternion.identity) as GameObject;
                        reader.ReadToFollowing("title");
                        reader.Read();
                        menu.name = reader.Value;
                        layers.Push(reader.Value);
                        topTitle = reader.Value;
                        doneParsing = true;
                    }

                    if (curElement == "folder")
                    {
                        reader.ReadToFollowing("title");
                        reader.Read();
                        string selfName = reader.Value;
						GameObject parent = GameObject.Find(layers.Peek());
						if (parent == null)
							Debug.Log ("null");
                        menu = Instantiate (menuItem) as GameObject;
                        menu.name = selfName;
                        ptc.AddPath(parent, menu);
                        layers.Push(reader.Value);
                    }

                    break;

                case XmlNodeType.Text:
                    break;

                case XmlNodeType.EndElement:
                    curElement = reader.Name;
                    if (curElement != "title")
                        layers.Pop();
                    break;
            }
        }
    }

    public void randomPlace()
    {
        float count = 0f;

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("MenuItem"))
        {
            count += 1.5f;
            g.transform.position = new Vector3(-count, 1, 1);
        }
    }

    public void printLayers()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("MenuItem"))
        {
            Debug.Log(g + ": " + g.GetComponent<SelectionBehavior>().Layer);
        }
    }
}
