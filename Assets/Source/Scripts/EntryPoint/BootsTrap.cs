using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrap : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(Scenes.Menu.ToString());
    }
}
