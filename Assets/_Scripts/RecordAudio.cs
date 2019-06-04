using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        
            foreach (var device in Microphone.devices)
            {
                Debug.Log("Name: " + device);
            }
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start("Realtek High Definition Audio", true, 10, 44100);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}