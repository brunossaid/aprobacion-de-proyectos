# Aprobacion de Proyectos

Trabajo practico 1 - Proyecto de Software

Aplicacion de consola desarrollada en C# para gestionar solicitudes de aprobación de proyectos, siguiendo los principios de Clean Architecture y Clean Code.

## Estructura del proyecto

- `Domain`: entidades del dominio.
- `Application`: interfaces y servicios de aplicacion.
- `Infrastructure`: acceso a datos e implementacion de servicios.
- `ConsoleApp`: interfaz de usuario por consola.

## Configuracion de la base de datos

Desde la raiz del proyecto ejecuta estos comandos para crear y aplicar la migracion:

```bash
dotnet ef migrations add InitialCreate --project Infrastructure
```

```bash
dotnet ef database update --project Infrastructure
```

## Como ejecutar

Desde la raiz del proyecto, ejecutar en consola:

```bash
dotnet run --project ConsoleApp/ConsoleApp.csproj
```
