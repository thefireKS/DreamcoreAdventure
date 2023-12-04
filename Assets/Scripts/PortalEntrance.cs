using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEntrance : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;
    [SerializeField] private string cameraToPortalName;
    [SerializeField] private float Ypos;

    public static event Action OnTeleportation;
    
    private void OnTriggerEnter(Collider other)
    {
        Vector3 newPos = Vector3.zero;
        if (!other.CompareTag("Player") || doorGameObject.activeSelf) return;
        var cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var camera in cameras)
        {
            if (camera.gameObject.name == cameraToPortalName)
            {
                newPos = camera.transform.position;
                newPos.y = Ypos;
                camera.gameObject.SetActive(false);
                break;
            }
        }
        other.transform.position = newPos;
        SceneManager.UnloadSceneAsync(gameObject.scene);
        OnTeleportation?.Invoke();
    }
    
    
}
