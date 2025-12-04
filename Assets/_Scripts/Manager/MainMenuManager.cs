using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Panel utama yang berisi tombol Play, Settings, Credits, Quit")]
    public GameObject panelMain;
    
    [Tooltip("Panel popup untuk pengaturan volume (default: inactive)")]
    public GameObject panelSettings;
    
    [Tooltip("Panel popup info pembuat game (default: inactive)")]
    public GameObject panelCredits;

    [Header("Audio Settings")]
    [Tooltip("AudioSource untuk memutar efek suara")]
    public AudioSource sfxAudioSource;
    
    [Tooltip("File suara klik tombol")]
    public AudioClip clickSound;

    void Start()
    {
        // Pastikan hanya panelMain yang aktif saat game dimulai
        if (panelMain != null) panelMain.SetActive(true);
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelCredits != null) panelCredits.SetActive(false);
        
        Debug.Log("✅ MainMenuManager: Main Menu siap!");
    }

    // ==================== NAVIGATION FUNCTIONS ====================
    
    /// <summary>
    /// Memulai game dengan load scene Kantin
    /// Dipanggil oleh tombol "Play"
    /// </summary>
    public void MulaiGame()
    {
        Debug.Log("🎮 Memulai Game - Loading Scene: 01_Kantin");
        SceneManager.LoadScene("01_Hub_Klub");
    }

    /// <summary>
    /// Keluar dari aplikasi
    /// Dipanggil oleh tombol "Quit"
    /// </summary>
    public void KeluarGame()
    {
        Debug.Log("👋 Keluar dari game...");
        
        #if UNITY_EDITOR
        // Di Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Di build, tutup aplikasi
        Application.Quit();
        #endif
    }

    // ==================== PANEL TOGGLE FUNCTIONS ====================
    
    /// <summary>
    /// Membuka panel Settings
    /// Dipanggil oleh tombol "Settings"
    /// </summary>
    public void BukaSettings()
    {
        Debug.Log("⚙️ Membuka Settings");
        
        if (panelMain != null) panelMain.SetActive(false);
        if (panelSettings != null) panelSettings.SetActive(true);
    }

    /// <summary>
    /// Membuka panel Credits
    /// Dipanggil oleh tombol "Credits"
    /// </summary>
    public void BukaCredits()
    {
        Debug.Log("📝 Membuka Credits");
        
        if (panelMain != null) panelMain.SetActive(false);
        if (panelCredits != null) panelCredits.SetActive(true);
    }

    /// <summary>
    /// Kembali ke panel Main dari Settings atau Credits
    /// Dipanggil oleh tombol "Back" / "X" di popup
    /// </summary>
    public void KembaliKeMain()
    {
        Debug.Log("🔙 Kembali ke Main Menu");
        
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelCredits != null) panelCredits.SetActive(false);
        if (panelMain != null) panelMain.SetActive(true);
    }

    // ==================== AUDIO FUNCTIONS ====================
    
    /// <summary>
    /// Memutar efek suara klik tombol
    /// Dipanggil oleh semua tombol UI untuk feedback audio
    /// </summary>
    public void PlayClickSound()
    {
        if (sfxAudioSource != null && clickSound != null)
        {
            sfxAudioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("⚠️ PlayClickSound: AudioSource atau AudioClip belum di-assign di Inspector!");
        }
    }
}
