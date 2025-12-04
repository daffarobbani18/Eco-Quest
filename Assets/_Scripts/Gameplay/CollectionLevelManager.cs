using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionLevelManager : MonoBehaviour
{
    public static CollectionLevelManager Instance;

    // --- HAPUS BAGIAN UI BRIEFING DI SINI ---
    // Kita hapus referensi ke Text Judul, Deskripsi, dll.
    // Biarkan BriefingSequence yang mengurusnya.

    [Header("UI Selesai (Akhir)")]
    public GameObject panelSelesai;

    [Header("Data Level Ini")]
    public LevelData dataLevelIni; // Tetap butuh ini untuk memberi info ke BriefingSequence

    [Header("Status")]
    public int totalSampahDiScene;
    public int sampahTerkumpul;

    public bool isGamePlaying = false;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }

    void Start()
    {
        // Setup Data Sampah
        CollectionItem[] semuaSampah = FindObjectsOfType<CollectionItem>();
        totalSampahDiScene = semuaSampah.Length;
        sampahTerkumpul = 0;

        // Matikan Panel Selesai
        if (panelSelesai != null) panelSelesai.SetActive(false);

        // --- LOGIKA BRIEFING DIPINDAHKAN ---
        // Kita tidak lagi menyalakan panel di sini.
        // Kita biarkan script 'BriefingSequence' yang membacanya dari 'Start'-nya sendiri.

        // Default: Game terkunci sampai tombol Mulai ditekan di BriefingSequence
        isGamePlaying = false;
    }

    // Fungsi ini dipanggil oleh Tombol "SIAP!" di Panel Dialog (via BriefingSequence)
    public void MulaiMain()
    {
        // --- HAPUS BARIS YANG ERROR INI: ---
        // if(panelBriefing != null) panelBriefing.SetActive(false); 
        // -----------------------------------

        isGamePlaying = true;
        Debug.Log("GAME DIMULAI! (Pemain sekarang bisa klik sampah)");
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