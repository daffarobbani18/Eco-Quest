# 📘 TUTORIAL SETUP STAR RATING & GOOGLE SHEETS LEADERBOARD

## 🎯 Overview
Tutorial ini akan memandu Anda untuk setup lengkap **Star Rating System** dan **Google Sheets Leaderboard** di project Eco-Quest Unity.

**Fitur yang akan diimplementasikan:**
- ⭐ Star Rating (1-3 bintang berdasarkan skor)
- 💾 Save best stars & score per level
- 📊 Google Sheets Leaderboard (real-time sync)
- 🏆 Display leaderboard di in-game panel
- ✨ Star display di Level Selection
- 🎉 "New Record!" popup animation

---

## 📂 PART 1: GOOGLE SHEETS SETUP

### Step 1: Buat Google Spreadsheet Baru

1. Buka [Google Sheets](https://sheets.google.com)
2. Klik **Blank** untuk buat spreadsheet baru
3. Rename spreadsheet: `Eco Quest Leaderboard`
4. Buat header kolom di **Row 1**:
   - **A1**: `PlayerName`
   - **B1**: `ClassName`
   - **C1**: `Level`
   - **D1**: `Score`
   - **E1**: `Stars`
   - **F1**: `Timestamp`

**Contoh isi sheet:**
```
| PlayerName | ClassName | Level | Score | Stars | Timestamp           |
|------------|-----------|-------|-------|-------|---------------------|
| Andi       | 3A        | 1     | 95    | 3     | 2025-01-15 10:30:00 |
| Budi       | 3A        | 1     | 82    | 2     | 2025-01-15 10:35:00 |
| Citra      | 3B        | 2     | 88    | 2     | 2025-01-15 11:00:00 |
```

---

### Step 2: Buat Google Apps Script

1. Di spreadsheet, klik **Extensions** → **Apps Script**
2. Delete semua code default
3. Paste code berikut:

```javascript
// ============================================================
// GOOGLE APPS SCRIPT - ECO QUEST LEADERBOARD API
// ============================================================

// Config: Nama sheet untuk menyimpan data
const SHEET_NAME = "Sheet1"; // Ganti jika nama sheet berbeda

/**
 * Fungsi utama untuk handle POST request (Upload Score)
 */
function doPost(e) {
  try {
    // Parse JSON dari Unity
    const data = JSON.parse(e.postData.contents);
    
    Logger.log("📥 POST Request Data: " + JSON.stringify(data));
    
    // Cek action
    if (data.action === "upload") {
      return uploadScore(data);
    }
    
    // Action tidak dikenali
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: "Unknown action" })
    ).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("❌ ERROR in doPost: " + error.toString());
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: error.toString() })
    ).setMimeType(ContentService.MimeType.JSON);
  }
}

/**
 * Fungsi utama untuk handle GET request (Get Leaderboard)
 */
function doGet(e) {
  try {
    const action = e.parameter.action;
    
    Logger.log("📥 GET Request Action: " + action);
    
    if (action === "getLeaderboard") {
      const className = e.parameter.className || "";
      const level = parseInt(e.parameter.level) || 0;
      
      Logger.log("   ClassName: " + className);
      Logger.log("   Level: " + level);
      
      return getLeaderboard(className, level);
    }
    
    // Action tidak dikenali
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: "Unknown action" })
    ).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("❌ ERROR in doGet: " + error.toString());
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: error.toString() })
    ).setMimeType(ContentService.MimeType.JSON);
  }
}

/**
 * Upload score ke spreadsheet
 */
function uploadScore(data) {
  try {
    const sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName(SHEET_NAME);
    
    if (!sheet) {
      throw new Error("Sheet tidak ditemukan: " + SHEET_NAME);
    }
    
    // Data yang akan dimasukkan
    const row = [
      data.playerName,
      data.className,
      data.level,
      data.score,
      data.stars,
      data.timestamp
    ];
    
    // Append ke sheet
    sheet.appendRow(row);
    
    Logger.log("✅ Score uploaded: " + JSON.stringify(row));
    
    return ContentService.createTextOutput(
      JSON.stringify({
        status: "success",
        message: "Score uploaded successfully",
        data: row
      })
    ).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("❌ ERROR in uploadScore: " + error.toString());
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: error.toString() })
    ).setMimeType(ContentService.MimeType.JSON);
  }
}

/**
 * Get leaderboard dengan filter class dan level
 */
function getLeaderboard(className, level) {
  try {
    const sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName(SHEET_NAME);
    
    if (!sheet) {
      throw new Error("Sheet tidak ditemukan: " + SHEET_NAME);
    }
    
    // Get all data (skip header row)
    const data = sheet.getDataRange().getValues();
    const headers = data[0];
    const rows = data.slice(1);
    
    Logger.log("📊 Total rows: " + rows.length);
    
    // Filter data berdasarkan class dan level
    let filtered = rows.filter(row => {
      const rowClassName = row[1]; // Column B: ClassName
      const rowLevel = row[2];     // Column C: Level
      
      // Filter by class
      if (className && rowClassName !== className) {
        return false;
      }
      
      // Filter by level (0 = all levels)
      if (level > 0 && rowLevel !== level) {
        return false;
      }
      
      return true;
    });
    
    Logger.log("📊 Filtered rows: " + filtered.length);
    
    // Convert to JSON objects
    const result = filtered.map(row => {
      return {
        playerName: row[0],
        className: row[1],
        level: row[2],
        score: row[3],
        stars: row[4],
        timestamp: row[5]
      };
    });
    
    // Sort by score descending (tertinggi ke terendah)
    result.sort((a, b) => b.score - a.score);
    
    Logger.log("✅ Leaderboard data ready: " + result.length + " entries");
    
    return ContentService.createTextOutput(
      JSON.stringify(result)
    ).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("❌ ERROR in getLeaderboard: " + error.toString());
    return ContentService.createTextOutput(
      JSON.stringify({ status: "error", message: error.toString() })
    ).setMimeType(ContentService.MimeType.JSON);
  }
}
```

4. **Save** script:
   - Klik **💾 Save** (atau `Ctrl+S`)
   - Beri nama project: `Eco Quest Leaderboard API`

---

### Step 3: Deploy Apps Script sebagai Web App

1. Klik **Deploy** → **New deployment**
2. Klik **⚙️ Settings icon** (gear) → Pilih **Web app**
3. Isi konfigurasi:
   - **Description**: `Eco Quest Leaderboard API v1`
   - **Execute as**: **Me** (your email)
   - **Who has access**: **Anyone** (penting!)
4. Klik **Deploy**
5. Klik **Authorize access**:
   - Pilih akun Google Anda
   - Klik **Advanced** → **Go to [Project Name] (unsafe)**
   - Klik **Allow**
6. **COPY WEB APP URL** yang muncul:
   - Format: `https://script.google.com/macros/s/[YOUR_DEPLOYMENT_ID]/exec`
   - **PENTING**: Simpan URL ini, akan digunakan di Unity!

✅ **Apps Script setup complete!**

---

### Step 4: Test Apps Script (Optional)

Test via browser untuk pastikan API bekerja:

**Test GET Leaderboard:**
```
https://script.google.com/macros/s/[YOUR_ID]/exec?action=getLeaderboard&className=3A&level=1
```

**Expected Response:**
```json
[
  {
    "playerName": "Andi",
    "className": "3A",
    "level": 1,
    "score": 95,
    "stars": 3,
    "timestamp": "2025-01-15 10:30:00"
  }
]
```

---

## 🎮 PART 2: UNITY SETUP

### Step 1: Cek Script Files

Pastikan semua script sudah ada di project:

```
Assets/_Scripts/
├── Manager/
│   ├── GameManager.cs ✅ (Updated dengan Star Rating)
│   └── LeaderboardManager.cs ✅ (NEW)
└── UI/
    ├── WinPanelController.cs ✅ (NEW)
    ├── LeaderboardUI.cs ✅ (NEW)
    └── LevelButton.cs ✅ (Updated dengan star display)
```

---

### Step 2: Setup LeaderboardManager GameObject

1. Buka scene **00_MainMenu** (atau scene pertama yang load)
2. Create Empty GameObject:
   - Right-click Hierarchy → **Create Empty**
   - Rename: `LeaderboardManager`
3. Attach script:
   - Add Component → **LeaderboardManager**
4. Configure Inspector:
   - **Google Apps Script URL**: Paste URL dari Step 3 deployment
   - **Class Name**: Isi dengan kelas default (contoh: `3A`)
   - **Player Name**: Isi dengan nama default (contoh: `Player1`)
   - **Request Timeout**: `10`
   - **Debug Mode**: ✅ Checked (untuk testing)

> **NOTE**: GameObject ini akan `DontDestroyOnLoad()`, jadi hanya perlu setup 1x di Main Menu.

---

### Step 3: Setup Win Panel UI

Buka scene game (contoh: **03_Game_Processing**)

#### A. Cek Win Panel Existing
1. Cari GameObject `Win Panel` di Hierarchy
2. Pastikan sudah ada UI berikut:
   - **textSkorAkhir** (TMP_Text)
   - **textWaktuAkhir** (TMP_Text)
   - Button **Next Level** / **Main Menu**

#### B. Tambah Star Display
1. Select `Win Panel`
2. Create 3 star icons:
   - Right-click `Win Panel` → **UI** → **Image**
   - Rename: `Star1`, `Star2`, `Star3`
   - Position: Horizontal layout di atas skor
   - Size: 64x64 pixels (adjust as needed)
3. **PENTING**: Set initial scale ke **Vector3.zero** (0, 0, 0) untuk animasi

#### C. Tambah New Record Popup
1. Right-click `Win Panel` → **UI** → **Image**
2. Rename: `NewRecordPopup`
3. Add child:
   - Right-click `NewRecordPopup` → **UI** → **Text - TextMeshPro**
   - Rename: `PopupText`
   - Text: `🎉 NEW RECORD! 🎉`
   - Font Size: 48
   - Alignment: Center
4. Set `NewRecordPopup` **inactive** (uncheck checkbox di Inspector)

#### D. Tambah Upload Button
1. Right-click `Win Panel` → **UI** → **Button - TextMeshPro**
2. Rename: `UploadLeaderboardButton`
3. Text: `Upload to Leaderboard`
4. Position: Below stars, di atas Next Level button

#### E. Attach WinPanelController
1. Select `Win Panel`
2. Add Component → **WinPanelController**
3. Configure Inspector:
   - **Star Objects**: Drag `Star1`, `Star2`, `Star3` (array size 3)
   - **Empty Star**: Assign sprite bintang kosong (outline/gray)
   - **Filled Star**: Assign sprite bintang terisi (gold/yellow)
   - **New Record Popup**: Drag `NewRecordPopup` GameObject
   - **Upload Leaderboard Button**: Drag `UploadLeaderboardButton`
   - **Upload Button Text**: Drag text child dari button
   - **Star Animation Delay**: `0.3`
   - **Star Animation Duration**: `0.2`
   - **Star Final Scale**: `1`
   - **Sfx Source**: Drag Audio Source dari scene
   - **Star Appear Sound**: Assign audio clip
   - **New Record Sound**: Assign audio clip

✅ **Win Panel setup complete!**

---

### Step 4: Setup Leaderboard Panel UI

Buka scene **00_MainMenu** (atau scene dimana leaderboard panel akan ditampilkan)

#### A. Buat Leaderboard Panel
1. Right-click Hierarchy → **UI** → **Panel**
2. Rename: `LeaderboardPanel`
3. Configure:
   - Anchor: **Stretch** (full screen)
   - Color: Semi-transparent black (untuk overlay)
4. Set **inactive** (uncheck checkbox) - akan diaktifkan via script

#### B. Buat Container Panel
1. Right-click `LeaderboardPanel` → **UI** → **Image**
2. Rename: `Container`
3. Configure:
   - Anchor: **Center**
   - Width: `800`, Height: `600`
   - Color: White atau custom background

#### C. Tambah Header
1. Right-click `Container` → **UI** → **Text - TextMeshPro**
2. Rename: `HeaderText`
3. Text: `🏆 LEADERBOARD 🏆`
4. Font Size: 48
5. Alignment: Center
6. Position: Top of container

#### D. Tambah Close Button
1. Right-click `Container` → **UI** → **Button - TextMeshPro**
2. Rename: `CloseButton`
3. Text: `✖`
4. Font Size: 36
5. Position: Top-right corner (Anchor: Top-Right)

#### E. Buat ScrollView
1. Right-click `Container` → **UI** → **Scroll View**
2. Rename: `LeaderboardScrollView`
3. Configure:
   - Delete **Horizontal Scrollbar** (only need vertical)
   - **Content** → Add Component → **Vertical Layout Group**:
     - **Spacing**: 10
     - **Child Force Expand**: Width ✅, Height ❌
     - **Child Control Size**: Width ✅, Height ✅
   - **Content** → Add Component → **Content Size Fitter**:
     - **Vertical Fit**: Preferred Size

#### F. Buat Player Row Prefab
1. Right-click `Content` → **UI** → **Image**
2. Rename: `PlayerRow`
3. Configure:
   - Width: Auto (from parent), Height: `80`
   - Add Component → **Horizontal Layout Group**:
     - **Spacing**: 20
     - **Padding**: Left 20, Right 20
     - **Child Force Expand**: Width ❌, Height ✅
     - **Child Control Size**: Width ❌, Height ✅
4. Add child elements:
   - **RankText** (TMP_Text):
     - Text: `1`
     - Font Size: 36
     - Alignment: Center
     - Preferred Width: 80
   - **NameText** (TMP_Text):
     - Text: `Player Name`
     - Font Size: 28
     - Alignment: Left
     - Preferred Width: 250
   - **ScoreText** (TMP_Text):
     - Text: `100`
     - Font Size: 28
     - Alignment: Center
     - Preferred Width: 100
   - **StarsText** (TMP_Text):
     - Text: `⭐⭐⭐`
     - Font Size: 28
     - Alignment: Center
     - Preferred Width: 120
5. **PENTING**: Drag `PlayerRow` ke folder **Prefabs** untuk buat prefab
6. Delete `PlayerRow` dari Hierarchy (tinggalkan `Content` kosong)

#### G. Tambah Refresh Button
1. Right-click `Container` → **UI** → **Button - TextMeshPro**
2. Rename: `RefreshButton`
3. Text: `🔄 Refresh`
4. Position: Bottom of container

#### H. Tambah Loading Indicator
1. Right-click `Container` → **UI** → **Text - TextMeshPro**
2. Rename: `LoadingIndicator`
3. Text: `Loading...`
4. Font Size: 32
5. Alignment: Center
6. Position: Center of ScrollView
7. Set **inactive** (will be toggled by script)

#### I. Tambah Empty State Panel
1. Right-click `Container` → **UI** → **Panel**
2. Rename: `EmptyStatePanel`
3. Add child:
   - Right-click `EmptyStatePanel` → **UI** → **Text - TextMeshPro**
   - Text: `No scores yet.\nBe the first to play!`
   - Font Size: 28
   - Alignment: Center
4. Set **inactive** (will be toggled by script)

#### J. Attach LeaderboardUI Script
1. Select `LeaderboardPanel`
2. Add Component → **LeaderboardUI**
3. Configure Inspector:
   - **Content Parent**: Drag `Content` (inside ScrollView)
   - **Player Row Prefab**: Drag prefab dari folder Prefabs
   - **Refresh Button**: Drag `RefreshButton`
   - **Close Button**: Drag `CloseButton`
   - **Refresh Button Text**: Drag text child dari button
   - **Level Filter**: `0` (0 = all levels)
   - **Level Dropdown**: Leave empty (optional feature)
   - **Highlight Color**: RGB(255, 235, 4, 77) - Gold semi-transparent
   - **Normal Color**: RGB(255, 255, 255, 26) - White semi-transparent
   - **Loading Indicator**: Drag `LoadingIndicator`
   - **Empty State Panel**: Drag `EmptyStatePanel`
   - **Sfx Source**: Drag Audio Source
   - **Refresh Sound**: Assign audio clip

#### K. Buat Button untuk Open Leaderboard
1. Buka scene **00_MainMenu**
2. Cari button existing atau buat baru:
   - Right-click Canvas → **UI** → **Button - TextMeshPro**
   - Rename: `LeaderboardButton`
   - Text: `🏆 Leaderboard`
3. Configure button **OnClick()**:
   - Drag `LeaderboardPanel` ke slot
   - Function: **GameObject.SetActive**
   - Check: ✅ (true)

✅ **Leaderboard Panel setup complete!**

---

### Step 5: Update Level Selection Stars

Buka scene **01_Hub_Klub** (atau scene dengan Level Selection)

#### A. Update Each Level Button
For each level button (Level 1, Level 2, dst):

1. Select level button GameObject
2. Add 3 star icons:
   - Right-click button → **UI** → **Image**
   - Rename: `Star1`, `Star2`, `Star3`
   - Position: Below level number/name
   - Size: 32x32 pixels (smaller than Win Panel stars)
   - **PENTING**: Set semua stars **inactive** by default
3. Add best score text:
   - Right-click button → **UI** → **Text - TextMeshPro**
   - Rename: `BestScoreText`
   - Text: `Best: 0`
   - Font Size: 18
   - Position: Below stars
4. Select level button GameObject
5. Configure **LevelButton** component Inspector:
   - **Star Icons**: Drag `Star1`, `Star2`, `Star3` (array size 3)
   - **Best Score Text**: Drag `BestScoreText`

> Script akan auto-update stars & best score di `Start()`

✅ **Level Selection setup complete!**

---

## 🧪 PART 3: TESTING

### Test 1: Star Rating System

1. Play game di Unity Editor
2. Mainkan level sampai selesai
3. Cek Win Panel:
   - ✅ Bintang muncul dengan animasi (1-3 bintang)
   - ✅ "New Record!" popup muncul (jika pertama kali main)
   - ✅ Score & time displayed correctly
4. Back to Level Selection:
   - ✅ Stars displayed on level button
   - ✅ "Best: [score]" displayed
5. Replay level dengan skor lebih rendah:
   - ✅ Best stars tetap (tidak overwrite)
6. Replay level dengan skor lebih tinggi:
   - ✅ Best stars & score updated

**Debug Console Check:**
```
⭐ [SHOW WIN PANEL] Skor: 95 → 3 Bintang
🎉 [SHOW WIN PANEL] NEW RECORD untuk Level 1!
⭐ [STAR RATING] Level 1 - New Best: 3 bintang! (Previous: 0)
🏆 [STAR RATING] Level 1 - New High Score: 95! (Previous: 0)
```

---

### Test 2: Upload to Leaderboard

1. Play game sampai Win Panel
2. Klik button **"Upload to Leaderboard"**
3. Cek Debug Console:
   ```
   📤 [WIN PANEL] Uploading score: 95 (3★) for Level 1
   📤 [LEADERBOARD] Uploading score...
      Player: Player1
      Class: 3A
      Level: 1
      Score: 95
      Stars: 3
   ✅ [LEADERBOARD] Upload SUCCESS!
   ```
4. Button text berubah: `Uploading...` → `Uploaded! ✓`
5. Cek Google Sheets:
   - ✅ Row baru muncul dengan data pemain

---

### Test 3: View Leaderboard

1. Buka scene Main Menu
2. Klik button **"🏆 Leaderboard"**
3. Cek Leaderboard Panel:
   - ✅ Panel muncul dengan smooth transition
   - ✅ Loading indicator muncul sebentar
   - ✅ Player rows populated dengan data
   - ✅ Ranking sorted (tertinggi ke terendah)
   - ✅ Top 3 dapat emoji trophy (🥇🥈🥉)
   - ✅ Current player row highlighted (gold background)
4. Klik **"🔄 Refresh"**:
   - ✅ Leaderboard reload dari server
5. Klik **"✖ Close"**:
   - ✅ Panel close smoothly

**Debug Console Check:**
```
🔄 [LEADERBOARD UI] Refreshing leaderboard (Level: All)
📥 [LEADERBOARD] Downloading leaderboard...
   Class: 3A
   Level: All Levels
✅ [LEADERBOARD] Download SUCCESS!
   Total Entries: 5
✅ [LEADERBOARD UI] Populated 5 entries
✨ [LEADERBOARD UI] Highlighted player 'Player1' at rank 2
```

---

### Test 4: Filter by Class & Level

1. Update **LeaderboardManager** → **Class Name**: `3B`
2. Upload score baru
3. View leaderboard:
   - ✅ Hanya muncul scores dari class 3B
4. Update **LeaderboardUI** → **Level Filter**: `1`
5. Refresh:
   - ✅ Hanya muncul scores dari Level 1

---

## 🐛 TROUBLESHOOTING

### Problem 1: "LeaderboardManager tidak ditemukan"
**Cause**: GameObject `LeaderboardManager` belum ada di scene atau script belum attached.

**Solution**:
1. Cek Hierarchy: ada GameObject `LeaderboardManager`?
2. Cek Inspector: script `LeaderboardManager.cs` attached?
3. Pastikan scene Main Menu di-load first (DontDestroyOnLoad)

---

### Problem 2: Upload Failed - Response Code 404
**Cause**: Google Apps Script URL salah atau deployment belum aktif.

**Solution**:
1. Buka Apps Script → **Deploy** → **Manage deployments**
2. Cek status: **Active** ✅
3. Copy URL lagi, pastikan tidak ada extra space/character
4. Test URL di browser: harus return JSON (bukan error page)

---

### Problem 3: Upload Failed - CORS Error
**Cause**: Google Apps Script execution permissions salah.

**Solution**:
1. Buka Apps Script → **Deploy** → **Manage deployments**
2. Edit deployment:
   - **Execute as**: Me (bukan User accessing the web app)
   - **Who has access**: Anyone (bukan Only myself)
3. Save deployment
4. Test lagi dari Unity

---

### Problem 4: Leaderboard Kosong (No Data)
**Cause**: Filter class/level terlalu strict atau belum ada data.

**Solution**:
1. Cek Google Sheets: ada data?
2. Cek **LeaderboardManager** → **Class Name**: match dengan data di sheet?
3. Set **Level Filter** = `0` untuk test (all levels)
4. Cek Debug Console: ada error parsing JSON?

---

### Problem 5: Stars Tidak Muncul di Win Panel
**Cause**: Star GameObjects belum di-link atau sprite kosong.

**Solution**:
1. Select `Win Panel` → Cek `WinPanelController` Inspector
2. **Star Objects** array: size 3, semua slot terisi?
3. **Empty Star** & **Filled Star** sprites assigned?
4. Cek star GameObjects: initial scale = (0, 0, 0)?
5. Play mode → Cek Debug Console:
   ```
   ⭐ [WIN PANEL] Displaying 3 stars (Record: true)
   ✅ [WIN PANEL] Star animation complete!
   ```

---

### Problem 6: "New Record!" Tidak Muncul Padahal Beat Record
**Cause**: `IsNewRecord()` check before `SaveBestStars()`.

**Solution**:
Code sudah fix di `GameManager.ShowWinPanel()`:
```csharp
bool isRecord = IsNewRecord(indexLevelSaatIni); // Check BEFORE save
SaveBestStars(indexLevelSaatIni, stars);       // Then save
```

Pastikan order ini benar di code Anda.

---

### Problem 7: PlayerPrefs Tidak Save
**Cause**: Unity Editor tidak auto-save PlayerPrefs saat stop play mode.

**Solution**:
1. Pastikan ada `PlayerPrefs.Save()` di code (sudah ada di `SaveBestStars()`)
2. Test di **Build** (bukan Editor) untuk produksi
3. Debug: tambah log setelah `PlayerPrefs.SetInt()`:
   ```csharp
   Debug.Log($"PlayerPrefs saved: {key} = {value}");
   ```

---

### Problem 8: Time.timeScale = 0 Blocking Animations
**Cause**: Win Panel muncul saat `Time.timeScale = 0`, tapi animasi pakai `Time.deltaTime`.

**Solution**:
Code sudah fix di `WinPanelController`:
```csharp
yield return new WaitForSecondsRealtime(delay); // Use REALTIME
elapsed += Time.unscaledDeltaTime;              // Use UNSCALED
```

Pastikan semua coroutine di Win Panel pakai `unscaled`.

---

## ⚙️ ADVANCED CONFIGURATION

### Custom Star Thresholds

Edit di `GameManager.CalculateStars()`:

```csharp
public int CalculateStars()
{
    if (totalSkor >= 95) return 3;  // 3★ = 95-100 (lebih strict)
    if (totalSkor >= 80) return 2;  // 2★ = 80-94
    if (totalSkor >= 60) return 1;  // 1★ = 60-79
    return 0;
}
```

---

### Multiple Leaderboards (Per Level)

Update `WinPanelController.OnClickUploadToLeaderboard()`:

```csharp
// Filter leaderboard by current level
LeaderboardManager.Instance.GetLeaderboard(levelIndex, OnLeaderboardReceived);
```

Update UI dengan dropdown untuk switch levels.

---

### Lock Levels by Stars

Update `LevelSelectionManager.Start()`:

```csharp
// Unlock level jika previous level dapat min 2 bintang
foreach (LevelButton button in allLevelButtons)
{
    if (button.levelIndex == 1)
    {
        button.SetStatus(false); // Level 1 selalu unlock
    }
    else
    {
        // Cek stars level sebelumnya
        int prevStars = GameManager.Instance.GetBestStars(button.levelIndex - 1);
        bool unlock = prevStars >= 2; // Min 2★ untuk unlock next level
        button.SetStatus(!unlock);
    }
}
```

---

### Player Name Input

Buat input field di Main Menu:

```csharp
public TMP_InputField playerNameInput;

void Start()
{
    string savedName = PlayerPrefs.GetString("PlayerName", "");
    if (!string.IsNullOrEmpty(savedName))
    {
        playerNameInput.text = savedName;
    }
}

public void OnPlayerNameChanged()
{
    string name = playerNameInput.text;
    LeaderboardManager.Instance.SetPlayerName(name);
}
```

---

## 📊 ANALYTICS & MONITORING

### Google Sheets Analytics

Tambah sheet baru untuk analytics:

1. Sheet: **Analytics**
2. Columns:
   - **Date**: Extract dari Timestamp
   - **Total Players**: COUNTUNIQUE(PlayerName)
   - **Total Plays**: COUNT(Timestamp)
   - **Avg Score**: AVERAGE(Score)
   - **3-Star Count**: COUNTIF(Stars, 3)

Formulas:
```
=COUNTUNIQUE(Sheet1!A2:A)  // Total Players
=COUNT(Sheet1!F2:F)         // Total Plays
=AVERAGE(Sheet1!D2:D)       // Avg Score
=COUNTIF(Sheet1!E2:E, 3)    // 3-Star Count
```

---

### Teacher Dashboard

Buat Google Data Studio dashboard:
1. Connect data source: Google Sheets
2. Add charts:
   - **Bar Chart**: Top 10 Players by Score
   - **Pie Chart**: Stars Distribution (1★, 2★, 3★)
   - **Line Chart**: Plays Over Time
   - **Table**: Full Leaderboard with Filters

---

## ✅ FINAL CHECKLIST

### Before Build:
- [ ] Google Apps Script deployed & URL copied
- [ ] LeaderboardManager → **Google Apps Script URL** filled
- [ ] LeaderboardManager → **Class Name** set (default: `3A`)
- [ ] All Win Panels have **WinPanelController** attached
- [ ] Star sprites assigned (Empty & Filled)
- [ ] Leaderboard Panel setup with ScrollView & Prefab
- [ ] Level Selection buttons have star icons & best score text
- [ ] Audio clips assigned (star appear, new record, refresh)
- [ ] Test upload & download di Editor ✅
- [ ] Cek Google Sheets: data muncul ✅

### After Build:
- [ ] Test di device: Upload skor dari HP/Tablet
- [ ] Test di multiple devices: Leaderboard sync antar device
- [ ] Test di classroom: 10+ siswa upload skor bersamaan
- [ ] Monitor Google Sheets: check performance & data integrity
- [ ] Backup data regularly (File → Download → CSV)

---

## 🎉 CONGRATULATIONS!

Setup lengkap! Game Anda sekarang memiliki:
- ⭐ Star Rating System (motivation to replay)
- 🏆 Real-time Leaderboard (competition)
- 💾 Best Score Tracking (progression)
- 📊 Teacher Analytics (monitoring)

**Next Steps:**
1. Test dengan siswa real di kelas
2. Gather feedback: UI/UX, difficulty thresholds
3. Iterate: adjust star thresholds, add badges, etc.
4. Scale: add more levels, events, seasonal leaderboards

Happy Teaching! 📚🎮
