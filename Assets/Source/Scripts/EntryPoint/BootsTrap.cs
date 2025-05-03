using Assets.Scripts.General;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootsTrap : MonoBehaviour
{
    private IEnumerator Start()
    {
        SceneChangerSingleton.Instance.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneChangerSingleton.Instance.LoadScene(Scenes.Menu.ToString());
    }
}
