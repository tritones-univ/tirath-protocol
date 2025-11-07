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
  public AudioClip efecto;
}


public class IntroSequence : MonoBehaviour
{
  [Header("Referencia UI")]
  public Image panelFondo;
  public TextMeshProUGUI textoPrincipal;
  public TextMeshProUGUI textoTitulo;
  public Image panelNegro;
  public Image panelTexto;

  [Header("Botones Finales")]
  public Button botonReintentar;
  public Button botonSalir;

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
    if (textoTitulo == null) textoTitulo.alpha = 0;
    if (botonReintentar != null) botonReintentar.gameObject.SetActive(false);
    if (botonSalir != null) botonSalir.gameObject.SetActive(false);

    StartCoroutine(ReproducirIntro());
  }

  IEnumerator ReproducirIntro()
  {
    foreach (var slide in diapositivas)
    {
      // Fade out antes de cambiar
      yield return StartCoroutine(FadeImage(panelFondo, 0f, 1.5f));

      // Cambiar imagen
      panelFondo.sprite = slide.imagen;

      // Fade in nueva imagen
      yield return StartCoroutine(FadeImage(panelFondo, 1f, 1.5f));
      textoPrincipal.text = "";

      // Fade in texto
      textoPrincipal.CrossFadeAlpha(0, 0, true);
      textoPrincipal.CrossFadeAlpha(1, 1f, false);

      if (slide.efecto != null && fxSource != null)
        fxSource.PlayOneShot(slide.efecto);

      // Escribir texto con efecto
      yield return StartCoroutine(EscribirTexto(slide.texto, 0.05f, textoPrincipal));

      // Esperar duración
      yield return new WaitForSeconds(slide.duracion);
    }
    // --- Comportamiento al terminar depende de la escena ---
    string escenaActual = SceneManager.GetActiveScene().name;
    if (escenaActual == "IntroScene")
    {
      yield return StartCoroutine(FinalIntro());
      SceneManager.LoadScene(siguienteEscena);
    }
    else if (escenaActual == "BadEndScene")
    {
      yield return StartCoroutine(FinalMalo());
    }
    else if (escenaActual == "GodEndScene")
    {
      yield return StartCoroutine(FinalBueno());
    }
  }

  IEnumerator FinalIntro()
  {
    yield return StartCoroutine(FadeImage(panelNegro, 1f, 0.5f));
    yield return new WaitForSeconds(1f);
    textoTitulo.text = "";
    textoTitulo.alpha = 1;
    StartCoroutine(EscribirTexto("TIRATH PROTOCOL", 0.2f, textoTitulo));
    yield return new WaitForSeconds(6f);
  }

  IEnumerator FinalMalo()
  {
    // Desvanecer el panel del texto
    textoPrincipal.text = "";
    yield return StartCoroutine(FadeImage(panelFondo, 0f, 5f));
    yield return StartCoroutine(FadeImage(panelTexto, 0f, 5f));
    yield return new WaitForSeconds(1f);

    // Mostrar mensaje de "MISIÓN FALLIDA"
    textoTitulo.text = "";
    textoTitulo.alpha = 1;
    yield return StartCoroutine(EscribirTexto("MISIÓN FALLIDA", 0.15f, textoTitulo));

    yield return new WaitForSeconds(1.5f);
    // Mostrar botones
    botonSalir.gameObject.SetActive(true);
  }
  IEnumerator FinalBueno()
  {
    // Desvanecer el panel del texto
    textoPrincipal.text = "";
    yield return StartCoroutine(FadeImage(panelFondo, 0f, 5f));
    yield return StartCoroutine(FadeImage(panelTexto, 0f, 5f));
    yield return new WaitForSeconds(1f);

    // Mostrar mensaje de "MISIÓN FALLIDA"
    textoTitulo.text = "";
    textoTitulo.alpha = 1;
    yield return StartCoroutine(EscribirTexto("MISIÓN CUMPLIDA", 0.15f, textoTitulo));

    yield return new WaitForSeconds(1.5f);
    // Mostrar botones
    botonReintentar.gameObject.SetActive(true);
    botonSalir.gameObject.SetActive(true);
  }

  IEnumerator EscribirTexto(string texto, float velocidad, TextMeshProUGUI textMeshProUGUI)
  {
    textMeshProUGUI.text = "";
    foreach (char c in texto)
    {
      textMeshProUGUI.text += c;
      yield return new WaitForSeconds(velocidad);
    }
  }

  IEnumerator FadeImage(Image img, float targetAlpha, float fadeSpeed)
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
  public void ReiniciarJuego()
  {
    StopAllCoroutines();
    SceneManager.LoadScene("Base");
  }
  public void InicioJuego()
  {
    StopAllCoroutines();
    SceneManager.LoadScene("Base");
  }
}
