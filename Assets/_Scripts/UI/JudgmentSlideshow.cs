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
    public float wasteIconSize = 50f;
    
    [Tooltip("Posisi dasar spawn icon (offset dari center container)")]
    public Vector2 baseIconPosition = new Vector2(0, 0);
    
    [Tooltip("Jarak acak antara icon (untuk efek menumpuk berserakan)")]
    public float randomOffsetRange = 30f;
    
    [Tooltip("Rotasi acak icon (derajat, untuk efek tidak kaku)")]
    public float randomRotationRange = 15f;
    
    [Header("Settings - Animasi Icon")]
    [Tooltip("Durasi animasi spawn icon (detik)")]
    public float iconSpawnDuration = 0.3f;
    
    [Tooltip("Delay antara spawn icon (detik, untuk efek berurutan)")]
    public float iconSpawnDelay = 0.1f;
    
    [Tooltip("Efek spawn: Scale (membesar dari kecil), Fade (muncul transparan), Drop (jatuh dari atas)")]
    public bool useScaleAnimation = true;
    public bool useFadeAnimation = true;
    public bool useDropAnimation = true;
    
    [Header("Settings - Animasi Idle (Melayang)")]
    [Tooltip("Aktifkan animasi melayang setelah spawn")]
    public bool useFloatingAnimation = true;
    
    [Tooltip("Jarak melayang (pixel, semakin besar semakin jauh)")]
    public float floatingDistance = 2f;
    
    [Tooltip("Kecepatan melayang (semakin besar semakin cepat)")]
    public float floatingSpeed = 1f;

    [Header("Settings - Timing")]
    [Tooltip("Durasi setiap slide tong marah (detik) - anak SD butuh waktu baca")]
    public float slideDuration = 5.0f;
    
    [Tooltip("Panel Judgment keseluruhan (parent dari semua UI)")]
    public GameObject judgmentPanel;
    
    [Header("UI References - Transisi Intro")]
    [Tooltip("Panel overlay hitam untuk transisi (CanvasGroup untuk fade in/out)")]
    public CanvasGroup panelTransisiOverlay;
    
    [Tooltip("Text informasi transisi (contoh: 'Ups, kamu ada salah sortir sampah...')")]
    public TMP_Text textTransisiInfo;
    
    [Header("Settings - Animasi Transisi Intro")]
    [Tooltip("Durasi fade in overlay (detik)")]
    public float transisiFadeInDuration = 0.5f;
    
    [Tooltip("Durasi tampil teks transisi (detik)")]
    public float transisiTextDuration = 2.0f;
    
    [Tooltip("Durasi fade out overlay (detik)")]
    public float transisiFadeOutDuration = 0.5f;
    
    [Tooltip("Warna overlay transisi (default: hitam semi-transparan)")]
    public Color transisiOverlayColor = new Color(0, 0, 0, 0.9f);
    
    [Header("Settings - Animasi Transisi Outro (ke Win Panel)")]
    [Tooltip("Aktifkan transisi ke Win Panel")]
    public bool useOutroTransition = true;
    
    [Tooltip("Durasi fade in outro (detik)")]
    public float outroFadeInDuration = 0.5f;
    
    [Tooltip("Durasi tampil teks outro (detik)")]
    public float outroTextDuration = 1.5f;
    
    [Tooltip("Durasi fade out outro (detik)")]
    public float outroFadeOutDuration = 0.5f;

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
        Debug.Log($"[JUDGMENT] 😠 Ada {affectedBins} tong yang marah! Memulai slideshow dengan transisi...");
        
        // Tampilkan panel (tapi sembunyikan dulu konten utama)
        if (judgmentPanel != null)
        {
            judgmentPanel.SetActive(true);
            
            // Sembunyikan konten utama dulu (akan dimunculkan setelah transisi)
            if (imageTongMarah != null) imageTongMarah.enabled = false;
            if (textDialog != null) textDialog.enabled = false;
            if (textProgress != null) textProgress.enabled = false;
        }
        
        // Mulai dengan animasi transisi, lalu slideshow
        StartCoroutine(TransitionIntroCoroutine());
    }
    
    /// <summary>
    /// Animasi transisi intro sebelum slideshow tong marah
    /// Fade in → Tampil teks → Fade out → Mulai slideshow
    /// </summary>
    IEnumerator TransitionIntroCoroutine()
    {
        // Setup overlay transisi
        if (panelTransisiOverlay != null)
        {
            // Set warna overlay (jika ada Image component)
            Image overlayImage = panelTransisiOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                overlayImage.color = transisiOverlayColor;
            }
            
            panelTransisiOverlay.alpha = 0f; // Mulai transparan
            panelTransisiOverlay.gameObject.SetActive(true);
        }
        
        // Setup teks transisi
        if (textTransisiInfo != null)
        {
            // Pilih teks random yang sesuai untuk anak SD
            string[] pesanTransisi = new string[]
            {
                "Ayo Kita Lihat Kesalahanmu! 👀",
                "Ada Yang Salah Nih... 🤔",
                "Yuk Belajar Dari Kesalahan! 📚",
                "Tong Sampah Mau Ngomong Nih! 🗣️",
                "Wah, Ada Yang Kurang Tepat! 😅"
            };
            
            int mistakeCount = GameManager.Instance.GetAffectedBinCount();
            textTransisiInfo.text = pesanTransisi[Random.Range(0, pesanTransisi.Length)];
            textTransisiInfo.alpha = 0f; // Mulai transparan
        }
        
        Debug.Log("[JUDGMENT TRANSISI] 🎬 Memulai animasi intro...");
        
        // === FASE 1: FADE IN OVERLAY ===
        float elapsed = 0f;
        while (elapsed < transisiFadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / transisiFadeInDuration;
            
            if (panelTransisiOverlay != null)
            {
                panelTransisiOverlay.alpha = Mathf.Lerp(0f, 1f, t);
            }
            
            yield return null;
        }
        
        if (panelTransisiOverlay != null) panelTransisiOverlay.alpha = 1f;
        
        // === FASE 2: TAMPILKAN TEKS (FADE IN + SCALE) ===
        elapsed = 0f;
        Vector3 textStartScale = Vector3.one * 0.5f; // Mulai kecil
        
        while (elapsed < 0.5f) // 0.5 detik untuk animasi teks muncul
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.5f;
            float easedT = 1f - Mathf.Pow(1f - t, 3f); // Ease out cubic
            
            if (textTransisiInfo != null)
            {
                textTransisiInfo.alpha = easedT;
                textTransisiInfo.transform.localScale = Vector3.Lerp(textStartScale, Vector3.one, easedT);
            }
            
            yield return null;
        }
        
        if (textTransisiInfo != null)
        {
            textTransisiInfo.alpha = 1f;
            textTransisiInfo.transform.localScale = Vector3.one;
        }
        
        // === FASE 3: TAHAN TEKS (BIAR ANAK SEMPAT BACA) ===
        yield return new WaitForSecondsRealtime(transisiTextDuration);
        
        // === FASE 4: FADE OUT OVERLAY & TEKS ===
        elapsed = 0f;
        while (elapsed < transisiFadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / transisiFadeOutDuration;
            
            if (panelTransisiOverlay != null)
            {
                panelTransisiOverlay.alpha = Mathf.Lerp(1f, 0f, t);
            }
            
            if (textTransisiInfo != null)
            {
                textTransisiInfo.alpha = Mathf.Lerp(1f, 0f, t);
            }
            
            yield return null;
        }
        
        // Sembunyikan overlay
        if (panelTransisiOverlay != null)
        {
            panelTransisiOverlay.alpha = 0f;
            panelTransisiOverlay.gameObject.SetActive(false);
        }
        
        Debug.Log("[JUDGMENT TRANSISI] ✅ Transisi selesai! Mulai slideshow...");
        
        // Tampilkan konten utama
        if (imageTongMarah != null) imageTongMarah.enabled = true;
        if (textDialog != null) textDialog.enabled = true;
        if (textProgress != null) textProgress.enabled = true;
        
        // Mulai slideshow tong marah
        StartCoroutine(ShowSlidesCoroutine());
    }
    
    /// <summary>
    /// Animasi transisi outro setelah slideshow selesai (ke Win Panel)
    /// Sembunyikan konten → Fade in → Teks motivasi → Fade out
    /// </summary>
    IEnumerator TransitionOutroCoroutine()
    {
        // Sembunyikan konten slideshow (tong marah, dialog, dll)
        if (imageTongMarah != null) imageTongMarah.enabled = false;
        if (textDialog != null) textDialog.enabled = false;
        if (textProgress != null) textProgress.enabled = false;
        
        // Setup overlay
        if (panelTransisiOverlay != null)
        {
            Image overlayImage = panelTransisiOverlay.GetComponent<Image>();
            if (overlayImage != null)
            {
                overlayImage.color = transisiOverlayColor;
            }
            
            panelTransisiOverlay.alpha = 0f;
            panelTransisiOverlay.gameObject.SetActive(true);
        }
        
        // Setup teks outro (pesan motivasi/positif)
        if (textTransisiInfo != null)
        {
            // Pilih teks motivasi random untuk anak SD
            string[] pesanOutro = new string[]
            {
                "Bagus! Sekarang Kamu Sudah Tahu! 🎉",
                "Hebat! Kamu Belajar Hal Baru! ⭐",
                "Mantap! Besok Lebih Baik Lagi! 💪",
                "Yay! Kamu Makin Pintar! 🧠",
                "Keren! Jangan Lupa Ya! 👍"
            };
            
            textTransisiInfo.text = pesanOutro[Random.Range(0, pesanOutro.Length)];
            textTransisiInfo.alpha = 0f;
            textTransisiInfo.transform.localScale = Vector3.one * 0.5f; // Mulai kecil
        }
        
        Debug.Log("[JUDGMENT OUTRO] 🎬 Memulai animasi outro...");
        
        // === FASE 1: FADE IN OVERLAY ===
        float elapsed = 0f;
        while (elapsed < outroFadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / outroFadeInDuration;
            
            if (panelTransisiOverlay != null)
            {
                panelTransisiOverlay.alpha = Mathf.Lerp(0f, 1f, t);
            }
            
            yield return null;
        }
        
        if (panelTransisiOverlay != null) panelTransisiOverlay.alpha = 1f;
        
        // === FASE 2: TAMPILKAN TEKS MOTIVASI (FADE IN + SCALE + BOUNCE) ===
        elapsed = 0f;
        Vector3 textStartScale = Vector3.one * 0.5f;
        
        while (elapsed < 0.6f) // 0.6 detik untuk animasi teks (lebih cepat dari intro)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / 0.6f;
            
            // Bounce effect dengan overshoot
            float easedT = t < 0.5f 
                ? 2f * t * t // Ease in
                : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f; // Ease out with bounce
            
            if (textTransisiInfo != null)
            {
                textTransisiInfo.alpha = easedT;
                
                // Scale dengan sedikit overshoot (bounce)
                float scaleT = Mathf.Min(easedT * 1.1f, 1f);
                textTransisiInfo.transform.localScale = Vector3.Lerp(textStartScale, Vector3.one, scaleT);
            }
            
            yield return null;
        }
        
        if (textTransisiInfo != null)
        {
            textTransisiInfo.alpha = 1f;
            textTransisiInfo.transform.localScale = Vector3.one;
        }
        
        // === FASE 3: TAHAN TEKS (LEBIH SINGKAT DARI INTRO) ===
        yield return new WaitForSecondsRealtime(outroTextDuration);
        
        // === FASE 4: FADE OUT OVERLAY & TEKS ===
        elapsed = 0f;
        while (elapsed < outroFadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / outroFadeOutDuration;
            
            if (panelTransisiOverlay != null)
            {
                panelTransisiOverlay.alpha = Mathf.Lerp(1f, 0f, t);
            }
            
            if (textTransisiInfo != null)
            {
                textTransisiInfo.alpha = Mathf.Lerp(1f, 0f, t);
            }
            
            yield return null;
        }
        
        // Sembunyikan overlay
        if (panelTransisiOverlay != null)
        {
            panelTransisiOverlay.alpha = 0f;
            panelTransisiOverlay.gameObject.SetActive(false);
        }
        
        Debug.Log("[JUDGMENT OUTRO] ✅ Transisi outro selesai! Win Panel akan muncul...");
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
        
        // Cleanup icon terakhir
        ClearSpawnedIcons();
        
        // Cek apakah pakai transisi outro atau langsung Win Panel
        if (useOutroTransition && panelTransisiOverlay != null && textTransisiInfo != null)
        {
            Debug.Log("[JUDGMENT] 🎬 Memulai transisi outro ke Win Panel...");
            yield return StartCoroutine(TransitionOutroCoroutine());
        }
        else
        {
            Debug.Log("[JUDGMENT] ⏩ Langsung ke Win Panel tanpa transisi outro");
        }
        
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
                wasteNames += ", ";
            }
        }
        
        // Tambah "dan lainnya" jika lebih dari 3
        if (wrongWastes.Count > 3)
        {
            wasteNames += $" dan {wrongWastes.Count - 3} lainnya";
        }
        
        // Dialog template berdasarkan tipe tong (dengan personality berbeda)
        switch (binType)
        {
            case WasteType.Organik:
                return $"Aduh! Aku Tong Hijau untuk sampah organik!\n\n" +
                       $"Kok {wasteNames} dimasukkan ke sini? 😢\n\n" +
                       $"Sampah organik itu yang berasal dari makhluk hidup dan bisa membusuk, seperti sisa makanan!";
            
            case WasteType.Anorganik:
                return $"Hei! Aku Tong Kuning untuk sampah anorganik!\n\n" +
                       $"Masa {wasteNames} masuk ke sini sih? 😠\n\n" +
                       $"Sampah anorganik itu seperti plastik, kertas, dan kaleng yang bisa didaur ulang!";
            
            case WasteType.B3:
                return $"AWAS! Aku Tong Merah khusus B3 (Bahan Berbahaya)!\n\n" +
                       $"{wasteNames} bukan sampah berbahaya! 😤\n\n" +
                       $"B3 itu seperti baterai, lampu, dan obat-obatan yang bisa meracuni lingkungan!";
            
            default:
                return "Aku bingung... Sampah ini salah tempat deh! 🤔";
        }
    }
    
    /// <summary>
    /// Spawn icon sampah yang salah masuk ke container
    /// Layout: Menumpuk berserakan (overlap dengan random offset & rotation)
    /// </summary>
    void SpawnWasteIcons(List<WasteData> wrongWastes)
    {
        if (containerWasteIcons == null)
        {
            Debug.LogError("[JUDGMENT] containerWasteIcons NULL! Tidak bisa spawn icon.");
            return;
        }
        
        if (prefabWasteIcon == null)
        {
            Debug.LogError("[JUDGMENT] prefabWasteIcon NULL! Assign prefab di Inspector.");
            return;
        }
        
        // Cleanup icon lama (jika ada)
        ClearSpawnedIcons();
        
        Debug.Log($"[JUDGMENT] Spawn {wrongWastes.Count} icon sampah dengan animasi...");
        
        // Spawn setiap icon sampah dengan animasi berurutan
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
            
            // Set size & position
            RectTransform rt = iconObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(wasteIconSize, wasteIconSize);
                
                // Random offset untuk efek menumpuk berserakan (tidak kaku)
                float offsetX = Random.Range(-randomOffsetRange, randomOffsetRange);
                float offsetY = Random.Range(-randomOffsetRange, randomOffsetRange);
                
                // Posisi akhir = base position + random offset
                Vector2 finalPosition = baseIconPosition + new Vector2(offsetX, offsetY);
                
                // Random rotation untuk efek lebih natural
                float rotation = Random.Range(-randomRotationRange, randomRotationRange);
                
                // Mulai animasi spawn dengan delay berurutan
                StartCoroutine(AnimateIconSpawn(iconObj, rt, iconImage, finalPosition, rotation, i * iconSpawnDelay));
            }
            
            Debug.Log($"[JUDGMENT]   - Spawn icon: {waste.namaSampah} dengan animasi (delay: {i * iconSpawnDelay}s)");
        }
    }
    
    /// <summary>
    /// Animasi spawn untuk icon sampah (Scale + Fade + Drop)
    /// </summary>
    IEnumerator AnimateIconSpawn(GameObject iconObj, RectTransform rt, Image iconImage, Vector2 finalPosition, float finalRotation, float delay)
    {
        // Tunggu delay (untuk efek berurutan)
        yield return new WaitForSecondsRealtime(delay);
        
        // Setup kondisi awal animasi
        Vector2 startPosition = finalPosition;
        
        if (useScaleAnimation)
        {
            rt.localScale = Vector3.zero; // Mulai dari kecil
        }
        
        if (useFadeAnimation && iconImage != null)
        {
            Color startColor = iconImage.color;
            startColor.a = 0f; // Mulai transparan
            iconImage.color = startColor;
        }
        
        if (useDropAnimation)
        {
            startPosition = finalPosition + new Vector2(0, 100f); // Mulai dari atas
        }
        
        rt.anchoredPosition = startPosition;
        
        // Animasi dengan lerp
        float elapsed = 0f;
        
        while (elapsed < iconSpawnDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Gunakan unscaled agar tidak terpengaruh Time.timeScale
            float t = elapsed / iconSpawnDuration;
            
            // Smooth easing (ease out cubic)
            float easedT = 1f - Mathf.Pow(1f - t, 3f);
            
            // Scale animation
            if (useScaleAnimation)
            {
                rt.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, easedT);
            }
            
            // Fade animation
            if (useFadeAnimation && iconImage != null)
            {
                Color currentColor = iconImage.color;
                currentColor.a = Mathf.Lerp(0f, 1f, easedT);
                iconImage.color = currentColor;
            }
            
            // Drop animation
            if (useDropAnimation)
            {
                rt.anchoredPosition = Vector2.Lerp(startPosition, finalPosition, easedT);
            }
            
            // Rotation (smooth dari 0 ke final rotation)
            rt.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(0f, finalRotation, easedT));
            
            yield return null;
        }
        
        // Pastikan nilai akhir tepat
        rt.localScale = Vector3.one;
        rt.anchoredPosition = finalPosition;
        rt.rotation = Quaternion.Euler(0, 0, finalRotation);
        
        if (iconImage != null)
        {
            Color finalColor = iconImage.color;
            finalColor.a = 1f;
            iconImage.color = finalColor;
        }
        
        // Mulai animasi melayang setelah spawn selesai
        if (useFloatingAnimation && iconObj != null)
        {
            StartCoroutine(FloatingAnimation(rt, finalPosition, finalRotation));
        }
    }
    
    /// <summary>
    /// Animasi melayang tipis (idle animation) untuk icon sampah
    /// </summary>
    IEnumerator FloatingAnimation(RectTransform rt, Vector2 basePosition, float baseRotation)
    {
        if (rt == null) yield break;
        
        // Random offset untuk setiap icon biar tidak sinkron semua
        float randomOffset = Random.Range(0f, Mathf.PI * 2f);
        
        while (rt != null && rt.gameObject.activeInHierarchy)
        {
            float time = Time.unscaledTime * floatingSpeed + randomOffset;
            
            // Gerakan melayang pakai sine wave (naik-turun halus) - LEBIH KECIL
            float offsetY = Mathf.Sin(time) * floatingDistance;
            float offsetX = Mathf.Cos(time * 0.8f) * (floatingDistance * 0.3f); // Horizontal lebih halus
            
            // Update posisi dengan offset melayang
            rt.anchoredPosition = basePosition + new Vector2(offsetX, offsetY);
            
            // Rotasi sedikit mengikuti gerakan (untuk efek lebih natural) - DIKURANGI
            float rotationOffset = Mathf.Sin(time * 1.5f) * 1.5f; // Max ±1.5 derajat (lebih halus)
            rt.rotation = Quaternion.Euler(0, 0, baseRotation + rotationOffset);
            
            yield return null;
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
