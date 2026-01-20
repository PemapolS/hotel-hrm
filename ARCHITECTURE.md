# Hotel HRM System - Architecture Guide for Beginners

## Overview

This is a **Hotel Human Resource Management (HRM) System** built with **Blazor Server** and **.NET 10**. It helps manage hotel employees and their payroll. The application is designed with a clean, layered architecture to make it easy to maintain and potentially add a backend API in the future.

---

## What is Blazor?

**Blazor** is a web framework from Microsoft that lets you build interactive web applications using **C#** instead of JavaScript. In Blazor Server (what this app uses), the application runs on the server, and updates are sent to the browser in real-time using **SignalR** (a communication technology).

### Why Blazor Server?
- Write everything in C# (no need to switch between C# and JavaScript)
- Share code between server and client
- Full .NET runtime on the server
- Automatic state management

---

## Project Structure

The solution is organized into **4 main projects**, each with a specific responsibility:

```
hotel-hrm/
├── HotelHRM.Models/          # Data structures
├── HotelHRM.Data/            # Data storage
├── HotelHRM.Services/        # Business logic
└── HotelHRM.Web/             # User interface
```

---

## 1. HotelHRM.Models Project

**Purpose**: Defines the **data structures** (models) used throughout the application.

### What are Models?
Models are like blueprints for data. They define what information we store and how it's structured.

### Files:

#### `Employee.cs`
Represents a hotel employee with properties like:
- `Id` - Unique identifier
- `FirstName`, `LastName` - Name information
- `Email`, `PhoneNumber` - Contact details
- `Department`, `Position` - Job information
- `HireDate` - When they were hired
- `BaseSalary` - Annual salary
- `Status` - Whether they're Active, On Leave, or Terminated

```csharp
// Example: Creating an employee
var employee = new Employee {
    FirstName = "John",
    LastName = "Doe",
    Email = "john.doe@hotel.com",
    Department = "Front Desk",
    BaseSalary = 45000
};
```

#### `PayrollRecord.cs`
Represents a payroll payment with:
- `EmployeeId` - Which employee this is for
- `PayPeriodStart/End` - The period covered
- `BaseSalary` - Base pay for the period
- `Bonus`, `Deductions` - Additional amounts
- `GrossPay` - Total before deductions
- `NetPay` - Final amount paid
- `Status` - Pending, Processed, or Paid

#### `User.cs`
Represents a system user (note: authentication was removed, so this is currently unused)

### Key Concepts:
- **Properties**: Variables that store data (like `FirstName`)
- **Enums**: Predefined choices (like `EmploymentStatus.Active`)
- **DateTime**: Represents dates and times

---

## 2. HotelHRM.Data Project

**Purpose**: Handles **data storage and retrieval**. Think of this as the "memory" of the application.

### What is a Repository?
A repository is a pattern that provides a clean interface for data operations (Create, Read, Update, Delete - CRUD). It separates data access logic from business logic.

### Files:

#### `Repositories/IEmployeeRepository.cs`
**Interface** (contract) that defines what operations are available for employees:
- `GetAllAsync()` - Get all employees
- `GetByIdAsync(id)` - Get one specific employee
- `AddAsync(employee)` - Add a new employee
- `UpdateAsync(employee)` - Update existing employee
- `DeleteAsync(id)` - Remove an employee

**What's an Interface?** It's like a contract that says "any class implementing this must provide these methods."

#### `Repositories/InMemoryEmployeeRepository.cs`
**Implementation** that stores employee data in memory (RAM) using a `List<Employee>`.

**Important**: Data is lost when the app restarts because it's only in memory. This is fine for development/testing.

```csharp
// Simplified example of how it works
private List<Employee> _employees = new();  // Stores employees in memory

public Task<Employee> AddAsync(Employee employee) {
    employee.Id = _nextId++;  // Assign unique ID
    _employees.Add(employee);  // Add to list
    return Task.FromResult(employee);
}
```

The repository also **seeds** (pre-populates) with 3 sample employees when the app starts.

#### `Repositories/IPayrollRepository.cs` & `InMemoryPayrollRepository.cs`
Same concept as employee repositories, but for payroll records.

#### `Repositories/IUserRepository.cs` & `InMemoryUserRepository.cs`
Stores user accounts (currently unused since authentication was removed).

### Key Concepts:
- **In-Memory Storage**: Data lives in RAM, fast but temporary
- **Singleton Pattern**: Only one instance exists for the entire application
- **Async/Await**: Operations that don't block the application while waiting

---

## 3. HotelHRM.Services Project

**Purpose**: Contains **business logic** - the rules and operations that make the application work.

### What is Business Logic?
The "rules" of your application. For example: "How do we calculate net pay?" or "Who can modify employee data?"

### Files:

#### `Services/IEmployeeService.cs` & `EmployeeService.cs`
Handles employee-related operations:
- Getting all employees
- Adding new employees
- Updating employee information
- Deleting employees

**Why a separate service?** It sits between the UI and data layers. You could add validation, logging, or other logic here without changing the UI or repository.

```csharp
// Example: The service uses the repository
public class EmployeeService : IEmployeeService {
    private readonly IEmployeeRepository _repository;
    
    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() {
        // Could add business logic here (filtering, sorting, etc.)
        return await _repository.GetAllAsync();
    }
}
```

#### `Services/IPayrollService.cs` & `PayrollService.cs`
Handles payroll operations:
- **Processing payroll**: Creates a new payroll record
- **Calculating pay**: Uses the formula `NetPay = BaseSalary + Bonus - Deductions`
- Getting all payroll records

Example of business logic:
```csharp
public async Task ProcessPayrollAsync(int employeeId, ...) {
    var employee = await _employeeRepository.GetByIdAsync(employeeId);
    
    // Calculate pay based on period
    var monthlyBaseSalary = employee.BaseSalary / 12;
    var grossPay = monthlyBaseSalary + bonus;
    var netPay = grossPay - deductions;
    
    // Create record
    var record = new PayrollRecord {
        EmployeeId = employeeId,
        GrossPay = grossPay,
        NetPay = netPay,
        // ...
    };
    
    await _payrollRepository.AddAsync(record);
}
```

### Dependencies:
This project references both `HotelHRM.Models` (for data structures) and `HotelHRM.Data` (to access repositories).

---

## 4. HotelHRM.Web Project

**Purpose**: The **user interface** - what users see and interact with. This is a Blazor Server application.

### Structure:

```
HotelHRM.Web/
├── Program.cs                    # App startup and configuration
├── Components/
│   ├── App.razor                 # Root component (HTML structure)
│   ├── Routes.razor              # URL routing
│   ├── _Imports.razor            # Shared using statements
│   ├── Layout/
│   │   ├── MainLayout.razor      # Page layout template
│   │   └── NavMenu.razor         # Navigation sidebar
│   └── Pages/
│       ├── Home.razor            # Home page
│       ├── Employees.razor       # Employee management
│       ├── Payroll.razor         # Payroll management
│       └── Error.razor           # Error page
└── wwwroot/                      # Static files (CSS, images)
```

---

### Key Files Explained:

#### `Program.cs`
The **entry point** - this runs when the app starts.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Blazor components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register our services (Dependency Injection)
builder.Services.AddSingleton<IEmployeeRepository, InMemoryEmployeeRepository>();
builder.Services.AddSingleton<IPayrollRepository, InMemoryPayrollRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

var app = builder.Build();

// Configure HTTP pipeline
app.UseStaticFiles();      // Serve CSS, images
app.UseAntiforgery();      // Security

// Map Blazor components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();  // Start the app
```

**Dependency Injection**: This is how .NET provides objects to your code. Instead of creating objects yourself, you ask for them and the framework provides them.

- **Singleton**: One instance for the whole application (repositories)
- **Scoped**: One instance per user request/connection (services)

---

#### `Components/App.razor`
The root HTML structure. Defines the `<html>`, `<head>`, and `<body>` tags.

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <link rel="stylesheet" href="bootstrap/bootstrap.min.css" />
    <link rel="stylesheet" href="app.css" />
</head>
<body>
    <Routes />  <!-- This loads the routing component -->
    <script src="_framework/blazor.web.js"></script>
</body>
</html>
```

---

#### `Components/Routes.razor`
Handles **URL routing** - which page to show based on the URL.

```razor
<Router AppAssembly="typeof(Program).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="routeData" DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

When you visit `/employees`, it finds the page with `@page "/employees"` and displays it.

---

#### `Components/Layout/MainLayout.razor`
The **page template** used by all pages. Contains the navigation menu and content area.

```razor
<div class="page">
    <div class="sidebar">
        <NavMenu />  <!-- Navigation menu -->
    </div>
    
    <main>
        <article class="content px-4">
            @Body  <!-- Page content goes here -->
        </article>
    </main>
</div>
```

`@Body` is replaced with the actual page content (Home, Employees, etc.).

---

#### `Components/Layout/NavMenu.razor`
The **navigation sidebar** with links to different pages.

```razor
<nav>
    <NavLink href="" Match="NavLinkMatch.All">
        Home
    </NavLink>
    <NavLink href="employees">
        Employees
    </NavLink>
    <NavLink href="payroll">
        Payroll
    </NavLink>
</nav>
```

`NavLink` automatically highlights the current page.

---

#### `Components/Pages/Home.razor`
The **home page** (`/`). Shows a welcome message and links to other sections.

```razor
@page "/"

<PageTitle>Hotel HRM</PageTitle>

<div class="container mt-4">
    <h1>Hotel HRM System</h1>
    <p class="lead">Welcome to the Hotel Human Resource Management System</p>
    
    <div class="row mt-4">
        <div class="col-md-6">
            <div class="card">
                <div class="card-body">
                    <h5>Personnel Management</h5>
                    <a href="/employees" class="btn btn-primary">View Employees</a>
                </div>
            </div>
        </div>
        <!-- ... more cards ... -->
    </div>
</div>
```

**Syntax explained**:
- `@page "/"` - This page responds to the root URL
- HTML mixed with C# (that's Blazor!)
- Bootstrap CSS classes for styling

---

#### `Components/Pages/Employees.razor`
The **employee management page**. This is more complex - it lets you:
- View all employees in a table
- Add new employees
- Edit existing employees
- Delete employees

**Key sections**:

1. **Page directive and dependencies**:
```razor
@page "/employees"
@inject IEmployeeService EmployeeService
@rendermode InteractiveServer
```
- `@page` - URL route
- `@inject` - Dependency injection (get the service)
- `@rendermode` - Enable interactive features

2. **Add/Edit form**:
```razor
@if (showAddForm || selectedEmployee != null)
{
    <EditForm Model="employeeForm" OnValidSubmit="SaveEmployee">
        <InputText @bind-Value="employeeForm.FirstName" />
        <InputText @bind-Value="employeeForm.LastName" />
        <!-- ... more inputs ... -->
        <button type="submit">Save</button>
    </EditForm>
}
```
- `EditForm` - Blazor form component
- `@bind-Value` - Two-way data binding (changes update the object)
- `OnValidSubmit` - Method to call when form is submitted

3. **Employee table**:
```razor
<table class="table">
    @foreach (var employee in displayedEmployees)
    {
        <tr>
            <td>@employee.FullName</td>
            <td>@employee.Email</td>
            <td>
                <button @onclick="() => EditEmployee(employee)">Edit</button>
                <button @onclick="() => DeleteEmployee(employee.Id)">Delete</button>
            </td>
        </tr>
    }
</table>
```
- `@foreach` - Loop through employees
- `@employee.FullName` - Display data
- `@onclick` - Handle button clicks

4. **Code section**:
```razor
@code {
    private IEnumerable<Employee>? employees;
    private Employee employeeForm = new();
    private bool showAddForm = false;
    
    protected override async Task OnInitializedAsync()
    {
        await LoadEmployees();  // Load data when page starts
    }
    
    private async Task LoadEmployees()
    {
        employees = await EmployeeService.GetAllEmployeesAsync();
        displayedEmployees = employees;
    }
    
    private async Task SaveEmployee()
    {
        if (selectedEmployee == null) {
            await EmployeeService.AddEmployeeAsync(employeeForm);
        } else {
            await EmployeeService.UpdateEmployeeAsync(employeeForm);
        }
        await LoadEmployees();
    }
}
```

**Code concepts**:
- `OnInitializedAsync()` - Lifecycle method that runs when the component loads
- `async Task` - Asynchronous methods
- State variables control what's displayed

---

#### `Components/Pages/Payroll.razor`
Similar to Employees page, but for payroll:
- View payroll records
- Process new payroll (calculate and create records)
- Shows gross pay, deductions, and net pay

**Unique feature - payroll processing**:
```razor
<EditForm Model="payrollForm" OnValidSubmit="ProcessPayroll">
    <InputSelect @bind-Value="payrollForm.EmployeeId">
        @foreach (var emp in employees)
        {
            <option value="@emp.Id">@emp.FullName</option>
        }
    </InputSelect>
    <InputDate @bind-Value="payrollForm.PeriodStart" />
    <InputDate @bind-Value="payrollForm.PeriodEnd" />
    <InputNumber @bind-Value="payrollForm.Bonus" />
    <InputNumber @bind-Value="payrollForm.Deductions" />
</EditForm>
```

Different input types for different data types!

---

### `wwwroot/` - Static Files

Contains files served directly to browsers:
- `app.css` - Custom styles
- `bootstrap/` - Bootstrap CSS framework for styling

---

## How Data Flows Through the Application

Let's trace what happens when you add a new employee:

1. **User fills out form** on `Employees.razor` page
2. **User clicks "Save"** → triggers `SaveEmployee()` method
3. **Component calls service**: `EmployeeService.AddEmployeeAsync(employeeForm)`
4. **Service validates and processes**, then calls repository
5. **Repository adds to in-memory list**: `_employees.Add(employee)`
6. **Component reloads data**: `LoadEmployees()` fetches updated list
7. **UI updates automatically** showing the new employee in the table

```
User → UI Component → Service → Repository → In-Memory Storage
                     ↓
                  UI Updates
```

---

## Key Blazor Concepts

### 1. **Components**
Reusable UI pieces. Every `.razor` file is a component. They can contain:
- HTML markup
- C# code
- Parameters (inputs)
- Events (outputs)

### 2. **Data Binding**
Automatically sync data between code and UI:
- **One-way**: `@employee.Name` (display only)
- **Two-way**: `@bind-Value="employee.Name"` (both ways)

### 3. **Event Handling**
Respond to user actions:
```razor
<button @onclick="HandleClick">Click Me</button>

@code {
    private void HandleClick() {
        // Do something
    }
}
```

### 4. **Lifecycle Methods**
Methods that run at specific times:
- `OnInitializedAsync()` - When component first loads
- `OnParametersSet()` - When parameters change
- `OnAfterRender()` - After component renders

### 5. **Render Modes**
- **InteractiveServer**: Component can respond to events, runs on server
- **Static**: Just renders HTML, no interactivity

---

## Project Configuration Files

### `HotelHRM.sln`
**Solution file** - groups all projects together. Open this in Visual Studio or Rider.

### `*.csproj` Files
**Project files** - define each project's settings:
- Target framework (net10.0)
- Dependencies (what other projects/packages it needs)
- Build settings

Example (`HotelHRM.Web.csproj`):
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <ProjectReference Include="..\HotelHRM.Services\HotelHRM.Services.csproj" />
  </ItemGroup>
</Project>
```

### `appsettings.json`
**Configuration file** - stores app settings (currently minimal).

---

## Docker Support

### `Dockerfile`
Instructions to build a Docker container:

```dockerfile
# Build stage - compile the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
COPY . .
RUN dotnet restore
RUN dotnet build -c Release

# Runtime stage - run the application
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HotelHRM.Web.dll"]
```

**What's Docker?** Packages your app with everything it needs to run (OS, .NET runtime, dependencies) into a portable container.

---

## Kubernetes Support

### `k8s/deployment.yaml`
Configuration for deploying to Kubernetes:
- How many instances to run
- Resource limits (CPU, memory)
- Health checks
- Networking

**What's Kubernetes?** Orchestration system for running containerized applications at scale.

---

## Common Patterns Used

### 1. **Repository Pattern**
Abstracts data access behind interfaces. Benefits:
- Swap implementations (in-memory → database) without changing other code
- Easier testing
- Clear separation of concerns

### 2. **Dependency Injection**
Objects declare what they need, framework provides it. Benefits:
- Loose coupling
- Easier testing (can provide mock objects)
- Centralized configuration

### 3. **Async/Await**
Handle long-running operations without blocking:
```csharp
public async Task<Employee> GetEmployeeAsync(int id) {
    await Task.Delay(100);  // Simulate database call
    return employee;
}
```

### 4. **Model-View-ViewModel (MVVM-ish)**
- **Model**: Data structures (Employee, PayrollRecord)
- **View**: Razor components (UI)
- **ViewModel/Code**: `@code` blocks in components

---

## How to Run the Application

### Prerequisites:
- .NET 10 SDK installed
- A code editor (Visual Studio, VS Code, or Rider)

### Steps:

1. **Navigate to the Web project**:
```bash
cd HotelHRM.Web
```

2. **Restore dependencies**:
```bash
dotnet restore
```

3. **Build the application**:
```bash
dotnet build
```

4. **Run the application**:
```bash
dotnet run
```

5. **Open browser**:
Navigate to `http://localhost:5293`

---

## Understanding the Architecture Benefits

### Why Separate Projects?

1. **HotelHRM.Models**: 
   - Can be shared across multiple applications
   - Changes here don't force rebuilding everything

2. **HotelHRM.Data**: 
   - Easy to swap storage (in-memory → SQL → cloud)
   - Data logic separate from business logic

3. **HotelHRM.Services**: 
   - Business rules in one place
   - Can add features without touching UI or data layers

4. **HotelHRM.Web**: 
   - UI can change independently
   - Could add API project using same services

### Future Possibilities

Because of this architecture, you could easily:
- Add a REST API for mobile apps
- Replace in-memory storage with SQL Server
- Add authentication/authorization
- Deploy to cloud (Azure, AWS)
- Scale horizontally (run multiple instances)

---

## Troubleshooting Tips

### "Service not found" errors
Check `Program.cs` - make sure service is registered:
```csharp
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
```

### Data disappears on restart
This is expected with in-memory storage. To persist:
- Add database (Entity Framework + SQL Server)
- Or use file storage

### Port already in use
Kill the process or change port in `launchSettings.json`

### Build errors
Run `dotnet clean` then `dotnet build`

---

## Glossary

- **Blazor**: Web framework for building interactive web apps with C#
- **Component**: Reusable piece of UI (`.razor` file)
- **Dependency Injection (DI)**: Pattern where objects receive their dependencies from external source
- **Razor**: Syntax that mixes HTML and C#
- **Repository**: Pattern that provides interface for data operations
- **Service**: Contains business logic
- **Singleton**: One instance for entire application lifetime
- **Scoped**: One instance per user connection/request
- **Async/Await**: Handle long-running operations without blocking
- **CRUD**: Create, Read, Update, Delete operations

---

## Next Steps for Learning

1. **Modify existing features**: Change how payroll is calculated
2. **Add new properties**: Add phone number to employees
3. **Create new pages**: Add a departments page
4. **Add database**: Replace in-memory with Entity Framework
5. **Add validation**: Ensure email format is correct
6. **Add authentication**: Bring back login system (properly)

---

## Additional Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [ASP.NET Core Fundamentals](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) (for adding database)

---

**Questions?** This architecture makes it easy to learn step-by-step. Start with understanding Models, then Data, then Services, and finally the Web UI. Each layer builds on the previous!
