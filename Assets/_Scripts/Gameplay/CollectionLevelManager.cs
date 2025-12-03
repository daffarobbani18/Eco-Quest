using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene

public class CollectionLevelManager : MonoBehaviour
{
    // Singleton Lokal: Supaya sampah bisa lapor ke sini dengan mudah
    public static CollectionLevelManager Instance;

    [Header("UI References")]
    public GameObject panelSelesai; // Tempat kita menaruh Panel_MisiSelesai nanti

    [Header("Status Game")]
    public int totalSampahDiScene; // Target total
    public int sampahTerkumpul;    // Yang sudah diambil

    void Awake()
    {
        Instance = this; // Sayalah manager di scene ini
    }

    void Start()
    {
        // 1. Hitung otomatis ada berapa sampah di scene saat game mulai
        // Script ini akan mencari semua benda yang punya script 'CollectionItem'
        CollectionItem[] semuaSampah = FindObjectsOfType<CollectionItem>();

        totalSampahDiScene = semuaSampah.Length;
        sampahTerkumpul = 0;

        Debug.Log("Level Dimulai. Total Sampah yang harus dicari: " + totalSampahDiScene);

        // 2. Pastikan panel mati duluan (Jaga-jaga kalau Anda lupa mematikannya di Inspector)
        if (panelSelesai != null)
        {
            panelSelesai.SetActive(false);
        }
    }

    // Fungsi ini akan dipanggil oleh Sampah saat dia masuk tas
    public void LaporSampahTerambil()
    {
        sampahTerkumpul++;
        Debug.Log("Sampah terambil: " + sampahTerkumpul + "/" + totalSampahDiScene);

        // Cek apakah sudah habis?
        if (sampahTerkumpul >= totalSampahDiScene)
        {
            MisiSelesai();
        }
    }

    void MisiSelesai()
    {
        Debug.Log("SEMUA SAMPAH HABIS! MISI SELESAI.");

        // Munculkan Panel Kemenangan
        if (panelSelesai != null)
        {
            panelSelesai.SetActive(true);
        }

        // (Opsional) Di sini nanti tempat pasang suara 'Victory'
    }

    // Fungsi ini akan dipasang di Tombol "KE RUANG SORTIR" pada Panel
    public void PindahKeSortir()
    {
        // Pindah ke scene Fase 2
        // Pastikan nama scene di Build Settings sama persis dengan ini
        SceneManager.LoadScene("03_Game_Processing");
    }
}