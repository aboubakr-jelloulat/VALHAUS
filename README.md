# VALHÄUS

A full-stack e commerce platform built with ASP.NET Core MVC. Features secure payment processing through Stripe, Google OAuth authentication, and role-based authorization for admins and customers. Uses Entity Framework Core with SQL Server for development and PostgreSQL for production deployments.

## Setup

**Windows:**
```powershell
.\setup-windows.ps1
```

**Linux:**
```bash
chmod +x setup-linux.sh
sudo ./setup-linux.sh
```

## Running the App

**Windows:**
```powershell
dotnet run --project VALHÄUS\VALHAUS.csproj
```

**Linux:**
```bash
dotnet run --project VALHÄUS/VALHAUS.csproj
```

Open http://localhost:8080

