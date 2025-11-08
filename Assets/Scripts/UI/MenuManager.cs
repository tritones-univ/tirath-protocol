using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsPanel;

    void Start()
    {
        optionsPanel.SetActive(false);
    }
    public void IniciarJuego()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }
    public void HideOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void SalirJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
