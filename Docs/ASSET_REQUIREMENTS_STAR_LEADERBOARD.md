# 🎨 ASSET REQUIREMENTS - STAR RATING & LEADERBOARD

## 📋 Overview
Dokumen ini berisi daftar lengkap asset yang dibutuhkan untuk sistem **Star Rating & Leaderboard**, termasuk **AI Prompts** untuk generate asset menggunakan tools seperti Canva, MidJourney, Leonardo AI, atau DALL-E.

---

## ⭐ STAR ICONS

### 1. Empty Star (Outline)
**Deskripsi**: Bintang outline/kosong untuk menunjukkan bintang yang belum didapat

**Spesifikasi Teknis**:
- Format: PNG dengan transparent background
- Resolusi: 512x512 pixels (akan di-scale di Unity)
- Color: Gray (#B0B0B0) atau soft gold outline
- Style: Cartoon/flat design, stroke 4-6px

**AI Prompt (Canva/MidJourney/Leonardo):**
```
A simple 2D cartoon star icon, outline only, no fill, gray color (#B0B0B0), 
thick stroke (6px), rounded corners, flat design style, transparent background, 
centered, icon design for mobile game UI, cute and friendly style, 
perfect for children educational game
```

**AI Prompt (DALL-E):**
```
Create a simple star icon with gray outline only, no fill color, 
flat cartoon style, thick stroke, transparent background, 
cute design suitable for kids game interface, 512x512 pixels
```

---

### 2. Filled Star (Gold)
**Deskripsi**: Bintang terisi penuh berwarna emas untuk menunjukkan bintang yang sudah didapat

**Spesifikasi Teknis**:
- Format: PNG dengan transparent background
- Resolusi: 512x512 pixels
- Color: Gold gradient (#FFD700 to #FFA500)
- Style: Cartoon/flat design dengan subtle shine/sparkle
- Effect: Soft glow/shadow untuk 3D feeling

**AI Prompt (Canva/MidJourney/Leonardo):**
```
A shiny 2D cartoon gold star icon, filled with golden yellow gradient 
(#FFD700 to #FFA500), flat design style with subtle shine effect, 
soft drop shadow, sparkle highlights, transparent background, centered, 
icon design for mobile game achievement UI, cute and friendly style, 
perfect for children educational game, celebratory feeling
```

**AI Prompt (DALL-E):**
```
Create a bright golden yellow star icon, filled completely, 
shiny cartoon style with sparkle effect, soft glow, 
transparent background, cute and celebratory design 
for kids game rewards, 512x512 pixels
```

---

### 3. Filled Star (Silver) - OPTIONAL
**Deskripsi**: Bintang silver untuk tier menengah (jika mau 3 variasi warna)

**Spesifikasi Teknis**:
- Same as Gold Star
- Color: Silver gradient (#C0C0C0 to #A8A8A8)

**AI Prompt:**
```
A shiny 2D cartoon silver star icon, filled with metallic silver gradient 
(#C0C0C0 to #A8A8A8), flat design style, subtle shine effect, 
soft drop shadow, transparent background, icon for game UI
```

---

### 4. Star Variations Set (BUNDLE)
**Deskripsi**: Bundle 3 bintang dalam satu asset untuk consistency

**AI Prompt (Complete Set):**
```
Create a set of 3 star icons in the same cartoon style: 
1) Empty gray outline star, 2) Filled gold shiny star, 3) Filled silver star. 
All with transparent background, flat design, cute style for kids educational game, 
arranged horizontally in one image, 512x512 pixels each star, 
consistent stroke width and corner radius, modern UI design
```

---

## 🏆 TROPHY & MEDAL ICONS (Leaderboard Top 3)

### 5. Gold Trophy (Rank 1)
**Deskripsi**: Trophy emas untuk juara 1

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 256x256 pixels
- Color: Gold (#FFD700)
- Style: Simple trophy cup silhouette

**AI Prompt:**
```
A simple 2D golden trophy cup icon, flat cartoon style, 
shiny gold color (#FFD700), minimal design, no text, 
transparent background, centered, icon for first place winner, 
cute design suitable for kids game leaderboard, 256x256 pixels
```

**Alternative**: Bisa pakai emoji existing: 🥇 (Medal 1st Place)

---

### 6. Silver Trophy (Rank 2)
**Spesifikasi**: Same as Gold Trophy, color: Silver (#C0C0C0)

**AI Prompt:**
```
A simple 2D silver trophy cup icon, flat cartoon style, 
shiny silver color (#C0C0C0), minimal design, no text, 
transparent background, icon for second place, 256x256 pixels
```

**Alternative**: 🥈 (Medal 2nd Place)

---

### 7. Bronze Trophy (Rank 3)
**Spesifikasi**: Same as Gold Trophy, color: Bronze (#CD7F32)

**AI Prompt:**
```
A simple 2D bronze trophy cup icon, flat cartoon style, 
bronze color (#CD7F32), minimal design, no text, 
transparent background, icon for third place, 256x256 pixels
```

**Alternative**: 🥉 (Medal 3rd Place)

---

## 🎨 LEADERBOARD PANEL BACKGROUNDS

### 8. Leaderboard Panel Background
**Deskripsi**: Background pattern/texture untuk Leaderboard Panel

**Spesifikasi Teknis**:
- Format: PNG atau JPG
- Resolusi: 1024x1024 pixels (tileable optional)
- Color: Light pastel (tidak over-bright)
- Style: Subtle pattern/texture, tidak mengganggu text

**AI Prompt:**
```
A subtle background pattern for game leaderboard UI panel, 
light pastel blue and white colors, soft geometric pattern 
(hexagons or rounded squares), very low opacity (10-20%), 
seamless tileable texture, clean and minimal design, 
suitable for children educational game interface, 1024x1024 pixels
```

**Color Variations**:
- **Blue**: Primary (default)
- **Green**: Nature/eco theme (eco-quest!)
- **Purple**: Achievement/premium feel

**AI Prompt (Eco Theme):**
```
A subtle background pattern for eco-themed game UI, 
light green and leaf motifs, very soft and minimal, 
low opacity nature pattern (leaves, recycling symbols), 
seamless tileable, clean design for kids game, 1024x1024 pixels
```

---

### 9. Player Row Background (Highlight)
**Deskripsi**: Background untuk highlight current player row

**Spesifikasi Teknis**:
- Format: PNG transparent atau solid color
- Resolusi: 800x100 pixels (akan di-stretch)
- Color: Gold/yellow highlight (#FFE873)
- Style: Soft gradient or solid with glow

**AI Prompt:**
```
A horizontal banner background for UI highlight effect, 
soft golden yellow color (#FFE873) with subtle gradient to orange, 
slightly transparent (70% opacity), rounded corners (20px radius), 
minimal design, 800x100 pixels, PNG with alpha channel
```

**Note**: Bisa juga pakai **Solid Color** di Unity (tidak perlu asset):
```csharp
highlightColor = new Color(1f, 0.92f, 0.016f, 0.3f); // Gold RGBA
```

---

## 🎉 POPUP & CELEBRATION EFFECTS

### 10. "New Record!" Popup Banner
**Deskripsi**: Banner popup untuk celebrate new personal record

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 800x300 pixels
- Color: Bright celebratory (gold, confetti colors)
- Style: Ribbon banner dengan text "NEW RECORD!"

**AI Prompt:**
```
A celebration banner design with text "NEW RECORD!" in bold letters, 
cartoon style ribbon banner, bright gold and orange colors, 
sparkle and confetti decorations around the text, 
transparent background, flat design suitable for kids game UI, 
excitement and achievement feeling, 800x300 pixels
```

**AI Prompt (Without Text - akan pakai TMP_Text):**
```
A decorative ribbon banner background, cartoon style, 
bright gold color with orange accents, sparkle effects, 
confetti decorations, empty center (no text), 
transparent background, celebration theme, 800x300 pixels
```

---

### 11. Sparkle/Confetti Particle Effect
**Deskripsi**: Particle sprite untuk animasi confetti saat new record

**Spesifikasi Teknis**:
- Format: PNG transparent (sprite sheet optional)
- Resolusi: 64x64 pixels per particle
- Color: Multiple (gold, yellow, orange, blue)
- Shapes: Stars, circles, squares, ribbons

**AI Prompt:**
```
A sprite sheet of celebration confetti particles for game animation, 
contains 12 small icons: stars, circles, squares, ribbon shapes, 
bright colors (gold, yellow, orange, blue, pink), 
cartoon flat style, transparent background, 
each icon 64x64 pixels, arranged in 4x3 grid, 
suitable for Unity particle system
```

---

## 📊 LEADERBOARD UI DECORATIONS

### 12. Rank Number Badges (1-10)
**Deskripsi**: Badge background untuk ranking number 1-10

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 128x128 pixels each
- Color: Gradient (gold for 1-3, silver for 4-10)
- Style: Circular badge dengan number di center

**AI Prompt:**
```
Create a set of 10 circular badge icons for game leaderboard rankings, 
numbered 1 to 10, cartoon flat style: 
- Badges 1-3: Gold gradient background (#FFD700)
- Badges 4-10: Light gray gradient (#D0D0D0)
Each badge has large bold white number in center, 
drop shadow for depth, transparent background, 
128x128 pixels each, arranged in 2 rows (5 per row)
```

**Note**: Bisa pakai emoji/text saja untuk simplicity:
- 🥇🥈🥉 (Top 3)
- `1` `2` `3` ... `10` (Text)

---

### 13. Leaderboard Header Banner
**Deskripsi**: Decorative banner untuk header "LEADERBOARD"

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 1000x200 pixels
- Color: Gold/blue gradient
- Style: Ribbon banner dengan decorative edges

**AI Prompt:**
```
A decorative header banner for game leaderboard UI, 
horizontal ribbon banner design with decorative wavy edges, 
gold and blue gradient colors, sparkle decorations, 
trophy icons on sides, center space for text "LEADERBOARD", 
cartoon flat style, transparent background, 1000x200 pixels
```

---

### 14. Scroll Bar Custom Design
**Deskripsi**: Custom scrollbar untuk Leaderboard ScrollView

**Spesifikasi Teknis**:
- Format: PNG transparent (2 parts: track + handle)
- Track: 20x500 pixels (vertical bar)
- Handle: 20x80 pixels (draggable thumb)
- Color: Light gray track, gold handle
- Style: Rounded, modern

**AI Prompt (Track):**
```
A vertical scrollbar track background, rounded rectangle shape, 
light gray color (#E0E0E0), subtle inner shadow, 
transparent background, 20x500 pixels, minimal design
```

**AI Prompt (Handle):**
```
A scrollbar handle (thumb) button, rounded rectangle shape, 
golden yellow gradient (#FFD700 to #FFA500), 
soft drop shadow, 20x80 pixels, transparent background
```

---

## 🔘 BUTTONS & UI ELEMENTS

### 15. "Upload to Leaderboard" Button
**Deskripsi**: Prominent button untuk upload score

**Spesifikasi Teknis**:
- Format: PNG transparent (3 states: normal, hover, pressed)
- Resolusi: 400x80 pixels
- Color: Green gradient (action button)
- Style: Rounded rectangle, 3D-ish effect

**AI Prompt:**
```
A game UI button design in 3 states (normal, hover, pressed), 
rectangular shape with rounded corners (20px), 
bright green gradient (#4CAF50 to #45A049), 
3D effect with subtle drop shadow, 
text space in center (no text), 
cartoon flat style, transparent background, 400x80 pixels each state, 
arranged horizontally (normal | hover | pressed)
```

**Color Variations**:
- **Green**: Upload/Action
- **Blue**: Refresh/Info
- **Red**: Close/Cancel
- **Gold**: Premium/Special

---

### 16. "Refresh" Button Icon
**Deskripsi**: Icon for refresh leaderboard button

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 64x64 pixels
- Color: White or blue
- Style: Circular arrow (clockwise)

**AI Prompt:**
```
A simple refresh icon, circular arrow clockwise rotation, 
clean line design, blue color (#2196F3), 
transparent background, 64x64 pixels, 
suitable for game UI button icon
```

**Alternative**: Bisa pakai emoji: 🔄 (Refresh symbol)

---

### 17. "Close" Button Icon
**Deskripsi**: X icon untuk close leaderboard panel

**Spesifikasi Teknis**:
- Format: PNG transparent
- Resolusi: 64x64 pixels
- Color: Dark gray or red
- Style: X cross, rounded edges

**AI Prompt:**
```
A simple close button icon, X shape cross, 
rounded edges, dark gray color (#424242), 
thick stroke (6px), transparent background, 
64x64 pixels, minimal design for game UI
```

**Alternative**: Emoji ✖ atau character `X`

---

## 📐 SIZE REFERENCE CHART

| Asset Type | Recommended Size | Unity Import Settings |
|------------|------------------|----------------------|
| Star Icons | 512x512 px | Sprite (2D/UI), Max Size: 512, Compression: None |
| Trophy Icons | 256x256 px | Sprite (2D/UI), Max Size: 256 |
| Panel Backgrounds | 1024x1024 px | Texture, Max Size: 1024, Compression: High Quality |
| Popup Banners | 800x300 px | Sprite (2D/UI), Max Size: 1024 |
| Button Assets | 400x80 px | Sprite (2D/UI), Max Size: 512, Compression: None |
| Particle Sprites | 64x64 px | Sprite (2D/UI), Max Size: 128 |
| UI Icons | 64x64 px | Sprite (2D/UI), Max Size: 128 |

---

## 🎨 COLOR PALETTE

### Primary Colors (Star Rating)
```
Gold Star:        #FFD700 (main)
                  #FFA500 (gradient to)
Empty Star:       #B0B0B0 (gray outline)
Silver Star:      #C0C0C0 (optional)
Bronze:           #CD7F32 (rank 3)
```

### UI Background Colors
```
Panel BG:         #FFFFFF (white) with 90% opacity
                  or #F5F5F5 (light gray)
Highlight:        #FFE873 (gold highlight) with 30% opacity
Normal Row:       #FFFFFF (white) with 10% opacity
```

### Button Colors
```
Action (Green):   #4CAF50
Info (Blue):      #2196F3
Warning (Orange): #FF9800
Danger (Red):     #F44336
Premium (Purple): #9C27B0
```

### Text Colors
```
Primary Text:     #212121 (dark gray, almost black)
Secondary Text:   #757575 (medium gray)
Disabled Text:    #BDBDBD (light gray)
Highlight Text:   #FFD700 (gold)
```

---

## 🛠️ TOOLS RECOMMENDATIONS

### For Simple Icons & UI:
1. **Canva** (https://canva.com)
   - Pros: Easy, templates available, no coding
   - Cons: Limited customization for complex designs
   - Best for: Buttons, text banners, simple shapes

2. **Figma** (https://figma.com)
   - Pros: Professional UI design tool, free tier
   - Cons: Learning curve
   - Best for: Complete UI mockups, precise designs

### For AI-Generated Assets:
1. **Leonardo AI** (https://leonardo.ai)
   - Pros: Game-focused presets, consistent style
   - Cons: Credit-based (free tier limited)
   - Best for: Stars, badges, decorative elements

2. **MidJourney** (https://midjourney.com)
   - Pros: Highest quality outputs
   - Cons: Paid subscription, Discord-based
   - Best for: Hero images, complex illustrations

3. **DALL-E 3** (via ChatGPT Plus)
   - Pros: Follows prompts accurately, good for icons
   - Cons: Subscription required
   - Best for: Simple icons, UI elements

4. **Bing Image Creator** (https://bing.com/create)
   - Pros: FREE (powered by DALL-E 3)
   - Cons: Slower generation
   - Best for: Budget option, icons & sprites

### For Free Assets (No AI):
1. **Flaticon** (https://flaticon.com)
   - Search: "star icon", "trophy icon", "medal icon"
   - License: Free with attribution (or Premium)

2. **Freepik** (https://freepik.com)
   - Search: "game ui elements", "leaderboard design"
   - License: Free with attribution

3. **Kenney Assets** (https://kenney.nl)
   - Free game assets including UI elements
   - Public domain license

---

## 📦 ASSET CHECKLIST

### MUST HAVE (Required):
- [ ] ⭐ Empty Star (outline)
- [ ] ⭐ Filled Star (gold)
- [ ] 🏆 Trophy/Medal icons (or emoji 🥇🥈🥉)
- [ ] 🎉 New Record popup banner
- [ ] 📊 Leaderboard panel background

### NICE TO HAVE (Optional):
- [ ] ⭐ Silver Star (3rd variation)
- [ ] 💫 Sparkle particle sprites
- [ ] 🏅 Rank number badges (1-10)
- [ ] 🎨 Header banner decorative
- [ ] 🔘 Custom scrollbar design
- [ ] 🔄 Refresh button icon
- [ ] ✖ Close button icon

### CAN USE EXISTING (No asset needed):
- [ ] Background colors (solid colors di Unity)
- [ ] Text elements (TextMeshPro)
- [ ] Button backgrounds (9-slice sprites existing)
- [ ] Emoji for top 3 ranks (🥇🥈🥉)

---

## 🚀 QUICK START WORKFLOW

### Method 1: AI Generation (Recommended)
1. Copy **AI Prompts** dari dokumen ini
2. Paste ke tool pilihan (Leonardo AI / DALL-E / Bing Image Creator)
3. Generate → Download PNG
4. Import ke Unity: `Assets/Art/UI/StarRating/` folder
5. Setup Sprite import settings (Sprite 2D/UI, Max Size)
6. Drag sprites ke Inspector fields

### Method 2: Free Assets
1. Browse **Flaticon** atau **Freepik**
2. Search: "star icon flat", "trophy gold icon", etc.
3. Download PNG (512x512 or higher)
4. (Optional) Edit di Photoshop/GIMP untuk adjust colors
5. Import ke Unity

### Method 3: Manual Design
1. Buka **Canva** atau **Figma**
2. Create canvas 512x512px
3. Use shape tools untuk buat star/trophy
4. Apply gradients & effects
5. Export PNG transparent
6. Import ke Unity

---

## 💡 TIPS & BEST PRACTICES

### Consistency is Key
- Use **same art style** untuk semua icons (flat, cartoon, realistic)
- Use **consistent stroke width** (4-6px untuk outline icons)
- Use **consistent color palette** (lihat section Color Palette)
- Use **same corner radius** untuk rounded elements (20px recommended)

### Transparent Backgrounds
- Always export dengan **alpha channel** (PNG format)
- Check transparency di Photoshop: `Select → Select Color Range → Whites`
- Unity will auto-detect transparent pixels

### Naming Convention
```
star_empty.png
star_filled_gold.png
star_filled_silver.png
trophy_gold.png
trophy_silver.png
trophy_bronze.png
popup_new_record.png
bg_leaderboard_panel.png
btn_upload_normal.png
btn_upload_hover.png
btn_upload_pressed.png
icon_refresh.png
icon_close.png
```

### Organize Folders
```
Assets/
└── Art/
    └── UI/
        ├── StarRating/
        │   ├── star_empty.png
        │   ├── star_filled_gold.png
        │   └── star_filled_silver.png
        ├── Leaderboard/
        │   ├── trophy_gold.png
        │   ├── bg_panel.png
        │   └── popup_new_record.png
        └── Buttons/
            ├── btn_upload_normal.png
            └── icon_refresh.png
```

---

## ✅ FINAL NOTES

- **Prioritas**: Focus on **Empty Star** & **Filled Star** dulu (core feature)
- **Emoji Fallback**: Bisa pakai emoji (🥇🥈🥉⭐🏆) untuk prototype cepat
- **Iterate**: Test di game, adjust colors/sizes based on feedback
- **Performance**: Compress PNGs sebelum final build (use TinyPNG.com)
- **Backup**: Save source files (.psd/.fig) untuk future edits

**Happy Designing! 🎨✨**
