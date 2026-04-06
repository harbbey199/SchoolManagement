# Database Setup Guide

## Prerequisites

- PostgreSQL 12 or higher
- .NET 8 SDK
- Entity Framework Core CLI (optional)

## Step 1: Create Database

### Using PostgreSQL CLI

```bash
psql -U postgres
```

```sql
CREATE DATABASE SchoolManagement;
```

### Using pgAdmin

1. Right-click on "Databases"
2. Select "Create" → "Database"
3. Enter name: `SchoolManagement`
4. Click "Create"

## Step 2: Update Connection String

Edit `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=SchoolManagement;Username=postgres;Password=your_password"
}
```

## Step 3: Install Entity Framework Core CLI (Optional)

```bash
dotnet tool install --global dotnet-ef
```

## Step 4: Create Initial Migration

```bash
dotnet ef migrations add InitialCreate
```

This will create:
- `Migrations/` folder
- Migration file with timestamp

## Step 5: Apply Migration

```bash
dotnet ef database update
```

## Step 6: Verify Database

Connect to PostgreSQL and verify tables:

```sql
\c SchoolManagement
\dt
```

You should see these tables:
- Users
- Students
- Parents
- StudentParents
- Attendances
- Payments
- Grades
- Reports

## Common Commands

### Create a new migration after schema changes

```bash
# After modifying entities
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Remove last migration

```bash
dotnet ef migrations remove
```

### Repeat last update

```bash
dotnet ef database update
```

### List all migrations

```bash
dotnet ef migrations list
```

### Generate SQL script

```bash
dotnet ef migrations script > migration.sql
```

## Resetting Database (Development Only)

```bash
# Drop database
dotnet ef database drop

# Recreate and apply migrations
dotnet ef database update
```

## Seed Initial Data

Create a seeding service to add initial data:

```csharp
// In Program.cs, add after creating the app:
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Add seed data here
    await db.SaveChangesAsync();
}
```

## Troubleshooting

### Connection refused
- Verify PostgreSQL is running
- Check connection string
- Verify database exists

### Migration failed
```bash
# Check migration status
dotnet ef migrations list

# Roll back to specific migration
dotnet ef database update MigrationName
```

### Permission denied
```sql
-- Grant permissions
GRANT ALL PRIVILEGES ON DATABASE SchoolManagement TO postgres;
```

## Production Deployment

1. Use managed database service (AWS RDS, Azure Database)
2. Create backup before migrations
3. Test migrations on staging first
4. Use connection pooling
5. Enable encryption

## Additional Resources

- [EF Core Migrations Documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
