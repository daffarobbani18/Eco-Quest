using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections; // Untuk IEnumerator

public class CollectionLevelManager : MonoBehaviour
{
    public static CollectionLevelManager Instance;

    [Header("Referensi Sistem")]
    public BriefingSequence briefingScript;

    [Header("UI Selesai (Akhir)")]
    public GameObject panelSelesai;

    [Header("Data Level Ini")]
    public LevelData dataLevelIni; // Tetap butuh ini untuk memberi info ke BriefingSequence
    
    [Tooltip("Isi 1 untuk Kantin, 2 untuk Lab IPA, 3 untuk Gudang")]
    public int urutanLevel = 1;

    [Header("Status")]
    public int totalSampahDiScene;
    public int sampahTerkumpul;

    public bool isGamePlaying = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    IEnumerator Start()
    {
        Debug.Log("==================================================");
        Debug.Log("[CollectionLevelManager] START");
        
        // Set index level untuk progression system
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetIndexLevel(urutanLevel);
            Debug.Log($"✅ Level Index diset ke: {urutanLevel}");
        }
        
        // Setup Data Sampah
        CollectionItem[] semuaSampah = FindObjectsOfType<CollectionItem>();
        totalSampahDiScene = semuaSampah.Length;
        sampahTerkumpul = 0;
        Debug.Log($"✅ Total sampah di scene: {totalSampahDiScene}");

        // Matikan Panel Selesai
        if (panelSelesai != null) panelSelesai.SetActive(false);

        // Default: Game terkunci sampai tombol Mulai ditekan
        isGamePlaying = false;
        
        Debug.Log("⏸️ Game terkunci. Menunggu briefing selesai...");
        Debug.Log("==================================================");
        
        // Listener akan di-setup oleh BriefingSequence.SelesaiDialog()
        // Tidak perlu setup di sini karena button belum muncul
        yield break;
    }

    // Fungsi ini dipanggil oleh Tombol "SIAP!" di Panel Dialog (via BriefingSequence)
    public void MulaiMain()
    {
        Debug.Log("==================================================");
        Debug.Log("[COLLECTION] MulaiMain() DIPANGGIL");
        
        isGamePlaying = true;
        
        Debug.Log("✅ isGamePlaying = true");
        Debug.Log("🎮 GAME DIMULAI! Pemain sekarang bisa klik sampah");
        Debug.Log("==================================================");
    }

    public void LaporSampahTerambil()
    {
        sampahTerkumpul++;
        if (sampahTerkumpul >= totalSampahDiScene)
        {
            MisiSelesai();
        }
    }

    void MisiSelesai()
    {
        if (panelSelesai != null) panelSelesai.SetActive(true);
    }

    public void PindahKeSortir()
    {
        SceneManager.LoadScene("03_Game_Processing");
    }
}