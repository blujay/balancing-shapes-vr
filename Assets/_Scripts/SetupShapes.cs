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
        if (Application.isPlaying) return;
        Debug.Log("Setting up shapes");
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {
            var ss = shape.GetComponent<ShapeScript>();
            if (ss == null)
            {
                ss = shape.AddComponent<ShapeScript>();
            }

            if (ss.parentPrefab == null || reset)
            {
                var parent = AssetDatabase.LoadAssetAtPath<Transform>(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(shape));
                ss.parentPrefab = parent;
                Debug.Log(ss.parentPrefab);
            }
        }
    }

}

