using UnityEngine;

// ATRIBUT PENTING: Ini membuat menu klik kanan di Unity untuk membuat data baru.
[CreateAssetMenu(fileName = "DataSampahBaru", menuName = "EcoQuest/Waste Data", order = 1)]
public class WasteData : ScriptableObject // PERHATIKAN: Kita ganti 'MonoBehaviour' jadi 'ScriptableObject'
{
    [Header("Identitas Sampah")]
    public string namaSampah; // Contoh: "Sisa Apel"

    // Kita pakai 'enum' (pilihan ganda) agar tidak salah ketik tipe sampah.
    public WasteType tipeSampah;

    [Header("Visual")]
    public Sprite iconSampah; // Tempat menaruh gambar sprite sampahnya

    [Header("Scoring (Scene 03 - Pemilahan)")]
    [Tooltip("Skor yang didapat jika sampah dibuang ke tong yang BENAR")]
    public int skorBenar = 10;
    
    [Tooltip("Skor yang dikurangi jika sampah dibuang ke tong yang SALAH (gunakan angka positif, misal 5 untuk -5)")]
    public int skorSalah = 5;
}

// Ini adalah definisi pilihan ganda untuk tipe sampah.
// Bisa ditaruh di file terpisah, tapi untuk sekarang taruh di sini saja biar mudah.
public enum WasteType
{
    Organik,
    Anorganik,
    B3
}