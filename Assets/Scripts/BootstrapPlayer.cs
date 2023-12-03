using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapPlayer : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync("Player", LoadSceneMode.Additive);
        
        Destroy(this);
    }
}
