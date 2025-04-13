# 🚗 Car Manufacturing System

A full-scale Windows Forms desktop application that simulates a car manufacturing and assembling system using:
- ✅ **SOLID principles**
- 🏭 **Factory Pattern** for component creation
- 🗃 **Repository Pattern** for data access
- 💉 **Dependency Injection**
- 🧠 Separation of concerns between UI, Logic, and Data

---

### 📦 Project Architecture Summary

| 📁 Folder / 📄 File                        | 📄 Description                                                                 |
|------------------------------------------|-------------------------------------------------------------------------------|
| `Domain/Entities/Car.cs`                 | Represents the assembled car entity.                                          |
| `Domain/Entities/Component.cs`           | Represents the car components (Wheel, Door, etc).                             |
| `Domain/Interfaces/ICarRepository.cs`    | Interface for car data access operations.                                     |
| `Domain/Interfaces/IComponentRepository.cs` | Interface for component data access operations.                              |
| `Domain/Interfaces/IComponentFactory.cs` | Interface for the factory that creates components.                            |
| `Domain/Interfaces/IDllAssembler.cs`     | Interface to interact with the external DLL.                                  |
| `Domain/Enums/ComponentType.cs`          | Enum for all component types (`Wheel`, `Motor`, etc.).                        |
| `Infrastructure/Repositories/CarRepository.cs` | Concrete implementation to handle car storage.                         |
| `Infrastructure/Repositories/ComponentRepository.cs` | Concrete implementation to handle component storage.             |
| `Application/Factories/ComponentFactory.cs` | Implements factory logic to create component objects.                    |
| `Application/Services/CarAssemblerService.cs` | Orchestrates assembling process, validations, and saving cars.         |
| `UI/Form1.cs`                            | UI logic and events for user interaction.                                     |
| `UI/Form1.Designer.cs`                   | Designer file that contains layout/controls of the form.                      |
| `AssemblerDLL/Assembler.cs`              | Exposes `void Assemble(List<string> instructions)` to assemble the car.       |
| `README.md`                              | Project documentation and architecture overview.                              |


---

## 🧱 Machines Overview

### 1️⃣ Generator Machine

#### 🔧 Purpose:
- Generates individual **Component** objects (Wheel, Door, Motor, Glass).
- Centralized using the **Factory Pattern**.
- Each component has:
  - ID
  - Type (Wheel, Motor, etc.)
  - Subtype (Steel, Electric, etc.)
  - Assembly Instruction

#### 🧠 Design Pattern:
> **Factory Pattern** – Enables easy addition of new components without changing creation logic.

#### 🔍 Example:
```csharp
var wheel = ComponentFactory.CreateComponent("wheel", "BigWheel");
