using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraRotation : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y,
            mainCamera.transform.position.z);
    }
}
