using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEntrance : MonoBehaviour
{
    [SerializeField] private string cameraToPortalName;

    public static event Action OnTeleportation;

    private bool _isActive;

    public void SetActive()
    {
        _isActive = true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(!_isActive) return;
        
        
        
        Vector3 newPos = Vector3.zero;
        if (!other.CompareTag("Player")) return;
        var cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var cam in cameras)
        {
            if (cam.gameObject.name == cameraToPortalName)
            {
                newPos = cam.transform.position;
                newPos.y = 0;
                cam.gameObject.SetActive(false);
                break;
            }
        }
        other.transform.position = newPos;
        SceneManager.UnloadSceneAsync(gameObject.scene);
        OnTeleportation?.Invoke();
    }
}
