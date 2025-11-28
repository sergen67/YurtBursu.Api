# Yurt Bursu Hesaplama Projesi

Bu proje, yurt bursu hesaplama ve yönetim sistemi için geliştirilmiş full-stack bir uygulamadır.

## Proje Yapısı

- **YurtBursu.Api**: ASP.NET Core 8.0 Web API (Backend)
- **YurtBursu.Web**: React + TypeScript + Vite (Frontend)

## Teknolojiler

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- Firebase Cloud Messaging (Notifications)
- Swagger/OpenAPI

### Frontend
- React 18
- TypeScript
- Vite
- React Router
- Axios
- Supabase Storage

## Kurulum

### Backend (YurtBursu.Api)

1. SQL Server'ı başlatın
2. `appsettings.json` dosyasında connection string'i güncelleyin
3. Migration'ları çalıştırın:
   ```bash
   dotnet ef database update
   ```
4. Projeyi çalıştırın:
   ```bash
   dotnet run
   ```

### Frontend (YurtBursu.Web)

1. Bağımlılıkları yükleyin:
   ```bash
   npm install
   ```
2. Development server'ı başlatın:
   ```bash
   npm run dev
   ```

## Deployment

### Frontend (Firebase Hosting)
```bash
cd YurtBursu.Web
npm run build
firebase deploy --only hosting
```

## Environment Variables

### Frontend
- `VITE_API_BASE_URL`: Backend API base URL (opsiyonel)

### Backend
- Connection string `appsettings.json` içinde yapılandırılır

## Lisans

Bu proje özel bir projedir.

