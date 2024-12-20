using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] private string newGameLevel = "StartGame";
    public void NewGameButton()
    {
        SceneManager.LoadScene(newGameLevel);
    }
}
