using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    public Transform[] ShapeList;
    public bool savedStack = false;
    public string saveFilename = "prefabData.json";

    void Start()
    {
        Debug.Log(Application.persistentDataPath);
          LoadStack();
    }

    void OnApplicationQuit() {
        SaveStack();
        Debug.Log("stack saved");
    }

    public void SaveStack()
    {
        var saves = new Dictionary<string, SavedItem>();
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {
            var item = new SavedItem();
            item.position = shape.transform.position;
            item.rotation = shape.transform.eulerAngles;
            item.scale = shape.transform.localScale.x;
            item.prefabIndex = shape.GetComponent<ShapeScript>().prefabIndex;

            saves[shape.gameObject.name] = item;
            //shape.GetComponent<Rigidbody>().isKinematic = true;
        }

        string fileNamePos = Path.Combine(Application.persistentDataPath, saveFilename);
        string json = JsonConvert.SerializeObject(saves);
        File.WriteAllText(fileNamePos, json);

    }

    public void LoadStack()
    {
        string fileName = Path.Combine(Application.persistentDataPath, saveFilename);
        string newJson = File.ReadAllText(fileName);
        var savedData = JsonConvert.DeserializeObject<Dictionary<string, SavedItem>>(newJson);
        var shapesInScene = new Dictionary<string, GameObject>();
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {

            shapesInScene[shape.name] = shape;
            //shape.GetComponent<Rigidbody>().isKinematic = true;
            if (savedData.ContainsKey(shape.gameObject.name))
            {
                shape.gameObject.transform.position = savedData[shape.gameObject.name].position;
                shape.gameObject.transform.eulerAngles = savedData[shape.gameObject.name].rotation;
                var scale = savedData[shape.gameObject.name].scale;
                shape.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            }
            
        }

        foreach (var key in savedData.Keys) {
             if(!shapesInScene.ContainsKey(key))
            {
                var originalShape = ShapeList[savedData[key].prefabIndex];
                var newObject = Instantiate(originalShape.transform);
                newObject.name = key;
                newObject.transform.position = savedData[key].position;
                newObject.transform.eulerAngles = savedData[key].rotation;
                var scale = savedData[key].scale;
                newObject.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

}
