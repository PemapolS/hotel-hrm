# Hotel HRM - Human Resource Management System

A basic HRM application built with Blazor .NET featuring personnel record management and payroll calculation modules. The application is architected to support future backend API integration with AWS and Kubernetes containerization.

**Note:** This is currently a development/demo version without authentication. All features are openly accessible for testing and demonstration purposes.

## Features

### Personnel Record Module
- Employee information management (CRUD operations)
- Track employee details including:
  - Personal information (name, email, phone)
  - Employment details (department, position, hire date)
  - Salary information
  - Employment status (Active, On Leave, Terminated)
- Pre-loaded with sample employee data

**Note:** Currently uses in-memory storage. Data is reset when the application restarts.

### Payroll Calculation Module
- Basic payroll processing
- Salary calculations based on pay period
- Support for bonuses and deductions
- Gross and net pay calculations
- Payroll record tracking and history

## Architecture

The application follows a layered architecture pattern to support future scalability and API integration:

```
HotelHRM.Web         - Blazor Server UI Layer
HotelHRM.Services    - Business Logic Layer
HotelHRM.Data        - Data Access Layer (Repository Pattern)
HotelHRM.Models      - Domain Models
```

This architecture enables:
- Easy transition to a separate API backend
- Cloud deployment with AWS
- Containerized deployment with Kubernetes
- Scalable microservices architecture in the future

For a detailed explanation of the architecture and components, see [ARCHITECTURE.md](ARCHITECTURE.md).

## Technology Stack

- **.NET 10.0** - Framework
- **Blazor Server** - UI Framework
- **C#** - Programming Language
- **Docker** - Containerization
- **Kubernetes** - Container Orchestration

## Prerequisites

- .NET 10.0 SDK or later
- Docker (for containerization)
- Kubernetes cluster (for deployment)

## Getting Started

### Local Development

1. Clone the repository:
```bash
git clone https://github.com/PemapolS/hotel-hrm.git
cd hotel-hrm
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run the application:
```bash
cd HotelHRM.Web
dotnet run
```

5. Open your browser and navigate to `http://localhost:5293`

## Docker Deployment

### Build Docker Image

```bash
docker build -t hotel-hrm:latest .
```

### Run with Docker

```bash
docker run -p 8080:8080 hotel-hrm:latest
```

Access the application at `http://localhost:8080`

## Kubernetes Deployment

### Prerequisites
- Kubernetes cluster (local or cloud-based)
- kubectl configured to access your cluster

### Deploy to Kubernetes

1. Build and tag the Docker image:
```bash
docker build -t hotel-hrm:latest .
```

2. If using a remote registry (e.g., AWS ECR, Docker Hub):
```bash
docker tag hotel-hrm:latest <your-registry>/hotel-hrm:latest
docker push <your-registry>/hotel-hrm:latest
```

3. Update the image reference in `k8s/deployment.yaml` if using a remote registry

4. Deploy to Kubernetes:
```bash
kubectl apply -f k8s/deployment.yaml
```

5. Check deployment status:
```bash
kubectl get pods
kubectl get services
```

6. Access the application:
```bash
# For LoadBalancer type service
kubectl get service hotel-hrm-service

# For local testing with port-forward
kubectl port-forward service/hotel-hrm-service 8080:80
```

## AWS Deployment (Future Ready)

The application is designed to be deployed on AWS with the following potential architecture:

### Option 1: AWS ECS/Fargate
- Container orchestration with AWS ECS
- Serverless containers with Fargate
- Application Load Balancer for traffic distribution
- RDS for database (when migrating from in-memory storage)

### Option 2: AWS EKS
- Managed Kubernetes service
- Auto-scaling with Kubernetes HPA
- Integration with AWS services (RDS, S3, etc.)

### Option 3: AWS App Runner
- Simplified container deployment
- Automatic scaling and load balancing
- Direct deployment from source code or container image

## Future Enhancements

The current architecture supports the following future enhancements:

1. **Backend API Separation**
   - Extract business logic into RESTful API
   - Separate frontend and backend deployments
   - Enable multiple client applications

2. **Database Integration**
   - Replace in-memory repositories with database repositories
   - Support for SQL Server, PostgreSQL, or DynamoDB
   - Entity Framework Core for ORM

3. **Additional HRM Modules**
   - Leave management
   - Attendance tracking
   - Performance reviews
   - Training and development
   - Authentication and user management

4. **Cloud-Native Features**
   - Distributed caching (Redis)
   - Message queuing (AWS SQS, RabbitMQ)
   - Monitoring and logging (CloudWatch, Application Insights)

## Project Structure

```
hotel-hrm/
├── HotelHRM.Web/              # Blazor Server application
│   ├── Components/
│   │   ├── Pages/             # Razor pages
│   │   └── Layout/            # Layout components
│   └── Program.cs             # Application entry point
├── HotelHRM.Services/         # Business logic services
│   └── Services/
├── HotelHRM.Data/             # Data access layer
│   └── Repositories/          # Repository implementations
├── HotelHRM.Models/           # Domain models
├── k8s/                       # Kubernetes manifests
│   └── deployment.yaml
├── Dockerfile                 # Docker configuration
└── HotelHRM.sln              # Solution file
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the [MIT License](LICENSE).