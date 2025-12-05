using UnityEngine;
using System.Collections;

public class IntroPanelController : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Animator component dari panel intro")]
    public Animator introAnimator;
    
    [Tooltip("Durasi animasi intro (dalam detik)")]
    public float introDuration = 2f;
    
    [Tooltip("Nama state animasi intro di Animator")]
    public string introAnimationName = "IntroIn";

    [Header("Auto Settings")]
    [Tooltip("Otomatis matikan panel setelah animasi selesai")]
    public bool autoHideAfterIntro = true;

    void Start()
    {
        // Auto-assign animator jika belum diassign
        if (introAnimator == null)
        {
            introAnimator = GetComponent<Animator>();
        }

        // PENTING: Set animator ke UnscaledTime mode
        // Agar animasi tetap jalan meskipun Time.timeScale = 0
        if (introAnimator != null)
        {
            introAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            Debug.Log("✅ Animator mode diset ke UnscaledTime - Animasi tidak terpengaruh pause");
        }
        else
        {
            Debug.LogError("❌ IntroPanelController: Animator tidak ditemukan!");
        }
    }

    /// <summary>
    /// Mainkan animasi intro dengan durasi yang tepat
    /// Dipanggil dari script lain (misal: BriefingSequence)
    /// </summary>
    public void PlayIntro()
    {
        StartCoroutine(PlayIntroCoroutine());
    }

    /// <summary>
    /// Coroutine untuk menjalankan animasi intro
    /// </summary>
    IEnumerator PlayIntroCoroutine()
    {
        Debug.Log($"🎬 Memulai animasi intro: {introAnimationName}");

        // Pastikan panel aktif
        gameObject.SetActive(true);

        // Play animasi
        if (introAnimator != null && !string.IsNullOrEmpty(introAnimationName))
        {
            introAnimator.Play(introAnimationName);
        }

        // PENTING: Gunakan WaitForSecondsRealtime (tidak terpengaruh Time.timeScale)
        // Jangan pakai WaitForSeconds biasa!
        yield return new WaitForSecondsRealtime(introDuration);

        Debug.Log("✅ Animasi intro selesai");

        // Matikan panel setelah animasi selesai
        if (autoHideAfterIntro)
        {
            gameObject.SetActive(false);
            Debug.Log("📴 Panel intro dimatikan");
        }
    }

    /// <summary>
    /// Stop intro secara paksa (untuk skip button)
    /// </summary>
    public void StopIntro()
    {
        StopAllCoroutines();
        
        if (autoHideAfterIntro)
        {
            gameObject.SetActive(false);
        }
        
        Debug.Log("⏹️ Intro dihentikan paksa");
    }

    /// <summary>
    /// Set durasi intro secara dinamis dari script lain
    /// </summary>
    public void SetDuration(float duration)
    {
        introDuration = duration;
        Debug.Log($"⏱️ Durasi intro diubah menjadi: {duration}s");
    }
}
