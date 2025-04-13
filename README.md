# 🚗 Car Manufacturing System

A full-scale Windows Forms desktop application that simulates a car manufacturing and assembling system using:
- ✅ **SOLID principles**
- 🏭 **Factory Pattern** for component creation
- 🗃 **Repository Pattern** for data access
- 💉 **Dependency Injection**
- 🧠 Separation of concerns between UI, Logic, and Data

---

## 📦 Project Architecture
CarManufacturingSystem/
│
├── Domain/
│   ├── Entities/
│   │   ├── Component.cs
│   │   └── Car.cs
│   │
│   ├── Interfaces/
│   │   ├── IComponentFactory.cs
│   │   ├── IComponentRepository.cs
│   │   ├── ICarRepository.cs
│   │   └── IDllAssembler.cs
│   │
│   └── Enums/
│       └── ComponentType.cs
│
├── Infrastructure/
│   └── Repositories/
│       ├── ComponentRepository.cs
│       └── CarRepository.cs
│
├── Application/
│   ├── Factories/
│   │   └── ComponentFactory.cs
│   │
│   └── Services/
│       └── CarAssemblerService.cs
│
├── UI/
│   ├── Form1.cs
│   └── Form1.Designer.cs
│
├── AssemblerDLL/
│   └── Assembler.cs  // Exposes: void Assemble(List<string> instructions)
│
└── README.md


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
