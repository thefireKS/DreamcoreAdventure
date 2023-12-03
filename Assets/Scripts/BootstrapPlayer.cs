using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapPlayer : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync("SC_player", LoadSceneMode.Additive);
        
        Destroy(this);
    }
}
