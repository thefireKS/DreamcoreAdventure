using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapPlayer : MonoBehaviour
{
    public static event Action playerLoaded;

    public static bool isLoaded;
    
    private void Awake()
    {
        if (!SceneManager.GetSceneByName("SC_player").isLoaded)
        {
            SceneManager.LoadSceneAsync("SC_player", LoadSceneMode.Additive);
            isLoaded = true;
        }
        
        Destroy(this);
    }
}
