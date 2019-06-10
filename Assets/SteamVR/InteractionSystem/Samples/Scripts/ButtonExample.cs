using UnityEngine;
using System.Collections;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ButtonExample : MonoBehaviour
    {

        public GameObject prefab;
        private Hand hand;


        public void DoPlant()
        {
            GameObject planting = GameObject.Instantiate<GameObject>(prefab);
            planting.transform.position = this.transform.position;
            planting.transform.rotation = this.transform.localRotation;
            planting.transform.name = planting.transform.name + Time.time;
        }


        //Rigidbody rigidbody = planting.GetComponent<Rigidbody>();
        // if (rigidbody != null)
        //rigidbody.isKinematic = true;


        //Vector3 initialScale = Vector3.one * 0.01f;
        //Vector3 targetScale = Vector3.one * (1 + (Random.value * 0.25f));

        //float startTime = Time.time;
        //float overTime = 0.5f;
        //float endTime = startTime + overTime;

        //while (Time.time < endTime)
        //{
        // planting.transform.localScale = Vector3.Slerp(initialScale, targetScale, (Time.time - startTime) / overTime);
        // yield return null;
        //}


        //  if (rigidbody != null)
        //rigidbody.isKinematic = false;
    }
    }