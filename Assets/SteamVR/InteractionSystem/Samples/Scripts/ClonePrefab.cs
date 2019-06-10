using UnityEngine;
using System.Collections;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ClonePrefab : MonoBehaviour
    {

        public GameObject prefab;
        private Hand hand;
        
        public void Clone()
        {
            GameObject planting = GameObject.Instantiate<GameObject>(prefab);
            planting.transform.position = this.transform.position;
            planting.transform.rotation = this.transform.localRotation;
            planting.transform.name = planting.transform.name + Time.time;
        }
    }
}