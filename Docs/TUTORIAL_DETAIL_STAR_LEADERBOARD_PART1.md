# 🎓 TUTORIAL LENGKAP: STAR RATING & LEADERBOARD SYSTEM
## 📚 Untuk Pemula - Step by Step Sangat Detail

---

# 📖 DAFTAR ISI

## BAGIAN 1: PERSIAPAN (15 menit)
- [Step 1.1: Buat Google Spreadsheet](#step-11-buat-google-spreadsheet)
- [Step 1.2: Buka Apps Script Editor](#step-12-buka-apps-script-editor)
- [Step 1.3: Copy-Paste Code](#step-13-copy-paste-code)
- [Step 1.4: Run Setup Function](#step-14-run-setup-function)
- [Step 1.5: Deploy Web App](#step-15-deploy-web-app)
- [Step 1.6: Test API](#step-16-test-api)

## BAGIAN 2: UNITY SETUP - LEADERBOARD MANAGER (10 menit)
- [Step 2.1: Create GameObject](#step-21-create-gameobject)
- [Step 2.2: Add Component](#step-22-add-component)
- [Step 2.3: Configure Inspector](#step-23-configure-inspector)

## BAGIAN 3: UNITY SETUP - WIN PANEL (30 menit)
- [Step 3.1: Locate Win Panel](#step-31-locate-win-panel)
- [Step 3.2: Create Star Icons](#step-32-create-star-icons)
- [Step 3.3: Create New Record Popup](#step-33-create-new-record-popup)
- [Step 3.4: Create Upload Button](#step-34-create-upload-button)
- [Step 3.5: Add WinPanelController](#step-35-add-winpanelcontroller)
- [Step 3.6: Configure Inspector](#step-36-configure-inspector)

## BAGIAN 4: UNITY SETUP - LEADERBOARD PANEL (45 menit)
- [Step 4.1: Create Leaderboard Panel](#step-41-create-leaderboard-panel)
- [Step 4.2: Create Header](#step-42-create-header)
- [Step 4.3: Create ScrollView](#step-43-create-scrollview)
- [Step 4.4: Create Player Row Prefab](#step-44-create-player-row-prefab)
- [Step 4.5: Create Buttons](#step-45-create-buttons)
- [Step 4.6: Add LeaderboardUI Component](#step-46-add-leaderboardui-component)
- [Step 4.7: Configure Inspector](#step-47-configure-inspector)

## BAGIAN 5: UNITY SETUP - LEVEL SELECTION (20 menit)
- [Step 5.1: Locate Level Buttons](#step-51-locate-level-buttons)
- [Step 5.2: Add Star Icons](#step-52-add-star-icons)
- [Step 5.3: Add Best Score Text](#step-53-add-best-score-text)
- [Step 5.4: Configure LevelButton](#step-54-configure-levelbutton)

## BAGIAN 6: TESTING (15 menit)
- [Step 6.1: Test Star Rating](#step-61-test-star-rating)
- [Step 6.2: Test Upload Score](#step-62-test-upload-score)
- [Step 6.3: Test Leaderboard Display](#step-63-test-leaderboard-display)

## BAGIAN 7: TROUBLESHOOTING
- [Common Problems & Solutions](#common-problems--solutions)

---
---
---

# BAGIAN 1: PERSIAPAN GOOGLE SHEETS
## ⏱️ Estimasi: 15 menit

Bagian ini kita akan setup Google Sheets sebagai database leaderboard.

---

## Step 1.1: Buat Google Spreadsheet

### 1.1.1 - Buka Google Drive
1. Buka browser (Chrome/Firefox/Edge)
2. Ketik di address bar: `drive.google.com`
3. Login dengan akun Google kamu

### 1.1.2 - Create New Spreadsheet
1. Klik tombol **"+ New"** (kiri atas)
2. Pilih **"Google Sheets"** → **"Blank spreadsheet"**
3. Browser akan buka tab baru dengan spreadsheet kosong

### 1.1.3 - Rename Spreadsheet
1. Di kiri atas, klik **"Untitled spreadsheet"**
2. Ketik nama baru: **"Eco Quest Leaderboard"**
3. Tekan **Enter**

### 1.1.4 - Copy Spreadsheet URL
1. Lihat URL di address bar browser:
   ```
   https://docs.google.com/spreadsheets/d/1a2b3c4d5e6f7g8h9i0j/edit
   ```
2. **Jangan close tab ini**, kita akan balik lagi nanti

**✅ Checklist Step 1.1:**
- [ ] Spreadsheet baru sudah terbuka
- [ ] Nama spreadsheet: "Eco Quest Leaderboard"
- [ ] URL spreadsheet sudah dicopy (optional untuk backup)

---

## Step 1.2: Buka Apps Script Editor

### 1.2.1 - Akses Menu Extensions
1. Di Google Sheets yang baru dibuat
2. Klik menu **"Extensions"** (menu bar atas)
3. Pilih **"Apps Script"**

**Screenshot description:**
```
Menu bar: File | Edit | View | Insert | Format | Data | Tools | Extensions | Help
                                                                    ^^^^^^^^^
                                                                    Klik ini
```

### 1.2.2 - Tunggu Apps Script Editor Loading
1. Browser akan buka **tab baru**
2. Tunggu beberapa detik sampai Apps Script Editor muncul
3. Kamu akan lihat:
   - Judul project: "Untitled project"
   - File panel kiri: "Code.gs"
   - Code editor tengah dengan code default:
     ```javascript
     function myFunction() {
     
     }
     ```

### 1.2.3 - Rename Project (Optional)
1. Klik "Untitled project" di kiri atas
2. Ketik: **"Eco Quest Leaderboard API"**
3. Click OK atau tekan Enter

**✅ Checklist Step 1.2:**
- [ ] Apps Script Editor sudah terbuka di tab baru
- [ ] Ada file "Code.gs" di panel kiri
- [ ] Ada code default `function myFunction()`

---

## Step 1.3: Copy-Paste Code

### 1.3.1 - Buka File Code di Project Kamu
1. Buka **VS Code** atau **File Explorer**
2. Navigate ke folder project: `d:\Project Game Edukasi\eco-quest\`
3. Masuk ke folder: `Docs\`
4. Buka file: **`GoogleAppsScript_Leaderboard.js`**

**Path lengkap:**
```
d:\Project Game Edukasi\eco-quest\Docs\GoogleAppsScript_Leaderboard.js
```

### 1.3.2 - Select All & Copy
1. Di file `GoogleAppsScript_Leaderboard.js`
2. Tekan **Ctrl+A** (select all)
3. Tekan **Ctrl+C** (copy)

### 1.3.3 - Balik ke Apps Script Editor
1. Switch ke tab browser Apps Script Editor
2. Di code editor tengah, **SELECT ALL code** yang ada (Ctrl+A)
3. **DELETE** (tekan Delete atau Backspace)
4. Code editor sekarang kosong

### 1.3.4 - Paste Code Baru
1. Tekan **Ctrl+V** (paste)
2. Code baru akan muncul (sekitar 300+ lines)
3. Scroll ke atas, pastikan dimulai dengan:
   ```javascript
   // ============================================
   // GOOGLE APPS SCRIPT - ECO QUEST LEADERBOARD
   // ============================================
   ```

### 1.3.5 - Save Code
1. Tekan **Ctrl+S** (save)
2. Atau klik icon **Save** (💾 disk icon) di toolbar
3. Tunggu beberapa detik, akan muncul "Saved" di atas

**✅ Checklist Step 1.3:**
- [ ] Code lama sudah dihapus
- [ ] Code baru sudah di-paste (300+ lines)
- [ ] Code dimulai dengan comment header "ECO QUEST LEADERBOARD"
- [ ] Code sudah di-save

---

## Step 1.4: Run Setup Function

### 1.4.1 - Select Function
1. Di toolbar atas, ada dropdown **function selector**
2. Default tertulis: "Select function" atau "myFunction"
3. **Klik dropdown** tersebut
4. **Pilih**: `setupSpreadsheet`

**Screenshot description:**
```
Toolbar: [💾 Save] [▶ Run] [🐞 Debug] [setupSpreadsheet ▼] [Execution log ▼]
                                        ^^^^^^^^^^^^^^^^^
                                        Klik dropdown ini
```

### 1.4.2 - Click Run Button
1. Klik tombol **▶ Run** (play button)
2. Tunggu beberapa detik

### 1.4.3 - Authorization Required (PERTAMA KALI)
Jika ini **pertama kali** run script, akan muncul popup:

**Popup 1: "Authorization required"**
1. Muncul message: "This project requires your permission to access your data"
2. Klik **"Review permissions"**

**Popup 2: "Choose an account"**
1. Pilih **akun Google kamu** (yang sama dengan spreadsheet)
2. Klik akun tersebut

**Popup 3: "This app isn't verified"** ⚠️ PENTING!
1. Muncul warning: "Google hasn't verified this app"
2. **JANGAN PANIC!** Ini normal karena app buatan kita sendiri
3. Klik **"Advanced"** (di kiri bawah text)
4. Klik **"Go to Eco Quest Leaderboard API (unsafe)"**

**Popup 4: "Grant permissions"**
1. Muncul list permissions:
   - "See, edit, create, and delete all your Google Sheets spreadsheets"
2. **Scroll ke bawah**
3. Klik **"Allow"**

### 1.4.4 - Wait for Execution
1. Script akan mulai running (ada loading icon)
2. Tunggu 5-10 detik
3. Eksekusi selesai jika loading icon hilang

### 1.4.5 - Check Execution Log
1. Klik **"Execution log"** (dropdown di toolbar)
2. Atau klik **"View"** menu → **"Logs"**
3. Akan muncul panel log di bawah

**Expected Log:**
```
[Date Time] Info    ✅ Created new sheet: Sheet1
[Date Time] Info    ✅ Spreadsheet setup complete!
[Date Time] Info    Sheet Name: Sheet1
[Date Time] Info    Spreadsheet URL: https://docs.google.com/...
```

**✅ Jika ada log seperti di atas** = SUCCESS! ✅
**❌ Jika ada error** = Lihat [Troubleshooting](#common-problems--solutions)

### 1.4.6 - Check Google Sheets (Verification)
1. **Switch ke tab Google Sheets** (spreadsheet yang tadi dibuat)
2. **Refresh page** (F5 atau Ctrl+R)
3. Sekarang kamu akan lihat:

**Row 1 (Header) - Warna hijau, bold:**
```
| PlayerName | ClassName | Level | Score | Stars | Timestamp           |
```

**Row 2-4 (Sample Data):**
```
| Daffa      | 3A        | 1     | 95    | 3     | 2025-12-08 10:30:00 |
| Budi       | 3A        | 1     | 87    | 2     | 2025-12-08 10:31:00 |
| Siti       | 3A        | 1     | 78    | 2     | 2025-12-08 10:32:00 |
```

**✅ Checklist Step 1.4:**
- [ ] Function `setupSpreadsheet` sudah dipilih
- [ ] Authorization sudah di-allow
- [ ] Execution log menunjukkan "✅ Spreadsheet setup complete!"
- [ ] Google Sheets menampilkan headers + 3 sample data

---

## Step 1.5: Deploy Web App

Sekarang kita akan "publish" Apps Script sebagai Web App yang bisa diakses dari Unity.

### 1.5.1 - Click Deploy Button
1. Di Apps Script Editor, klik tombol **"Deploy"** (kanan atas)
2. Dropdown menu akan muncul
3. Pilih **"New deployment"**

**Screenshot description:**
```
Toolbar kanan atas: [🔍 Search] [⚙️ Project Settings] [🚀 Deploy ▼]
                                                           ^^^^^^^
                                                           Klik ini
```

### 1.5.2 - Configure Deployment Settings

**Popup "New deployment" muncul:**

#### Field 1: "Select type"
1. Klik icon **gear** (⚙️) di sebelah "Select type"
2. Pilih **"Web app"**

#### Field 2: "Description" (Optional)
1. Ketik: `Version 1 - Initial Release`
2. Atau biarkan kosong (tidak masalah)

#### Field 3: "Execute as"
1. Pastikan terpilih: **"Me (your-email@gmail.com)"**
2. Jangan ganti!

#### Field 4: "Who has access"
1. **SANGAT PENTING!** Pilih: **"Anyone"**
2. Jangan pilih "Only myself" (Unity tidak akan bisa akses)

**Screenshot description:**
```
╔════════════════════════════════╗
║   New deployment               ║
║                                ║
║  Select type: ⚙️ Web app       ║
║  Description: [Version 1...]   ║
║  Execute as:  Me (email)       ║
║  Who has access: Anyone  ▼     ║
║                    ^^^^^^       ║
║                    PILIH INI!   ║
║                                ║
║  [Cancel]  [Deploy]            ║
╚════════════════════════════════╝
```

### 1.5.3 - Click Deploy
1. Klik tombol **"Deploy"** (biru, kanan bawah)
2. Tunggu beberapa detik (loading)

### 1.5.4 - Copy Web App URL

**Popup "Deployment" muncul dengan pesan success:**

```
╔════════════════════════════════════════════╗
║  ✅ Deployment successful                  ║
║                                            ║
║  Web app URL:                              ║
║  https://script.google.com/macros/s/       ║
║  AKfycby1a2b3c4d5e6f7.../exec             ║
║  [Copy] 📋                                 ║
║                                            ║
║  [Done]                                    ║
╚════════════════════════════════════════════╝
```

1. **Klik tombol "Copy"** (📋) di sebelah URL
2. Web App URL sudah tercopy ke clipboard
3. **PASTE ke Notepad** atau text editor (backup URL ini!)
4. Klik **"Done"**

**✅ Checklist Step 1.5:**
- [ ] Deployment type: "Web app"
- [ ] Execute as: "Me"
- [ ] Who has access: "Anyone" ← PENTING!
- [ ] Web App URL sudah di-copy
- [ ] URL disimpan di Notepad (backup)

---

## Step 1.6: Test API

Sebelum ke Unity, kita test dulu apakah API sudah working.

### 1.6.1 - Open URL in Browser
1. Buka **tab browser baru**
2. **Paste** Web App URL yang tadi di-copy
3. Tekan **Enter**

**Example URL:**
```
https://script.google.com/macros/s/AKfycby1a2b3c4d5e6f7g8h9i0j-abcdefg/exec
```

### 1.6.2 - Check Response

**✅ EXPECTED (Success):**
Browser menampilkan JSON:
```json
{
  "status": "success",
  "message": "Eco Quest Leaderboard API is running!",
  "timestamp": "2025-12-08T03:30:00.123Z",
  "endpoints": {
    "test": "GET ?action=test",
    "getLeaderboard": "GET ?action=getLeaderboard&className=3A&level=1",
    "upload": "POST with JSON body {action:'upload', ...}"
  },
  "version": "1.0"
}
```

**✅ Jika muncul JSON seperti di atas** = API WORKING! 🎉

**❌ WRONG (Error):**
```json
{
  "status": "error",
  "message": "Unknown action"
}
```
→ Balik ke Step 1.3, paste ulang code yang benar.

### 1.6.3 - Test Get Leaderboard Endpoint
1. Di browser, **edit URL** dengan menambahkan parameter:
   ```
   https://script.google.com/.../exec?action=getLeaderboard&className=3A&level=1
                                     ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                                     Tambahkan ini
   ```

2. Tekan **Enter**

**✅ EXPECTED:**
```json
[
  {
    "playerName": "Daffa",
    "className": "3A",
    "level": 1,
    "score": 95,
    "stars": 3,
    "timestamp": "2025-12-08 10:30:00",
    "rank": 1
  },
  {
    "playerName": "Budi",
    "className": "3A",
    "level": 1,
    "score": 87,
    "stars": 2,
    "timestamp": "2025-12-08 10:31:00",
    "rank": 2
  },
  {
    "playerName": "Siti",
    "className": "3A",
    "level": 1,
    "score": 78,
    "stars": 2,
    "timestamp": "2025-12-08 10:32:00",
    "rank": 3
  }
]
```

**✅ Jika muncul array JSON dengan 3 player** = GET LEADERBOARD WORKING! 🎉

**✅ Checklist Step 1.6:**
- [ ] Browser menampilkan JSON (bukan error HTML)
- [ ] Response memiliki `"status": "success"`
- [ ] Endpoint test berhasil
- [ ] Endpoint getLeaderboard menampilkan 3 sample data

---

## 🎉 BAGIAN 1 SELESAI!

**Apa yang sudah kita capai:**
- ✅ Google Spreadsheet dibuat dengan headers
- ✅ Apps Script code sudah di-deploy
- ✅ Web App URL sudah didapat & di-test
- ✅ API sudah confirmed working

**Next:** Bagian 2 - Setup Unity LeaderboardManager

---
---
---

# BAGIAN 2: UNITY SETUP - LEADERBOARD MANAGER
## ⏱️ Estimasi: 10 menit

Sekarang kita akan setup LeaderboardManager di Unity untuk komunikasi dengan Google Sheets API.

---

## Step 2.1: Create GameObject

### 2.1.1 - Open Unity Project
1. Buka **Unity Hub**
2. Pilih project **"eco-quest"**
3. Click **Open** (tunggu Unity loading)

### 2.1.2 - Open Main Menu Scene
1. Di **Project window** (bawah), navigate ke:
   ```
   Assets > _Scenes
   ```
2. Double-click **`00_MainMenu.unity`**
3. Scene Main Menu akan terbuka di **Scene view**

### 2.1.3 - Create Empty GameObject
1. Di **Hierarchy window** (kiri), **klik kanan** di area kosong
2. Pilih **"Create Empty"**
3. GameObject baru muncul dengan nama "GameObject"

### 2.1.4 - Rename GameObject
1. GameObject masih terpilih (highlighted biru)
2. Tekan **F2** (atau klik kanan → Rename)
3. Ketik nama baru: **`LeaderboardManager`**
4. Tekan **Enter**

**Hierarchy sekarang:**
```
Hierarchy
├── Canvas
├── EventSystem
├── AudioManager (atau object lain)
└── LeaderboardManager  ← GameObject baru kita
```

**✅ Checklist Step 2.1:**
- [ ] Scene 00_MainMenu sudah terbuka
- [ ] GameObject "LeaderboardManager" sudah dibuat di Hierarchy
- [ ] GameObject nama persis: "LeaderboardManager" (case-sensitive)

---

## Step 2.2: Add Component

### 2.2.1 - Select LeaderboardManager GameObject
1. Klik **`LeaderboardManager`** di Hierarchy (jika belum terpilih)
2. **Inspector window** (kanan) akan menampilkan properties

**Inspector awal:**
```
Inspector
══════════════════════════════
LeaderboardManager
──────────────────────────────
Transform
  Position: X 0  Y 0  Z 0
  Rotation: X 0  Y 0  Z 0
  Scale:    X 1  Y 1  Z 1

[Add Component]
══════════════════════════════
```

### 2.2.2 - Click Add Component
1. Di **Inspector**, scroll ke bawah
2. Klik tombol **"Add Component"**
3. Search box akan muncul

### 2.2.3 - Search for Script
1. Di search box, ketik: `LeaderboardManager`
2. Akan muncul suggestion: **"Leaderboard Manager (Script)"**
3. **Klik** suggestion tersebut

**⚠️ Jika tidak muncul:**
- Pastikan file `LeaderboardManager.cs` ada di folder `Assets/_Scripts/Manager/`
- Klik menu **Assets** → **Refresh** (atau tekan Ctrl+R)
- Coba search lagi

### 2.2.4 - Verify Component Added
**Inspector sekarang:**
```
Inspector
══════════════════════════════
LeaderboardManager
──────────────────────────────
Transform
  ...

Leaderboard Manager (Script)  ← Component baru
  Script: LeaderboardManager
  
  Google Sheets Configuration
    Google Apps Script URL: [empty]
    Class Name: 3A
    Player Name: Player1
    
  Timeout Settings
    Request Timeout: 10
    
  Debug Settings
    ☐ Debug Mode
══════════════════════════════
```

**✅ Checklist Step 2.2:**
- [ ] Component "Leaderboard Manager (Script)" sudah ditambahkan
- [ ] Inspector menampilkan fields: Google Apps Script URL, Class Name, dll.

---

## Step 2.3: Configure Inspector

### 2.3.1 - Paste Web App URL
1. Di **Inspector**, cari field **"Google Apps Script URL"**
2. **Klik** di field input (kotak text kosong)
3. **Paste** Web App URL yang tadi di-copy (Ctrl+V)
   ```
   https://script.google.com/macros/s/AKfycby1a2b3c4d5.../exec
   ```
4. **Pastikan tidak ada spasi** di awal/akhir URL

**⚠️ PENTING:**
- URL harus lengkap (mulai `https://` sampai `/exec`)
- Tidak boleh ada spasi atau enter
- Copy-paste langsung dari Apps Script deployment

### 2.3.2 - Set Class Name
1. Field **"Class Name"**: Default `3A`
2. Biarkan `3A` (atau ganti sesuai kelas target)
3. Format: `"3A"`, `"3B"`, `"4A"`, dll. (huruf besar)

### 2.3.3 - Set Player Name (Temporary)
1. Field **"Player Name"**: Default `Player1`
2. Biarkan `Player1` untuk testing
3. Nanti bisa diganti dari code atau input field

### 2.3.4 - Enable Debug Mode
1. **Centang** checkbox **"Debug Mode"** ☑️
2. Ini akan enable detailed logs di Console
3. Berguna untuk testing & troubleshooting

### 2.3.5 - Verify Settings
**Inspector final configuration:**
```
Leaderboard Manager (Script)
  Google Sheets Configuration
    Google Apps Script URL: https://script.google.com/macros/s/AKfy...
    Class Name: 3A
    Player Name: Player1
    
  Timeout Settings
    Request Timeout: 10
    
  Debug Settings
    ☑ Debug Mode  ← Centang ini
```

### 2.3.6 - Save Scene
1. Tekan **Ctrl+S** (save scene)
2. Atau menu **File** → **Save**

**✅ Checklist Step 2.3:**
- [ ] Google Apps Script URL sudah di-paste (URL lengkap, tidak ada spasi)
- [ ] Class Name: "3A"
- [ ] Player Name: "Player1"
- [ ] Debug Mode: ☑ Centang
- [ ] Scene sudah di-save

---

## 🎉 BAGIAN 2 SELESAI!

**Apa yang sudah kita capai:**
- ✅ LeaderboardManager GameObject dibuat di scene Main Menu
- ✅ LeaderboardManager script component ditambahkan
- ✅ Google Sheets API URL sudah dikonfigurasi
- ✅ Debug mode enabled untuk testing

**Next:** Bagian 3 - Setup Win Panel dengan Star Display

---
---
---

# BAGIAN 3: UNITY SETUP - WIN PANEL
## ⏱️ Estimasi: 30 menit

Sekarang kita akan setup Win Panel untuk display bintang dan upload score button.

---

## Step 3.1: Locate Win Panel

### 3.1.1 - Open Game Scene
1. Di **Project window**, navigate ke: `Assets > _Scenes`
2. Double-click scene **`03_Game_Processing.unity`**
   (Atau scene lain dimana ada Win Panel)

### 3.1.2 - Find Win Panel in Hierarchy
1. Di **Hierarchy window**, search di search box: `win`
2. Atau expand Canvas dan cari manual
3. GameObject biasanya bernama: **`Win Panel`** atau **`PanelWin`** atau **`panelWinScene2`**

**Example Hierarchy:**
```
Hierarchy
├── Canvas
│   ├── HUD
│   ├── GameplayUI
│   ├── Win Panel  ← Cari ini
│   └── Lose Panel
```

### 3.1.3 - Select & Inspect Win Panel
1. **Klik** Win Panel di Hierarchy
2. **Inspector** akan menampilkan components

**Typical Win Panel structure:**
```
Inspector
══════════════════════════════
Win Panel
──────────────────────────────
Canvas Group (Component)
  Alpha: 1
  Interactable: ☑
  Block Raycasts: ☑

Image (Component)
  Source Image: (panel background)

[Children GameObjects]:
  - Text Skor Akhir
  - Text Waktu Akhir
  - Button Next Level
  - Button Restart
══════════════════════════════
```

**✅ Checklist Step 3.1:**
- [ ] Scene game (Processing) sudah terbuka
- [ ] Win Panel GameObject sudah ditemukan di Hierarchy
- [ ] Win Panel terpilih, Inspector menampilkan components

---

## Step 3.2: Create Star Icons

Sekarang kita akan tambahkan 3 icon bintang di Win Panel.

### 3.2.1 - Create First Star GameObject
1. **Klik kanan** pada **Win Panel** di Hierarchy
2. Pilih **UI** → **Image**
3. GameObject baru muncul dengan nama "Image"
4. **Rename** jadi: **`Star1`** (tekan F2)

### 3.2.2 - Position Star1
1. Star1 masih terpilih, lihat **Inspector**
2. Di component **Rect Transform**:
   ```
   Pos X: -100
   Pos Y: 50
   Width: 80
   Height: 80
   ```
3. Ini akan posisikan star di atas text skor, sebelah kiri

### 3.2.3 - Set Star1 Initial Scale to Zero
1. Masih di **Rect Transform**
2. Di bagian **Scale**:
   ```
   X: 0
   Y: 0
   Z: 1
   ```
3. Ini penting untuk animasi! Bintang akan di-scale dari 0 → 1

### 3.2.4 - Set Star1 Sprite (Temporary)
1. Di component **Image**, cari field **"Source Image"**
2. Klik **circle icon** (⊙) di sebelah kanan field
3. Pilih sprite **"Knob"** atau **"UISprite"** (placeholder)
4. Nanti kita ganti dengan star sprite yang proper

### 3.2.5 - Duplicate for Star2 & Star3
1. Star1 masih terpilih, tekan **Ctrl+D** (duplicate)
2. GameObject "Star1 (1)" muncul, **rename** jadi: **`Star2`**
3. Di **Rect Transform**, ubah **Pos X: 0** (center)

4. Star2 masih terpilih, tekan **Ctrl+D** lagi
5. Rename jadi: **`Star3`**
6. Ubah **Pos X: 100** (kanan)

**Hierarchy sekarang:**
```
Win Panel
├── Text Skor Akhir
├── Text Waktu Akhir
├── Star1  ← Pos X: -100, Scale: (0,0,1)
├── Star2  ← Pos X: 0, Scale: (0,0,1)
├── Star3  ← Pos X: 100, Scale: (0,0,1)
├── Button Next Level
└── Button Restart
```

**Scene View:**
```
╔══════════════════════════╗
║     WIN PANEL            ║
║                          ║
║   ⭐  ⭐  ⭐           ║  ← 3 stars (tapi scale 0, invisible)
║                          ║
║  Skor Akhir: 85          ║
║  Waktu: 01:30            ║
║                          ║
║  [Next Level] [Restart]  ║
╚══════════════════════════╝
```

**✅ Checklist Step 3.2:**
- [ ] 3 GameObject star dibuat: Star1, Star2, Star3
- [ ] Position: X = -100, 0, 100 (Y sama semua, contoh 50)
- [ ] Scale semua star: (0, 0, 1) ← PENTING!
- [ ] Sprite temporary (Knob/UISprite) sudah di-set

---

## Step 3.3: Create New Record Popup

### 3.3.1 - Create Panel GameObject
1. **Klik kanan** Win Panel di Hierarchy
2. Pilih **UI** → **Panel**
3. GameObject "Panel" muncul, **rename** jadi: **`NewRecordPopup`**

### 3.3.2 - Position & Size Popup
1. NewRecordPopup terpilih, di **Rect Transform**:
   ```
   Pos X: 0
   Pos Y: -50
   Width: 400
   Height: 100
   ```

### 3.3.3 - Change Background Color
1. Di component **Image**
2. Field **"Color"**: Klik color box
3. Pilih warna gold/kuning:
   ```
   R: 255
   G: 225
   B: 50
   A: 255
   ```

### 3.3.4 - Add Text for "NEW RECORD!"
1. **Klik kanan** NewRecordPopup
2. Pilih **UI** → **Text - TextMeshPro**
3. GameObject "Text (TMP)" muncul, **rename** jadi: **`TextNewRecord`**

4. Di **Rect Transform** (stretch full):
   ```
   Anchors: Stretch (Alt+Shift+klik tengah)
   Left: 0, Right: 0, Top: 0, Bottom: 0
   ```

5. Di component **TextMeshPro - Text**:
   ```
   Text: NEW RECORD! 🎉
   Font Size: 36
   Alignment: Center + Middle
   Color: White
   Font Style: Bold
   ```

### 3.3.5 - Set Popup Inactive by Default
1. Select **NewRecordPopup** (parent)
2. Di **Inspector** paling atas, **uncheck** checkbox ☐
3. Popup sekarang inactive (abu-abu di Hierarchy)

**Hierarchy:**
```
Win Panel
├── ...
├── Star1, Star2, Star3
├── NewRecordPopup  ← Inactive (abu-abu)
│   └── TextNewRecord
└── ...
```

**✅ Checklist Step 3.3:**
- [ ] NewRecordPopup Panel dibuat
- [ ] Size: 400x100, positioned di bawah stars
- [ ] Background color: Gold/kuning
- [ ] Text "NEW RECORD! 🎉" sudah dibuat (bold, center, white)
- [ ] Popup set inactive ☐ (default hidden)

---

## Step 3.4: Create Upload Button

### 3.4.1 - Create Button GameObject
1. **Klik kanan** Win Panel
2. Pilih **UI** → **Button - TextMeshPro**
3. GameObject "Button (TMP)" muncul, **rename**: **`ButtonUploadLeaderboard`**

### 3.4.2 - Position & Size Button
1. Di **Rect Transform**:
   ```
   Pos X: 0
   Pos Y: -150
   Width: 350
   Height: 60
   ```

### 3.4.3 - Change Button Color
1. Di component **Image**
2. Field **"Color"**: Pilih hijau
   ```
   R: 76
   G: 175
   B: 80
   A: 255
   ```

### 3.4.4 - Edit Button Text
1. Expand **ButtonUploadLeaderboard** di Hierarchy
2. Select child GameObject **"Text (TMP)"**
3. Di **TextMeshPro - Text**:
   ```
   Text: 📤 Upload to Leaderboard
   Font Size: 24
   Alignment: Center + Middle
   Color: White
   Font Style: Bold
   ```

**Scene View sekarang:**
```
╔══════════════════════════════╗
║       WIN PANEL              ║
║                              ║
║     ⭐  ⭐  ⭐             ║
║                              ║
║  Skor Akhir: 85              ║
║  Waktu: 01:30                ║
║                              ║
║ ┌──────────────────────────┐ ║
║ │ 📤 Upload to Leaderboard │ ║ ← Button baru
║ └──────────────────────────┘ ║
║                              ║
║  [Next Level]  [Restart]     ║
╚══════════════════════════════╝
```

**✅ Checklist Step 3.4:**
- [ ] ButtonUploadLeaderboard dibuat
- [ ] Size 350x60, positioned di bawah popup
- [ ] Background color hijau
- [ ] Text: "📤 Upload to Leaderboard" (bold, white, center)

---

## Step 3.5: Add WinPanelController

### 3.5.1 - Select Win Panel GameObject
1. Klik **Win Panel** (parent object) di Hierarchy
2. **Inspector** menampilkan Win Panel components

### 3.5.2 - Add Component
1. Di Inspector, klik **"Add Component"**
2. Search: `WinPanelController`
3. Klik **"Win Panel Controller (Script)"**

**Inspector sekarang:**
```
Inspector
══════════════════════════════
Win Panel
──────────────────────────────
... (existing components)

Win Panel Controller (Script)  ← Component baru
  Script: WinPanelController
  
  Star Display
    Size: 0  ← Kita akan isi array
    Empty Star: None (Sprite)
    Filled Star: None (Sprite)
    
  New Record Popup
    New Record Popup: None
    
  Leaderboard Button
    Upload Leaderboard Button: None
    Upload Button Text: None
    
  Animation Settings
    Star Animation Delay: 0.3
    Star Animation Duration: 0.2
    Star Final Scale: 1
    
  Audio
    Sfx Source: None
    Star Appear Sound: None
    New Record Sound: None
══════════════════════════════
```

**✅ Checklist Step 3.5:**
- [ ] Win Panel Controller component sudah ditambahkan
- [ ] Inspector menampilkan fields (masih kosong/None)

---

## Step 3.6: Configure Inspector

Sekarang kita link semua GameObject ke Inspector fields.

### 3.6.1 - Configure Star Objects Array

#### Set Array Size
1. Di field **"Star Objects"**, ada **"Size: 0"**
2. Klik di angka **0**, ganti jadi: **3**
3. Tekan Enter
4. Array expand, muncul:
   ```
   Star Objects
     Size: 3
     Element 0: None (Game Object)
     Element 1: None (Game Object)
     Element 2: None (Game Object)
   ```

#### Drag Stars ke Array
1. Di **Hierarchy**, cari **Star1**
2. **Drag** Star1 ke field **"Element 0"**
3. **Drag** Star2 ke field **"Element 1"**
4. **Drag** Star3 ke field **"Element 2"**

**Result:**
```
Star Objects
  Size: 3
  Element 0: Star1 (GameObject)  ✓
  Element 1: Star2 (GameObject)  ✓
  Element 2: Star3 (GameObject)  ✓
```

### 3.6.2 - Set Star Sprites (Temporary)

#### Empty Star Sprite
1. Field **"Empty Star"**, klik **circle icon** (⊙)
2. Pilih sprite **"Knob"** (placeholder)
3. Nanti ganti dengan proper star sprite

#### Filled Star Sprite
1. Field **"Filled Star"**, klik **circle icon** (⊙)
2. Pilih sprite **"UISprite"** (placeholder berbeda dari empty)

**⚠️ Note**: Ini placeholder. Nanti import star sprite yang proper!

### 3.6.3 - Link New Record Popup
1. Field **"New Record Popup"**
2. Dari **Hierarchy**, **drag** GameObject **NewRecordPopup**
3. Drop ke field tersebut

**Result:**
```
New Record Popup
  New Record Popup: NewRecordPopup (GameObject)  ✓
```

### 3.6.4 - Link Upload Button & Text

#### Upload Button
1. Field **"Upload Leaderboard Button"**
2. Drag **ButtonUploadLeaderboard** dari Hierarchy

#### Button Text
1. Field **"Upload Button Text"**
2. Expand **ButtonUploadLeaderboard** di Hierarchy
3. Drag child **"Text (TMP)"** ke field

**Result:**
```
Leaderboard Button
  Upload Leaderboard Button: ButtonUploadLeaderboard (Button)  ✓
  Upload Button Text: Text (TMP) (TextMeshPro - Text)  ✓
```

### 3.6.5 - Configure Animation Settings (Optional)

Default values sudah OK, tapi bisa adjust:
```
Animation Settings
  Star Animation Delay: 0.3    ← Delay antar bintang (detik)
  Star Animation Duration: 0.2  ← Durasi scale animation
  Star Final Scale: 1           ← Scale akhir (1 = normal size)
```

Biarkan default atau adjust sesuai preferensi.

### 3.6.6 - Link Audio (Optional)

Jika ada AudioSource & AudioClip:
```
Audio
  Sfx Source: [Drag AudioSource GameObject]
  Star Appear Sound: [Drag AudioClip asset]
  New Record Sound: [Drag AudioClip asset]
```

Jika tidak ada audio, **biarkan None** (tidak masalah, script handle null check).

### 3.6.7 - Final Verification

**Inspector Final:**
```
Win Panel Controller (Script)
  Star Display
    Star Objects
      Size: 3
      Element 0: Star1  ✓
      Element 1: Star2  ✓
      Element 2: Star3  ✓
    Empty Star: Knob  ✓ (temp)
    Filled Star: UISprite  ✓ (temp)
    
  New Record Popup
    New Record Popup: NewRecordPopup  ✓
    
  Leaderboard Button
    Upload Leaderboard Button: ButtonUploadLeaderboard  ✓
    Upload Button Text: Text (TMP)  ✓
    
  Animation Settings
    ✓ (Default OK)
    
  Audio
    (Optional - biarkan None OK)
```

### 3.6.8 - Save Scene
1. Tekan **Ctrl+S**
2. Scene saved!

**✅ Checklist Step 3.6:**
- [ ] Star Objects array size: 3
- [ ] Element 0, 1, 2: Star1, Star2, Star3 sudah di-link
- [ ] Empty Star & Filled Star sprite di-set (temp placeholder)
- [ ] New Record Popup di-link
- [ ] Upload Button & Text di-link
- [ ] Scene sudah di-save

---

## 🎉 BAGIAN 3 SELESAI!

**Apa yang sudah kita capai:**
- ✅ Win Panel sudah ditemukan
- ✅ 3 Star icons dibuat (Star1, Star2, Star3) dengan scale 0
- ✅ New Record Popup dibuat (inactive by default)
- ✅ Upload to Leaderboard button dibuat
- ✅ WinPanelController component ditambahkan & dikonfigurasi
- ✅ Semua GameObjects sudah di-link ke Inspector

**Next:** Bagian 4 - Setup Leaderboard Panel (ScrollView, Player Rows)

Tutorial masih berlanjut ke Bagian 4, 5, 6, dan 7...

---

**Mau saya lanjutkan ke Bagian 4-7? Atau ada yang kurang jelas dari Bagian 1-3?** 🤔
