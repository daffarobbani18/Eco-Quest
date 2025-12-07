using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// UI Controller untuk Leaderboard Panel - Menampilkan ranking pemain
/// Attach script ini ke GameObject "Leaderboard Panel" di scene
/// 
/// HIERARCHY STRUCTURE:
/// Leaderboard Panel
/// ├── Header (Title "Leaderboard", Close Button)
/// ├── ScrollView
/// │   └── Viewport
/// │       └── Content (Vertical Layout Group)
/// │           └── [PlayerRow Prefabs akan di-instantiate di sini]
/// └── Refresh Button
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("ScrollView Content transform (parent untuk player rows)")]
    public Transform contentParent;
    
    [Tooltip("Prefab untuk satu row pemain di leaderboard")]
    public GameObject playerRowPrefab;
    
    [Tooltip("Button untuk refresh leaderboard")]
    public Button refreshButton;
    
    [Tooltip("Button untuk close panel")]
    public Button closeButton;
    
    [Tooltip("Text di refresh button")]
    public TMP_Text refreshButtonText;
    
    [Header("Level Filter")]
    [Tooltip("Filter by level (0 = all levels, 1-10 = specific level)")]
    public int levelFilter = 0;
    
    [Tooltip("Dropdown untuk pilih level (optional)")]
    public TMP_Dropdown levelDropdown;
    
    [Header("Highlight Settings")]
    [Tooltip("Warna highlight untuk row pemain saat ini")]
    public Color highlightColor = new Color(1f, 0.92f, 0.016f, 0.3f); // Gold semi-transparent
    
    [Tooltip("Warna normal untuk row pemain lain")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.1f); // White semi-transparent
    
    [Header("Loading State")]
    [Tooltip("GameObject loading spinner/text (aktif saat loading)")]
    public GameObject loadingIndicator;
    
    [Tooltip("GameObject empty state (aktif jika leaderboard kosong)")]
    public GameObject emptyStatePanel;
    
    [Header("Audio")]
    [Tooltip("Audio Source untuk sfx")]
    public AudioSource sfxSource;
    
    [Tooltip("Sound effect saat refresh")]
    public AudioClip refreshSound;
    
    // Internal state
    private List<GameObject> spawnedRows = new List<GameObject>();
    private bool isLoading = false;

    void Start()
    {
        // Setup button listeners
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(OnClickRefresh);
        }
        
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnClickClose);
        }
        
        // Setup dropdown listener
        if (levelDropdown != null)
        {
            levelDropdown.onValueChanged.AddListener(OnLevelFilterChanged);
        }
    }

    void OnEnable()
    {
        // Auto-refresh saat panel dibuka
        RefreshLeaderboard();
    }

    /// <summary>
    /// Refresh leaderboard dari server
    /// </summary>
    public void RefreshLeaderboard()
    {
        if (isLoading) return; // Jangan refresh jika sedang loading
        
        if (LeaderboardManager.Instance == null)
        {
            Debug.LogError("❌ [LEADERBOARD UI] LeaderboardManager tidak ditemukan!");
            return;
        }
        
        // Play sound
        if (sfxSource != null && refreshSound != null)
        {
            sfxSource.PlayOneShot(refreshSound);
        }
        
        // Show loading state
        SetLoadingState(true);
        
        // Request data dari LeaderboardManager
        Debug.Log($"🔄 [LEADERBOARD UI] Refreshing leaderboard (Level: {(levelFilter == 0 ? "All" : levelFilter.ToString())})");
        LeaderboardManager.Instance.GetLeaderboard(levelFilter, OnLeaderboardDataReceived);
    }

    /// <summary>
    /// Callback saat data leaderboard diterima
    /// </summary>
    void OnLeaderboardDataReceived(List<LeaderboardEntry> entries)
    {
        SetLoadingState(false);
        
        if (entries == null)
        {
            Debug.LogError("❌ [LEADERBOARD UI] Failed to get leaderboard data!");
            ShowEmptyState(true, "Failed to load data.\nPlease try again.");
            return;
        }
        
        if (entries.Count == 0)
        {
            Debug.Log("📭 [LEADERBOARD UI] Leaderboard kosong (belum ada data)");
            ShowEmptyState(true, "No scores yet.\nBe the first to play!");
            return;
        }
        
        ShowEmptyState(false, "");
        
        // Populate leaderboard
        PopulateLeaderboard(entries);
    }

    /// <summary>
    /// Populate leaderboard dengan data entries
    /// </summary>
    void PopulateLeaderboard(List<LeaderboardEntry> entries)
    {
        // Clear existing rows
        ClearLeaderboard();
        
        // Spawn new rows
        string currentPlayerName = LeaderboardManager.Instance.playerName;
        
        for (int i = 0; i < entries.Count; i++)
        {
            LeaderboardEntry entry = entries[i];
            
            // Instantiate row
            GameObject row = Instantiate(playerRowPrefab, contentParent);
            spawnedRows.Add(row);
            
            // Get components
            TMP_Text rankText = row.transform.Find("RankText")?.GetComponent<TMP_Text>();
            TMP_Text nameText = row.transform.Find("NameText")?.GetComponent<TMP_Text>();
            TMP_Text scoreText = row.transform.Find("ScoreText")?.GetComponent<TMP_Text>();
            TMP_Text starsText = row.transform.Find("StarsText")?.GetComponent<TMP_Text>();
            Image background = row.GetComponent<Image>();
            
            // Set data
            if (rankText != null)
            {
                // Top 3 dapat emoji trophy
                if (entry.rank == 1) rankText.text = "🥇";
                else if (entry.rank == 2) rankText.text = "🥈";
                else if (entry.rank == 3) rankText.text = "🥉";
                else rankText.text = entry.rank.ToString();
            }
            
            if (nameText != null)
                nameText.text = entry.playerName;
            
            if (scoreText != null)
                scoreText.text = entry.score.ToString();
            
            if (starsText != null)
            {
                // Display stars sebagai emoji
                string starDisplay = "";
                for (int s = 0; s < entry.stars; s++)
                {
                    starDisplay += "⭐";
                }
                starsText.text = starDisplay;
            }
            
            // Highlight row jika ini pemain saat ini
            if (background != null)
            {
                if (entry.playerName == currentPlayerName)
                {
                    background.color = highlightColor;
                    Debug.Log($"✨ [LEADERBOARD UI] Highlighted player '{currentPlayerName}' at rank {entry.rank}");
                }
                else
                {
                    background.color = normalColor;
                }
            }
        }
        
        Debug.Log($"✅ [LEADERBOARD UI] Populated {entries.Count} entries");
    }

    /// <summary>
    /// Clear semua rows yang sudah di-spawn
    /// </summary>
    void ClearLeaderboard()
    {
        foreach (GameObject row in spawnedRows)
        {
            if (row != null) Destroy(row);
        }
        spawnedRows.Clear();
    }

    /// <summary>
    /// Set loading state UI
    /// </summary>
    void SetLoadingState(bool loading)
    {
        isLoading = loading;
        
        if (loadingIndicator != null)
            loadingIndicator.SetActive(loading);
        
        if (refreshButton != null)
            refreshButton.interactable = !loading;
        
        if (refreshButtonText != null)
        {
            refreshButtonText.text = loading ? "Loading..." : "Refresh";
        }
    }

    /// <summary>
    /// Tampilkan/sembunyikan empty state panel
    /// </summary>
    void ShowEmptyState(bool show, string message)
    {
        if (emptyStatePanel != null)
        {
            emptyStatePanel.SetActive(show);
            
            // Update text jika ada
            TMP_Text emptyText = emptyStatePanel.GetComponentInChildren<TMP_Text>();
            if (emptyText != null && !string.IsNullOrEmpty(message))
            {
                emptyText.text = message;
            }
        }
    }

    /// <summary>
    /// Event handler saat Refresh button diklik
    /// </summary>
    void OnClickRefresh()
    {
        RefreshLeaderboard();
    }

    /// <summary>
    /// Event handler saat Close button diklik
    /// </summary>
    void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Event handler saat level filter dropdown berubah
    /// </summary>
    void OnLevelFilterChanged(int dropdownIndex)
    {
        // Dropdown index 0 = "All Levels", 1 = "Level 1", dst
        levelFilter = dropdownIndex;
        
        Debug.Log($"🔍 [LEADERBOARD UI] Level filter changed to: {(levelFilter == 0 ? "All Levels" : $"Level {levelFilter}")}");
        
        RefreshLeaderboard();
    }

    void OnDestroy()
    {
        // Cleanup listeners
        if (refreshButton != null)
            refreshButton.onClick.RemoveListener(OnClickRefresh);
        
        if (closeButton != null)
            closeButton.onClick.RemoveListener(OnClickClose);
        
        if (levelDropdown != null)
            levelDropdown.onValueChanged.RemoveListener(OnLevelFilterChanged);
    }
}
