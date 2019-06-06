using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SetupShapes : MonoBehaviour
{

    public bool reset;

    void Awake()
    {
        DoSetup();
    }

    private void OnValidate()
    {
        DoSetup();
    }

    void DoSetup()
    {
        if (Application.isPlaying)
        {
            return;
        }
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {
            var parent = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(shape);
            var ss = shape.GetComponent<ShapeScript>();
            if (ss == null)
            {
                ss = shape.gameObject.AddComponent<ShapeScript>();
            }

            if (string.IsNullOrEmpty(ss.parentPrefab) || reset)
            {
                shape.GetComponent<ShapeScript>().parentPrefab = parent;
            }
        }
    }

}

