using UnityEngine;
using System.Collections.Generic;

public class WasteSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject prefabSampah;
    public Transform titikSpawn;
    public float intervalSpawn = 2.0f;

    [Header("Data Test (Isi minimal 3 data beda)")]
    public List<WasteData> daftarSampahTest;
    private List<WasteData> daftarSampahFinal;

    private float timer;
    private int indexSampah = 0;

    void Start()
    {
        // --- LOGIKA PEMILIHAN DATA ---
        // 1. Cek apakah ada data kiriman dari Fase 1 (GameManager)?
        if (GameManager.Instance != null && GameManager.Instance.trashInventory.Count > 0)
        {
            Debug.Log("Spawner: Menggunakan Data dari Inventaris Pemain (Fase 1).");
            // Salin isi inventory ke list lokal kita agar aman
            daftarSampahFinal = new List<WasteData>(GameManager.Instance.trashInventory);
        }
        else
        {
            Debug.LogWarning("Spawner: Tidak ada data inventaris. Menggunakan DATA TEST INSPECTOR.");
            daftarSampahFinal = daftarSampahTest;
        }

        // Mulai logika game level (Timer dll)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MulaiLevel(60f);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Cek daftarSampahFinal, bukan daftarSampahTest lagi
        if (daftarSampahFinal != null && daftarSampahFinal.Count > 0)
        {
            if (timer >= intervalSpawn)
            {
                SpawnSampah();
                timer = 0f;
            }
        }
    }

    void SpawnSampah()
    {
        // Pastikan kita tidak mencoba spawn lebih dari jumlah sampah yang ada
        if (indexSampah < daftarSampahFinal.Count)
        {
            // Ambil Data dari List FINAL
            WasteData dataSekarang = daftarSampahFinal[indexSampah];

            if (dataSekarang.iconSampah == null) return;

            // Instantiate & Setup (Sama seperti sebelumnya)
            GameObject sampahBaru = Instantiate(prefabSampah, titikSpawn.position, Quaternion.identity);
            sampahBaru.name = "Sampah_" + dataSekarang.namaSampah;

            WasteItem scriptSampah = sampahBaru.GetComponent<WasteItem>();
            if (scriptSampah != null) scriptSampah.dataSampah = dataSekarang;

            SpriteRenderer renderGambar = sampahBaru.GetComponentInChildren<SpriteRenderer>();
            if (renderGambar != null) renderGambar.sprite = dataSekarang.iconSampah;

            indexSampah++; // Lanjut ke item berikutnya
        }
        else
        {
            // Jika index sudah habis (semua sampah sudah keluar)
            Debug.Log("Semua sampah sudah keluar dari meja!");
            // Di sini nanti kita bisa set state "Menunggu Selesai"
        }
    }
}