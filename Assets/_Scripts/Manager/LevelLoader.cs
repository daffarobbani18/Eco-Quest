using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [Header("Transition Settings")]
    [Tooltip("CanvasGroup untuk mengontrol Alpha (transparansi) panel transisi")]
    public CanvasGroup transitionPanel;
    
    [Tooltip("Durasi transisi fade in/out dalam detik")]
    public float transitionTime = 1f;

    void Start()
    {
        // Transisi "Reveal" saat game dimulai
        // Panel hitam perlahan menjadi transparan
        if (transitionPanel != null)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogError("❌ LevelLoader: transitionPanel belum di-assign di Inspector!");
        }
    }

    /// <summary>
    /// Memuat scene baru dengan efek transisi crossfade
    /// </summary>
    /// <param name="sceneName">Nama scene yang akan dimuat</param>
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    /// <summary>
    /// Coroutine untuk transisi Fade In (Hitam → Transparan)
    /// Digunakan saat game pertama kali dimulai
    /// </summary>
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        transitionPanel.alpha = 1f; // Mulai dari hitam
        transitionPanel.blocksRaycasts = true; // Block input saat transisi

        // Animasi alpha dari 1 ke 0
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            transitionPanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionTime);
            yield return null; // Wait for next frame
        }

        // Pastikan alpha tepat 0 di akhir
        transitionPanel.alpha = 0f;
        transitionPanel.blocksRaycasts = false; // Unblock input
        
        Debug.Log("✅ Transisi Fade In selesai - UI siap digunakan");
    }

    /// <summary>
    /// Coroutine untuk transisi Fade Out (Transparan → Hitam) lalu load scene
    /// </summary>
    /// <param name="sceneName">Nama scene tujuan</param>
    IEnumerator TransitionToScene(string sceneName)
    {
        Debug.Log($"🔄 Memulai transisi ke scene: {sceneName}");

        // Block input agar pemain tidak bisa klik tombol ganda
        transitionPanel.blocksRaycasts = true;

        float elapsedTime = 0f;

        // Animasi alpha dari 0 ke 1 (Transparan → Hitam)
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            transitionPanel.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionTime);
            yield return null; // Wait for next frame
        }

        // Pastikan alpha tepat 1 di akhir
        transitionPanel.alpha = 1f;

        Debug.Log($"✅ Transisi Fade Out selesai - Loading scene: {sceneName}");

        // Load scene setelah layar sepenuhnya hitam
        SceneManager.LoadScene(sceneName);
    }
}
