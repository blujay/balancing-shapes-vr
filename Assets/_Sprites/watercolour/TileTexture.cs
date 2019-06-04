using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TileTexture : MonoBehaviour
{

    public float BumpScaleFactor = 1f;
    Material mat;
    public Texture m_Normal;
    Renderer m_Renderer;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");
        GetComponent<Renderer>().material.EnableKeyword("_NORMALMAP");
       


    }

    // Update is called once per frame
    void Update()
    {

        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            Debug.Log("The transform has changed!");
            GetComponent<Renderer>().material.SetTexture("_BumpMap", m_Normal);
            GetComponent<Renderer>().material.SetFloat("_BumpScale", BumpScaleFactor);
            transform.hasChanged = false;
        }

    }
}