using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorProperty : MonoBehaviour
{
    Vector3 defaultScale;
    Vector3 enlargeScale;
    Material colorMat;

    void Start()
    {
        defaultScale = this.transform.localScale;
        enlargeScale = defaultScale * 1.2f;
        colorMat = this.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
       
    }

    public void Enlarge()
    {
		this.transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f) * 1.2f;
    }

    public void Shrink()
    {
		this.transform.localScale = new Vector3 (0.15f, 0.15f, 0.15f);
    }

    public void ChangeMenuColors()
    {
		List<GameObject> menus = new List<GameObject> ();
		List<GameObject> texts = new List<GameObject>();

		foreach (KeyValuePair<GameObject, int> kvp in GameObject.Find("ReadXML").GetComponent<XMLReader>().layerMap)
		{
			GameObject g = kvp.Key;
			g.GetComponent<Renderer> ().material = colorMat;

			for (int i = 0; i < g.transform.childCount; i++)
			{
				GameObject child = g.transform.GetChild (i).gameObject;

				if (this.gameObject.name == "WhiteColor")
					child.GetComponent<TextMesh>().color = Color.black;
				else
					child.GetComponent<TextMesh>().color = Color.white;
			}
		}     
    }
}
