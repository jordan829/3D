using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System;

public class Texture3DRenderer : MonoBehaviour
{
    public Transform TexturePosition;
    Texture3D tex;
    Material mat;
    public Shader shader;
    public string FilePath = "Resources/head/head-pgm";
    public string FileNamePrefix = "head-";
    public string FileTypeExtension = ".pgm";
    public int FileLength = 3;
    public int Width = 256, Height = 256, Depth = 128;
    public int Multiplier = 1;

    // lookup table
    public Color[] lookUpTable = new Color[256];
    public Color[] lookUpQuick = new Color[8];
    public bool enableLookUpQuick = true;
    Texture2D lookUpProxy;

    // Use this for initialization
    void Start()
    {
        tex = new Texture3D(Width, Height, Depth, TextureFormat.Alpha8, false);

        Color[] newC = new Color[Width * Height * Depth];
        float oneOverWidth = 1.0f / (1.0f * Width - 1.0f);
        float oneOverHeight = 1.0f / (1.0f * Width - 1.0f);
        float oneOverDepth = 1.0f / (1.0f * Width - 1.0f);

        string path = Path.Combine(Application.dataPath, FilePath);

        for (int i = 0; i < Depth; i++)
        {
            // load ppm/pgm file
            switch (FileTypeExtension)
            {
                case ".pgm":
                    {
                        string filePath = string.Format("{0}/{1}{2:d" + FileLength + "}{3}", path, FileNamePrefix, i * Multiplier, FileTypeExtension);
                        if (File.Exists(filePath))
                        {
                            using (StreamReader sr = new StreamReader(filePath))
                            {
                                string format = sr.ReadLine();
                                string heightWidth = sr.ReadLine();
                                string maxBitS = sr.ReadLine();
                                int maxBit;
                                int.TryParse(maxBitS, out maxBit);
                                byte[] bytes = new byte[256];

                                // read bytes
                                for (int k = 0; k < Height; k++)
                                {
                                    //Debug.Log("Reading line K = " + k);
                                    sr.BaseStream.Read(bytes, 0, Width);

                                    for (int j = 0; j < Width; j++)
                                    {
                                        float val = (float)bytes[j] / (float)maxBit;
                                        newC[j + (k * Width) + (i * Width * Height)]
                                            = new Color(1, 1, 1, val);
                                    }
                                }
                            }
                        }
                        else
                            Debug.LogFormat("File '{0}' does not exist!", filePath);
                    }
                    break;
                case ".tif":
                    try
                    {
                        string filePath = string.Format("{0}/{1}{2:d" + FileLength + "}", FilePath, FileNamePrefix, i * Multiplier);
                        object obj = Resources.Load(filePath);
                        if (obj != null)
                        {
                            if (obj is Texture2D)
                            {
                                Texture2D text = (Texture2D)obj;
                                for (int k = 0; k < Height; k++)
                                {
                                    for (int j = 0; j < Width; j++)
                                    {
                                        newC[j + (k * Width) + (i * Width * Height)]
                                            = text.GetPixel(j, k);
                                    }
                                }
                            }
                            else
                                Debug.Log("File is not Texture2D, it is " + obj.GetType());
                        }
                        else
                            Debug.LogFormat("Resource '{0}' does not exist!", filePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                    }
                    break;
            }

        }

        tex.SetPixels(newC);
        tex.Apply();

        Renderer renderer = GetComponent<Renderer>();
        mat = renderer.material;
        mat.shader = shader;
        mat.SetTexture("_Volume", tex);
        mat.SetFloat("_Depth", Depth);


        //setup lookUpTable
        lookUpProxy = new Texture2D(256, 1);
        for (int i = 0; i < 256; i++)
        {

            lookUpTable[i] = new Color(0.5f, 0.5f, 0, 1);
            lookUpQuick[i / 32] = new Color(0.5f, 0.5f, 0, 1);
        }
        //lookUpTable[0] = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enableLookUpQuick)
        {
            for (int i = 0; i < 256; i++)
            {
                lookUpTable[i] = lookUpQuick[i / 32];
            }
        }
        lookUpProxy.SetPixels(lookUpTable, 0);
        lookUpProxy.Apply();
        mat.SetTexture("_Lookup", lookUpProxy);
    }
    public void OnPostRender()
    {

        //GL.LoadPixelMatrix();
        //GL.Viewport(new Rect(0, 0, Screen.width, Screen.height));
        //GL.Color(new Color(1f, 0.0f, 0.0f, 1f));
        GL.Begin(GL.QUADS);
        mat.SetPass(0);
        mat.SetMatrix("_Transform", TexturePosition.transform.localToWorldMatrix);

        for (int i = 0; i < Depth; i++)
        {
            float d = (float)i / Depth;
            GL.Vertex3(0, 0, d);
            GL.Vertex3(0, 1, d);
            GL.Vertex3(1, 1, d);
            GL.Vertex3(1, 0, d);
        }
        GL.End();
    }
}
