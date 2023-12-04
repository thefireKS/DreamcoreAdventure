using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapPlayer : MonoBehaviour
{
    private void Awake()
    {
        if(!SceneManager.GetSceneByName("SC_player").isLoaded)
            SceneManager.LoadSceneAsync("SC_player", LoadSceneMode.Additive);
        
        Destroy(this);
    }
}
