using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Controller untuk Win Panel - Menampilkan bintang, skor, dan tombol upload leaderboard
/// Attach script ini ke GameObject "Win Panel" di scene
/// </summary>
public class WinPanelController : MonoBehaviour
{
    [Header("Star Display")]
    [Tooltip("Array 3 GameObjects untuk icon bintang (Index 0 = Star 1, Index 1 = Star 2, dst)")]
    public GameObject[] starObjects; // 3 GameObject bintang di UI
    
    [Tooltip("Sprite bintang KOSONG (outline/abu-abu)")]
    public Sprite emptyStar;
    
    [Tooltip("Sprite bintang TERISI (gold/kuning)")]
    public Sprite filledStar;
    
    [Header("New Record Popup")]
    [Tooltip("GameObject popup 'New Record!' (animasi muncul jika beat record)")]
    public GameObject newRecordPopup;
    
    [Header("Leaderboard Button")]
    [Tooltip("Tombol untuk upload skor ke leaderboard")]
    public Button uploadLeaderboardButton;
    
    [Tooltip("Text di button (ubah ke 'Uploading...' saat proses)")]
    public TMP_Text uploadButtonText;
    
    [Header("Animation Settings")]
    [Tooltip("Delay antar bintang muncul (detik)")]
    public float starAnimationDelay = 0.3f;
    
    [Tooltip("Durasi animasi scale per bintang (detik)")]
    public float starAnimationDuration = 0.2f;
    
    [Tooltip("Scale akhir bintang (1f = normal, 1.2f = sedikit membesar)")]
    public float starFinalScale = 1f;
    
    [Header("Audio")]
    [Tooltip("Audio Source untuk sfx bintang")]
    public AudioSource sfxSource;
    
    [Tooltip("Sound effect saat bintang muncul")]
    public AudioClip starAppearSound;
    
    [Tooltip("Sound effect saat new record popup muncul")]
    public AudioClip newRecordSound;
    
    // Internal state
    private int currentStars = 0;
    private bool isAnimating = false;

    void OnEnable()
    {
        // Dipanggil otomatis saat panel aktif
        DisplayStars();
    }

    /// <summary>
    /// Tampilkan bintang dengan animasi
    /// Dipanggil otomatis saat panel aktif via OnEnable()
    /// </summary>
    public void DisplayStars()
    {
        if (isAnimating) return; // Jangan jalankan 2x
        
        // Ambil data dari GameManager
        int stars = GameManager.Instance.CalculateStars();
        bool isRecord = GameManager.Instance.IsNewRecord(GameManager.Instance.indexLevelSaatIni);
        
        currentStars = stars;
        
        Debug.Log($"⭐ [WIN PANEL] Displaying {stars} stars (Record: {isRecord})");
        
        // Mulai animasi bintang
        StartCoroutine(AnimateStars(stars, isRecord));
    }

    /// <summary>
    /// Coroutine animasi bintang muncul satu-per-satu
    /// </summary>
    IEnumerator AnimateStars(int starsToShow, bool isNewRecord)
    {
        isAnimating = true;
        
        // Reset semua bintang ke empty & scale 0
        foreach (GameObject star in starObjects)
        {
            if (star != null)
            {
                Image img = star.GetComponent<Image>();
                if (img != null) img.sprite = emptyStar;
                star.transform.localScale = Vector3.zero;
            }
        }
        
        // Hide new record popup dulu
        if (newRecordPopup != null)
            newRecordPopup.SetActive(false);
        
        // Tunggu sebentar sebelum animasi start
        yield return new WaitForSecondsRealtime(0.5f);
        
        // Animasi bintang muncul satu-per-satu
        for (int i = 0; i < starsToShow && i < starObjects.Length; i++)
        {
            GameObject star = starObjects[i];
            if (star == null) continue;
            
            // Ubah sprite ke filled star
            Image img = star.GetComponent<Image>();
            if (img != null) img.sprite = filledStar;
            
            // Play sound effect
            if (sfxSource != null && starAppearSound != null)
            {
                sfxSource.PlayOneShot(starAppearSound);
            }
            
            // Animasi scale dari 0 → starFinalScale
            float elapsed = 0f;
            while (elapsed < starAnimationDuration)
            {
                elapsed += Time.unscaledDeltaTime; // unscaled karena Time.timeScale = 0 saat win
                float t = elapsed / starAnimationDuration;
                float scale = Mathf.Lerp(0f, starFinalScale, t);
                star.transform.localScale = Vector3.one * scale;
                yield return null;
            }
            
            // Pastikan scale pas di akhir
            star.transform.localScale = Vector3.one * starFinalScale;
            
            // Delay sebelum bintang berikutnya
            yield return new WaitForSecondsRealtime(starAnimationDelay);
        }
        
        // Tampilkan bintang kosong yang tidak didapat
        for (int i = starsToShow; i < starObjects.Length; i++)
        {
            GameObject star = starObjects[i];
            if (star == null) continue;
            
            Image img = star.GetComponent<Image>();
            if (img != null) img.sprite = emptyStar;
            star.transform.localScale = Vector3.one * starFinalScale;
        }
        
        // Tampilkan NEW RECORD popup jika beat record
        if (isNewRecord)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            
            if (newRecordPopup != null)
            {
                newRecordPopup.SetActive(true);
                
                // Play sound effect
                if (sfxSource != null && newRecordSound != null)
                {
                    sfxSource.PlayOneShot(newRecordSound);
                }
            }
        }
        
        isAnimating = false;
        Debug.Log("✅ [WIN PANEL] Star animation complete!");
    }

    /// <summary>
    /// Fungsi dipanggil saat tombol "Upload ke Leaderboard" diklik
    /// </summary>
    public void OnClickUploadToLeaderboard()
    {
        // Cek apakah LeaderboardManager ada
        if (LeaderboardManager.Instance == null)
        {
            Debug.LogError("❌ [WIN PANEL] LeaderboardManager tidak ditemukan di scene!");
            return;
        }
        
        // Disable button agar tidak diklik 2x
        if (uploadLeaderboardButton != null)
        {
            uploadLeaderboardButton.interactable = false;
        }
        
        // Ubah text button
        if (uploadButtonText != null)
        {
            uploadButtonText.text = "Uploading...";
        }
        
        // Upload score via LeaderboardManager
        int score = GameManager.Instance.totalSkor;
        int stars = currentStars;
        int levelIndex = GameManager.Instance.indexLevelSaatIni;
        
        Debug.Log($"📤 [WIN PANEL] Uploading score: {score} ({stars}★) for Level {levelIndex}");
        
        // Call LeaderboardManager (akan dibuat di Task berikutnya)
        LeaderboardManager.Instance.UploadScore(score, stars, levelIndex, OnUploadComplete);
    }

    /// <summary>
    /// Callback setelah upload selesai
    /// </summary>
    void OnUploadComplete(bool success)
    {
        // Re-enable button
        if (uploadLeaderboardButton != null)
        {
            uploadLeaderboardButton.interactable = true;
        }
        
        // Ubah text button
        if (uploadButtonText != null)
        {
            if (success)
            {
                uploadButtonText.text = "Uploaded! ✓";
            }
            else
            {
                uploadButtonText.text = "Upload Failed ✗";
            }
        }
        
        Debug.Log($"📤 [WIN PANEL] Upload {(success ? "SUCCESS" : "FAILED")}");
    }
}
