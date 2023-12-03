using UnityEngine;

public class LevelDataHolder : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    private void OnEnable()
    {
        PortalEntrance.OnTeleportation += SetNewMusic;
    }
    
    private void OnDisable()
    {
        PortalEntrance.OnTeleportation -= SetNewMusic;
    }

    private void SetNewMusic()
    {
        MusicManager.instance.ChangeMusic(levelData.Embient);
    }
}
