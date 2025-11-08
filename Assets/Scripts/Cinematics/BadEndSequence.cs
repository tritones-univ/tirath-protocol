using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class BadEndSequence : MonoBehaviour
{
  [Header("Referencia UI")]
  public Image panelFondo;
  public TextMeshProUGUI textoPrincipal;

  [Header("Audio")]
  public AudioSource musica;
  public AudioSource fxSource;
  [Header("Configuración")]
  public IntroSlide[] diapositivas;
  public string siguienteEscena = "GameScene";
  public float fadeSpeed = 1f;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (musica != null && !musica.isPlaying)
      musica.Play();

    StartCoroutine(ReproducirIntro());
  }

  IEnumerator ReproducirIntro()
  {
    foreach (var slide in diapositivas)
    {
      // Fade out antes de cambiar
      yield return StartCoroutine(FadeImage(panelFondo, 0f));

      // Cambiar imagen
      panelFondo.sprite = slide.imagen;

      // Fade in nueva imagen
      yield return StartCoroutine(FadeImage(panelFondo, 1f));
      textoPrincipal.text = "";

      // Fade in texto
      textoPrincipal.CrossFadeAlpha(0, 0, true);
      textoPrincipal.CrossFadeAlpha(1, 1f, false);

      if (slide.efecto != null && fxSource != null)
        fxSource.PlayOneShot(slide.efecto);

      // Escribir texto con efecto
      yield return StartCoroutine(EscribirTexto(slide.texto, 0.05f));

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

  IEnumerator FadeImage(Image img, float targetAlpha)
  {
    Color color = img.color;
    float startAlpha = color.a;

    for (float t = 0; t < 1; t += Time.deltaTime * fadeSpeed)
    {
      color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
      img.color = color;
      yield return null;
    }

    color.a = targetAlpha;
    img.color = color;
  }

  public void SaltarIntro()
  {
    StopAllCoroutines();
    SceneManager.LoadScene(siguienteEscena);
  }
}
