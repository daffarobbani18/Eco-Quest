# 🔧 FIX ERROR "Unknown action" - Google Apps Script Setup

## ❌ Error Yang Kamu Alami

```json
{
  "status": "error",
  "message": "Unknown action"
}
```

**Penyebab**: Apps Script tidak menerima parameter dengan benar atau code belum di-update.

---

## ✅ SOLUSI LENGKAP (Step-by-Step)

### STEP 1: Buka Google Sheets Kamu

1. Buka Google Sheets yang sudah kamu buat
2. **URL akan seperti ini**:
   ```
   https://docs.google.com/spreadsheets/d/1a2b3c4d5e6f7g8h9i0j/edit
                                        ^^^^^^^^^^^^^^^^^
                                        INI SPREADSHEET ID
   ```
3. **COPY Spreadsheet ID** (bagian setelah `/d/` dan sebelum `/edit`)

---

### STEP 2: Buka Apps Script Editor

1. Di Google Sheets, klik menu **Extensions** → **Apps Script**
2. Browser akan buka tab baru dengan Apps Script Editor

---

### STEP 3: Hapus Code Default & Paste Code Baru

1. Di Apps Script Editor, **HAPUS SEMUA** code yang ada (biasanya ada `function myFunction()`)
2. **COPY SELURUH CODE** dari file `Docs/GoogleAppsScript_Leaderboard.js` yang baru saya buat
3. **PASTE** ke Apps Script Editor

---

### STEP 4: Ganti SPREADSHEET_ID

1. Cari baris ini di code (baris 11):
   ```javascript
   var SPREADSHEET_ID = "YOUR_SPREADSHEET_ID_HERE";
   ```

2. **GANTI** dengan Spreadsheet ID kamu yang sudah dicopy di Step 1:
   ```javascript
   var SPREADSHEET_ID = "1a2b3c4d5e6f7g8h9i0j"; // <-- ID kamu
   ```

3. ⚠️ **JANGAN LUPA TANDA KUTIP** `"..."`

---

### STEP 5: Cek Nama Sheet

1. Balik ke Google Sheets kamu
2. Lihat tab di bawah (default: "Sheet1")
3. Jika nama sheet kamu **BUKAN** "Sheet1", update code baris 14:
   ```javascript
   var SHEET_NAME = "Sheet1"; // <-- Ganti jika beda
   ```

---

### STEP 6: Setup Spreadsheet Headers

1. Balik ke Apps Script Editor
2. Di atas, ada **dropdown** untuk pilih function
3. **Pilih**: `setupSpreadsheet` (bukan `doGet` atau `doPost`)
4. **Klik tombol Run ▶** (play button)

5. **Pop-up muncul**: "Authorization required"
   - Klik **Review permissions**
   - Pilih akun Google kamu
   - Klik **Advanced** → **Go to [Your Project Name] (unsafe)**
   - Klik **Allow**

6. **Tunggu** sampai execution selesai (icon loading hilang)
7. **Check Logs**: Klik `Execution log` di bawah
   - Harus ada: `✅ Spreadsheet setup complete!`

8. **Balik ke Google Sheets** → Refresh page
   - Headers sudah muncul: PlayerName, ClassName, Level, Score, Stars, Timestamp
   - Ada 3 sample data (Daffa, Budi, Siti)

---

### STEP 7: Deploy Web App

1. **Balik ke Apps Script Editor**
2. Klik tombol **Deploy** (kanan atas) → **New deployment**

3. **Settings**:
   - Type: **Web app**
   - Description: `Eco Quest Leaderboard v1` (optional)
   - Execute as: **Me** (your email)
   - Who has access: **Anyone**

4. Klik **Deploy**

5. **Pop-up muncul**: Copy **Web app URL**
   ```
   https://script.google.com/macros/s/AKfycby.../exec
   ```

6. **PASTE URL ini** ke Unity Inspector di `LeaderboardManager` → `Google Apps Script URL`

---

### STEP 8: TEST Web App URL (PENTING!)

#### Test 1: Test Endpoint (Browser)
1. **Buka Web App URL di browser** (paste URL dari Step 7)
2. **Expected Response**:
   ```json
   {
     "status": "success",
     "message": "Eco Quest Leaderboard API is running!",
     "timestamp": "2025-12-07T10:30:00.000Z",
     "endpoints": {
       "test": "GET ?action=test",
       "getLeaderboard": "GET ?action=getLeaderboard&className=3A&level=1",
       "upload": "POST with JSON body {action:'upload', ...}"
     },
     "version": "1.0"
   }
   ```

3. ✅ **Jika response seperti di atas** = API WORKING!
4. ❌ **Jika masih "Unknown action"** = ada yang salah di Step 1-7

---

#### Test 2: Test Action (Browser)
1. **Tambahkan parameter** ke URL:
   ```
   https://script.google.com/macros/s/AKfycby.../exec?action=test
   ```

2. **Expected Response**:
   ```json
   {
     "status": "success",
     "message": "Test endpoint working!",
     "receivedParams": {
       "action": "test"
     },
     "timestamp": "2025-12-07T10:30:00.000Z"
   }
   ```

---

#### Test 3: Get Leaderboard (Browser)
1. **Tambahkan parameter** untuk get leaderboard:
   ```
   https://script.google.com/macros/s/AKfycby.../exec?action=getLeaderboard&className=3A&level=1
   ```

2. **Expected Response**:
   ```json
   [
     {
       "playerName": "Daffa",
       "className": "3A",
       "level": 1,
       "score": 95,
       "stars": 3,
       "timestamp": "2025-12-07T10:30:00.000Z",
       "rank": 1
     },
     {
       "playerName": "Budi",
       "className": "3A",
       "level": 1,
       "score": 87,
       "stars": 2,
       "timestamp": "2025-12-07T10:30:00.000Z",
       "rank": 2
     }
   ]
   ```

---

## 🐛 TROUBLESHOOTING

### Problem 1: Masih dapat "Unknown action"
**Solusi**:
1. ✅ Pastikan kamu sudah **COPY-PASTE CODE BARU** dari `GoogleAppsScript_Leaderboard.js`
2. ✅ Pastikan sudah **GANTI SPREADSHEET_ID** di baris 11
3. ✅ Pastikan sudah **SAVE** (Ctrl+S atau File → Save)
4. ✅ Pastikan sudah **RE-DEPLOY** (Deploy → Manage deployments → Edit → Version: New version → Deploy)

---

### Problem 2: "Exception: Document ... is missing (perhaps it was deleted?)"
**Solusi**:
1. ❌ Spreadsheet ID SALAH
2. ✅ Check URL spreadsheet lagi
3. ✅ Copy ulang ID yang benar (antara `/d/` dan `/edit`)
4. ✅ Paste ke code dengan tanda kutip: `"1a2b3c4d5e6f7g8h9i0j"`

---

### Problem 3: "Authorization required" tidak muncul
**Solusi**:
1. ✅ Klik icon **Advanced** (gear/roda gigi) → **View Logs**
2. ✅ Check error di Execution log
3. ✅ Bisa jadi ada typo di Spreadsheet ID

---

### Problem 4: Test berhasil, tapi Unity gagal upload
**Solusi**:
1. ✅ Check Unity Console untuk error message
2. ✅ Pastikan `LeaderboardManager` → `Google Apps Script URL` sudah di-paste
3. ✅ Pastikan URL **TIDAK ADA SPASI** di awal/akhir
4. ✅ Enable Debug Mode di `LeaderboardManager` Inspector
5. ✅ Check Debug Log untuk detail error

---

### Problem 5: "The script completed but did not return anything"
**Solusi**:
1. ✅ Pastikan function `doGet()` dan `doPost()` ada di code
2. ✅ Pastikan ada `return ContentService.createTextOutput(...)`
3. ✅ Re-save dan re-deploy

---

## 📋 CHECKLIST SETUP

Copy checklist ini dan centang satu-per-satu:

```
Setup Google Sheets:
☐ 1. Buat Google Spreadsheet baru
☐ 2. Copy Spreadsheet ID dari URL
☐ 3. Buka Extensions → Apps Script

Setup Apps Script:
☐ 4. Hapus code default
☐ 5. Paste code dari GoogleAppsScript_Leaderboard.js
☐ 6. Ganti SPREADSHEET_ID di baris 11
☐ 7. Check SHEET_NAME di baris 14
☐ 8. Save (Ctrl+S)

Run Setup Function:
☐ 9. Dropdown → Select "setupSpreadsheet"
☐ 10. Click Run ▶
☐ 11. Authorize permissions (Review → Allow)
☐ 12. Check Execution log: "✅ Spreadsheet setup complete!"
☐ 13. Refresh Google Sheets → Headers muncul

Deploy Web App:
☐ 14. Click Deploy → New deployment
☐ 15. Type: Web app
☐ 16. Execute as: Me
☐ 17. Who has access: Anyone
☐ 18. Click Deploy
☐ 19. Copy Web App URL

Test di Browser:
☐ 20. Paste URL di browser (tanpa parameter)
☐ 21. Response: {"status":"success", "message":"...API is running!"}
☐ 22. Test dengan ?action=test
☐ 23. Test dengan ?action=getLeaderboard&className=3A&level=1

Unity Integration:
☐ 24. Open Unity Project
☐ 25. Select LeaderboardManager GameObject
☐ 26. Paste Web App URL ke Inspector
☐ 27. Set Class Name = "3A"
☐ 28. Set Player Name = "TestPlayer"
☐ 29. Enable Debug Mode
☐ 30. Play test scene → Upload score → Check Console
```

---

## 🎯 EXPECTED RESULTS

### Setelah Setup Berhasil:

#### 1. Google Sheets
```
| PlayerName | ClassName | Level | Score | Stars | Timestamp           |
|------------|-----------|-------|-------|-------|---------------------|
| Daffa      | 3A        | 1     | 95    | 3     | 2025-12-07 10:30:00 |
| Budi       | 3A        | 1     | 87    | 2     | 2025-12-07 10:31:00 |
| Siti       | 3A        | 1     | 78    | 2     | 2025-12-07 10:32:00 |
```

#### 2. Browser Test (tanpa parameter)
```json
{
  "status": "success",
  "message": "Eco Quest Leaderboard API is running!",
  "timestamp": "2025-12-07T10:30:00.000Z"
}
```

#### 3. Unity Console (saat upload)
```
📤 [LEADERBOARD] Uploading score...
   Player: TestPlayer
   Class: 3A
   Level: 1
   Score: 85
   Stars: 2
📄 [LEADERBOARD] JSON Data: {"action":"upload","playerName":"TestPlayer",...}
✅ [LEADERBOARD] Upload SUCCESS!
   Response: {"status":"success","message":"Score uploaded successfully!"}
```

#### 4. Unity Console (saat download leaderboard)
```
📥 [LEADERBOARD] Downloading leaderboard...
   Class: 3A
   Level: 1
📄 [LEADERBOARD] JSON Response: [{"playerName":"Daffa","score":95,...},...]
✅ [LEADERBOARD] Download SUCCESS!
   Total Entries: 3
```

---

## 💡 TIPS PRO

### Tip 1: Version Management
- Setiap kali update code, deploy dengan **New version**
- Bisa rollback ke version lama jika ada masalah
- Deploy → Manage deployments → Edit → Version dropdown

### Tip 2: Logging
- Semua logs ada di **View → Executions**
- Filter by: Date, Status (Success/Error)
- Berguna untuk debug production issues

### Tip 3: Testing
- Selalu test di browser dulu sebelum Unity
- Jika browser error = Apps Script issue
- Jika browser OK tapi Unity error = Unity code issue

### Tip 4: Security
- Jangan share Web App URL secara public (bisa spam)
- Bisa add validation di Apps Script (check IP, rate limit, dst)
- Untuk production: consider Firebase/PlayFab

### Tip 5: Backup
- Google Sheets auto-save, tapi bisa manual backup
- File → Download → CSV atau Excel
- Simpan code Apps Script di file `.js` lokal (sudah saya buat!)

---

## 🚀 NEXT STEPS

Setelah Web App URL sudah working:

1. ✅ **Paste URL ke Unity** (`LeaderboardManager` Inspector)
2. ✅ **Import Star Icons** (lihat `ASSET_REQUIREMENTS_STAR_LEADERBOARD.md`)
3. ✅ **Setup Win Panel UI** (add star GameObjects, buttons)
4. ✅ **Setup Leaderboard Panel UI** (ScrollView, PlayerRow prefab)
5. ✅ **Test Upload Score** (play game → win → click upload button)
6. ✅ **Test View Leaderboard** (main menu → click leaderboard button)

**Full tutorial**: `Docs/TUTORIAL_STAR_RATING_LEADERBOARD.md`

---

## 📞 NEED MORE HELP?

**Check Logs**:
- Apps Script: View → Executions
- Unity: Console window dengan Debug Mode enabled
- Google Sheets: Check if new rows added

**Common Issues**:
- "Unknown action" = Code belum di-update atau salah deploy
- "Missing document" = Spreadsheet ID salah
- "Authorization" = Belum allow permissions
- Unity timeout = Check internet connection

**All working?** 🎉 **Congrats!** Sistem leaderboard kamu sudah live!

---

**Created by**: GitHub Copilot  
**Date**: 2025-12-07  
**Version**: 1.0
