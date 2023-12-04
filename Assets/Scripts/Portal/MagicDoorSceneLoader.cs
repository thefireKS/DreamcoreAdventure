using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicDoorSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private PortalEntrance myEntrance;

    public static event Action LoadScene;

    private bool _isLoading;

    private async void OnMouseDown()
    {
        await AsyncLoadScene(sceneName);
        myEntrance.SetActive();
    }

    private async Task AsyncLoadScene(string scene)
    {
        if (_isLoading) return;

        _isLoading = true;
        
        Debug.Log("Async load Scene");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            await Task.Delay(100);
        }
        
        LoadScene?.Invoke();
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));

        transform.gameObject.SetActive(false);

        _isLoading = false;
    }
}
