# Veygo Platform — RESTful API

Backend de **Veygo** (startup Zyntra), el marketplace de alquiler de vehículos de Lima.
Proyecto del Trabajo Final del curso 1ASI0730 Aplicaciones Web (UPC, 2026-20), grupo 3.

Construido con **ASP.NET Core 10**, **C# 14**, **EF Core 10 + MySQL**, **Cortex Mediator** (eventos de dominio)
y **OpenAPI + Scalar**, siguiendo la misma arquitectura que trabajamos en clase (CatchUp Platform y
Learning Center Platform): DDD con bounded contexts, capas Domain / Application / Infrastructure / Interfaces,
CQS (command y query services), Repository + Unit of Work, patrón Result y Problem Details (RFC 9457).

> **IAM todavía no está incluido** (se implementa en la siguiente entrega). Por ahora `ownerId`, `renterId`,
> `senderId` y `userId` son ids enteros de los usuarios que siguen viviendo en la Fake API (`/users`).

## Despliegue

| Componente | Servicio | URL |
|---|---|---|
| RESTful API (este repositorio) | Render (Web Service, Docker) | https://veygo-platform.onrender.com |
| Documentación de la API | Scalar | https://veygo-platform.onrender.com/scalar |
| Documento OpenAPI | — | https://veygo-platform.onrender.com/openapi/v1.json |
| Base de datos | Aiven for MySQL | `veygo-mysql` (SSL obligatorio) |
| Aplicación web (frontend) | Firebase Hosting | https://veygo-web-app.web.app |
| Usuarios (temporal, hasta IAM) | Fake API en Render | https://veygo-fake-api.onrender.com/api/v1/users |

Render vuelve a desplegar automáticamente con cada push a `main`. Al iniciar, la aplicación aplica las migraciones
pendientes sobre la base de Aiven.

> El plan gratuito de Render se suspende tras unos 15 minutos sin tráfico: la primera petición puede tardar
> alrededor de un minuto mientras el servicio despierta.

## Tecnologías

| Paquete | Versión | Uso |
|---|---|---|
| .NET / ASP.NET Core | 10.0 | Web API |
| Microsoft.EntityFrameworkCore (+ Relational, Design) | 10.0.14 | ORM y migraciones |
| MySql.EntityFrameworkCore | 10.0.12 | Proveedor MySQL (`UseMySQL`) |
| Humanizer | 3.0.10 | Nombres de tablas y columnas en snake_case y plural |
| Cortex.Mediator | 3.1.2 | Publicación de eventos de dominio |
| Microsoft.AspNetCore.OpenApi | 10.0.14 | Documento OpenAPI |
| Scalar.AspNetCore | 2.18.2 | Interfaz de documentación de la API |

## Estructura del repositorio

```
backend/
├── veygo-platform.sln
├── .dockerignore
└── veygo-platform/
    ├── Dockerfile
    ├── Program.cs
    ├── appsettings.json                # Configuración base (Production)
    ├── appsettings.Development.json    # MySQL local y carga de datos de ejemplo
    ├── Shared/                         # Base común + AppDbContext, migraciones y seeder
    ├── Fleet/
    ├── Booking/
    ├── Engagement/
    ├── Reputation/
    ├── Communication/
    ├── Notification/
    └── SampleData/veygo-sample-data.json
```

Cada bounded context tiene las capas `Domain`, `Application`, `Infrastructure` e `Interfaces`.

## Bounded contexts

| Contexto | Responsabilidad | Agregados | Endpoints |
|---|---|---|---|
| **Shared** | Base común: `AggregateRoot`, `Result<T>`, `Error`, repositorio base, unit of work, interceptores, manejo de errores, localización | — | — |
| **Fleet** | Vehículos de los propietarios y su calendario de disponibilidad | `Vehicle`, `BlockedDate` | `/api/v1/vehicles`, `/api/v1/blocked-dates` |
| **Booking** | Reservas: solicitud, confirmación, rechazo, cancelación y cierre | `Booking` | `/api/v1/bookings` |
| **Engagement** | Vehículos favoritos de los arrendatarios | `Favorite` | `/api/v1/favorites` |
| **Reputation** | Calificaciones de alquileres finalizados | `Review` | `/api/v1/reviews` |
| **Communication** | Chat entre arrendatario y propietario | `Conversation`, `Message` | `/api/v1/conversations`, `/api/v1/messages` |
| **Notification** | Notificaciones dentro de la app | `Notification` | `/api/v1/notifications` |

Payment no tiene endpoints: el frontend calcula las transacciones a partir de las reservas.

Comunicación entre contextos (Anti-Corruption Layer):
- Booking, Engagement y Communication consultan a Fleet mediante `IFleetContextFacade`.
- Reputation consulta a Booking mediante `IBookingContextFacade`.
- Booking publica `BookingStatusChangedEvent`, que Notification escucha (`BookingStatusChangedEventHandler`).

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/v1/vehicles` | Registra un vehículo (placa única) |
| GET | `/api/v1/vehicles?ownerId=&published=` | Lista vehículos (todos, de un propietario o el catálogo publicado) |
| GET | `/api/v1/vehicles/{id}` | Detalle de un vehículo |
| PUT | `/api/v1/vehicles/{id}` | Actualiza un vehículo (el propietario no cambia) |
| PATCH | `/api/v1/vehicles/{id}` | Publica o despublica (`published`) y/o actualiza `rating` y `reviewsCount` |
| DELETE | `/api/v1/vehicles/{id}` | Elimina un vehículo y sus fechas bloqueadas |
| POST | `/api/v1/blocked-dates` | Bloquea un día de un vehículo |
| GET | `/api/v1/blocked-dates?vehicleId=` | Días bloqueados de un vehículo |
| DELETE | `/api/v1/blocked-dates/{id}` | Libera un día |
| POST | `/api/v1/bookings` | Solicita una reserva (valida disponibilidad, precio y propietario desde Fleet) |
| GET | `/api/v1/bookings?renterId=` · `?ownerId=` · `?vehicleId=` | Reservas de un arrendatario, de un propietario o de un vehículo |
| GET | `/api/v1/bookings/{id}` | Detalle de una reserva |
| PATCH | `/api/v1/bookings/{id}` | Cambia el estado: `confirmed`, `rejected`, `cancelled`, `completed` |
| POST | `/api/v1/favorites` | Guarda un favorito |
| GET | `/api/v1/favorites?renterId=` | Favoritos de un arrendatario |
| DELETE | `/api/v1/favorites/{id}` | Quita un favorito |
| POST | `/api/v1/reviews` | Califica un alquiler finalizado (una vez por reserva) |
| GET | `/api/v1/reviews?ownerId=` · `?renterId=` · `?vehicleId=` | Calificaciones |
| POST | `/api/v1/conversations` | Inicia una conversación |
| GET | `/api/v1/conversations?renterId=` · `?ownerId=` | Conversaciones de un usuario |
| PATCH | `/api/v1/conversations/{id}` | Actualiza la vista previa del último mensaje |
| POST | `/api/v1/messages` | Envía un mensaje (actualiza la conversación) |
| GET | `/api/v1/messages?conversationId=` | Mensajes de una conversación |
| PATCH | `/api/v1/messages/{id}` | Marca un mensaje como leído (`read: true`) |
| POST | `/api/v1/notifications` | Crea una notificación |
| GET | `/api/v1/notifications?userId=&_limit=` | Últimas notificaciones de un usuario (30 por defecto) |
| PATCH | `/api/v1/notifications/{id}` | Marca una notificación como leída (`read: true`) |

Todos los endpoints `GET /{id}` también existen para cada recurso. La documentación interactiva está en `/scalar`
y el documento OpenAPI en `/openapi/v1.json`.

Los errores responden con Problem Details (`title`, `status`, `detail`, `instance` y la extensión `code`, por
ejemplo `Booking.VehicleNotAvailable`), traducidos al español si la petición envía `Accept-Language: es`.

## Ejecución local

1. Tener MySQL corriendo en `localhost:3306`. `appsettings.Development.json` usa usuario `root` y clave `password`;
   si tu clave es otra, no la escribas en ese archivo, guárdala en **User Secrets** (no se sube al repositorio):

   ```bash
   cd veygo-platform
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=localhost;port=3306;user=root;password=TU_CLAVE;database=veygo-platform"
   ```

2. Abrir `veygo-platform.sln` en Rider y ejecutar el perfil **http**. Se abre Scalar en `http://localhost:5080/scalar`.
3. Al arrancar se aplican las migraciones (`Database.MigrateAsync()`), que crean la base `veygo-platform` y sus
   tablas. En Development, si la base está vacía, se cargan los datos de ejemplo del `db.json` del frontend
   (`SampleData/veygo-sample-data.json`). Para no cargarlos: `Database:SeedSampleData = false`.

### Migraciones

Las migraciones están en `Shared/Infrastructure/Persistence/EntityFrameworkCore/Migrations`. Si cambias el modelo,
crea una nueva desde la carpeta `veygo-platform`:

```bash
dotnet tool install --global dotnet-ef   # solo la primera vez
dotnet ef migrations add NombreDelCambio --output-dir Shared/Infrastructure/Persistence/EntityFrameworkCore/Migrations
```

La migración se aplica sola al iniciar la aplicación, tanto en local como en Render.

> En Windows, si `dotnet ef` muestra *"hostfxr.dll not found"*, ejecuta antes en PowerShell:
> `$env:DOTNET_ROOT = Split-Path (Get-Command dotnet).Source`

## Configuración en producción

`appsettings.json` arma la cadena de conexión con variables de entorno: `DATABASE_HOST`, `DATABASE_PORT`,
`DATABASE_USER`, `DATABASE_PASSWORD` y `DATABASE_SCHEMA` (si falta alguna, la aplicación lo indica al iniciar).
También se puede reemplazar la cadena completa con la variable `ConnectionStrings__DefaultConnection`, que es lo que
usa el despliegue en Render:

| Variable | Valor |
|---|---|
| `ConnectionStrings__DefaultConnection` | `server=<host-aiven>;port=<puerto>;user=avnadmin;password=<clave>;database=defaultdb;SslMode=Required` |
| `PORT` | `8080` |
| `Database__SeedSampleData` | `true` (carga los datos de ejemplo solo si la base está vacía) |

Las credenciales se configuran únicamente en el panel de Render (**Environment**) y nunca se suben al repositorio.

CORS permite `http://localhost:5173`, `https://veygo-web-app.web.app` y `https://veygo-web-app.firebaseapp.com`
(`Cors:AllowedOrigins`).

### Docker

Desde la raíz del repositorio:

```bash
docker build -f veygo-platform/Dockerfile -t veygo-platform .
docker run -p 8080:8080 \
  -e DATABASE_HOST=host.docker.internal -e DATABASE_PORT=3306 \
  -e DATABASE_USER=root -e DATABASE_PASSWORD=password -e DATABASE_SCHEMA=veygo-platform \
  veygo-platform
```

### Render

Web Service conectado a este repositorio:

- **Language:** Docker · **Branch:** `main` · **Region:** Ohio (US East) · **Instance:** Free
- **Dockerfile Path:** `veygo-platform/Dockerfile` · **Docker Build Context:** raíz del repositorio (`.`)
- **Environment:** las variables de la tabla anterior.

## Conectar el frontend

Variables del frontend (las rutas del backend son kebab-case):

| Archivo | `VITE_VEYGO_API_URL` | `VITE_IAM_API_URL` |
|---|---|---|
| `.env.development` | `http://localhost:5080/api/v1` | `http://localhost:3000/api/v1` (json-server) |
| `.env.production` | `https://veygo-platform.onrender.com/api/v1` | `https://veygo-fake-api.onrender.com/api/v1` |

En ambos: `VITE_BLOCKED_DATES_ENDPOINT_PATH="/blocked-dates"`.

`error.interceptor.js` lee primero `data.detail`, porque los errores del backend vienen como Problem Details:
`message = error.response.data?.detail || error.response.data?.message || ...`.

## Flujo de trabajo (GitFlow)

- `main`: versión desplegada en Render.
- `develop`: integración.
- `feature/*`: una rama por bounded context o cambio; se integra a `develop` mediante pull request.
- Commits con Conventional Commits (`feat:`, `fix:`, `docs:`, `chore:`...).

## Decisiones respecto a las plantillas de clase

- Los eventos de dominio se publican **después** de guardar (`SavedChanges`), como indica el ADR de eventos:
  así los ids ya existen y ningún handler corre si el guardado falla.
- Los command services hacen su propio `CompleteAsync`. No se incluyen los pipeline behaviors de Cortex porque los
  comandos no se envían por el mediator (solo se usa para eventos).
- Los ids son `int` (como en CatchUp) para que `BaseRepository.FindByIdAsync(int)` funcione.
- Las reglas de negocio esperadas (no encontrado, conflicto, no disponible) se devuelven como `Result`; las
  invariantes violadas (`ArgumentException`) responden 400 desde `GlobalExceptionHandler`.
- La fecha de negocio es la de Lima (`BusinessCalendar.Today`), no la UTC del servidor.
- El esquema se gestiona con migraciones de EF Core (`InitialCreate`) y `Database.MigrateAsync()` al iniciar, en
  lugar de `EnsureCreated`, para poder evolucionar la base desplegada sin borrarla.
- Los value objects propios (`VehicleLocation`, `VehicleInsurance`) se guardan en la tabla `vehicles` como owned
  types; la convención snake_case no renombra su clave para no chocar con la de `Vehicle`.
