using UnityEngine;
using System.Collections.Generic;

public class WasteSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject prefabSampah;
    public Transform titikSpawn;
    public float intervalSpawn = 2.0f;

    [Header("Data Test (Isi minimal 3 data beda)")]
    public List<WasteData> daftarSampahTest; // Data cadangan buat test di Scene 3 langsung

    // List yang benar-benar akan dipakai (bisa dari Test, bisa dari GameManager)
    private List<WasteData> daftarSampahFinal;

    private float timer;
    private int indexSampah = 0;

    void Start()
    {
        // --- LOGIKA PEMILIHAN DATA ---

        // Cek 1: Apakah GameManager ada DAN Inventory-nya ada isinya? (Artinya pemain datang dari Scene 2)
        if (GameManager.Instance != null &&
            GameManager.Instance.trashInventory != null &&
            GameManager.Instance.trashInventory.Count > 0)
        {
            Debug.Log("Spawner: Menggunakan Data dari Inventaris Pemain (Fase 1).");
            // Salin isi inventory pemain ke list lokal
            daftarSampahFinal = new List<WasteData>(GameManager.Instance.trashInventory);
        }
        // Cek 2: Jika tidak ada data GameManager (Misal lagi test play langsung di Scene 3)
        else
        {
            Debug.LogWarning("Spawner: Tidak ada data inventaris/GameManager. Menggunakan DATA TEST INSPECTOR.");
            // Gunakan data dummy yang kamu set di Inspector
            daftarSampahFinal = new List<WasteData>(daftarSampahTest);
        }

        // Mulai logika game level (Timer dll) jika perlu
        // if (GameManager.Instance != null) { GameManager.Instance.MulaiLevel(60f); }
    }

    void Update()
    {
        // Pastikan list ada isinya sebelum menjalankan timer
        if (daftarSampahFinal != null && daftarSampahFinal.Count > 0)
        {
            timer += Time.deltaTime;

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

            // Cek safety kalau datanya kosong
            if (dataSekarang == null)
            {
                indexSampah++;
                return;
            }

            // Instantiate
            GameObject sampahBaru = Instantiate(prefabSampah, titikSpawn.position, Quaternion.identity);
            sampahBaru.name = "Sampah_" + dataSekarang.namaSampah;

            // Masukkan data ke script WasteItem di sampah yang baru lahir
            WasteItem scriptSampah = sampahBaru.GetComponent<WasteItem>();
            if (scriptSampah != null)
            {
                scriptSampah.dataSampah = dataSekarang;
            }

            // Ubah gambarnya sesuai data
            SpriteRenderer renderGambar = sampahBaru.GetComponentInChildren<SpriteRenderer>();
            if (renderGambar != null)
            {
                renderGambar.sprite = dataSekarang.iconSampah;
            }

            indexSampah++; // Lanjut ke item berikutnya
        }
        else
        {
            // Jika index sudah habis (semua sampah sudah keluar)
            // Debug.Log("Semua sampah sudah keluar dari meja!");
            // Matikan script ini biar update ga jalan terus
            this.enabled = false;
        }
    }
}