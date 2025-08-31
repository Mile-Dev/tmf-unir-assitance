# Documentación de Flujos y Procesos - Sistema MOK Tracking

## Resumen Ejecutivo

Este documento describe los flujos y procesos implementados en el sistema de tracking MOK (Medical Operations Kernel), que gestiona la orquestación de servicios de asistencia médica a través de AWS Step Functions, Lambda Functions y integraciones con APIs externas.

## Arquitectura de Componentes

### 1. AWS Step Functions (State Machine)
**Archivo**: `@infra/lib/stack/statemachines/statemachineStack.ts`
**Nombre**: `trackingMokExpressStateMachine`
**Tipo**: STANDARD

### 2. TrackingMok Lambda
**Archivo**: `TrackingMokLambda/lambda_function.py`
**Runtime**: Python 3.13
**Propósito**: Procesamiento de operaciones MOK

### 3. SNS Notification Lambda
**Archivo**: `@infra/src/lambdas/sns_notification/lambda_function.py`
**Runtime**: Python 3.13
**Propósito**: Envío de notificaciones

## Flujo Principal del State Machine

### Diagrama de Estados
```
[Inicio] → [isMOK?] → [TrackingMok] → [SnsNotification] → [Fin]
                  ↓
            [HandleMissingSourceCode] → [Fin]
```

### Configuración del State Machine

```typescript
const state1_checkMok = new sfn.Choice(this, 'isMOK?')
  .when(sfn.Condition.stringEquals('$.logData.sourceCode', "MOK"), state1_trackingMok)
  .otherwise(new sfn.Pass(this, 'HandleMissingSourceCode'))
  .afterwards();

state1_trackingMok.next(state1_snsNotification)
```

### Estados Definidos

1. **Choice State (isMOK?)**
   - **Condición**: `$.logData.sourceCode == "MOK"`
   - **Si es verdadero**: Ejecuta TrackingMok Lambda
   - **Si es falso**: Ejecuta HandleMissingSourceCode (Pass state)

2. **TrackingMok State**
   - **Tipo**: LambdaInvoke
   - **Función**: `trackingMokServicesLambda`
   - **Output Path**: `$.Payload`

3. **SnsNotification State**
   - **Tipo**: LambdaInvoke
   - **Función**: `SnsNotificationLambda`
   - **Output Path**: `$.Payload`

## TrackingMok Lambda - Procesamiento Principal

### Estructura de Entrada
```json
{
  "logData": {
    "action": "EVENT_PROVIDER_CREATE|EVENT_PROVIDER_START|EVENT_PROVIDER_RESOLVE|NOTE_REGISTER",
    "eventId": 12345,
    "eventCode": "EVT001",
    "description": "Descripción de la acción"
  },
  "PK": "partition_key",
  "SK": "sort_key"
}
```

### Acciones Soportadas

#### 1. EVENT_PROVIDER_CREATE
**Propósito**: Crear una nueva orden de trabajo (OT) en el sistema MOK

**Flujo**:
1. Obtiene datos del EventProvider cerrado por EventId
2. Extrae CaseId del ExternalRequestId
3. Construye payload para crear OT:
   ```json
   {
     "IdCase": 12345,
     "IdProvider": 90229,
     "IdService": 1124,
     "DateScheduled": "2025-01-01T10:00:00Z",
     "Comments": "Descripción"
   }
   ```
4. Llama a API MOK `/createOT`
5. Actualiza EventProvider con ExternalData y ExternalRequestId

#### 2. EVENT_PROVIDER_START
**Propósito**: Iniciar un servicio existente

**Flujo**:
1. Obtiene EventProvider abierto por EventId
2. Extrae IdOt del ExternalRequestId
3. Construye payload:
   ```json
   {
     "addComment": 1,
     "IdOT": 789656544,
     "startService": 0,
     "comment": "Descripción"
   }
   ```
4. Llama a API MOK `/processService`

#### 3. EVENT_PROVIDER_RESOLVE
**Propósito**: Finalizar/resolver un servicio

**Flujo**:
1. Obtiene EventProvider abierto por EventId
2. Extrae IdOt del ExternalRequestId
3. Construye payload:
   ```json
   {
     "addComment": 1,
     "IdOT": 789656544,
     "startService": 1,
     "comment": "Descripción"
   }
   ```
4. Llama a API MOK `/processService`

#### 4. NOTE_REGISTER
**Propósito**: Registrar una nota en el sistema

**Flujo**:
1. Obtiene EventProvider (abierto o cerrado) por EventId
2. Construye payload para nota:
   ```json
   {
     "addComment": 0,
     "IdOT": 789656544,
     "DateScheduled": "2025-01-01T10:00:00Z",
     "comment": "Descripción"
   }
   ```
3. Llama a API MOK `/processService`
4. Ejecuta `handle_update_script` para actualizar datos adicionales

### Funciones de Base de Datos

#### get_event_provider_open_by_event_id()
```sql
SELECT Id, EventProviderStatusId, ExternalRequestId
FROM EventProvider
WHERE EventId = %s AND EventProviderStatusId != '4'
ORDER BY CreatedAt DESC
```

#### get_event_provider_close_by_event_id()
```sql
SELECT Id, EventProviderStatusId, ExternalRequestId
FROM EventProvider
WHERE EventId = %s AND EventProviderStatusId = '4'
ORDER BY CreatedAt DESC
```

#### get_SummaryEventMOK_by_case_id()
```sql
SELECT *
FROM VwSummaryEventMOK
WHERE CaseId = %s
ORDER BY GuaranteePaymentCreatedAt ASC
```

### Parsing de ExternalRequestId
**Formato**: `"CaseId|IdOt"` (ejemplo: `"321321|789656544"`)

```python
def parse_external_request_id(external_request_id: str) -> dict:
    if external_request_id and '|' in external_request_id:
        parts = external_request_id.split('|')
        return {
            "case_id": parts[0].strip(),
            "id_ot": parts[1].strip()
        }
    return {"case_id": "", "id_ot": ""}
```

## API Client - Integración con MOK

### Autenticación OAuth2
**Flujo de Token**:
1. Obtiene credenciales desde AWS Secrets Manager
2. Solicita token OAuth2 con `client_credentials`
3. Cachea token hasta 5 minutos antes de expiración
4. Renueva automáticamente en caso de 401

### Endpoints MOK

#### 1. /createOT
**Método**: POST
**Propósito**: Crear orden de trabajo
**Payload**:
```json
{
  "IdCase": 20836453,
  "IdProvider": 90229,
  "IdService": 1124,
  "DateScheduled": "2025-06-26T07:34:00.173699+00:00",
  "Comments": "Descripción"
}
```

#### 2. /processService
**Método**: POST
**Propósito**: Procesar servicio (iniciar/finalizar/comentar)
**Payload**:
```json
{
  "addComment": 1,
  "IdOT": 15252900,
  "startService": 0,
  "comment": "Comentario"
}
```

#### 3. /updateScript
**Método**: POST
**Propósito**: Actualizar script de caso
**Payload**:
```json
{
  "caseId": 20836453,
  "comment": "Descripción",
  "script": {
    "selfAssistanceType": "tipo",
    "selfAssistance": "asistencia",
    "initialStimate": "estimado_inicial",
    "finalStimate": "estimado_final",
    "trm": "trm_value",
    "finalCIE10Diagnosis": "diagnostico"
  }
}
```

### Manejo de Errores y Reintentos
- **Timeout**: 30 segundos por defecto
- **Reintentos**: 1 intento por defecto
- **Backoff**: Exponencial con base 1 segundo
- **Manejo 401**: Limpia cache de token y reintenta

## SNS Notification Lambda

### Propósito
Envía notificaciones SNS cuando ocurren errores en el procesamiento MOK.

### Lógica de Notificación
```python
if statusCode == 200:
    return {"status_notification": "proceso exitoso, no se notifico"}
else:
    # Envía notificación SNS con detalles del error
    sns.publish(
        TopicArn=topic_arn,
        Message=error_details,
        Subject=f"Tracking Mok Lambda {action}"
    )
    return {"status_notification": "envio_exitoso"}
```

### Estructura de Entrada
```json
{
  "action": "EVENT_PROVIDER_CREATE",
  "current_status": "error",
  "statusCode": 500,
  "body": "{\"error\": \"Error message\", \"details\": \"Error details\"}"
}
```

## Integración con DynamoDB

### Tabla: AssistancesLoggerDB
**Propósito**: Actualizar registros con client_id de correlación

```python
response = table.update_item(
    Key={'PK': PK, 'SK': SK},
    UpdateExpression='SET #clientIdAttr = :clientIdVal',
    ExpressionAttributeNames={"#clientIdAttr": "client_id"},
    ExpressionAttributeValues={':clientIdVal': client_id}
)
```

## Configuración y Variables de Entorno

### TrackingMok Lambda
- `DB_SECRET_NAME`: Nombre del secreto de base de datos en Secrets Manager
- `MOK_SECRET_NAME`: Nombre del secreto de API MOK en Secrets Manager
- `LOG_LEVEL`: Nivel de logging (INFO por defecto)

### SNS Notification Lambda
- `SNS_TOPIC_ARN`: ARN del tópico SNS para notificaciones

### Secrets Manager - MOK Configuration
```json
{
  "MOK_AUTH_URL": "https://api.mok.com/oauth/token",
  "MOK_BASE_URL": "https://api.mok.com/v1",
  "MOK_CLIENT_ID": "client_id",
  "MOK_CLIENT_SECRET": "client_secret"
}
```

## Logging y Monitoreo

### CloudWatch Logs
- **Step Functions**: Logging completo habilitado
- **Lambda Functions**: Logs estructurados con contexto
- **API Calls**: Latencia y estado de respuesta registrados

### Métricas Clave
- Tiempo de ejecución de Step Functions
- Latencia de llamadas API MOK
- Tasa de éxito/error por acción
- Uso de cache de tokens OAuth2

## Manejo de Errores

### Errores de Base de Datos
```python
except ValueError as e:
    return {
        'statusCode': 400,
        'body': json.dumps({
            'error': 'Parametros invalidos',
            'details': str(e)
        })
    }
```

### Errores de API
```python
except Exception as e:
    return {
        'statusCode': 500,
        'body': json.dumps({
            'error': 'Error interno del servidor',
            'details': str(e)
        })
    }
```

## Casos de Uso Típicos

### 1. Creación de Orden de Trabajo
```
Cliente → Step Function → TrackingMok Lambda → MOK API (/createOT) → DB Update → SNS (si error)
```

### 2. Inicio de Servicio
```
Cliente → Step Function → TrackingMok Lambda → MOK API (/processService) → SNS (si error)
```

### 3. Registro de Nota
```
Cliente → Step Function → TrackingMok Lambda → MOK API (/processService) → Update Script → SNS (si error)
```

## Consideraciones de Rendimiento

### Optimizaciones Implementadas
- **Cache de tokens OAuth2**: Evita solicitudes innecesarias de autenticación
- **Conexiones de DB reutilizables**: Pool de conexiones MySQL
- **Logging estructurado**: Facilita debugging y monitoreo
- **Manejo de timeouts**: Previene bloqueos indefinidos

### Limitaciones Actuales
- **Reintentos limitados**: Solo 1 reintento por defecto
- **Sin circuit breaker**: No hay protección contra cascadas de fallos
- **Cache en memoria**: Se pierde entre invocaciones Lambda

## Próximas Mejoras Recomendadas

1. **Implementar Circuit Breaker** para APIs externas
2. **Agregar métricas personalizadas** de CloudWatch
3. **Implementar cache distribuido** (Redis/ElastiCache)
4. **Agregar validación de esquemas** con Pydantic
5. **Implementar dead letter queues** para errores persistentes

