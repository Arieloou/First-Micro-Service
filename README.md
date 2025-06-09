# First-Micro-Service
By Ariel Anchapaxi

¡Por supuesto! Un buen README.md es la tarjeta de presentación de tu proyecto. Debe ser claro, conciso y guiar a cualquier persona (incluido tu profesor) a través de tu trabajo, explicando no solo qué hace, sino también cómo y por qué lo hace de esa manera.

Aquí tienes una plantilla completa y profesional que puedes adaptar. Está escrita en Markdown y diseñada para destacar los puntos clave de tu tarea.

Plantilla para README.md

Copia y pega este contenido en un archivo llamado README.md en la raíz de tu proyecto y rellena los detalles.

# Microservicio de Notificaciones

Este proyecto es un microservicio desarrollado como parte de la tarea universitaria para [Nombre de la Asignatura]. Su principal objetivo es demostrar la implementación de una arquitectura de microservicios robusta, aplicando buenas prácticas de diseño de software y los pilares fundamentales de este estilo arquitectónico.

## 1. Dominio Funcional

El microservicio se enfoca en un único dominio: **la gestión y envío de notificaciones**. Su responsabilidad exclusiva es recibir solicitudes para enviar notificaciones a través de diferentes canales (como email o SMS), procesarlas y registrar el resultado, sin conocer el contexto de negocio que origina la solicitud (ej. confirmación de compra, reseteo de contraseña, etc.).

---

## 2. Arquitectura y Diseño

El proyecto sigue los principios de la **Arquitectura Limpia (Clean Architecture)**, separando las responsabilidades en capas bien definidas para lograr un bajo acoplamiento y alta cohesión.

*   **`Domain`**: El núcleo de la aplicación. Contiene las entidades de negocio (`Notification`, `NotificationLog`) con su lógica y reglas intrínsecas. No depende de ninguna otra capa.
*   **`Application`**: Orquesta los casos de uso. Contiene los servicios de aplicación (`NotificationAppService`) que utilizan las entidades del dominio para ejecutar las operaciones. Depende de `Domain`.
*   **`Infrastructure`**: Contiene los detalles externos y la implementación de las interfaces definidas en capas superiores. Aquí se encuentran la configuración del `DbContext` (Entity Framework), los repositorios y los proveedores de notificaciones externas (ej. `EmailProvider`). Depende de `Application`.
*   **`Presentation`**: El punto de entrada a la aplicación. En este caso, es la API REST (`NotificationsController`) que expone los endpoints al mundo exterior. Depende de `Application`.

### Principios Aplicados
*   **Inyección de Dependencias (DI)**: Utilizada extensivamente en todo el proyecto a través del contenedor de DI de ASP.NET Core para desacoplar los componentes.
*   **Interfaces y Abstracciones**: Se utilizan interfaces (`INotificationRepository`, `INotificationProvider`) para cumplir con el Principio de Inversión de Dependencias.
*   **Patrón Repositorio**: Para abstraer el acceso a datos y centralizar las consultas.
*   **Patrón Fábrica (Factory)**: Para seleccionar el proveedor de notificaciones (`EmailProvider`, `SmsProvider`) en tiempo de ejecución de forma desacoplada.

---

## 3. Pilares de Microservicios Implementados

1.  **Descomposición por Dominio**: El servicio tiene una responsabilidad única y bien definida, como se describió anteriormente.
2.  **Comunicación**: Se expone una API REST síncrona con endpoints claros y documentados a través de Swagger.
3.  **Gestión de Datos Distribuida**: El microservicio es dueño de su propia base de datos (SQLite), gestionando su esquema a través de migraciones de Entity Framework Core. No comparte su base de datos con ningún otro servicio.
4.  **Despliegue y Operación**: El proyecto incluye un `Dockerfile` para construir una imagen de contenedor independiente, permitiendo un despliegue aislado y consistente en cualquier entorno que soporte Docker.
5.  **Observabilidad y Resiliencia**:
    *   **Logging**: Se utiliza el sistema de logging estructurado de .NET (`ILogger`) para registrar información relevante, advertencias y errores.
    *   **Health Check**: Se expone un endpoint `GET /health` para facilitar el monitoreo del estado del servicio por parte de orquestadores de contenedores.

---

## 4. Stack Tecnológico

*   **Lenguaje**: C# 12
*   **Framework**: ASP.NET Core 8
*   **ORM**: Entity Framework Core 8
*   **Base de Datos**: SQLite (para simplicidad y portabilidad)
*   **Contenerización**: Docker

---

## 5. Cómo Ejecutar el Proyecto

### Requisitos Previos
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop) (Opcional, para ejecución en contenedor)

### Configuración de Secretos

Para el desarrollo local, el proyecto utiliza el **Secret Manager** de .NET para manejar las claves de API.

1.  Abre una terminal en la carpeta raíz del proyecto (`/NotificationsService`).
2.  Inicializa el Secret Manager:
    ```bash
    dotnet user-secrets init
    ```
3.  Añade las claves de API (puedes usar valores de prueba):
    ```bash
    dotnet user-secrets set "NotificationProviders:EmailApiKey" "TU_EMAIL_API_KEY_AQUI"
    dotnet user-secrets set "NotificationProviders:SmsApiKey" "TU_SMS_API_KEY_AQUI"
    ```

### Ejecución Local (Usando la CLI de .NET)

1.  **Restaurar dependencias:**
    ```bash
    dotnet restore
    ```
2.  **Aplicar las migraciones a la base de datos:**
    El proyecto está configurado para aplicar las migraciones automáticamente al iniciar en modo de desarrollo. Si prefieres hacerlo manualmente, ejecuta:
    ```bash
    dotnet ef database update
    ```
3.  **Iniciar la aplicación:**
    ```bash
    dotnet run
    ```
4.  La API estará disponible en `https://localhost:XXXX` y `http://localhost:YYYY`. Puedes acceder a la documentación interactiva de Swagger en `https://localhost:XXXX/swagger`.

### Ejecución con Docker

1.  **Construir la imagen de Docker:**
    ```bash
    docker build -t notification-service .
    ```
2.  **Ejecutar el contenedor:**
    Pasa las claves de API como variables de entorno al contenedor.
    ```bash
    docker run -p 8080:8080 -e "NotificationProviders:EmailApiKey=TU_EMAIL_API_KEY_AQUI" -e "NotificationProviders:SmsApiKey=TU_SMS_API_KEY_AQUI" -e "ASPNETCORE_URLS=http://+:8080" notification-service
    ```
    *   `-p 8080:8080` mapea el puerto 8080 de tu máquina al puerto 8080 del contenedor.
    *   `-e` establece las variables de entorno necesarias.

---

## 6. API Endpoint

### Enviar una Notificación

*   **Endpoint**: `POST /api/notifications`
*   **Descripción**: Crea y envía una nueva notificación.
*   **Cuerpo de la Petición (Request Body)**:
    ```json
    {
      "type": "email",
      "recipient": "destinatario@ejemplo.com",
      "subject": "Asunto del Correo",
      "body": "Este es el cuerpo del mensaje de la notificación."
    }
    ```
    *   `type`: Puede ser `"email"` o `"sms"`.
    *   `subject`: Requerido si el `type` es `"email"`.

*   **Respuesta Exitosa (200 OK)**:
    ```json
    {
      "status": "procesado",
      "notificationId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
    }
    ```
*   **Respuesta de Error (400 Bad Request)**:
    ```json
    {
      "status": "error",
      "message": "El proveedor de notificación 'whatsapp' no está soportado."
    }
    ```

---
**Autor**: [Tu Nombre]
**Fecha**: [Fecha de Entrega]


Este README no solo cumple con los requisitos, sino que también sirve como una excelente pieza de documentación que demuestra tu profundo entendimiento de los conceptos aplicados. ¡Mucha suerte con tu entrega
