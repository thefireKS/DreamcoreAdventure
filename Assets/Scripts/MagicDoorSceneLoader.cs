using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MagicDoorSceneLoader : MonoBehaviour
{
    [SerializeField] private string SceneName;

    private Coroutine currentSceneLoadingCoroutine;

    private void OnMouseDown()
    {
        currentSceneLoadingCoroutine ??= StartCoroutine(AsyncLoadScene(SceneName));
    }

    private IEnumerator AsyncLoadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
        
        currentSceneLoadingCoroutine = null;
        transform.gameObject.SetActive(false);
    }
}
