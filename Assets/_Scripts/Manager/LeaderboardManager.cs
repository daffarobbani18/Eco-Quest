using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Manager untuk komunikasi dengan Google Sheets API via Google Apps Script
/// Menangani upload skor dan download leaderboard
/// 
/// SETUP REQUIRED:
/// 1. Deploy Google Apps Script (lihat tutorial di Docs/)
/// 2. Copy Web App URL ke Inspector field
/// 3. Test dengan tombol di Win Panel
/// </summary>
public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    [Header("Google Sheets Configuration")]
    [Tooltip("URL Google Apps Script Web App (dari deployment)")]
    public string googleAppsScriptURL = "";
    
    [Tooltip("Nama kelas untuk filter leaderboard (contoh: '3A', '3B', '4A')")]
    public string className = "3A";
    
    [Tooltip("Nama pemain (bisa diambil dari input field atau PlayerPrefs)")]
    public string playerName = "Player1";
    
    [Header("Timeout Settings")]
    [Tooltip("Timeout untuk request upload/download (detik)")]
    public int requestTimeout = 10;
    
    [Header("Debug Settings")]
    [Tooltip("Aktifkan untuk log detail request/response")]
    public bool debugMode = true;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load player name dari PlayerPrefs jika ada
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName = PlayerPrefs.GetString("PlayerName");
            Debug.Log($"📝 [LEADERBOARD] Player Name loaded: {playerName}");
        }
        
        // Validasi URL
        if (string.IsNullOrEmpty(googleAppsScriptURL))
        {
            Debug.LogWarning("⚠️ [LEADERBOARD] Google Apps Script URL belum diisi! Set di Inspector.");
        }
    }

    /// <summary>
    /// Upload skor pemain ke Google Sheets
    /// </summary>
    /// <param name="score">Skor akhir pemain</param>
    /// <param name="stars">Bintang yang didapat (1-3)</param>
    /// <param name="levelIndex">Index level yang dimainkan</param>
    /// <param name="callback">Callback dipanggil setelah selesai (success true/false)</param>
    public void UploadScore(int score, int stars, int levelIndex, Action<bool> callback)
    {
        if (string.IsNullOrEmpty(googleAppsScriptURL))
        {
            Debug.LogError("❌ [LEADERBOARD] Cannot upload - Google Apps Script URL kosong!");
            callback?.Invoke(false);
            return;
        }
        
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("❌ [LEADERBOARD] Cannot upload - Player Name kosong!");
            callback?.Invoke(false);
            return;
        }
        
        StartCoroutine(UploadScoreCoroutine(score, stars, levelIndex, callback));
    }

    /// <summary>
    /// Coroutine untuk upload skor dengan POST request
    /// </summary>
    IEnumerator UploadScoreCoroutine(int score, int stars, int levelIndex, Action<bool> callback)
    {
        Debug.Log("==================================================");
        Debug.Log($"📤 [LEADERBOARD] Uploading score...");
        Debug.Log($"   Player: {playerName}");
        Debug.Log($"   Class: {className}");
        Debug.Log($"   Level: {levelIndex}");
        Debug.Log($"   Score: {score}");
        Debug.Log($"   Stars: {stars}");
        
        // Buat data JSON untuk dikirim
        PlayerScoreData data = new PlayerScoreData
        {
            action = "upload",
            playerName = playerName,
            className = className,
            level = levelIndex,
            score = score,
            stars = stars,
            timestamp = GetCurrentTimestamp()
        };
        
        string jsonData = JsonUtility.ToJson(data);
        
        if (debugMode)
        {
            Debug.Log($"📄 [LEADERBOARD] JSON Data: {jsonData}");
        }
        
        // Buat POST request
        UnityWebRequest request = UnityWebRequest.Post(googleAppsScriptURL, jsonData, "application/json");
        request.timeout = requestTimeout;
        
        // Kirim request
        yield return request.SendWebRequest();
        
        // Cek hasil
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ [LEADERBOARD] Upload SUCCESS!");
            Debug.Log($"   Response: {request.downloadHandler.text}");
            Debug.Log("==================================================");
            callback?.Invoke(true);
        }
        else
        {
            Debug.LogError($"❌ [LEADERBOARD] Upload FAILED!");
            Debug.LogError($"   Error: {request.error}");
            Debug.LogError($"   Response Code: {request.responseCode}");
            Debug.LogError($"   Response: {request.downloadHandler.text}");
            Debug.Log("==================================================");
            callback?.Invoke(false);
        }
        
        request.Dispose();
    }

    /// <summary>
    /// Download leaderboard dari Google Sheets
    /// </summary>
    /// <param name="levelIndex">Level untuk filter leaderboard (0 = all levels)</param>
    /// <param name="callback">Callback dengan list LeaderboardEntry (null jika error)</param>
    public void GetLeaderboard(int levelIndex, Action<List<LeaderboardEntry>> callback)
    {
        if (string.IsNullOrEmpty(googleAppsScriptURL))
        {
            Debug.LogError("❌ [LEADERBOARD] Cannot get leaderboard - Google Apps Script URL kosong!");
            callback?.Invoke(null);
            return;
        }
        
        StartCoroutine(GetLeaderboardCoroutine(levelIndex, callback));
    }

    /// <summary>
    /// Coroutine untuk download leaderboard dengan GET request
    /// </summary>
    IEnumerator GetLeaderboardCoroutine(int levelIndex, Action<List<LeaderboardEntry>> callback)
    {
        Debug.Log("==================================================");
        Debug.Log($"📥 [LEADERBOARD] Downloading leaderboard...");
        Debug.Log($"   Class: {className}");
        Debug.Log($"   Level: {(levelIndex == 0 ? "All Levels" : levelIndex.ToString())}");
        
        // Buat URL dengan query parameters
        string url = $"{googleAppsScriptURL}?action=getLeaderboard&className={className}&level={levelIndex}";
        
        if (debugMode)
        {
            Debug.Log($"🔗 [LEADERBOARD] Request URL: {url}");
        }
        
        // Buat GET request
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = requestTimeout;
        
        // Kirim request
        yield return request.SendWebRequest();
        
        // Cek hasil
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            
            if (debugMode)
            {
                Debug.Log($"📄 [LEADERBOARD] JSON Response: {jsonResponse}");
            }
            
            // Parse JSON response
            List<LeaderboardEntry> entries = ParseLeaderboardJSON(jsonResponse);
            
            Debug.Log($"✅ [LEADERBOARD] Download SUCCESS!");
            Debug.Log($"   Total Entries: {entries.Count}");
            Debug.Log("==================================================");
            
            callback?.Invoke(entries);
        }
        else
        {
            Debug.LogError($"❌ [LEADERBOARD] Download FAILED!");
            Debug.LogError($"   Error: {request.error}");
            Debug.LogError($"   Response Code: {request.responseCode}");
            Debug.LogError($"   Response: {request.downloadHandler.text}");
            Debug.Log("==================================================");
            
            callback?.Invoke(null);
        }
        
        request.Dispose();
    }

    /// <summary>
    /// Parse JSON response dari Google Sheets ke List<LeaderboardEntry>
    /// </summary>
    List<LeaderboardEntry> ParseLeaderboardJSON(string json)
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        
        try
        {
            // Wrap JSON dalam object agar bisa di-parse Unity
            LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>("{\"entries\":" + json + "}");
            
            if (response != null && response.entries != null)
            {
                entries = response.entries;
                
                // Sort by score descending (tertinggi ke terendah)
                entries.Sort((a, b) => b.score.CompareTo(a.score));
                
                // Assign ranking
                for (int i = 0; i < entries.Count; i++)
                {
                    entries[i].rank = i + 1;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ [LEADERBOARD] JSON Parse Error: {e.Message}");
        }
        
        return entries;
    }

    /// <summary>
    /// Get timestamp saat ini dalam format ISO 8601
    /// </summary>
    string GetCurrentTimestamp()
    {
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Set player name (simpan di PlayerPrefs)
    /// </summary>
    public void SetPlayerName(string name)
    {
        playerName = name;
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
        Debug.Log($"📝 [LEADERBOARD] Player Name set to: {name}");
    }

    /// <summary>
    /// Set class name untuk filter leaderboard
    /// </summary>
    public void SetClassName(string className)
    {
        this.className = className;
        Debug.Log($"📝 [LEADERBOARD] Class Name set to: {className}");
    }
}

// ============ DATA STRUCTURES ============

/// <summary>
/// Data structure untuk upload score ke Google Sheets
/// </summary>
[Serializable]
public class PlayerScoreData
{
    public string action;       // "upload" atau "getLeaderboard"
    public string playerName;   // Nama pemain
    public string className;    // Kelas (3A, 3B, dst)
    public int level;           // Index level (1, 2, 3, dst)
    public int score;           // Skor akhir
    public int stars;           // Bintang (1-3)
    public string timestamp;    // Waktu upload (ISO 8601)
}

/// <summary>
/// Data structure untuk satu entry di leaderboard
/// </summary>
[Serializable]
public class LeaderboardEntry
{
    public int rank;            // Ranking (1 = tertinggi)
    public string playerName;   // Nama pemain
    public string className;    // Kelas
    public int level;           // Level
    public int score;           // Skor
    public int stars;           // Bintang
    public string timestamp;    // Waktu upload
}

/// <summary>
/// Wrapper untuk parse JSON array dari Google Sheets
/// </summary>
[Serializable]
public class LeaderboardResponse
{
    public List<LeaderboardEntry> entries;
}
