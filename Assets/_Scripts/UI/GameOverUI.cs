using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    /// <summary>
    /// Mengulang level dari scene pengumpulan (Kantin/Lab IPA)
    /// Dipanggil oleh tombol "Ulangi Level" / "Coba Lagi"
    /// </summary>
    public void UlangiLevel()
    {
        // PENTING: Reset time scale ke normal (karena game di-pause saat Game Over)
        Time.timeScale = 1f;
        
        // Cek index level dari GameManager untuk tahu scene asal
        string sceneAsal = "";
        
        if (GameManager.Instance != null)
        {
            int levelIndex = GameManager.Instance.indexLevelSaatIni;
            
            // Mapping level index ke scene pengumpulan
            switch (levelIndex)
            {
                case 1:
                    sceneAsal = "02_Game_Kantin"; // Level 1 = Scene Kantin
                    break;
                case 2:
                    sceneAsal = "02_Lab_IPA"; // Level 2 = Scene Lab IPA
                    break;
                case 3:
                    sceneAsal = "05_Game_Gudang"; // Level 3 = Scene Gudang (future)
                    break;
                default:
                    // Fallback: Reload scene saat ini jika tidak ketemu mapping
                    sceneAsal = SceneManager.GetActiveScene().name;
                    Debug.LogWarning($"⚠️ Level index {levelIndex} tidak dikenali, reload scene saat ini");
                    break;
            }
            
            Debug.Log($"🔄 Game Over! Kembali ke scene pengumpulan: {sceneAsal} (Level {levelIndex})");
            
            // Clear inventory untuk mulai fresh
            GameManager.Instance.ClearInventory();
        }
        else
        {
            // Fallback jika GameManager tidak ada (untuk testing)
            sceneAsal = SceneManager.GetActiveScene().name;
            Debug.LogWarning("⚠️ GameManager tidak ditemukan! Reload scene saat ini");
        }
        
        SceneManager.LoadScene(sceneAsal);
    }

    /// <summary>
    /// Kembali ke scene menu pemilihan level (Hub)
    /// Dipanggil oleh tombol "Kembali ke Menu" / "Exit"
    /// </summary>
    public void KembaliKeHub()
    {
        // PENTING: Reset time scale ke normal
        Time.timeScale = 1f;
        
        Debug.Log("🏠 Kembali ke Hub Level Selection");
        
        SceneManager.LoadScene("01_Hub_Klub");
    }

    /// <summary>
    /// BONUS: Quit ke Main Menu (jika ada)
    /// </summary>
    public void KembaliKeMainMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("📋 Kembali ke Main Menu");
        SceneManager.LoadScene("00_MainMenu");
    }

    /// <summary>
    /// BONUS: Quit aplikasi (untuk tombol Quit di build)
    /// </summary>
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("👋 Quit Game");
        
        #if UNITY_EDITOR
        // Di Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Di build, tutup aplikasi
        Application.Quit();
        #endif
    }
}
