# 📦 TAHAP 1: PERSIAPAN & VERIFIKASI AWAL
## Tutorial Setup Unity Eco-Quest

⏱️ **Estimasi Waktu:** 15 menit  
🎯 **Tujuan:** Memastikan project siap untuk perbaikan dan setup

---

## 📋 CHECKLIST TAHAP INI

- [ ] Backup project lengkap
- [ ] Verifikasi Unity version
- [ ] Cek struktur folder
- [ ] Verifikasi script sudah diperbaiki
- [ ] Siapkan testing environment
- [ ] Buat list WasteData yang ada

---

## 🔧 LANGKAH 1: BACKUP PROJECT

### **Mengapa Penting?**
- Jika terjadi kesalahan, Anda bisa kembali ke versi sebelumnya
- Mencegah kehilangan progress
- Memudahkan rollback jika ada masalah

### **Cara Backup:**

**Opsi A: Manual Backup (Recommended)**
```
1. Tutup Unity Editor
2. Buka File Explorer
3. Navigate ke: D:\Project Game Edukasi\
4. Klik kanan folder "eco-quest"
5. Copy folder tersebut
6. Paste dengan nama: "eco-quest_BACKUP_04Dec2025"
```

**Opsi B: Git Commit (Jika pakai Git)**
```powershell
# Di PowerShell, navigate ke folder project
cd "D:\Project Game Edukasi\eco-quest"

# Commit perubahan saat ini
git add .
git commit -m "Backup sebelum setup Scene 03"
git push
```

**✅ Verifikasi:**
- Pastikan folder backup ada dan ukurannya sama dengan original
- Jika pakai Git, cek di GitHub bahwa commit berhasil

---

## 🔧 LANGKAH 2: VERIFIKASI UNITY VERSION

### **Cek Version yang Dipakai:**

1. **Buka Unity Hub**
2. **Lihat project "eco-quest"**
3. **Catat Unity version** (misal: 2021.3.16f1, 2022.3.5f1, dll)

4. **Buka file ProjectVersion.txt:**
```
Lokasi: D:\Project Game Edukasi\eco-quest\ProjectSettings\ProjectVersion.txt
```

5. **Pastikan version cocok** dengan yang di Unity Hub

**❗ Jika Version Tidak Cocok:**
```
- Buka project dengan Unity version yang benar
- Atau update Unity ke version yang sesuai
- Hindari membuka project dengan version berbeda (bisa corrupt)
```

**✅ Verifikasi:**
- Unity Editor terbuka tanpa error
- Tidak ada warning "Project upgrade" atau "Downgrade"

---

## 🔧 LANGKAH 3: CEK STRUKTUR FOLDER

### **Pastikan Folder Penting Ada:**

**Di Project Window Unity, verifikasi:**

```
Assets/
├── _Scenes/
│   ├── 02_Game_Kantin.unity ✅
│   └── 03_Game_Processing.unity ✅
│
├── _Scripts/
│   ├── Manager/
│   │   └── GameManager.cs ✅
│   │
│   └── Gameplay/
│       ├── CollectionItem.cs ✅
│       ├── CollectionLevelManager.cs ✅
│       ├── ProcessingLevelManager.cs ✅
│       ├── WasteSpawner.cs ✅
│       ├── BriefingSequence.cs ✅
│       ├── DragController.cs ✅
│       ├── BinController.cs ✅
│       └── Data/
│           ├── WasteData.cs ✅
│           └── LevelData.cs ✅
│
├── Prefabs/
│   └── UI/
│       └── Panel_DialogGuru.prefab ✅
│
└── Art/ (Sprites, dll)
```

**❗ Jika Ada yang Hilang:**
```
1. Cek di folder backup
2. Atau cek di Git history
3. Restore file yang hilang
```

**✅ Verifikasi:**
- Semua file script ada
- Tidak ada "Missing Script" di Console
- Scene 02 dan 03 bisa dibuka

---

## 🔧 LANGKAH 4: VERIFIKASI SCRIPT SUDAH DIPERBAIKI

### **Cek Perbaikan yang Sudah Dilakukan:**

**4.1. Cek WasteData.cs:**
```csharp
1. Buka: Assets/_Scripts/Gameplay/Data/WasteData.cs
2. Scroll ke bawah
3. Pastikan ada field baru ini:

   [Header("Scoring (Scene 03 - Pemilahan)")]
   [Tooltip("Skor yang didapat jika sampah dibuang ke tong yang BENAR")]
   public int skorBenar = 10;
   
   [Tooltip("Skor yang dikurangi jika sampah dibuang ke tong yang SALAH")]
   public int skorSalah = 5;
```

**✅ Jika ADA:** Script sudah diperbaiki ✓  
**❌ Jika TIDAK ADA:** Baca file `PERBAIKAN_HIGH_MEDIUM.md` untuk fix

---

**4.2. Cek BinController.cs:**
```csharp
1. Buka: Assets/_Scripts/Gameplay/BinController.cs
2. Cari baris sekitar line 17
3. Pastikan ada ini:

   WasteItem scriptSampah = other.GetComponent<WasteItem>();
   
   (BUKAN CollectionItem!)
```

**✅ Jika BENAR (WasteItem):** Script sudah diperbaiki ✓  
**❌ Jika SALAH (CollectionItem):** Baca file `PERBAIKAN_CRITICAL_BUGS.md` untuk fix

---

**4.3. Cek DragController.cs:**
```csharp
1. Buka: Assets/_Scripts/Gameplay/DragController.cs
2. Cari baris sekitar line 80-85
3. Pastikan ada ini:

   if (GameManager.Instance != null)
   {
       GameManager.Instance.TambahSkor(myItem.dataSampah.skorBenar);
       GameManager.Instance.KurangiJumlahSampah();
   }
```

**✅ Jika ADA null check dan KurangiJumlahSampah:** Script sudah diperbaiki ✓  
**❌ Jika TIDAK ADA:** Baca file `PERBAIKAN_CRITICAL_BUGS.md` untuk fix

---

**4.4. Cek CollectionItem.cs:**
```csharp
1. Buka: Assets/_Scripts/Gameplay/CollectionItem.cs
2. Cari fungsi MasukKeTas()
3. Pastikan TIDAK ADA baris ini:

   GameManager.Instance.KurangiJumlahSampah(); ❌
   
   (Harus sudah dihapus!)
```

**✅ Jika TIDAK ADA KurangiJumlahSampah di MasukKeTas:** Script sudah diperbaiki ✓  
**❌ Jika MASIH ADA:** Baca file `PERBAIKAN_CRITICAL_BUGS.md` untuk fix

---

**4.5. Cek ProcessingLevelManager.cs:**
```csharp
1. Buka: Assets/_Scripts/Gameplay/ProcessingLevelManager.cs
2. Cari baris sekitar line 55-57
3. Pastikan ada null check lengkap:

   bool adaBriefing = (briefingScript != null && 
                      dataLevelIni != null && 
                      dataLevelIni.barisDialogSortir != null && 
                      dataLevelIni.barisDialogSortir.Length > 0);
```

**✅ Jika ADA null check untuk barisDialogSortir:** Script sudah diperbaiki ✓  
**❌ Jika TIDAK ADA:** Baca file `PERBAIKAN_HIGH_MEDIUM.md` untuk fix

---

### **📊 Summary Verifikasi:**

| Script | Yang Dicek | Status |
|--------|-----------|--------|
| WasteData.cs | Field skorBenar & skorSalah | ⬜ |
| BinController.cs | Pakai WasteItem (bukan CollectionItem) | ⬜ |
| DragController.cs | Ada null check & KurangiJumlahSampah | ⬜ |
| CollectionItem.cs | TIDAK ada KurangiJumlahSampah | ⬜ |
| ProcessingLevelManager.cs | Null check barisDialogSortir | ⬜ |

**Target: Semua ✅ sebelum lanjut ke TAHAP 2**

---

## 🔧 LANGKAH 5: SIAPKAN TESTING ENVIRONMENT

### **5.1. Buka Console Window:**
```
Unity Menu → Window → General → Console
atau tekan: Ctrl + Shift + C
```

**Pastikan:**
- Console terbuka dan visible
- "Collapse" TIDAK dicentang (agar semua log muncul)
- "Clear on Play" dicentang (agar log fresh setiap test)

---

### **5.2. Setup Play Mode Settings:**
```
Unity Menu → Edit → Project Settings → Editor

Cari section "Enter Play Mode Settings":
☑ Enter Play Mode Options
☐ Reload Domain (uncheck - agar lebih cepat)
☐ Reload Scene (uncheck - agar lebih cepat)
```

**⚠️ Warning:**
- Uncheck ini bisa menyebabkan static variable tidak reset
- Jika ada bug aneh, centang kembali sementara

---

### **5.3. Save Layout:**
```
1. Atur window sesuai kenyamanan:
   - Hierarchy di kiri
   - Scene/Game view di tengah
   - Inspector di kanan
   - Console di bawah

2. Save Layout:
   Unity Menu → Window → Layouts → Save Layout...
   Nama: "Eco-Quest Development"
```

**Keuntungan:**
- Bisa kembali ke layout ini kapan saja
- Semua window sudah di posisi optimal

---

## 🔧 LANGKAH 6: BUAT LIST WASTEDATA YANG ADA

### **Tujuan:**
Agar tahu WasteData mana yang perlu diupdate di TAHAP 3

### **Cara:**

**6.1. Buka Folder WasteData:**
```
Di Project Window:
Assets → _Scripts → Gameplay → Data

atau

Assets → (folder lain tempat Anda simpan WasteData)
```

**6.2. Catat Semua WasteData:**
```
Buat list di notepad/excel:

Nama File                  | Tipe Sampah | Skor Sudah Diisi?
---------------------------|-------------|-------------------
WasteData_BotolPlastik    | Anorganik   | ⬜ Belum
WasteData_KertasBekas     | Anorganik   | ⬜ Belum
WasteData_SisaNasi        | Organik     | ⬜ Belum
WasteData_BateraiBekas    | B3          | ⬜ Belum
...                       | ...         | ...
```

**6.3. Verifikasi Jumlah:**
```
Minimal harus ada: 5-8 WasteData berbeda
(1-2 Organik, 2-3 Anorganik, 1-2 B3)
```

**❗ Jika WasteData Kurang:**
```
Anda perlu membuat WasteData baru di TAHAP 3
```

---

## 🔧 LANGKAH 7: CEK PREFAB PANEL_DIALOGGURU

### **Verifikasi Prefab Briefing:**

**7.1. Lokasi Prefab:**
```
Assets → Prefabs → UI → Panel_DialogGuru.prefab
```

**7.2. Double-click Prefab (Masuk Prefab Mode)**

**7.3. Cek Struktur:**
```
Panel_DialogGuru
├── Panel_Intro
│   ├── Text_Judul (TMP_Text)
│   └── Text_Info (TMP_Text)
│
└── Panel_Dialog
    ├── Image_Guru
    ├── Text_Dialog (TMP_Text)
    ├── Button_Next
    └── Button_Mulai
```

**7.4. Cek Script BriefingSequence:**
```
Select Panel_DialogGuru (root)
Lihat Inspector → Harus ada component: BriefingSequence.cs

Verifikasi field terisi:
✅ Panel Intro → Linked
✅ Text Judul → Linked
✅ Text Info → Linked
✅ Panel Dialog → Linked
✅ Text Dialog Isi → Linked
✅ Tombol Next → Linked
✅ Tombol Mulai → Linked
```

**❗ Jika Ada yang NULL:**
```
1. Drag child object yang sesuai ke field
2. Apply prefab changes
3. Exit prefab mode
```

---

## ✅ CHECKLIST AKHIR TAHAP 1

Sebelum lanjut ke TAHAP 2, pastikan semua ini sudah ✅:

### **Backup & Environment:**
- [ ] Project sudah di-backup
- [ ] Unity version sudah diverifikasi
- [ ] Console window terbuka dan siap
- [ ] Layout sudah disave

### **Verifikasi Script:**
- [ ] WasteData.cs punya field skorBenar & skorSalah
- [ ] BinController.cs pakai WasteItem (bukan CollectionItem)
- [ ] DragController.cs ada null check & KurangiJumlahSampah
- [ ] CollectionItem.cs TIDAK ada KurangiJumlahSampah di MasukKeTas
- [ ] ProcessingLevelManager.cs ada null check barisDialogSortir

### **Struktur Project:**
- [ ] Semua folder penting ada
- [ ] Scene 02 dan 03 bisa dibuka
- [ ] Panel_DialogGuru prefab lengkap dan tersambung
- [ ] List WasteData sudah dibuat

### **Testing Ready:**
- [ ] Console clear on play aktif
- [ ] Play mode settings optimal
- [ ] Tidak ada error di Console saat buka project

---

## 🎯 HASIL YANG DIHARAPKAN

Setelah menyelesaikan TAHAP 1:

✅ **Project Aman:** Sudah ada backup, tidak takut corrupt  
✅ **Script Siap:** Semua perbaikan sudah diterapkan  
✅ **Environment Optimal:** Unity setup untuk development  
✅ **Checklist Lengkap:** Tahu apa yang harus dilakukan next  

---

## 🚨 TROUBLESHOOTING

### **Problem: Script Belum Diperbaiki**
**Solusi:**
1. Baca file dokumentasi perbaikan:
   - `PERBAIKAN_CRITICAL_BUGS.md`
   - `PERBAIKAN_HIGH_MEDIUM.md`
2. Apply perbaikan manual di script
3. Save & recompile
4. Ulangi verifikasi

### **Problem: Folder/File Hilang**
**Solusi:**
1. Restore dari backup
2. Atau check Git history
3. Jika masih hilang, recreate dari template

### **Problem: Unity Crash saat Buka Project**
**Solusi:**
1. Tutup Unity
2. Delete folder: `Library/`
3. Buka project lagi (akan re-import, butuh waktu)
4. Jika masih crash, restore dari backup

### **Problem: Prefab Panel_DialogGuru Rusak**
**Solusi:**
1. Restore prefab dari backup
2. Atau recreate manual:
   - Buat Panel baru
   - Tambah BriefingSequence script
   - Link semua child UI elements
   - Save as prefab

---

## ⏭️ LANGKAH SELANJUTNYA

**Jika SEMUA checklist ✅:**
- ✅ Lanjut ke **TAHAP 2: Setup Scene 02 (Kantin)**
- 📄 Buka file: `TUTORIAL_TAHAP_2_Scene_Kantin.md`

**Jika Ada yang ❌:**
- ⚠️ Selesaikan dulu masalahnya
- 🔄 Ulangi verifikasi
- 📞 Lihat Troubleshooting atau minta bantuan

---

**🎉 Selamat! TAHAP 1 Selesai!**

**Next:** TAHAP 2 - Setup Scene 02 (Kantin)

---

**Developer:** daffarobbani18  
**Project:** Eco-Quest  
**Last Updated:** December 4, 2025
