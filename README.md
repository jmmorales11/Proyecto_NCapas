# Proyecto de Arquitectura en Capas y Seguridad

## Descripción General
Este proyecto implementa un sistema de gestión para usuarios, productos y categorías utilizando una arquitectura en capas. Se enfoca en garantizar escalabilidad, mantenimiento y seguridad, aplicando buenas prácticas y previniendo vulnerabilidades comunes.

## Objetivos
- Implementar autenticación y autorización seguras.
- Proteger contra vulnerabilidades como inyección SQL, XSS y CSRF.
- Diseñar una arquitectura modular y escalable.

## Arquitectura
El sistema está estructurado en capas:
- **BLL (Business Logic Layer):** Contiene la lógica de negocio.
- **DAL (Data Access Layer):** Gestiona el acceso a la base de datos.
- **Entities:** Define las clases de datos que representan entidades del sistema.
- **Service Layer:** Implementa servicios API.
- **ProxyService:** Intermedia entre la capa de presentación y los servicios.
- **SL (Security Layer):** Maneja la seguridad, autenticación y autorización.
- **Presentation Layer:** Interfaz gráfica para el usuario.

## Funcionalidades
1. **Gestión de Usuarios:**
   - CRUD (Crear, Leer, Actualizar, Eliminar).
   - Control de intentos fallidos y bloqueo de cuentas.
2. **Gestión de Productos y Categorías:**
   - CRUD completo para cada entidad.
3. **Inicio de Sesión:**
   - Autenticación basada en JWT.
   - Protección contra fuerza bruta.
4. **Seguridad:**
   - Validación para prevenir inyección SQL.
   - Mitigación de XSS y CSRF.
5. **Auditoría:**
   - Registro de logs de eventos críticos y accesos.

## Requisitos
### Funcionales:
- Gestión de usuarios, productos y categorías.
- Control de acceso basado en roles.

### No Funcionales:
- Notificación de inicio de sesión fallido.
- Uso de algoritmos de encriptación Hash para contraseñas.

## Seguridad
1. **Protección contra amenazas comunes:**
   - Validación estricta de entradas para prevenir inyección SQL.
   - Encriptación de datos sensibles con algoritmos seguros como SHA2.
   - Uso de cookies seguras para autenticación.

2. **Política de contraseñas:**
   - Longitud mínima de 13 caracteres.
   - Inclusión de caracteres especiales.
   - Exclusión de datos personales como parte de la contraseña.

## Implementación
- **Tecnologías utilizadas:**
  - Entity Framework para la gestión de datos.
  - JWT para autenticación.
  - ASP.NET MVC como framework principal.
  - SQL Server como base de datos.
- **Validaciones:**
  - En la capa de negocio y entidades para garantizar integridad.

## Diagramas y Flujos
Incluye diagramas detallados de:
- Arquitectura del sistema.
- Flujos de gestión de usuarios, productos y categorías.
- Interfaces gráficas de inicio de sesión y recuperación de contraseña.

## Pruebas
- **Validación de roles y permisos:** Verifica accesos específicos según el rol del usuario.
- **Mitigación de ataques:**
  - Inyección SQL mediante validaciones de entradas.
  - XSS utilizando sanitización de datos.
- **Gestión de cuentas bloqueadas y recuperación de contraseñas.**

## Referencias
- **Bibliotecas:** EntityFramework, Serilog, Newtonsoft.Json.
- **Tecnologías:** C#, ASP.NET MVC, SQL Server.

## Autoría
- **Jeimy Marley Morales Sosa**
- **Steven Jefferson Pozo Analuisa**
- **Erick Patricio Ramírez Ortíz**
- **Sebastian Paúl Torres Tapia**
