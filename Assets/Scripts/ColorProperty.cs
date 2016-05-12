using UnityEngine;
using System.Collections;

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
        this.transform.localScale = enlargeScale;
    }

    public void Shrink()
    {
        this.transform.localScale = defaultScale;
    }

    public void ChangeMenuColors()
    {
        GameObject[] menus = GameObject.FindGameObjectsWithTag("MenuItem");
        GameObject[] texts = GameObject.FindGameObjectsWithTag("Text");

        foreach (GameObject m in menus)
        {
            m.GetComponent<Renderer>().material = colorMat;
        }

        foreach(GameObject t in texts)
        {
            if (this.gameObject.name == "WhiteColor")
                t.GetComponent<TextMesh>().color = Color.black;
            else
                t.GetComponent<TextMesh>().color = Color.white;
        }

        
    }
}
