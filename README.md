El proyecto aplica Arquitectura limpia y sigue principios de Domain-Driven Design, con una clara separación de las capas de dominio, aplicación e infraestructura.

## Patrones de Diseño y Principios
- Repositorio (Repository Pattern)
- Inyección de Dependencias (Dependency Injection)
- CQRS (Command Query Responsibility Segregation)
- Result
- SOLID
## Tecnologías Utilizadas
- C# y .NET8: Lenguaje de programación y framework principal.
- Entity Framework Core: Para la interacción con la base de datos SQLServer.
- MediatR: Para la implementación de patrones CQRS.
 -Xunit: Para pruebas unitarias.

## Instalación y Configuración
- Clonar el Repositorio: git clone <URL del repositorio>
- Restaurar Paquetes: Ejecutar dotnet restore para restaurar los paquetes NuGet.
- Configurar la Base de Datos: Asegurarse de que la cadena de conexión en appsettings.json esté configurada correctamente.
- Configurar parametros SMTP: Asegurarse de plasmar los valores para la conexión SMTP para el envío de correos electrónicos
- Ejecutar Migraciones: dotnet ef database update para aplicar las migraciones de la base de datos.
- Ejecutar la Aplicación: dotnet run para iniciar la aplicación.

Especialista en Ingeniría de Software Daniel Barros Agamez