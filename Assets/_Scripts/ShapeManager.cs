using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    private Dictionary<string, Transform> prefabs;
    public bool savedStack = false;
    public string saveFilename = "prefabData.json";

    void Start()
    {
          LoadStack();
    }

    void OnApplicationQuit() {
        SaveStack();
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
            item.parentPrefab = shape.GetComponent<ShapeScript>().parentPrefab;

            saves[shape.gameObject.name] = item;
            //shape.GetComponent<Rigidbody>().isKinematic = true;
        }

        string fileNamePos = Path.Combine(Application.persistentDataPath, saveFilename);
        string json = JsonConvert.SerializeObject(saves);
        File.WriteAllText(fileNamePos, json);

    }

    public void LoadStack()
    {
        
        prefabs = new Dictionary<string, Transform>();
        string fileName = Path.Combine(Application.persistentDataPath, saveFilename);
        string newJson;
        try
        {
            newJson = File.ReadAllText(fileName);
        }
        catch (FileNotFoundException)
        {
            return;
        }
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
                prefabs[shape.GetComponent<ShapeScript>().parentPrefab] = shape.transform;
            }
            
        }

        foreach (var key in savedData.Keys) {
            Debug.Log($"Reconstructing {key}");
             if(!shapesInScene.ContainsKey(key))
             {
                 var originalShape = prefabs[savedData[key].parentPrefab];
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
