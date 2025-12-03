using UnityEngine;
using UnityEngine.SceneManagement; // Wajib

public class LevelSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject panelLevelSelect; // Panel yang tadi dibuat

    void Start()
    {
        // Pastikan panel mati saat mulai
        //if (panelLevelSelect != null) panelLevelSelect.SetActive(false);
    }

    // Fungsi dipanggil oleh MadingController
    public void BukaPanel()
    {
        if (panelLevelSelect != null) panelLevelSelect.SetActive(true);
    }

    // Fungsi dipanggil oleh Tombol X
    public void TutupPanel()
    {
        if (panelLevelSelect != null) panelLevelSelect.SetActive(false);
    }

    // --- FUNGSI PINDAH LEVEL ---
    public void PilihLevelKantin()
    {
        // Kita kirim Data Level lewat GameManager (Nanti kita bahas detailnya)
        // Untuk sekarang, langsung pindah scene dulu
        LoadingKeScene("02_Game_Kantin");
    }

    public void PilihLevelLab()
    {
        // Pastikan Anda sudah buat scene ini atau arahkan ke Kantin dulu sbg tes
        // LoadingKeScene("04_Game_Lab"); 
        Debug.Log("Level Lab belum dibuat scenenya!");
    }

    public void PilihLevelGudang()
    {
        Debug.Log("Level Gudang belum dibuat scenenya!");
    }

    void LoadingKeScene(string namaScene)
    {
        SceneManager.LoadScene(namaScene);
    }
}