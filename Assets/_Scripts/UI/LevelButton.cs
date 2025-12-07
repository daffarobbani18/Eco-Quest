using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelButton : MonoBehaviour
{
    [Header("Level Info")]
    [Tooltip("Urutan level (1, 2, 3, dst)")]
    public int levelIndex;
    
    [Tooltip("Nama scene yang akan dimuat saat tombol diklik")]
    public string sceneName;

    [Header("UI References")]
    [Tooltip("GameObject overlay gembok (aktif jika level terkunci)")]
    public GameObject padlockOverlay;
    
    [Tooltip("Komponen Button dari GameObject ini")]
    public Button buttonComponent;
    
    [Header("Star Rating Display")]
    [Tooltip("Array 3 GameObjects untuk icon bintang (optional)")]
    public GameObject[] starIcons;
    
    [Tooltip("Text untuk display best score (optional)")]
    public TMP_Text bestScoreText;

    [Header("Audio Settings")]
    [Tooltip("Referensi ke Audio Source scene")]
    public AudioSource sfxSource;
    
    [Tooltip("Suara kertas saat level terbuka diklik")]
    public AudioClip paperSound;
    
    [Tooltip("Suara gembok/error saat level terkunci diklik")]
    public AudioClip lockedSound;
    
    [Header("Transition Settings")]
    [Tooltip("Delay sebelum load scene (agar animasi/audio selesai)")]
    public float loadDelay = 0.3f;

    void Start()
    {
        // Auto-assign button component jika belum diassign manual
        if (buttonComponent == null)
        {
            buttonComponent = GetComponent<Button>();
        }

        // Setup listener untuk tombol
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(OnClickLevel);
        }
        else
        {
            Debug.LogError($"❌ LevelButton [{levelIndex}]: Button component tidak ditemukan!");
        }
        
        // Display saved stars & best score
        UpdateStarDisplay();
    }
    
    /// <summary>
    /// Update display bintang dan best score dari PlayerPrefs
    /// </summary>
    void UpdateStarDisplay()
    {
        if (GameManager.Instance == null) return;
        
        int bestStars = GameManager.Instance.GetBestStars(levelIndex);
        int bestScore = GameManager.Instance.GetBestScore(levelIndex);
        
        // Update star icons
        if (starIcons != null && starIcons.Length >= 3)
        {
            for (int i = 0; i < starIcons.Length; i++)
            {
                if (starIcons[i] != null)
                {
                    // Aktifkan bintang jika pemain sudah dapat bintang tersebut
                    starIcons[i].SetActive(i < bestStars);
                }
            }
        }
        
        // Update best score text
        if (bestScoreText != null)
        {
            if (bestScore > 0)
            {
                bestScoreText.text = $"Best: {bestScore}";
            }
            else
            {
                bestScoreText.text = "Not Played";
            }
        }
        
        if (bestStars > 0)
        {
            Debug.Log($"⭐ LevelButton [{levelIndex}]: Best Stars = {bestStars}, Best Score = {bestScore}");
        }
    }

    /// <summary>
    /// Mengatur status level (terkunci atau terbuka)
    /// </summary>
    /// <param name="isLocked">True = level terkunci, False = level terbuka</param>
    public void SetStatus(bool isLocked)
    {
        if (isLocked)
        {
            // Level Terkunci
            if (padlockOverlay != null)
                padlockOverlay.SetActive(true);
            
            // Biarkan button tetap interactable agar bisa bunyi "Gembok"
            // Pengecekan lock/unlock dilakukan di OnClickLevel()
            
            Debug.Log($"🔒 Level {levelIndex} terkunci");
        }
        else
        {
            // Level Terbuka
            if (padlockOverlay != null)
                padlockOverlay.SetActive(false);
            
            Debug.Log($"✅ Level {levelIndex} terbuka");
        }
    }

    /// <summary>
    /// Fungsi yang dipanggil saat tombol level diklik
    /// </summary>
    public void OnClickLevel()
    {
        // Cek apakah level terkunci atau terbuka
        if (padlockOverlay != null && padlockOverlay.activeSelf)
        {
            // Level TERKUNCI - Mainkan suara gembok/error
            if (sfxSource != null && lockedSound != null)
            {
                sfxSource.PlayOneShot(lockedSound);
            }
            
            Debug.Log($"🔒 Level {levelIndex} terkunci! Selesaikan level sebelumnya terlebih dahulu.");
            return; // Jangan load scene
        }

        // Level TERBUKA - Mainkan suara kertas dan load scene
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"❌ LevelButton [{levelIndex}]: Scene Name belum diisi!");
            return;
        }

        // Mainkan suara kertas dan tunggu sebelum load scene
        if (sfxSource != null && paperSound != null)
        {
            sfxSource.PlayOneShot(paperSound);
        }

        // Disable button agar tidak diklik ganda
        if (buttonComponent != null)
        {
            buttonComponent.interactable = false;
        }

        Debug.Log($"🎮 Loading Level {levelIndex}: {sceneName} (delay: {loadDelay}s)");
        
        // Tunggu animasi/audio selesai baru load scene
        StartCoroutine(LoadSceneWithDelay());
    }

    /// <summary>
    /// Coroutine untuk menunggu animasi/audio selesai sebelum load scene
    /// </summary>
    IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        // Cleanup listener untuk mencegah memory leak
        if (buttonComponent != null)
        {
            buttonComponent.onClick.RemoveListener(OnClickLevel);
        }
    }
}
