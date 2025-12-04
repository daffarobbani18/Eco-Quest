# 📊 TAHAP 3: UPDATE WASTEDATA (SKOR DINAMIS)
## Tutorial Setup Unity Eco-Quest

⏱️ **Estimasi Waktu:** 20 menit  
🎯 **Tujuan:** Mengisi field skor di semua WasteData untuk sistem scoring dinamis

---

## 📋 CHECKLIST TAHAP INI

- [ ] Pahami sistem scoring baru
- [ ] Review semua WasteData yang ada
- [ ] Tentukan skor per sampah
- [ ] Update semua WasteData di Inspector
- [ ] Buat WasteData baru jika perlu
- [ ] Test skor dinamis

---

## 🎓 LANGKAH 1: PAHAMI SISTEM SCORING BARU

### **Apa yang Berubah?**

**Sebelum Perbaikan:**
```csharp
// Hardcode di script
if (benar) TambahSkor(10);    // ❌ Semua sampah +10
if (salah) KurangiSkor(5);     // ❌ Semua sampah -5
```

**Setelah Perbaikan:**
```csharp
// Dinamis dari WasteData
if (benar) TambahSkor(dataSampah.skorBenar);  // ✅ Sesuai WasteData
if (salah) KurangiSkor(dataSampah.skorSalah); // ✅ Sesuai WasteData
```

---

### **Keuntungan Sistem Baru:**

✅ **Game Balancing Mudah:**
```
Sampah sulit (B3) → Skor tinggi (20 poin)
Sampah mudah (Organik) → Skor rendah (5 poin)
```

✅ **Variasi Gameplay:**
```
Tidak semua sampah sama nilainya
Mendorong pemain lebih hati-hati dengan sampah B3
```

✅ **Edukasi Lebih Baik:**
```
Sampah berbahaya = Poin lebih banyak
Mengajarkan pentingnya pemilahan yang benar
```

---

### **Field Baru di WasteData:**

```
── Scoring (Scene 03 - Pemilahan) ──

Skor Benar: 10
  ↳ Poin yang didapat jika sampah dibuang ke tong BENAR

Skor Salah: 5
  ↳ Poin yang dikurangi jika sampah dibuang ke tong SALAH
  ↳ Tulis angka POSITIF (misal: 5 akan jadi -5 di game)
```

---

### **Default Values:**

Jika field tidak diisi:
```
skorBenar = 10  (default)
skorSalah = 5   (default)
```

**Backward Compatible:**
- WasteData lama tetap jalan dengan nilai default
- Tidak perlu update paksa, tapi **SANGAT DISARANKAN** untuk game balance

---

## 🔍 LANGKAH 2: REVIEW WASTEDATA YANG ADA

### **2.1. Buka Folder WasteData:**

```
Di Project Window:
Assets → _Scripts → Gameplay → Data

atau folder lain tempat Anda simpan WasteData
```

---

### **2.2. Catat Semua WasteData:**

**Buat tabel di notepad/excel:**

```
Nama WasteData          | Tipe Sampah | Kesulitan | Skor Benar | Skor Salah
------------------------|-------------|-----------|------------|------------
WasteData_SisaNasi      | Organik     | Mudah     |    ?       |    ?
WasteData_KulitPisang   | Organik     | Mudah     |    ?       |    ?
WasteData_BotolPlastik  | Anorganik   | Medium    |    ?       |    ?
WasteData_KalengSoda    | Anorganik   | Medium    |    ?       |    ?
WasteData_KertasBekas   | Anorganik   | Medium    |    ?       |    ?
WasteData_BateraiBekas  | B3          | Sulit     |    ?       |    ?
WasteData_LampuNeon     | B3          | Sulit     |    ?       |    ?
...                     | ...         | ...       |    ?       |    ?
```

---

### **2.3. Verifikasi Minimal WasteData:**

**Untuk gameplay seimbang, butuh minimal:**
```
- 2-3 Sampah Organik
- 3-4 Sampah Anorganik
- 1-2 Sampah B3

Total: Minimal 6-8 jenis sampah berbeda
```

**❗ Jika Kurang:**
- Lanjut ke LANGKAH 5 untuk membuat WasteData baru
- Atau sesuaikan desain game dengan sampah yang ada

---

## 💡 LANGKAH 3: TENTUKAN SKOR PER SAMPAH

### **3.1. Rekomendasi Skor (Balanced):**

#### **Tingkat Mudah (Organik):**
```
Karakteristik:
- Mudah dikenali (makanan, daun)
- Umum ditemukan
- Tidak berbahaya

Skor Benar: 5-8 poin
Skor Salah: 3-4 poin

Contoh:
WasteData_SisaNasi:
  skorBenar = 5
  skorSalah = 3

WasteData_KulitPisang:
  skorBenar = 5
  skorSalah = 3
```

---

#### **Tingkat Medium (Anorganik):**
```
Karakteristik:
- Butuh sedikit pemikiran (plastik, logam, kertas)
- Cukup umum
- Perlu dikelola tapi tidak darurat

Skor Benar: 10-15 poin
Skor Salah: 5-7 poin

Contoh:
WasteData_BotolPlastik:
  skorBenar = 10
  skorSalah = 5

WasteData_KalengSoda:
  skorBenar = 12
  skorSalah = 6

WasteData_KertasBekas:
  skorBenar = 10
  skorSalah = 5
```

---

#### **Tingkat Sulit (B3 - Bahan Berbahaya Beracun):**
```
Karakteristik:
- Berbahaya jika salah penanganan
- Jarang ditemukan
- Perlu perhatian khusus

Skor Benar: 20-30 poin
Skor Salah: 10-15 poin

Contoh:
WasteData_BateraiBekas:
  skorBenar = 20
  skorSalah = 10

WasteData_LampuNeon:
  skorBenar = 25
  skorSalah = 12

WasteData_ObatKadaluarsa:
  skorBenar = 30
  skorSalah = 15
```

---

### **3.2. Prinsip Game Balancing:**

**High Risk, High Reward:**
```
Sampah B3:
- Benar → Reward besar (+20 atau lebih)
- Salah → Penalty besar (-10 atau lebih)
```

**Easy & Safe:**
```
Sampah Organik:
- Benar → Reward kecil (+5)
- Salah → Penalty kecil (-3)
```

**Progressive Difficulty:**
```
Level Awal: Lebih banyak Organik (mudah)
Level Tengah: Mix Organik + Anorganik
Level Akhir: Banyak B3 (sulit)
```

---

### **3.3. Tabel Skor Lengkap (Template):**

Gunakan tabel ini sebagai panduan:

```
╔════════════════════╦═════════════╦════════════╦════════════╗
║ Jenis Sampah       ║ Tipe        ║ Skor Benar ║ Skor Salah ║
╠════════════════════╬═════════════╬════════════╬════════════╣
║ Sisa Nasi          ║ Organik     ║     5      ║     3      ║
║ Kulit Pisang       ║ Organik     ║     5      ║     3      ║
║ Daun Kering        ║ Organik     ║     5      ║     3      ║
╠════════════════════╬═════════════╬════════════╬════════════╣
║ Botol Plastik      ║ Anorganik   ║     10     ║     5      ║
║ Kaleng Soda        ║ Anorganik   ║     12     ║     6      ║
║ Kertas Bekas       ║ Anorganik   ║     10     ║     5      ║
║ Kantong Plastik    ║ Anorganik   ║     10     ║     5      ║
╠════════════════════╬═════════════╬════════════╬════════════╣
║ Baterai Bekas      ║ B3          ║     20     ║     10     ║
║ Lampu Neon         ║ B3          ║     25     ║     12     ║
║ Obat Kadaluarsa    ║ B3          ║     30     ║     15     ║
╚════════════════════╩═════════════╩════════════╩════════════╝
```

---

## 🛠️ LANGKAH 4: UPDATE SEMUA WASTEDATA

### **4.1. Buka WasteData di Inspector:**

**Untuk SETIAP WasteData:**

```
1. Di Project Window, klik WasteData_xxx.asset
2. Lihat Inspector di sebelah kanan
3. Scroll ke bagian bawah
4. Cari section:
   
   ── Scoring (Scene 03 - Pemilahan) ──
   Skor Benar: 10
   Skor Salah: 5
```

---

### **4.2. Isi Skor Sesuai Tabel:**

**Contoh: WasteData_SisaNasi**

```
Inspector:
┌─────────────────────────────────────┐
│ 📄 WasteData_SisaNasi               │
├─────────────────────────────────────┤
│ Nama Sampah: "Sisa Nasi"            │
│ Tipe Sampah: Organik                │
│ Icon Sampah: [sprite_nasi]          │
│                                     │
│ ── Scoring (Scene 03 - Pemilahan) ──│
│                                     │
│ Skor Benar: 5    ⬅️ ISI INI        │
│ Skor Salah: 3    ⬅️ ISI INI        │
└─────────────────────────────────────┘
```

**Langkah:**
```
1. Klik field "Skor Benar"
2. Ketik angka: 5
3. Klik field "Skor Salah"
4. Ketik angka: 3
5. Tekan Enter atau klik di tempat lain (auto-save)
```

---

### **4.3. Ulangi untuk Semua WasteData:**

**Workflow Efisien:**

```
1. Buka tabel skor yang sudah dibuat (LANGKAH 3.3)
2. Di Project Window, klik WasteData pertama
3. Isi skorBenar dan skorSalah sesuai tabel
4. Tekan panah bawah (↓) di keyboard untuk ke WasteData berikutnya
5. Ulangi hingga semua terisi
```

**💡 Tips:**
- Jangan tutup Inspector saat update
- Gunakan keyboard untuk navigasi cepat
- Ctrl+S setelah selesai untuk save

---

### **4.4. Verifikasi Update:**

**Cek Semua WasteData:**

```
Untuk SETIAP WasteData, pastikan:
✅ skorBenar > 0 (tidak 0 atau kosong)
✅ skorSalah > 0 (tidak 0 atau kosong)
✅ skorBenar >= skorSalah (reward >= penalty, biasanya)
✅ Skor sesuai dengan tingkat kesulitan
```

**Checklist:**

```
WasteData              | Skor Benar | Skor Salah | Status
-----------------------|------------|------------|--------
WasteData_SisaNasi     |     5      |     3      | ✅
WasteData_KulitPisang  |     5      |     3      | ✅
WasteData_BotolPlastik |    10      |     5      | ✅
WasteData_KalengSoda   |    12      |     6      | ✅
WasteData_BateraiBekas |    20      |    10      | ✅
...                    |    ...     |    ...     | ...
```

---

## ➕ LANGKAH 5: BUAT WASTEDATA BARU (OPSIONAL)

**Jika butuh WasteData tambahan:**

### **5.1. Create WasteData Baru:**

```
1. Di Project Window, navigate ke:
   Assets → _Scripts → Gameplay → Data

2. Klik kanan di area kosong
3. Pilih: Create → EcoQuest → Waste Data
   
   ⚠️ Jika tidak ada "EcoQuest":
   - Coba: Create → PjBL → Waste Data
   - Atau cek [CreateAssetMenu] di WasteData.cs

4. Rename: "WasteData_NamaSampah"
   Contoh: WasteData_BotolKaca
```

---

### **5.2. Isi Field WasteData Baru:**

**Select WasteData baru di Project, lalu di Inspector:**

```
┌─────────────────────────────────────┐
│ 📄 WasteData_BotolKaca              │
├─────────────────────────────────────┤
│ Nama Sampah: "Botol Kaca"           │  ⬅️ Nama display di game
│                                     │
│ Tipe Sampah: [Dropdown]             │  ⬅️ Pilih: Anorganik
│   ☐ Organik                         │
│   ☑ Anorganik                       │
│   ☐ B3                              │
│                                     │
│ Icon Sampah: [Drag sprite]          │  ⬅️ Drag sprite botol kaca
│                                     │
│ ── Scoring (Scene 03 - Pemilahan) ──│
│                                     │
│ Skor Benar: 12                      │  ⬅️ Sesuai tipe Anorganik
│ Skor Salah: 6                       │
└─────────────────────────────────────┘
```

---

### **5.3. Tambahkan ke WasteSpawner (Scene 03):**

**Agar sampah baru bisa spawn:**

```
⚠️ Langkah ini dilakukan di TAHAP 4 (Setup Scene 03)
Untuk sekarang, cukup buat WasteData-nya dulu
```

---

## 🧪 LANGKAH 6: TEST SKOR DINAMIS (PREVIEW)

**Kita belum bisa test di Scene 03 (belum setup), tapi bisa verifikasi:**

### **6.1. Buka Scene 02:**

```
Assets → _Scenes → 02_Game_Kantin.unity
```

---

### **6.2. Test Script Compilation:**

```
1. Klik menu: Build → Compile Scripts
   atau tunggu auto-compile selesai

2. Cek Console, pastikan TIDAK ada error:
   ✅ "Compilation completed successfully"
   
3. Jika ada error, lihat Troubleshooting
```

---

### **6.3. Verifikasi Field Muncul:**

```
1. Select salah satu sampah di Hierarchy Scene 02
2. Lihat Inspector → CollectionItem script
3. Expand field "Data Sampah"
4. Lihat WasteData yang terhubung
5. Expand WasteData preview
6. Pastikan muncul:
   
   skorBenar: 5 (atau nilai yang Anda set)
   skorSalah: 3
```

**✅ Jika terlihat:** Field berhasil ditambahkan!  
**❌ Jika tidak muncul:** Script WasteData.cs belum diperbaiki

---

## 📄 LANGKAH 7: DOKUMENTASI (OPSIONAL)

### **Buat Catatan Skor Game:**

**File: SkorBalance.txt atau Excel**

```
Dokumentasi Skor Sampah - Eco Quest
=====================================

Tingkat Mudah (Organik):
- Sisa Nasi: +5/-3
- Kulit Pisang: +5/-3
- Daun Kering: +5/-3

Tingkat Medium (Anorganik):
- Botol Plastik: +10/-5
- Kaleng Soda: +12/-6
- Kertas Bekas: +10/-5
- Botol Kaca: +12/-6

Tingkat Sulit (B3):
- Baterai Bekas: +20/-10
- Lampu Neon: +25/-12
- Obat Kadaluarsa: +30/-15

Total Skor Maksimal (jika semua benar):
- Level 1 (8 sampah): 5×3 + 10×3 + 20×2 = 85 poin
- Level 2 (10 sampah): ...

Catatan Balancing:
- Ratio B3:Anorganik:Organik = 1:2:2 untuk balance
- Skor maksimal target: 100-150 poin per level
```

---

## ✅ CHECKLIST AKHIR TAHAP 3

### **WasteData Update:**
- [ ] Semua WasteData sudah diupdate dengan skorBenar & skorSalah
- [ ] Tidak ada WasteData dengan skor 0 atau kosong
- [ ] Skor sesuai dengan tingkat kesulitan sampah
- [ ] Ratio Organik:Anorganik:B3 seimbang (sekitar 2:3:1)

### **Verifikasi:**
- [ ] Script compile tanpa error
- [ ] Field skor muncul di Inspector WasteData
- [ ] Tabel skor sudah dibuat untuk referensi
- [ ] (Opsional) Dokumentasi skor dibuat

### **Persiapan TAHAP 4:**
- [ ] WasteData siap digunakan di Scene 03
- [ ] Minimal 6-8 WasteData berbeda sudah ada
- [ ] Sprite sampah semua sudah linked

---

## 🚨 TROUBLESHOOTING

### **Problem: Field Skor Tidak Muncul di Inspector**
**Penyebab:**
- Script WasteData.cs belum diperbaiki
- Unity belum recompile

**Solusi:**
```
1. Buka WasteData.cs
2. Pastikan ada field:
   [Header("Scoring (Scene 03 - Pemilahan)")]
   public int skorBenar = 10;
   public int skorSalah = 5;

3. Save script (Ctrl+S)
4. Kembali ke Unity, tunggu recompile
5. Refresh Inspector (klik WasteData lain lalu kembali)
```

---

### **Problem: Compilation Error**
**Error:**
```
Assets/..../WasteData.cs(XX,XX): error CS0102: 
The type 'WasteData' already contains a definition for 'skorBenar'
```

**Penyebab:**
- Field ditulis 2 kali di script

**Solusi:**
```
1. Buka WasteData.cs
2. Cari duplikat field skorBenar atau skorSalah
3. Hapus salah satu
4. Save
```

---

### **Problem: Lupa Skor Berapa yang Sudah Diisi**
**Solusi:**
```
1. Buat script temporary untuk log semua skor:

using UnityEngine;

public class LogAllScores : MonoBehaviour
{
    void Start()
    {
        WasteData[] allData = Resources.FindObjectsOfTypeAll<WasteData>();
        
        foreach (WasteData data in allData)
        {
            Debug.Log($"{data.namaSampah}: +{data.skorBenar}/-{data.skorSalah}");
        }
    }
}

2. Attach ke GameObject, Play, lihat Console
3. Copy log untuk referensi
```

---

### **Problem: Bingung Menentukan Skor**
**Solusi:**
```
Pakai formula sederhana:

skorBenar = tingkatKesulitan × 5
skorSalah = skorBenar ÷ 2

Contoh:
- Mudah (1): skorBenar=5, skorSalah=3 (rounded)
- Medium (2): skorBenar=10, skorSalah=5
- Sulit (4): skorBenar=20, skorSalah=10
```

---

## ⏭️ LANGKAH SELANJUTNYA

**Jika SEMUA checklist ✅:**
- ✅ WasteData siap digunakan!
- ✅ Lanjut ke **TAHAP 4: Setup Scene 03 (Processing)**
- 📄 Buka file: `TUTORIAL_TAHAP_4_Scene_Processing.md`

**Jika Ada yang ❌:**
- ⚠️ Selesaikan dulu update WasteData
- 🔄 Verifikasi semua skor terisi
- 📞 Lihat Troubleshooting jika butuh bantuan

---

**🎉 Selamat! TAHAP 3 Selesai!**

**Next:** TAHAP 4 - Setup Scene 03 (Processing) - INI TAHAP TERPENTING!

---

**Developer:** daffarobbani18  
**Project:** Eco-Quest  
**Last Updated:** December 4, 2025
