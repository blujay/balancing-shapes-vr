using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    // Start is called before the first frame update
    void OnApplicationQuit()
    {
#if UNITY_EDITOR
        //UnityEditor.SceneManagement.EditorSceneManager.SaveScene(gameObject.scene);
#endif
    }


}
