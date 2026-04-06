# Quick Start Guide

Get the API running in 5 minutes!

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)
- Git
- A code editor (VS Code, Visual Studio, or Rider)

## Quick Setup

### 1. Create PostgreSQL Database

```bash
# On Windows/macOS using PostgreSQL CLI
psql -U postgres -c "CREATE DATABASE SchoolManagement;"

# On Linux
sudo -u postgres psql -c "CREATE DATABASE SchoolManagement;"
```

### 2. Clone/Extract Project

```bash
cd "School Management"
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Update Connection String

Edit `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SchoolManagement;Username=postgres;Password=**YOUR_PASSWORD**"
}
```

### 5. Run Migrations

```bash
dotnet ef database update
```

### 6. Start the API

```bash
dotnet run
```

**The API is now running at:**
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger: `http://localhost:5000/` or `https://localhost:5001/`

## First Test

### Register a User

```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@test.com",
    "password": "Admin123!",
    "confirmPassword": "Admin123!",
    "firstName": "Admin",
    "lastName": "User",
    "role": 0
  }'
```

### Login

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@test.com",
    "password": "Admin123!"
  }'
```

**Save the token from the response** 

### Get Students

```bash
curl -X GET http://localhost:5000/api/students \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Using Swagger UI

1. Open `http://localhost:5000` in your browser
2. Click "Authorize" button
3. Paste your JWT token
4. Test endpoints directly

## Common Tasks

### Create a Student

```bash
# Get your token first (see Login above)

curl -X POST http://localhost:5000/api/students \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "email": "student@test.com",
    "firstName": "John",
    "lastName": "Doe",
    "password": "Student123!",
    "rollNumber": "STU001",
    "grade": "10A",
    "section": "A",
    "dateOfBirth": "2008-05-15",
    "address": "123 Main St"
  }'
```

### Mark Attendance

```bash
curl -X POST http://localhost:5000/api/attendance/mark \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "studentId": 1,
    "attendanceDate": "2024-01-15",
    "status": 0,
    "remarks": "Present"
  }'
```

### Record Payment

```bash
curl -X POST http://localhost:5000/api/payments/record \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "studentId": 1,
    "amount": 500.00,
    "description": "Monthly Fee",
    "transactionId": "TXN001"
  }'
```

### Record Grade

```bash
curl -X POST http://localhost:5000/api/grades/record \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "studentId": 1,
    "subject": "Mathematics",
    "marks": 85,
    "maxMarks": 100,
    "term": "Term 1"
  }'
```

### Generate Report

```bash
curl -X POST "http://localhost:5000/api/reports/generate?studentId=1&term=Term 1" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

## Database Management

### Reset Database (Development)

```bash
# Drop database
dotnet ef database drop

# Recreate with migrations
dotnet ef database update
```

### View Database

```bash
# Connect using pgAdmin or CLI
psql -U postgres -d SchoolManagement
```

## Stop the API

```
Press Ctrl+C in the terminal
```

## Next Steps

1. Read the full [README.md](README.md)
2. Check the [API Documentation](API_DOCUMENTATION.md)
3. Review the [Database Setup Guide](DATABASE_SETUP.md)
4. Explore the codebase structure
5. Run unit tests: `dotnet test`
6. Deploy to production (see deployment section in README)

## Troubleshooting

### "Connection refused"
- Ensure PostgreSQL is running
- Check connection string in `appsettings.json`
- Verify database exists: `psql -U postgres -l`

### "Port already in use"
```bash
dotnet run --urls "http://localhost:5002"
```

### "Migration failed"
```bash
dotnet ef database drop
dotnet ef database update
```

### "Cannot read swagger"
- Wait 3 seconds after `dotnet run`
- Try `http://localhost:5000` (without trailing slash)
- Check browser console for errors

## Support Resources

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [JWT Guide](https://jwt.io/)
- [REST API Best Practices](https://restfulapi.net/)

---

**You're all set! Happy coding! 🚀**
