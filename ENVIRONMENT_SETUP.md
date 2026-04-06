# Environment Configuration Guide

## Development Environment (.env.development)

Create `.env.development` file in the project root:

```
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.rekpxjkktrkypubqtssj;Password=C7zqw!PD7vSM/q/;SSL Mode=Require;Trust Server Certificate=true
Jwt__Secret=dev-super-secret-key-min-32-characters-required-for-HS256-development
Jwt__Issuer=SchoolManagementAPI
Jwt__Audience=SchoolManagementApp
Jwt__ExpiryHours=24
```

## Production Environment (.env.production)

Create `.env.production` file:

```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.rekpxjkktrkypubqtssj;Password=C7zqw!PD7vSM/q/;SSL Mode=Require;Trust Server Certificate=true
Jwt__Secret=prod-super-secret-key-min-32-characters-required-for-HS256-production
Jwt__Issuer=SchoolManagementAPI
Jwt__Audience=SchoolManagementApp
Jwt__ExpiryHours=24
```

## Using User Secrets (Recommended for Development)

Initialize user secrets:

```bash
dotnet user-secrets init
```

Set secret values:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.rekpxjkktrkypubqtssj;Password=C7zqw!PD7vSM/q/;SSL Mode=Require;Trust Server Certificate=true"
dotnet user-secrets set "Jwt:Secret" "your-super-secret-key-min-32-characters-required-for-HS256"
dotnet user-secrets set "Jwt:Issuer" "SchoolManagementAPI"
dotnet user-secrets set "Jwt:Audience" "SchoolManagementApp"
dotnet user-secrets set "Jwt:ExpiryHours" "24"
```

View all secrets:

```bash
dotnet user-secrets list
```

## Docker Environment

Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15-alpine
    container_name: schoolmanagement_db
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: SchoolManagement
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  api:
    image: schoolmanagement_api:latest
    container_name: schoolmanagement_api
    build: .
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      ConnectionStrings__DefaultConnection: Host=aws-0-eu-west-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.rekpxjkktrkypubqtssj;Password=C7zqw!PD7vSM/q/;SSL Mode=Require;Trust Server Certificate=true
      Jwt__Secret: your-super-secret-key-min-32-characters-required
      Jwt__Issuer: SchoolManagementAPI
      Jwt__Audience: SchoolManagementApp
      Jwt__ExpiryHours: 24
    ports:
      - "5000:80"
      - "5001:443"
    depends_on:
      - postgres

volumes:
  postgres_data:
```

Run with Docker:

```bash
docker-compose up
```

## CI/CD Environment Variables

For GitHub Actions, set secrets in repository settings:

```
ConnectionStrings__DefaultConnection
Jwt__Secret
Jwt__Issuer
Jwt__Audience
Jwt__ExpiryHours
DATABASE_PASSWORD
REGISTRY_USERNAME
REGISTRY_PASSWORD
```

## Key Security Practices

1. **Never commit secrets** to version control
2. **Use strong JWT secrets** (minimum 32 characters)
3. **Use environment-specific configurations**
4. **Rotate secrets regularly**
5. **Use HTTPS in production**
6. **Enable CORS for specific domains only**
7. **Implement rate limiting**
8. **Use API keys for external integrations**

## Configuration Priority (Highest to Lowest)

1. User Secrets (when running locally)
2. Environment Variables
3. appsettings.{Environment}.json
4. appsettings.json
5. Default values in code

## Deployment Checklist

- [ ] Update connection string for production database
- [ ] Set strong JWT secret (32+ characters)
- [ ] Enable HTTPS only
- [ ] Disable detailed error messages (set ErrorDetails to "Generic")
- [ ] Set logging level to Warning or Error
- [ ] Enable CORS for specific domains
- [ ] Configure backup and disaster recovery
- [ ] Set up monitoring and alerting
- [ ] Test migrations on staging first
- [ ] Set up CI/CD pipeline

## Azure Configuration

For Azure App Service:

1. Create Application Settings in Azure Portal
2. Add configuration values:
   - ConnectionStrings__DefaultConnection
   - Jwt__Secret
   - Jwt__Issuer
   - Jwt__Audience
   - ASPNETCORE_ENVIRONMENT=Production

3. Deploy using Azure CLI or GitHub Actions

```bash
az webapp up --name SchoolManagementAPI --sku B1
```

## AWS Configuration

For AWS Elastic Beanstalk:

Create `.ebextensions/config.yml`:

```yaml
option_settings:
  aws:elasticbeanstalk:application:environment:
    ASPNETCORE_ENVIRONMENT: Production
    ConnectionStrings__DefaultConnection: ...
    Jwt__Secret: ...
```

Deploy:

```bash
eb deploy
```
