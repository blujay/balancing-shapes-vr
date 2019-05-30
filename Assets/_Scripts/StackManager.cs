using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StackManager : MonoBehaviour
{

    public bool savedStack = false;

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
        var savePos = new Dictionary<string, Vector3>();
        var saveRot = new Dictionary<string, Vector3>();
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {
            savePos[shape.gameObject.name] = shape.transform.position;
            saveRot[shape.gameObject.name] = shape.transform.eulerAngles ;
            //shape.GetComponent<Rigidbody>().isKinematic = true;
        }

        string fileNamePos = Path.Combine(Application.persistentDataPath, "positions.json");
        string posJson = JsonConvert.SerializeObject(savePos);
        File.WriteAllText(fileNamePos, posJson);

        string fileNameRot = Path.Combine(Application.persistentDataPath, "rotations.json");
        string rotJson = JsonConvert.SerializeObject(saveRot);
        File.WriteAllText(fileNameRot, rotJson);
    }

    public void LoadStack()
    {
        string fileNamePos = Path.Combine(Application.persistentDataPath, "positions.json");
        string newPosJson = File.ReadAllText(fileNamePos);
        string fileNameRot = Path.Combine(Application.persistentDataPath, "rotations.json");
        string newRotJson = File.ReadAllText(fileNameRot);
        var savedPositions = JsonConvert.DeserializeObject<Dictionary<string, Vector3>>(newPosJson);
        var savedRotations = JsonConvert.DeserializeObject<Dictionary<string, Vector3>>(newRotJson);
        Debug.Log(savedPositions.Values.Count);
        foreach (GameObject shape in GameObject.FindGameObjectsWithTag("shape"))
        {
            //shape.GetComponent<Rigidbody>().isKinematic = true;
            Debug.Log($"Name: {shape.gameObject.name}");
            Debug.Log($"Position: {savedPositions[shape.gameObject.name]}");
            shape.gameObject.transform.position = savedPositions[shape.gameObject.name];
            shape.gameObject.transform.eulerAngles = savedRotations[shape.gameObject.name];
        }
    }

}
