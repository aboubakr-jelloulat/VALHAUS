# Multi-stage Dockerfile for .NET 8 ASP.NET MVC Application
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY ["VALHÄUS.sln", "./"]
COPY ["VALHÄUS/VALHAUS.csproj", "VALHÄUS/"]
COPY ["Valhaus.Data/Valhaus.Data.csproj", "Valhaus.Data/"]
COPY ["Valhaus.Models/Valhaus.Models.csproj", "Valhaus.Models/"]
COPY ["Valhaus.Utils/Valhaus.Utils.csproj", "Valhaus.Utils/"]

# Restore dependencies for all projects
RUN dotnet restore "VALHÄUS/VALHAUS.csproj"

# Copy the entire source code
COPY . .

# Build the main web application
WORKDIR "/src/VALHÄUS"
RUN dotnet build "VALHAUS.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "VALHAUS.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Install PostgreSQL client tools (needed for migrations and troubleshooting)
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy published application from publish stage
COPY --from=publish /app/publish .

# Create a non-root user for security
RUN useradd -m -s /bin/bash appuser && chown -R appuser:appuser /app
USER appuser

# Expose port (Render uses PORT environment variable, defaults to 8080)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check (optional but recommended)
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl --fail http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "VALHAUS.dll"]
