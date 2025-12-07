using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// JUDGMENT PHASE - TONG MARAH FEEDBACK SYSTEM
/// Menampilkan tong sampah yang marah karena salah terima sampah
/// dengan dialog interaktif untuk anak SD (7-12 tahun)
/// 
/// Konsep: Tong sebagai karakter yang "protes" dengan bahasa anak-anak
/// Visual: Full illustration tong marah + dialog box + icon sampah menumpuk
/// </summary>
public class JudgmentSlideshow : MonoBehaviour
{
    [Header("UI References - Slide Components")]
    [Tooltip("Image untuk gambar FULL tong marah (512x512px+)")]
    public Image imageTongMarah;
    
    [Tooltip("Text dialog tong yang marah (dinamis, bahasa anak-anak)")]
    public TMP_Text textDialog;
    
    [Tooltip("Container untuk spawn icon sampah (akan menumpuk/overlap)")]
    public Transform containerWasteIcons;
    
    [Tooltip("Prefab icon sampah (Image dengan ukuran 80x80px untuk spawn)")]
    public GameObject prefabWasteIcon;
    
    [Tooltip("Text progress slide (contoh: 'Tong 1 dari 2')")]
    public TMP_Text textProgress;

    [Header("Full Illustration Sprites - Tong Marah")]
    [Tooltip("Gambar FULL Tong Organik Marah (1 file PNG dengan tong + dialog box)")]
    public Sprite spriteTongOrganikMarah;
    
    [Tooltip("Gambar FULL Tong Anorganik Marah (1 file PNG dengan tong + dialog box)")]
    public Sprite spriteTongAnorganikMarah;
    
    [Tooltip("Gambar FULL Tong B3 Marah (1 file PNG dengan tong + dialog box)")]
    public Sprite spriteTongB3Marah;

    [Header("Settings - Tampilan Sampah")]
    [Tooltip("Ukuran icon sampah yang muncul (pixel)")]
    public float wasteIconSize = 80f;
    
    [Tooltip("Jarak acak antara icon (untuk efek menumpuk berserakan)")]
    public float randomOffsetRange = 30f;
    
    [Tooltip("Rotasi acak icon (derajat, untuk efek tidak kaku)")]
    public float randomRotationRange = 15f;

    [Header("Settings - Timing")]
    [Tooltip("Durasi setiap slide tong marah (detik) - anak SD butuh waktu baca")]
    public float slideDuration = 5.0f;
    
    [Tooltip("Panel Judgment keseluruhan (parent dari semua UI)")]
    public GameObject judgmentPanel;

    // Callback setelah slideshow selesai
    private System.Action onSlideshowComplete;
    
    // List untuk track icon yang di-spawn (untuk cleanup)
    private List<GameObject> spawnedIcons = new List<GameObject>();

    /// <summary>
    /// Mulai Slideshow Tong Marah
    /// Dipanggil oleh ProcessingLevelManager setelah level selesai
    /// </summary>
    /// <param name="onComplete">Callback fungsi yang dipanggil setelah slideshow selesai</param>
    public void StartSlideshow(System.Action onComplete)
    {
        onSlideshowComplete = onComplete;
        
        // Cek apakah ada kesalahan
        if (GameManager.Instance == null || !GameManager.Instance.HasMistakes())
        {
            Debug.Log("[JUDGMENT] 🎉 Tidak ada kesalahan! Semua tong senang! Skip slideshow.");
            onSlideshowComplete?.Invoke();
            return;
        }
        
        int affectedBins = GameManager.Instance.GetAffectedBinCount();
        Debug.Log($"[JUDGMENT] 😠 Ada {affectedBins} tong yang marah! Memulai slideshow...");
        
        // Tampilkan panel
        if (judgmentPanel != null)
        {
            judgmentPanel.SetActive(true);
        }
        
        // Mulai coroutine slideshow
        StartCoroutine(ShowSlidesCoroutine());
    }

    /// <summary>
    /// Coroutine untuk menampilkan slide tong marah satu per satu
    /// Loop: Untuk setiap tong yang kena salah pilah
    /// </summary>
    IEnumerator ShowSlidesCoroutine()
    {
        var mistakesByBin = GameManager.Instance.mistakesByBin;
        int slideIndex = 0;
        int totalSlides = GameManager.Instance.GetAffectedBinCount();
        
        // Loop untuk setiap tipe tong (Organik, Anorganik, B3)
        foreach (var kvp in mistakesByBin)
        {
            WasteType binType = kvp.Key;
            List<WasteData> wrongWastes = kvp.Value;
            
            // Skip tong yang tidak kena salah pilah
            if (wrongWastes.Count == 0) continue;
            
            slideIndex++;
            
            Debug.Log($"[JUDGMENT] 😠 Slide {slideIndex}/{totalSlides}: Tong {binType} menerima {wrongWastes.Count} sampah salah");
            
            // Update UI untuk tong ini
            UpdateSlide(binType, wrongWastes, slideIndex, totalSlides);
            
            // Tunggu beberapa detik (gunakan realtime agar tidak terpengaruh Time.timeScale)
            // Anak SD butuh waktu lebih lama untuk baca dan pahami
            yield return new WaitForSecondsRealtime(slideDuration);
            
            // Cleanup icon sebelum slide berikutnya
            ClearSpawnedIcons();
        }
        
        Debug.Log("[JUDGMENT] 🎉 Slideshow selesai! Semua tong sudah protes!");
        
        // Sembunyikan panel
        if (judgmentPanel != null)
        {
            judgmentPanel.SetActive(false);
        }
        
        // Trigger callback (tampilkan Win Panel)
        onSlideshowComplete?.Invoke();
    }

    /// <summary>
    /// Update UI untuk 1 slide: Tong yang marah
    /// </summary>
    void UpdateSlide(WasteType angryBinType, List<WasteData> wrongWastes, int currentSlide, int totalSlides)
    {
        // 1. Tampilkan gambar FULL tong marah yang sesuai
        if (imageTongMarah != null)
        {
            imageTongMarah.sprite = GetAngryBinSprite(angryBinType);
            imageTongMarah.enabled = true;
        }
        
        // 2. Generate dialog tong dengan bahasa anak-anak
        if (textDialog != null)
        {
            textDialog.text = GenerateAngryDialog(angryBinType, wrongWastes);
        }
        
        // 3. Spawn icon sampah yang salah masuk (menumpuk/berserakan)
        SpawnWasteIcons(wrongWastes);
        
        // 4. Update progress counter
        if (textProgress != null)
        {
            textProgress.text = $"Tong {currentSlide} dari {totalSlides}";
        }
    }

    /// <summary>
    /// Get sprite FULL illustration tong marah berdasarkan tipe
    /// </summary>
    Sprite GetAngryBinSprite(WasteType type)
    {
        switch (type)
        {
            case WasteType.Organik:
                return spriteTongOrganikMarah;
            case WasteType.Anorganik:
                return spriteTongAnorganikMarah;
            case WasteType.B3:
                return spriteTongB3Marah;
            default:
                Debug.LogError($"[JUDGMENT] Sprite tong marah untuk {type} tidak ditemukan!");
                return null;
        }
    }

    /// <summary>
    /// Generate dialog tong yang marah dengan bahasa anak-anak yang fun
    /// Dialog DINAMIS berdasarkan sampah yang salah masuk
    /// </summary>
    string GenerateAngryDialog(WasteType binType, List<WasteData> wrongWastes)
    {
        // Nama-nama sampah yang salah masuk (maks 3 untuk tidak terlalu panjang)
        string wasteNames = "";
        int maxNamesShown = Mathf.Min(3, wrongWastes.Count);
        
        for (int i = 0; i < maxNamesShown; i++)
        {
            wasteNames += wrongWastes[i].namaSampah;
            if (i < maxNamesShown - 1)
            {
                wasteNames += \", \";
            }
        }
        
        // Tambah "dan lainnya" jika lebih dari 3
        if (wrongWastes.Count > 3)
        {
            wasteNames += $\" dan {wrongWastes.Count - 3} lainnya\";\n        }
        
        // Dialog template berdasarkan tipe tong (dengan personality berbeda)
        switch (binType)
        {
            case WasteType.Organik:
                return $\"Aduh! Aku Tong Hijau untuk sampah organik!\\n\\n\" +\n                       $\"Kok {wasteNames} dimasukkan ke sini? 😢\\n\\n\" +\n                       $\"Sampah organik itu yang berasal dari makhluk hidup dan bisa membusuk, seperti sisa makanan!\";\n            \n            case WasteType.Anorganik:
                return $\"Hei! Aku Tong Kuning untuk sampah anorganik!\\n\\n\" +\n                       $\"Masa {wasteNames} masuk ke sini sih? 😠\\n\\n\" +\n                       $\"Sampah anorganik itu seperti plastik, kertas, dan kaleng yang bisa didaur ulang!\";\n            \n            case WasteType.B3:
                return $\"AWAS! Aku Tong Merah khusus B3 (Bahan Berbahaya)!\\n\\n\" +\n                       $\"{wasteNames} bukan sampah berbahaya! 😤\\n\\n\" +\n                       $\"B3 itu seperti baterai, lampu, dan obat-obatan yang bisa meracuni lingkungan!\";\n            \n            default:
                return \"Aku bingung... Sampah ini salah tempat deh! 🤔\";\n        }
    }
    
    /// <summary>
    /// Spawn icon sampah yang salah masuk ke container
    /// Layout: Menumpuk berserakan (overlap dengan random offset & rotation)
    /// </summary>
    void SpawnWasteIcons(List<WasteData> wrongWastes)
    {
        if (containerWasteIcons == null)
        {
            Debug.LogError(\"[JUDGMENT] containerWasteIcons NULL! Tidak bisa spawn icon.\");
            return;
        }
        
        if (prefabWasteIcon == null)
        {
            Debug.LogError(\"[JUDGMENT] prefabWasteIcon NULL! Assign prefab di Inspector.\");
            return;
        }
        
        // Cleanup icon lama (jika ada)
        ClearSpawnedIcons();
        
        Debug.Log($\"[JUDGMENT] Spawn {wrongWastes.Count} icon sampah dengan efek menumpuk...\");
        
        // Spawn setiap icon sampah
        for (int i = 0; i < wrongWastes.Count; i++)
        {
            WasteData waste = wrongWastes[i];
            
            // Instantiate icon dari prefab
            GameObject iconObj = Instantiate(prefabWasteIcon, containerWasteIcons);
            spawnedIcons.Add(iconObj);
            
            // Set sprite
            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage != null && waste.iconSampah != null)
            {
                iconImage.sprite = waste.iconSampah;
            }
            
            // Set size
            RectTransform rt = iconObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(wasteIconSize, wasteIconSize);
                
                // Random offset untuk efek menumpuk berserakan (tidak kaku)
                float offsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
                float offsetY = Random.Range(-randomOffsetRange, randomOffsetRange);
                rt.anchoredPosition = new Vector2(offsetX, offsetY);
                
                // Random rotation untuk efek lebih natural
                float rotation = Random.Range(-randomRotationRange, randomRotationRange);
                rt.rotation = Quaternion.Euler(0, 0, rotation);
            }
            
            Debug.Log($\"[JUDGMENT]   - Spawn icon: {waste.namaSampah} (offset: {rt.anchoredPosition}, rotation: {rt.rotation.eulerAngles.z}°)\");
        }
    }
    
    /// <summary>
    /// Cleanup semua icon yang di-spawn sebelum slide berikutnya
    /// </summary>
    void ClearSpawnedIcons()
    {
        foreach (GameObject icon in spawnedIcons)
        {
            if (icon != null) Destroy(icon);
        }
        spawnedIcons.Clear();
    }
}
