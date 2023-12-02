using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntrance : MonoBehaviour
{
    [SerializeField] private string cameraToPortalName;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var newPos = GameObject.Find(cameraToPortalName).transform;
        other.transform.position = new Vector3(newPos.position.x, 0, newPos.position.z);
    }
}
