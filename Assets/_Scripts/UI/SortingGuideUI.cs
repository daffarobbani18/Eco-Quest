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

    void Start()
    {
        // Pause game agar pemain punya waktu membaca panduan
        Time.timeScale = 0f;
        Debug.Log("📋 Sorting Guide ditampilkan - Game di-pause");

        // Populate guide dengan icon sampah dari inventory
        PopulateGuide();
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
    /// Tutup panel guide dan lanjutkan game
    /// Dipanggil oleh tombol "Mengerti" / "Lanjut"
    /// </summary>
    public void TutupGuide()
    {
        Debug.Log("✅ Sorting Guide ditutup - Game dilanjutkan");

        // Matikan panel
        if (panelGuide != null)
        {
            panelGuide.SetActive(false);
        }

        // Lanjutkan game
        Time.timeScale = 1f;
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
