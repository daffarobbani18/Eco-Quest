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

    private float timer;
    private int indexSampah = 0;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= intervalSpawn)
        {
            SpawnSampah();
            timer = 0f; // Reset timer
        }
    }

    void SpawnSampah()
    {
        // Cek keamanan data
        if (daftarSampahTest == null || daftarSampahTest.Count == 0)
        {
            Debug.LogError("ERROR: Daftar Sampah Test di Inspector KOSONG!");
            return;
        }

        // 1. Reset index jika sudah habis
        if (indexSampah >= daftarSampahTest.Count) indexSampah = 0;

        // 2. Ambil Data Giliran Ini
        WasteData dataSekarang = daftarSampahTest[indexSampah];

        // ---------------------------------------------------------
        // DEBUG 1: Pastikan datanya punya gambar
        if (dataSekarang.iconSampah == null)
        {
            Debug.LogError("ERROR FATAL: Data " + dataSekarang.namaSampah + " TIDAK PUNYA GAMBAR (Sprite) di file datanya!");
            return;
        }
        // ---------------------------------------------------------

        // 3. Lahirkan Sampah (Instantiate)
        GameObject sampahBaru = Instantiate(prefabSampah, titikSpawn.position, Quaternion.identity);

        // Beri nama unik biar gampang dicek di Hierarchy
        sampahBaru.name = "Sampah_" + dataSekarang.namaSampah;

        // 4. Masukkan Data ke Script WasteItem
        WasteItem scriptSampah = sampahBaru.GetComponent<WasteItem>();
        if (scriptSampah != null)
        {
            scriptSampah.dataSampah = dataSekarang;
        }

        // 5. GANTI GAMBAR (MOMEN KEBENARAN)
        SpriteRenderer renderGambar = sampahBaru.GetComponent<SpriteRenderer>();
        if (renderGambar != null)
        {
            // Kita paksa ganti spritenya dengan yang ada di data
            renderGambar.sprite = dataSekarang.iconSampah;

            Debug.Log("Spawner mencoba memasang gambar bernama: " + dataSekarang.iconSampah.name);
            Debug.Log("SUKSES: Munculkan " + dataSekarang.namaSampah);
        }
        else
        {
            Debug.LogError("ERROR: Prefab tidak punya komponen Sprite Renderer!");
        }

        // 6. Lanjut antrian
        indexSampah++;
    }
}