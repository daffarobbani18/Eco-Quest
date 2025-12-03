using UnityEngine;
using System.Collections.Generic; // PENTING: Kita butuh ini untuk menggunakan 'List' (Daftar)

[CreateAssetMenu(fileName = "DataLevelBaru", menuName = "EcoQuest/Level Data", order = 2)]
public class LevelData : ScriptableObject
{
    [Header("Info Dasar Level")]
    public string namaLevel; // Contoh: "Proyek Kantin"
    [TextArea] public string deskripsiBriefing; // Teks untuk dialog Pak Budi di awal

    [Header("Aturan Main")]
    public float batasWaktuDetik; // Contoh: 60 untuk 1 menit
    public int targetJumlahSampah; // Berapa banyak sampah yang harus dikumpulkan

    [Header("Isi Level (Resep Sampah)")]
    // Ini adalah BAGIAN PENTING. Ini adalah daftar (List) yang akan menampung
    // file-file Data Sampah yang sudah Anda buat sebelumnya.
    public List<WasteData> daftarSampahLevelIni;
}