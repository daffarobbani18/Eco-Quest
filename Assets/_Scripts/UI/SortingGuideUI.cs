using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SortingGuideUI : MonoBehaviour
{
    [Header("Panel References")]
    [Tooltip("Panel utama Sorting Guide")]
    public GameObject panelGuide;

    [Header("Container References")]
    [Tooltip("Parent untuk ikon sampah Organik (hijau)")]
    public Transform containerOrganik;
    
    [Tooltip("Parent untuk ikon sampah Anorganik (kuning)")]
    public Transform containerAnorganik;
    
    [Tooltip("Parent untuk ikon sampah B3 (merah)")]
    public Transform containerB3;

    [Header("Prefab")]
    [Tooltip("Prefab Image kosong dengan komponen Image untuk menampilkan icon sampah")]
    public GameObject iconPrefab;

    void OnEnable()
    {
        Debug.Log("==================================================");
        Debug.Log("[SORTING GUIDE] OnEnable() DIPANGGIL");
        Debug.Log($"⏱️ Time.timeScale: {Time.timeScale}");
        Debug.Log($"📦 panelGuide assigned: {(panelGuide != null ? "YES" : "NULL")}");
        Debug.Log($"🟢 containerOrganik assigned: {(containerOrganik != null ? "YES" : "NULL")}");
        Debug.Log($"🟡 containerAnorganik assigned: {(containerAnorganik != null ? "YES" : "NULL")}");
        Debug.Log($"🔴 containerB3 assigned: {(containerB3 != null ? "YES" : "NULL")}");
        Debug.Log($"📋 iconPrefab assigned: {(iconPrefab != null ? "YES" : "NULL")}");

        // Populate guide dengan icon sampah dari inventory
        PopulateGuide();
        
        Debug.Log("==================================================");
    }

    /// <summary>
    /// Mengisi panel guide dengan icon sampah yang unik dari inventory
    /// </summary>
    void PopulateGuide()
    {
        // Validasi GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager tidak ditemukan! Tidak bisa populate guide.");
            return;
        }

        // Ambil inventory dari GameManager
        var inventory = GameManager.Instance.trashInventory;

        // Validasi inventory tidak kosong
        if (inventory == null || inventory.Count == 0)
        {
            Debug.LogWarning("⚠️ Inventory kosong! Tidak ada sampah untuk ditampilkan.");
            return;
        }

        Debug.Log($"📦 Inventory berisi {inventory.Count} sampah. Memfilter yang unik...");

        // Saring item UNIK berdasarkan namaSampah
        var uniqueItems = inventory
            .GroupBy(item => item.namaSampah)
            .Select(group => group.First())
            .ToList();

        Debug.Log($"✅ {uniqueItems.Count} jenis sampah unik ditemukan");

        // Loop untuk setiap item unik
        foreach (var item in uniqueItems)
        {
            // Instantiate icon prefab
            GameObject iconObj = Instantiate(iconPrefab);
            
            // Ambil komponen Image
            Image iconImage = iconObj.GetComponent<Image>();
            
            if (iconImage == null)
            {
                Debug.LogError($"❌ Prefab '{iconPrefab.name}' tidak memiliki komponen Image!");
                Destroy(iconObj);
                continue;
            }

            // Set sprite dari WasteData
            if (item.iconSampah != null)
            {
                iconImage.sprite = item.iconSampah;
            }
            else
            {
                Debug.LogWarning($"⚠️ Item '{item.namaSampah}' tidak memiliki icon!");
            }

            // Tentukan container berdasarkan tipe sampah
            Transform targetContainer = null;

            switch (item.tipeSampah)
            {
                case WasteType.Organik:
                    targetContainer = containerOrganik;
                    Debug.Log($"🟢 {item.namaSampah} → Container Organik");
                    break;

                case WasteType.Anorganik:
                    targetContainer = containerAnorganik;
                    Debug.Log($"🟡 {item.namaSampah} → Container Anorganik");
                    break;

                case WasteType.B3:
                    targetContainer = containerB3;
                    Debug.Log($"🔴 {item.namaSampah} → Container B3");
                    break;

                default:
                    Debug.LogWarning($"⚠️ Tipe sampah '{item.tipeSampah}' tidak dikenali!");
                    targetContainer = containerOrganik; // Fallback
                    break;
            }

            // Validasi container
            if (targetContainer == null)
            {
                Debug.LogError($"❌ Container untuk tipe '{item.tipeSampah}' NULL!");
                Destroy(iconObj);
                continue;
            }

            // Set parent ke container yang sesuai
            iconObj.transform.SetParent(targetContainer, false);

            // Reset scale agar tidak gepeng
            iconObj.transform.localScale = Vector3.one;
        }

        Debug.Log("✅ Sorting Guide berhasil di-populate!");
    }

    /// <summary>
    /// Tutup panel guide dan panggil ProcessingLevelManager.MulaiMain()
    /// Dipanggil oleh tombol "Mengerti" / "Lanjut"
    /// </summary>
    public void TutupGuide()
    {
        Debug.Log("==================================================");
        Debug.Log("[SORTING GUIDE] TutupGuide() DIPANGGIL (Tombol Lanjut diklik)");
        Debug.Log($"⏱️ Time.timeScale sebelum: {Time.timeScale}");

        // Panggil ProcessingLevelManager untuk mulai game
        if (ProcessingLevelManager.Instance != null)
        {
            Debug.Log("✅ ProcessingLevelManager.Instance ditemukan");
            Debug.Log("🚀 Memanggil ProcessingLevelManager.MulaiMain()...");
            ProcessingLevelManager.Instance.MulaiMain();
        }
        else
        {
            Debug.LogError("❌ ProcessingLevelManager.Instance NULL! Tidak bisa mulai game.");
        }
        
        Debug.Log("==================================================");
    }

    /// <summary>
    /// Clear semua icon yang sudah di-spawn (untuk testing atau reset)
    /// </summary>
    public void ClearGuide()
    {
        ClearContainer(containerOrganik);
        ClearContainer(containerAnorganik);
        ClearContainer(containerB3);
        
        Debug.Log("🗑️ Sorting Guide di-clear");
    }

    /// <summary>
    /// Helper: Hapus semua child dari container
    /// </summary>
    private void ClearContainer(Transform container)
    {
        if (container == null) return;

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }
}
