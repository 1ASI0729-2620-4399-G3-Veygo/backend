# Veygo Platform — RESTful API

Backend de **Veygo** (startup Zyntra), el marketplace de alquiler de vehículos de Lima.
Proyecto del Trabajo Final del curso 1ASI0730 Aplicaciones Web (UPC, 2026-20), grupo 3.

Construido con **ASP.NET Core 10**, **C# 14**, **EF Core 10 + MySQL**, **Cortex Mediator** (eventos de dominio)
y **OpenAPI + Scalar**, siguiendo la misma arquitectura que trabajamos en clase (CatchUp Platform y
Learning Center Platform): DDD con bounded contexts, capas Domain / Application / Infrastructure / Interfaces,
CQS (command y query services), Repository + Unit of Work, patrón Result y Problem Details (RFC 9457).

> **IAM todavía no está incluido** (se implementa en la siguiente entrega). Por ahora `ownerId`, `renterId`,
> `senderId` y `userId` son ids enteros de los usuarios que siguen viviendo en la Fake API (`/users`).

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

## Cómo ejecutarlo

1. Tener MySQL corriendo en `localhost:3306` (usuario `root`, clave `password`) o cambiar la cadena de conexión en
   `appsettings.Development.json`.
2. Abrir `veygo-platform.sln` en Rider y ejecutar el perfil **http**. Se abre Scalar en `http://localhost:5080/scalar`.
3. Al arrancar se crea la base `veygo-platform` (`EnsureCreated`) y, en Development, se cargan los datos de ejemplo
   del `db.json` del frontend (`SampleData/veygo-sample-data.json`). Para no cargarlos: `Database:SeedSampleData = false`.

En Production la cadena de conexión se arma con variables de entorno: `DATABASE_HOST`, `DATABASE_PORT`,
`DATABASE_USER`, `DATABASE_PASSWORD` y `DATABASE_SCHEMA`. Si falta alguna, la aplicación lo indica al iniciar.

Docker (desde la raíz del repositorio):

```bash
docker build -f Veygo.Platform/Dockerfile -t veygo-platform .
docker run -p 8080:8080 -e DATABASE_HOST=host.docker.internal -e DATABASE_PORT=3306 \
  -e DATABASE_USER=root -e DATABASE_PASSWORD=password -e DATABASE_SCHEMA=veygo-platform veygo-platform
```

## Conectar el frontend

En el `.env.development` del frontend:

- `VITE_BLOCKED_DATES_ENDPOINT_PATH="/blocked-dates"` (las rutas del backend son kebab-case).
- Mientras IAM no esté en el backend, `/users` debe seguir apuntando a la Fake API.
- `error.interceptor.js` lee `data.message`; los errores del backend vienen en `data.detail` (Problem Details):
  `message = error.response.data?.detail || error.response.data?.message || ...`.

## Decisiones respecto a las plantillas de clase

- Los eventos de dominio se publican **después** de guardar (`SavedChanges`), como indica el ADR de eventos:
  así los ids ya existen y ningún handler corre si el guardado falla.
- Los command services hacen su propio `CompleteAsync`. No se incluyen los pipeline behaviors de Cortex porque los
  comandos no se envían por el mediator (solo se usa para eventos).
- Los ids son `int` (como en CatchUp) para que `BaseRepository.FindByIdAsync(int)` funcione.
- Las reglas de negocio esperadas (no encontrado, conflicto, no disponible) se devuelven como `Result`; las
  invariantes violadas (`ArgumentException`) responden 400 desde `GlobalExceptionHandler`.
- La fecha de negocio es la de Lima (`BusinessCalendar.Today`), no la UTC del servidor.
- Todavía no hay migraciones: el esquema se crea con `EnsureCreated`. Cuando el modelo se estabilice se puede pasar
  a `dotnet ef migrations add InitialCreate` + `Database.Migrate()`.
