
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

##  Technology Stack

- **.NET 8.0** - Framework
- **PostgreSQL** - Production database (Railway)
- **SQL Server** - Local development database
- **Docker** - Containerization
- **Render** - Hosting platform
- **Railway** - PostgreSQL database hosting

##  Environment Variables

Required environment variables for production:

```env
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__constr=<railway-postgresql-url>
Stripe__Secretkey=<your-stripe-secret-key>
Stripe__Publishablekey=<your-stripe-publishable-key>
Authentication__Google__ClientId=<your-google-client-id>
Authentication__Google__ClientSecret=<your-google-client-secret>
```

im use : 


dotnet run --project VALHÄUS\VALHAUS.csproj 

http://localhost:8080!