# School Management System - ASP.NET Core Web API

A production-ready ASP.NET Core 8 Web API for managing school operations with clean architecture principles.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control (Admin, Parent, Student, Teacher)
- **Student Management**: CRUD operations for students with parent assignments
- **Attendance Tracking**: Mark and track student attendance with date range queries
- **Payment Management**: Record and track student payments
- **Grading System**: Record grades with automatic grade calculation
- **Report Generation**: Comprehensive student performance reports
- **Pagination & Filtering**: Built-in support for paginated responses
- **Validation**: FluentValidation for all request validation
- **Error Handling**: Global exception handling middleware
- **Logging**: Integrated Serilog for comprehensive logging
- **API Documentation**: Swagger/OpenAPI with JWT support
- **Security**: Password hashing, JWT tokens, HTTPS support

## Architecture

The project follows clean/layered architecture principles:

```
SchoolManagement/
├── Controllers/           # API endpoints
├── Services/              # Business logic
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/          # Data access layer
│   ├── Interfaces/
│   └── Implementations/
├── Models/Entities/       # Domain entities
├── DTOs/                  # Request/Response models
│   ├── Request/
│   └── Response/
├── Data/                  # DbContext
├── Middleware/            # Custom middleware
├── Validators/            # FluentValidation rules
├── Helpers/               # Utility classes
├── Tests/                 # Unit tests
└── Program.cs             # Application startup
```

## Tech Stack

- **.NET 8** - Latest .NET version
- **ASP.NET Core Web API** - RESTful API framework
- **Entity Framework Core 8** - ORM for data access
- **PostgreSQL** - Relational database
- **JWT** - Token-based authentication
- **FluentValidation** - Input validation
- **Serilog** - Logging framework
- **Swagger/OpenAPI** - API documentation
- **BCrypt.Net** - Password hashing
- **xUnit & Moq** - Unit testing

## Prerequisites

- .NET 8 SDK ([Download](https://dotnet.microsoft.com/download/dotnet/8.0))
- PostgreSQL 12 or higher ([Download](https://www.postgresql.org/download/))
- Visual Studio Code, Visual Studio, or JetBrains Rider (Optional)

## Getting Started

### 1. Clone or Extract the Project

```bash
cd School Management
```

### 2. Configure Database Connection

Edit `appsettings.json` and update the PostgreSQL connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SchoolManagement;Username=postgres;Password=your_password"
}
```

### 3. Create Database

Open PostgreSQL and create the database:

```sql
CREATE DATABASE SchoolManagement;
```

### 4. Apply Database Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Install Dependencies

```bash
dotnet restore
```

### 6. Update JWT Settings

Edit `appsettings.json` and update the JWT secret key (minimum 32 characters):

```json
"Jwt": {
  "Secret": "your-super-secret-key-min-32-characters-required-for-HS256",
  "Issuer": "SchoolManagementAPI",
  "Audience": "SchoolManagementApp",
  "ExpiryHours": "24"
}
```

### 7. Build the Project

```bash
dotnet build
```

### 8. Run the Application

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

## API Documentation

Access the Swagger documentation at:
- **Development**: `https://localhost:5001/` or `http://localhost:5000/`
- **Production**: `https://your-api-domain.com/`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login with email and password

### Students
- `GET /api/students` - Get all students (paginated)
- `GET /api/students/{id}` - Get student details
- `GET /api/students/grade/{grade}` - Get students by grade
- `POST /api/students` - Create new student (Admin only)
- `PUT /api/students/{id}` - Update student (Admin only)
- `DELETE /api/students/{id}` - Delete student (Admin only)
- `POST /api/students/{studentId}/parents` - Assign parent to student (Admin only)

### Attendance
- `POST /api/attendance/mark` - Mark attendance (Admin/Teacher only)
- `POST /api/attendance/mark-multiple` - Mark multiple attendances (Admin/Teacher only)
- `GET /api/attendance/student/{studentId}` - Get student attendance
- `GET /api/attendance/student/{studentId}/range` - Get attendance by date range
- `GET /api/attendance/student/{studentId}/summary` - Get attendance summary

### Payments
- `POST /api/payments/record` - Record payment (Admin only)
- `GET /api/payments/student/{studentId}` - Get payment history
- `PUT /api/payments/{paymentId}/status` - Update payment status (Admin only)

### Grades
- `POST /api/grades/record` - Record grade (Admin/Teacher only)
- `GET /api/grades/student/{studentId}` - Get student grades
- `GET /api/grades/student/{studentId}/term/{term}` - Get grades by term
- `PUT /api/grades/{gradeId}` - Update grade (Admin/Teacher only)

### Reports
- `POST /api/reports/generate` - Generate student report (Admin/Teacher only)
- `GET /api/reports/student/{studentId}` - Get student reports
- `GET /api/reports/{reportId}` - Get specific report

## Sample Requests

### Register User
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "student@school.com",
    "password": "Password123!",
    "confirmPassword": "Password123!",
    "firstName": "John",
    "lastName": "Doe",
    "role": 2
  }'
```

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "student@school.com",
    "password": "Password123!"
  }'
```

### Get Students (Paginated)
```bash
curl -X GET "http://localhost:5000/api/students?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### Mark Attendance
```bash
curl -X POST http://localhost:5000/api/attendance/mark \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
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
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "studentId": 1,
    "amount": 500.00,
    "description": "Monthly tuition fee",
    "transactionId": "TXN123456"
  }'
```

### Record Grade
```bash
curl -X POST http://localhost:5000/api/grades/record \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "studentId": 1,
    "subject": "Mathematics",
    "marks": 85,
    "maxMarks": 100,
    "term": "Term1"
  }'
```

### Generate Report
```bash
curl -X POST "http://localhost:5000/api/reports/generate?studentId=1&term=Term1" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Configuration

### JWT Settings
Configure JWT in `appsettings.json`:

```json
"Jwt": {
  "Secret": "your-secret-key-at-least-32-characters-long",
  "Issuer": "SchoolManagementAPI",
  "Audience": "SchoolManagementApp",
  "ExpiryHours": "24"
}
```

### Database
PostgreSQL connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SchoolManagement;Username=postgres;Password=your_password"
}
```

### CORS
CORS is configured to allow all origins in development. Modify in `Program.cs` for production.

## Database Migrations

Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

Apply pending migrations:
```bash
dotnet ef database update
```

Remove last migration:
```bash
dotnet ef migrations remove
```

## Running Tests

Run unit tests:
```bash
dotnet test
```

Run specific test:
```bash
dotnet test --filter MethodName
```

## Project Structure Details

### Entities
Located in `Models/Entities/`:
- **User**: Base user entity with roles
- **Student**: Student information linked to User
- **Parent**: Parent information linked to User
- **StudentParent**: Junction table for student-parent relationships
- **Attendance**: Attendance records with status
- **Payment**: Payment transaction records
- **Grade**: Student grades by subject
- **Report**: Generated performance reports

### DTOs
- **Request**: API request models
- **Response**: API response models
- **ApiResponse<T>**: Standard response wrapper for all endpoints

### Services
Business logic layer providing:
- Authentication and JWT token generation
- Student CRUD operations
- Attendance management
- Payment tracking
- Grade recording and calculation
- Report generation

### Repositories
Data access layer providing:
- Generic repository pattern
- Specialized repositories for each entity
- Query methods with pagination and filtering

## Best Practices Implemented

✅ **Clean Architecture** - Separation of concerns with clear layers
✅ **SOLID Principles** - Single responsibility, dependency injection
✅ **Async/Await** - All database operations are async
✅ **Error Handling** - Global exception handling middleware
✅ **Validation** - FluentValidation for input validation
✅ **Logging** - Serilog for comprehensive logging
✅ **Security** - JWT authentication, password hashing, role-based authorization
✅ **Documentation** - XML comments for Swagger
✅ **Testing** - xUnit and Moq for unit testing
✅ **Database Design** - Proper relationships and indexes
✅ **Pagination** - Built-in pagination support
✅ **Pagination** - Built-in pagination support

## Security Considerations

1. **Password Security**: Passwords are hashed using BCrypt.Net
2. **JWT Tokens**: Secure token generation with expiry
3. **Role-Based Access**: Different endpoints require specific roles
4. **HTTPS**: Enforce HTTPS in production
5. **Environment Variables**: Use user secrets for sensitive data

For production, set sensitive configuration via environment variables:

```bash
dotnet user-secrets set "Jwt:Secret" "your-production-secret-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-production-connection-string"
```

## Troubleshooting

### Migration Issues
```bash
# Reset database (Development only)
dotnet ef database drop
dotnet ef database update
```

### Port Already in Use
```bash
# Run on different port
dotnet run --urls "http://localhost:5002"
```

### Connection String Issues
- Verify PostgreSQL is running
- Check connection string in `appsettings.json`
- Ensure database exists
- Verify username and password

## Production Deployment

1. Set environment to Production
2. Update connection strings with production database
3. Update JWT secret with strong key
4. Enable HTTPS enforcement
5. Configure CORS for specific domains
6. Set up proper logging and monitoring
7. Use managed PostgreSQL database
8. Implement CI/CD pipeline
9. Add health checks
10. Configure backup and disaster recovery

## Contributing

For improvements:
1. Follow the existing code structure
2. Maintain clean architecture principles
3. Add tests for new features
4. Update documentation
5. Use meaningful commit messages

## License

This project is provided as-is for educational and commercial use.

## Support

For issues or questions, refer to:
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Documentation](https://docs.microsoft.com/ef/core)
- [PostgreSQL Documentation](https://www.postgresql.org/docs)

---

**Built with ❤️ using ASP.NET Core 8**
