
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrap : MonoBehaviour
{
    [SerializeField] private GameObject loadSymbol;
    private void Awake()
    {
        StartCoroutine(SymbolLoader());
       
    }
    
    private IEnumerator SymbolLoader()
    {
        loadSymbol.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(Scenes.Menu.ToString());

    }
}
