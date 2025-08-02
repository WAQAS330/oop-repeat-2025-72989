
# 🚗 AutoCraft — Workshop Management System

AutoCraft is a multi-layered .NET application designed to manage a vehicle service workshop. The project follows modern object-oriented principles and separates concerns across API, Razor Pages, Domain logic, and Testing layers.

## 🛠️ Technologies Used

- **.NET 6+**
- **ASP.NET Core Web API**
- **ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **C#**
- **xUnit** (for testing)
- **RESTful Architecture**
- **DTO Pattern** and **Exception Handling Middleware**


## ▶️ How to Run the Project Locally

### 🧱 Prerequisites

- [.NET 7 SDK or newer](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) 

### 🚀 Steps to Run

1. **Clone the repository**:
   git clone https://github.com/WAQAS330/oop-repeat-2025-72989.git
   cd oop-repeat-2025-72989/OOP


2. **Open the solution** in Visual Studio:

   * Open `AutoCraft.sln`

3. **Set Startup Projects**:

   * Right-click the solution → Set startup projects → Select `AutoCraft.API` and/or `AutoCraft.Razor`


## 📌 Key Features

* Add, update, delete cars, customers
* Clean separation between UI, API, Domain, and Testing layers
* Exception handling for validation, not found, and DB errors
* DTOs for safe data transfer
* Unit tests for business logic


## 🧪 Testing

Run tests using Visual Studio's Test Explorer or with the CLI:

dotnet test