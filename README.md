# Aprobacion de Proyectos

Trabajo practico 1 (corregido) - Proyecto de Software

Aplicacion de consola desarrollada en C# para gestionar solicitudes de aprobacion de proyectos.

## Correcciones realizadas

### Criterios de aceptacion

- Se elige la ApprovalRule mas especfica al crear los steps en caso de colisiones, antes para el programa era lo mismo si coincidian area y tipo, que si coincidia uno solo.

### Calidad del Codigo

- Se modularizaro la clase de Actions CreateProposal, el resto no eran tan largas y no se modificaron.
- Se mejoro la separacion de responsabilidades, evitando que ConsoleApp acceda directamente a la infraestructura usando DTOs.

### SOLID

- Se aplico el principio de Segregacion de Interfaces dividiendo algunas interfaces en lectura y escritura:
  - IProjectApprovalStepService se dividio en IProjectApprovalStepReader y IProjectApprovalStepWriter
  - IProjectApprovalService se dividio en IProjectProposalReader y IProjectProposalWriter

### Base de Datos

- Habia algunos datos iniciales mal cargados en el DbContext, por ejemplo en la ApprovalRule 1 el maxAmount estaba en 10000 y era 100000, pero ya fueron corregidos.

### Nuevas Implementaciones

- En la capa de aplicacion (`Application/Services/ApprovalStepManager`) se crearon algunos nuevos metodos para mejorar la gestion de los pasos de aprobacion:
  - `GetPendingStepsForUserAsync()`: Obtiene los pasos pendientes que un usuario debe evaluar.
  - `GetStepsForProposalAsync()`: Obtiene los pasos de aprobacion asociados a una propuesta especifica.
