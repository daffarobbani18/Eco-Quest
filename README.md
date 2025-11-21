<p align="center">
  <img src="https://img.icons8.com/color/96/recycle.png" width="120" alt="Eco Quest Logo"/>
</p>

<h1 align="center">🌱 Eco Quest: Petualangan Adiwiyata</h1>
<p align="center"><i>Game Edukasi Lingkungan Berbasis Project-Based Learning (PjBL)</i></p>

<p align="center">
  <!-- Badges -->
  <img src="https://img.shields.io/badge/Unity-2022.x%20LTS-000?logo=unity&logoColor=white" />
  <img src="https://img.shields.io/badge/Platform-Android-3DDC84?logo=android&logoColor=white" />
  <img src="https://img.shields.io/badge/Build%20Status-In%20Progress-yellow?style=flat-square" />
  <img src="https://img.shields.io/badge/License-TBD-lightgrey" />
</p>

---

Eco Quest adalah game simulasi edukasi 2D yang bertujuan meningkatkan kesadaran siswa terhadap pengelolaan sampah di lingkungan sekolah. Mengadaptasi program **Sekolah Adiwiyata**, pemain berperan sebagai siswa yang ditugaskan menyelesaikan proyek kebersihan di berbagai area sekolah.

---

## 🎯 Fitur Utama

- **Pendekatan Project-Based Learning (PjBL)**
  Tiap level → *proyek sekolah* dengan briefing & evaluasi seperti dunia nyata.
  
- **2-Phase Gameplay yang Menarik**
  - **Fase 1 – Koleksi:** Mengumpulkan sampah menggunakan mekanik drag-and-drop dalam batas waktu.
  - **Fase 2 – Sortir:** Menyortir sampah yang dikumpulkan sebelumnya ke tong yang benar di atas conveyor belt.

- **Fokus Edukasi Sampah**
  - Organik
  - Anorganik
  - B3 (Bahan Berbahaya & Beracun)

- **Dynamic Data System**
  Menggunakan *Scriptable Objects* untuk data sampah & level → mudah dikembangkan.

- **Mobile-Friendly Vertical Gameplay**
  Dioptimalkan khusus untuk Android orientasi portrait.

---

## 🎮 Gameplay Loop

1. **Briefing di Markas Klub**  
   Memilih proyek (contoh: Proyek Kantin)
2. **Fase 1 – Lokasi Sekolah**  
   Ambil sampah sesuai target dalam waktu tertentu
3. **Inventaris & Transisi**  
   Semua sampah tersimpan sebagai data actual
4. **Fase 2 – Ruang Pengolahan**  
   Sortir ke tong hijau/kuning/merah
5. **Rapor PjBL**  
   Penilaian berdasarkan kecepatan & akurasi

---

## 🛠️ Teknologi

| Komponen         | Teknologi |
|-----------------|-----------|
| Game Engine     | Unity 2022.x LTS (2D Core Template) |
| Bahasa          | C# |
| Target Platform | Android (Portrait) |
| Version Control | Git + GitHub |

---

## 📂 Struktur Proyek

Assets/
├── _Scenes/ # Level (.unity)
├── _Scripts/ # C# Code
│ ├── Data/ # ScriptableObject Templates
│ └── Managers/ # Singletons (GameManager, SceneLoader)
├── Art/ # Sprite, Background, UI
├── Audio/ # BGM + SFX
├── Prefabs/ # Prefab objek & sistem conveyor
└── Resources/Data/ # Asset data sampah & level

---

## 🚀 Status Pengembangan

| Fitur/Fase | Status |
|-----------|--------|
| Setup Proyek Unity | ✔️ |
| Integrasi Aset Visual | ✔️ |
| Sistem Data Sampah & Level | ✔️ |
| 36 Data Sampah & 3 Level | ✔️ |
| Core Game Manager & Scene Loader | ✔️ |
| 🔜 Sorting Gameplay Fase 2 | Sedang Dikerjakan |

---

## 🔮 Rencana Mendatang

- Mekanik Sorting lengkap + animasi
- Sistem Score lanjutan + grading Adiwiyata
- Penambahan area baru (Lab IPA, Gudang, Perpustakaan)
- UI/UX final + tutorial interaktif
- Publikasi APK Release

---

## 👥 Tim Pengembang

| Nama | Peran | GitHub |
|------|------|--------|
| Daffa Robbani | Game Programmer & Designer | https://github.com/daffarobbani18 |
| Athallah Budiman Devia Putra | Game Programmer & Designer | https://github.com/athalbudi |

---

## 📜 Lisensi
*(Dalam perencanaan — MIT/CC-BY-SA/GPL)*

---

> “Dari sekolah kita mulai menjaga bumi.” 🌍  
> Mari wujudkan Adiwiyata dalam bentuk permainan edukasi interaktif!

