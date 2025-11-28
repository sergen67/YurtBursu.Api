# GitHub'a Push Komutları

## ⚠️ ÖNEMLİ: Bu komutları sırayla çalıştırın!

### Adım 1: Mevcut .git klasörünü sil (varsa)
```powershell
if (Test-Path .git) { Remove-Item -Recurse -Force .git; Write-Host ".git klasörü silindi" } else { Write-Host ".git klasörü zaten yok" }
```

### Adım 2: Git repository'yi başlat
```powershell
git init
```

### Adım 3: Tüm dosyaları ekle
```powershell
git add .
```

### Adım 4: İlk commit'i oluştur
```powershell
git commit -m "Initial clean commit"
```

### Adım 5: Branch'i main olarak ayarla
```powershell
git branch -M main
```

### Adım 6: GitHub bilgilerinizi girin
Aşağıdaki bilgileri hazırlayın:
- **GitHub Kullanıcı Adınız**: (örneğin: sergen67)
- **Repository Adı**: (örneğin: TDVWEB veya YurtBursu)

### Adım 7: GitHub'da yeni repo oluşturun
Bu linke gidin ve yeni bir repository oluşturun:
👉 **https://github.com/new**

**Önemli:** Repository oluştururken:
- ✅ **Public** veya **Private** seçin
- ❌ **README, .gitignore, license eklemeyin** (zaten var)

### Adım 8: Remote ekle ve Push yap
GitHub'da repo oluşturduktan sonra, aşağıdaki komutta **KULLANICIADI** ve **REPOADI** kısımlarını değiştirin:

```powershell
# Örnek: git remote add origin https://github.com/sergen67/TDVWEB.git
git remote add origin https://github.com/KULLANICIADI/REPOADI.git
git push -u origin main
```

---

## 🔧 Sorun Giderme

### Eğer "secret detected" hatası alırsanız:

1. **Hassas dosyaları kontrol edin:**
```powershell
# firebase-admin.json dosyasını kontrol et
Get-ChildItem -Recurse -Filter "firebase-admin.json" | Select-Object FullName
```

2. **Dosyayı .gitignore'a ekleyin** (zaten ekli olmalı)

3. **Commit geçmişinden temizleyin:**
```powershell
# Git filter-repo yükle (ilk kez kullanıyorsanız)
pip install git-filter-repo

# Hassas dosyayı geçmişten sil
git filter-repo --path firebase-admin.json --invert-paths

# Veya manuel olarak:
git rm --cached YurtBursu.Api/firebase-admin.json
git commit -m "Remove sensitive files"
```

4. **Force push yapın:**
```powershell
git push -u origin main --force
```

---

## ✅ Tüm Komutları Tek Seferde Çalıştırma

Aşağıdaki komutları **KULLANICIADI** ve **REPOADI** kısımlarını değiştirerek çalıştırabilirsiniz:

```powershell
# 1. .git klasörünü sil
if (Test-Path .git) { Remove-Item -Recurse -Force .git }

# 2. Git init
git init

# 3. Dosyaları ekle
git add .

# 4. Commit
git commit -m "Initial clean commit"

# 5. Branch'i main yap
git branch -M main

# 6. Remote ekle (KULLANICIADI ve REPOADI değiştirin!)
git remote add origin https://github.com/KULLANICIADI/REPOADI.git

# 7. Push
git push -u origin main
```

---

## 📝 Notlar

- `.gitignore` dosyası hassas dosyaları (firebase-admin.json, appsettings.Development.json, vb.) otomatik olarak hariç tutar
- `node_modules/`, `bin/`, `obj/`, `dist/` gibi klasörler commit edilmez
- İlk push'tan sonra GitHub'da dosyalarınızı görebilirsiniz

