# Documentación de Arquitectura - Sistema de Asistencias Bigtree

## Resumen Ejecutivo

El sistema **Bigtree Asistencias Backend** es una solución serverless construida en AWS que gestiona servicios de asistencia al viajero. La arquitectura está implementada usando AWS CDK (Cloud Development Kit) con TypeScript y sigue un patrón de microservicios distribuidos.

## Stack Tecnológico

### Infraestructura como Código
- **AWS CDK v2** con TypeScript
- **Node.js 22** como runtime principal
- **AWS CloudFormation** para el despliegue de recursos

### Lenguajes y Frameworks
- **C# .NET 8** - Servicios principales (Lambda Functions)
- **Python 3.13** - Funciones auxiliares y procesamiento de datos
- **ASP.NET Core** - Framework web para APIs REST

### Servicios AWS Utilizados
- **AWS Lambda** - Funciones serverless
- **Amazon API Gateway** - Gateway de APIs REST
- **Amazon RDS MySQL 8.0** - Base de datos relacional principal
- **Amazon DynamoDB** - Base de datos NoSQL para datos no estructurados
- **Amazon S3** - Almacenamiento de archivos y documentos
- **Amazon SQS** - Colas de mensajes para procesamiento asíncrono
- **Amazon Cognito** - Autenticación y autorización
- **AWS Step Functions** - Orquestación de workflows
- **Amazon VPC** - Red privada virtual
- **AWS CodePipeline** - CI/CD automatizado
- **AWS WAF** - Firewall de aplicaciones web

## Arquitectura de Red

### VPC Configuration
```
CIDR: 10.9.0.0/16
Availability Zones: 3
Subnets:
  - Public Subnets: /19 CIDR mask
  - Private Subnets: /19 CIDR mask (with NAT Gateway)
NAT Gateways: 0 (configurado para optimización de costos)
```

### Security Groups
- **Database Security Group**: Permite acceso MySQL/Aurora desde Lambda SG
- **Lambda Security Group**: Permite tráfico saliente completo
- **Cross-SG Communication**: Configurado para comunicación interna

## Base de Datos

### Amazon RDS MySQL 8.0
```yaml
Engine: MySQL 8.0
Instance Type: t4g.micro
Multi-AZ: false
Storage: Encrypted (S3 Managed)
Backup: Automated
Access: Private subnets only
Connection: AWS Secrets Manager
```

### Amazon DynamoDB Tables
1. **AssistancesLoggerDB** - Logs de asistencias
2. **CountriesAndCities** - Datos geográficos
3. **EventDraft** - Borradores de eventos
4. **IssuanceMok** - Datos de emisión MOK
5. **PhoneConsultations** - Consultas telefónicas
6. **ProviderData** - Información de proveedores (con GSI)

## Servicios Lambda y APIs

### 1. MasterRdsServices
**Propósito**: Servicios maestros y catálogos
**Runtime**: .NET 8 / C#
**Endpoints**:
- `/v1/masters/categories` - Categorías y asistencias
- `/v1/masters/event-statuses` - Estados de eventos
- `/v1/masters/eventprovider-statuses` - Estados de proveedores
- `/v1/masters/voucher-statuses` - Estados de vouchers
- `/v1/masters/countries` - Países y ciudades

### 2. EventServices
**Propósito**: Gestión de eventos de asistencia
**Runtime**: .NET 8 / C#
**Endpoints**:
- `/v1/assistances/events/draft` - Borradores de eventos
- `/v1/assistances/events/{id}/statuses` - Cambio de estados
- `/v1/assistances/events/providers/{id}` - Proveedores de eventos
- `/v1/assistances/notes` - Notas y logs
- `/v1/assistances/phone-consultations` - Consultas telefónicas

### 3. ProviderService
**Propósito**: Gestión de proveedores de servicios
**Runtime**: .NET 8 / C#
**Endpoints**:
- `/v1/providers/searchs` - Búsqueda de proveedores
- `/v1/providers/{id}/locations/search` - Ubicaciones por proveedor
- `/v1/providers/{id}/paymentmethods/{idpayment}` - Métodos de pago

### 4. IssuanceMokServices
**Propósito**: Servicios de emisión MOK
**Runtime**: .NET 8 / C#
**Integración**: S3 buckets para documentos

### 5. TrackingMokServices
**Propósito**: Seguimiento de servicios MOK
**Runtime**: .NET 8 / C#

### 6. PhoneConsultationService
**Propósito**: Gestión de consultas telefónicas
**Runtime**: .NET 8 / C#
**Integración**: S3 para almacenamiento de archivos

### 7. VoucherService
**Propósito**: Gestión de vouchers y comprobantes
**Runtime**: .NET 8 / C#

### 8. EventStatusSwitchTempServices
**Propósito**: Cambios temporales de estado de eventos
**Runtime**: .NET 8 / C#

### 9. Python Lambda Functions

#### AssistancesLoggerWriter
**Propósito**: Escritura de logs de asistencias
**Runtime**: Python 3.13
**Trigger**: SQS Queue
**Integración**: DynamoDB, Step Functions

#### ListUsers
**Propósito**: Listado de usuarios
**Runtime**: Python 3.13
**Integración**: Cognito User Pool

#### TrackingMokLambda
**Propósito**: Procesamiento de tracking MOK
**Runtime**: Python 3.13
**Acciones**:
- EVENT_PROVIDER_CREATE
- EVENT_PROVIDER_START  
- EVENT_PROVIDER_RESOLVE
- NOTE_REGISTER

#### SnsNotification
**Propósito**: Envío de notificaciones
**Runtime**: Python 3.13
**Integración**: Amazon SNS

## Almacenamiento S3

### Buckets Configurados
1. **file-bucket** - Archivos generales
2. **datasourcemaster-bucket** - Fuentes de datos maestros
3. **assitsbucket** - Documentos de asistencias
4. **issuance-mok-docs-bucket** - Documentos MOK

**Configuración de Seguridad**:
- Encriptación: S3 Managed
- Acceso público: Bloqueado
- SSL/TLS: Obligatorio
- Política de eliminación: DESTROY (para entornos no productivos)

## Sistema de Colas (SQS)

### AssistancesLoggerSQS
```yaml
Queue Name: AssistancesLoggerSQS
Retention Period: 4 días
Visibility Timeout: 5 minutos
Encryption: KMS Managed
Batch Size: 1 mensaje
Consumer: AssistancesLoggerWriter Lambda
```

## Autenticación y Autorización (Cognito)

### User Pool Configuration
```yaml
Pool Name: solution-assist-cognito-pool
Sign-in: Email
Auto Verify: Email
Self Sign-up: Enabled
Feature Plan: LITE
Domain: solution-assist-{stage}
```

### User Groups
- **assistances-admin** - Administradores de asistencias
- **assistances-tmo** - Usuarios TMO

### OAuth2 Clients
1. **Kom-client** - Cliente para servicios KOM
2. **solution-assist-client** - Cliente principal
3. **frontend-assistences** - Cliente frontend

### Resource Servers
- **assistences-services** - Servicios de asistencias
- **kom-services** - Servicios KOM

**Scopes**: read, write

## API Gateway

### Configuración Principal
```yaml
API Name: MainApiTest
Description: API for serverless project
Rate Limiting: 10 req/sec
Burst Limit: 20 requests
Monthly Quota: 10,000 requests
```

### Autenticación
- **Lambda Authorizer** personalizado
- **API Keys** para clientes externos
- **Usage Plans** con throttling

## Step Functions (State Machines)

### trackingMokExpressStateMachine
**Tipo**: STANDARD
**Propósito**: Orquestación de procesos MOK

**Estados**:
1. **Choice State**: Verificación de sourceCode = "MOK"
2. **TrackingMok**: Procesamiento de tracking
3. **SnsNotification**: Envío de notificaciones
4. **Success/Error Handling**: Manejo de resultados

## CI/CD Pipeline

### CodePipeline Configuration
```yaml
Repository: bigtree-asistencias-backend
Owner: TERRAWIND-DEVELOP
Branches:
  - develop (dev)
  - qa (qa)  
  - main (prod)
Build Tool: AWS CodeBuild
Runtime: Node.js 22
```

### Deployment Strategy
**Orden de Despliegue**:
1. NetworkingStack
2. DatabaseStack  
3. CognitoStack
4. WafStack
5. S3Stack
6. DynamoStack
7. LambdaLayersStack
8. SqsStack
9. Individual Lambda Stacks
10. StatemachineStack
11. MainStack

### Build Optimization
- **Detección de cambios**: Git diff para despliegues selectivos
- **Caching**: node_modules y dependencias .NET
- **Artifacts**: CDK outputs y compilados

## Configuración por Ambiente

### Environments
- **dev**: us-east-1 (Account: 590183946089)
- **qa**: us-east-2 (Account: 625039860988)  
- **prod**: us-east-2 (Account: 625039860988)

### Variables por Ambiente
```typescript
VPC CIDR: 10.9.0.0/16 (todos los ambientes)
Database Access: Private (todos los ambientes)
Layer Versions: dev=1, qa=4, prod=1
```

## Seguridad

### WAF (Web Application Firewall)
- Protección contra ataques comunes
- Rate limiting por IP
- Geo-blocking configurable

### Encryption
- **RDS**: Encriptación en reposo
- **S3**: S3 Managed Encryption + SSL/TLS
- **SQS**: KMS Managed Encryption
- **DynamoDB**: Encriptación por defecto

### Network Security
- **VPC**: Red privada aislada
- **Security Groups**: Acceso restrictivo por servicio
- **Private Subnets**: Lambdas sin acceso directo a internet
- **Secrets Manager**: Credenciales de base de datos

## Monitoreo y Logging

### CloudWatch Integration
- **Lambda Logs**: Automático para todas las funciones
- **Step Functions**: Logging completo habilitado
- **API Gateway**: Logs de acceso y errores

### Tagging Strategy
```yaml
Environment: {stage}
Author: {partner}
Client: {client}
Stage: {stage.toUpperCase()}
Partner: {partner}
Project: {project}
```

## Flujos de Datos Principales

### 1. Flujo de Asistencia Estándar
```
Cliente → API Gateway → EventServices → RDS → Response
```

### 2. Flujo de Logging Asíncrono
```
Evento → SQS → AssistancesLoggerWriter → DynamoDB
```

### 3. Flujo MOK con State Machine
```
Evento MOK → Step Function → TrackingMok → SNS Notification
```

### 4. Flujo de Búsqueda de Proveedores
```
Cliente → API Gateway → ProviderService → DynamoDB (GSI) → Response
```

## Consideraciones de Escalabilidad

### Auto-scaling
- **Lambda**: Concurrencia automática
- **DynamoDB**: On-demand billing
- **RDS**: Instancia única (escalable verticalmente)

### Performance Optimization
- **Lambda Layers**: Reutilización de dependencias
- **VPC**: Configuración optimizada para cold starts
- **DynamoDB GSI**: Índices optimizados para consultas

### Cost Optimization
- **NAT Gateway**: Deshabilitado (0 gateways)
- **RDS**: Instancia t4g.micro
- **Lambda**: Pay-per-use model
- **S3**: Lifecycle policies (recomendado)
