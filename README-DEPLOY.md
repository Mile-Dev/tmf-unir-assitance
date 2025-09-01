# CDK + GitHub Actions (OIDC) for .NET 8 Lambdas

This repo contains an `infra/` CDK TypeScript app that builds and deploys three .NET 8 Lambda functions:
EventServices, PhoneConsultationService, and ProviderService.

## Prereqs (one-time)
1) Create an IAM role in AWS with a trust to GitHub OIDC and permissions for CDK deploy.
   - Save its ARN as the GitHub secret `AWS_ROLE_TO_ASSUME` in your repository.
2) Bootstrap CDK in the target account/region (once):
   ```bash
   npm --prefix infra ci
   npx cdk bootstrap aws://<ACCOUNT_ID>/us-east-2
   ```

## Branch to environment mapping
- `dev` branch -> `env=dev`
- `qa` branch -> `env=qa`
- `main` branch -> `env=prod`

## Paths expected
Put your services at repo root:
- EventServices/EventServices.csproj
- PhoneConsultationService/PhoneConsultationService.csproj
- ProviderService/ProviderService.csproj

## Config loading convention (in code)
Each Lambda receives:
- `ASPNETCORE_ENVIRONMENT` (DEV/QA/PROD)
- `CONFIG_PREFIX` (e.g., `/tw/dev/EventServices/`)

In your .NET Program.cs, load config from SSM and Secrets Manager using the prefix above.

## Deploy
Push to `dev`, `qa`, or `main` and the workflow deploys automatically.
