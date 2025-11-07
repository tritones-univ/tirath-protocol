using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


[System.Serializable]
public class IntroSlide
{
  public Sprite imagen;
  [TextArea(2, 5)] public string texto;
  public float duracion = 8f;
}


public class IntroSequence : MonoBehaviour
{

  public Image panelFondo;
  public TextMeshProUGUI textoPrincipal;
  public AudioSource musica;
  public IntroSlide[] diapositivas;
  public string siguienteEscena = "GameScene";
  public float fadeSpeed = 1f;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    StartCoroutine(ReproducirIntro());
  }

  IEnumerator ReproducirIntro()
  {
    foreach (var slide in diapositivas)
    {
      // Cambia imagen
      panelFondo.sprite = slide.imagen;
      textoPrincipal.text = "";

      // Fade in texto
      textoPrincipal.CrossFadeAlpha(0, 0, true);
      textoPrincipal.CrossFadeAlpha(1, 1f, false);

      // Escribir texto con efecto
      yield return StartCoroutine(EscribirTexto(slide.texto, 0.03f));

      // Esperar duración
      yield return new WaitForSeconds(slide.duracion);
    }

    // Transición final al juego
    SceneManager.LoadScene(siguienteEscena);
  }

  IEnumerator EscribirTexto(string texto, float velocidad)
  {
    textoPrincipal.text = "";
    foreach (char c in texto)
    {
      textoPrincipal.text += c;
      yield return new WaitForSeconds(velocidad);
    }
  }

  public void SaltarIntro()
  {
    StopAllCoroutines();
    SceneManager.LoadScene(siguienteEscena);
  }
}
