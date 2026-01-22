using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] string nextScene = "";

    public void Change()
    {
        Change(nextScene);
    }

    public void Change(string s) { 
        SceneManager.LoadScene(s);
    }
}
