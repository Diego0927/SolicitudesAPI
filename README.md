# Solicitudes API

API RESTful desarrollada en **.NET** para la gestión de solicitudes, aplicando principios de **arquitectura en capas**, reglas de negocio bien definidas y acceso a datos optimizado con **Dapper** sobre **Oracle Database**.

Este proyecto fue diseñado como una solución técnica robusta, escalable y fácil de mantener, siguiendo buenas prácticas de desarrollo backend.

---

## Características principales

- Arquitectura en capas (N-Tier)
- Separación clara de responsabilidades
- Validación de reglas de negocio en la capa de servicios
- Acceso a datos mediante Dapper (micro-ORM)
- Integración con Oracle Database
- Manejo correcto de códigos de estado HTTP
- Uso de DTOs para desacoplar la API del modelo de datos
- Cancelación lógica de registros

---

## Arquitectura del proyecto

La solución está organizada bajo una **arquitectura en capas**, lo que facilita el mantenimiento, la escalabilidad y las pruebas unitarias.

/API
└── Controllers
/Services
└── Interfaces
/Repositories
└── Interfaces
/Models
/DTOs


### Capas

- **Controllers**
  - Punto de entrada de la API.
  - Manejan las solicitudes HTTP y las respuestas.
  - No contienen lógica de negocio.

- **Services**
  - Contienen la lógica de negocio.
  - Validan reglas como estados permitidos y flujos de operación.
  - Actúan como intermediarios entre Controllers y Repositories.

- **Repositories**
  - Encapsulan el acceso a datos.
  - Ejecutan consultas SQL directamente sobre Oracle usando Dapper.
  - Aíslan la base de datos del resto de la aplicación.

- **Models / Entities**
  - Representan las tablas de la base de datos Oracle.

- **DTOs (Data Transfer Objects)**
  - Definen los contratos de entrada y salida de la API.
  - Evitan exponer directamente la estructura de la base de datos.

---

## Decisiones técnicas

### ¿Por qué Dapper y no Entity Framework?

- Mayor control sobre SQL
- Mejor rendimiento
- Compatibilidad directa con características específicas de Oracle
- Ideal para escenarios donde el esquema ya está definido

---

##  Modelo de datos

Entidad principal:
Solicitud

-----------------------------------------------------------------------------------------------------------------------------------

CAMPOS CLAVE:
- ID
- NombreUsuario
- TipoSolicitud
- Descripción
- Estado (ACTIVA / CANCELADA)
- FechaCreación
- FechaCancelación
La cancelación de una solicitud se realiza mediante retiro lógico, conservando la información histórica.

REGLAS DE NEGOCIO:
- Una solicitud solo puede cancelarse si está ACTIVA
- No se permite cancelar una solicitud ya CANCELADA
- El estado no puede modificarse directamente desde la API
- Las validaciones de negocio viven exclusivamente en la capa de servicios

ENDOPOINTS DSIPONIBLES:
- Crear solicitud
- POST /api/solicitudes
- Retorna 201 Created
- Incluye la URL del recurso creado

LISTAR SOLICITUDES:
- GET /api/solicitudes
- Filtros opcionales:
- estado
- fechaInicio
- fechaFin

OBTENER SOLICITUD POR ID:
- GET /api/solicitudes/{id}
- 200 OK si existe
- 404 Not Found si no existe

CANCELAR SOLICITUD (RETIRO LÓGICO):
DELETE /api/solicitudes/{id}
- 200 OK si la cancelación es exitosa
- 400 Bad Request si ya está cancelada
- 404 Not Found si no existe

MANEJO DE CÓDIGOS HTTP:
-----------------------------------------------
|  CÓDIGO  |	         DESCRIPCIÓN            |
-----------------------------------------------
|   200    |   Operación exitosa              |
-----------------------------------------------
|   201    |   Recurso creado                 |
-----------------------------------------------
|   400    |   Error de validación de negocio |
-----------------------------------------------
|   404    |   Recurso no encontrado          |
-----------------------------------------------
|   500    |   Error interno del servidor     |
-----------------------------------------------

TECNOLOGÍAS UTILIZADAS:
- .NET
- ASP.NET Core Web API
- Dapper
- Oracle Database
- C#

BUENAS PRÁCTICAS APLICADAS:
- Controladores delgados
- Reglas de negocio centralizadas
- SQL optimizado y parametrizado
- Manejo explícito de excepciones
- Código desacoplado y testeable

NOTAS FINALES:
- Este proyecto sirve como base sólida para:
- Pruebas técnicas
- APIs empresariales
- Sistemas que requieren control estricto de reglas de negocio
- Integraciones con Oracle Database

AUTOR:
Diego Alejandro Giraldo Duque
Backend Developer – .NET
