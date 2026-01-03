# VALHÄUS - Docker Deployment Files

This directory contains production-ready Docker deployment configuration for the VALHÄUS .NET 8 MVC application.

## 📁 Files Overview

- **Dockerfile** - Multi-stage production Dockerfile for .NET 8
- **.dockerignore** - Excludes unnecessary files from Docker build
- **docker-compose.yml** - Local development and testing setup
- **DEPLOYMENT.md** - Complete deployment guide for Render + Railway

## 🚀 Quick Start

### Local Testing with Docker Compose

```bash
# Start the application with PostgreSQL database
docker-compose up --build

# Access the application
# http://localhost:8080

# Stop the application
docker-compose down
```

### Build Docker Image Manually

```bash
# Build the image
docker build -t valhaus:latest .

# Run the container
docker run -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Production valhaus:latest
```

## 📖 Full Deployment Guide

See **[DEPLOYMENT.md](./DEPLOYMENT.md)** for complete instructions on deploying to Render with Railway PostgreSQL.

## 🔧 Technology Stack

- **.NET 8.0** - Framework
- **PostgreSQL** - Production database (Railway)
- **SQL Server** - Local development database
- **Docker** - Containerization
- **Render** - Hosting platform
- **Railway** - PostgreSQL database hosting

## 🌍 Environment Variables

Required environment variables for production:

```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__constr=<railway-postgresql-url>
Stripe__Secretkey=<your-stripe-secret-key>
Stripe__Publishablekey=<your-stripe-publishable-key>
Authentication__Google__ClientId=<your-google-client-id>
Authentication__Google__ClientSecret=<your-google-client-secret>
```

## 📝 Notes

- The application uses **PostgreSQL in production** and **SQL Server for local development**
- HTTPS is handled by Render (SSL termination)
- Application listens on port 8080 inside Docker
- Health check endpoint available at `/health`

## 🆘 Support

If you encounter issues, check:
1. **DEPLOYMENT.md** - Troubleshooting section
2. Render logs - For runtime errors
3. Railway dashboard - For database issues

---

**Happy Deploying! 🎉**



 Running on Linux - Step by Step
Option 1: Direct .NET Runtime (No Docker)
If you have .NET 8 installed on Linux:

bash
# 1. Navigate to your project directory
cd /path/to/VALHÄUS
# 2. Restore dependencies
dotnet restore VALHÄUS.sln
# 3. Build the project
dotnet build VALHÄUS.sln --configuration Release
# 4. Run the application
dotnet run --project VALHÄUS/VALHAUS.csproj
Your app will be available at: http://localhost:8080

Option 2: Using Docker (Recommended for Production)
If you have Docker installed on Linux:

bash
# 1. Navigate to your project directory
cd /path/to/VALHÄUS
# 2. Build the Docker image
docker build -t valhaus:latest .
# 3. Run the Docker container
docker run -d -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DATABASE_URL="your-railway-postgresql-url" \
  --name valhaus-app \
  valhaus:latest
# 4. Check if it's running
docker ps
# 5. View logs (if needed)
docker logs valhaus-app
# 6. Stop the container (when done)
docker stop valhaus-app
# 7. Remove the container (when done)
docker rm valhaus-app
Your app will be available at: http://localhost:8080

Option 3: Docker Compose (Best for Development)
bash
# 1. Navigate to your project directory
cd /path/to/VALHÄUS
# 2. Start the app with PostgreSQL database
docker-compose up --build
# 3. Stop everything (Ctrl+C or in another terminal)
docker-compose down
Your app will be available at: http://localhost:8080

🚀 For Render Deployment (Automatic)
You don't run commands! Render does it automatically:

bash
# 1. Push to GitHub
git add .
git commit -m "Ready for deployment"
git push origin main
# 2. On Render.com dashboard:
# - Create Web Service
# - Connect GitHub repo
# - Select Docker runtime
# - Add environment variables
# - Click "Create Web Service"
# Render automatically runs:
# - docker build
# - docker run
# - Assigns public URL


##### im use : 


dotnet run --project VALHÄUS\VALHAUS.csproj 

http://localhost:8080!