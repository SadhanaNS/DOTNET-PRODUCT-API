# 🛒 Product Catalogue API

> A simple **ASP.NET Core 8 Web API** built to practice **CI/CD with Azure DevOps** and cloud deployment to **Azure App Service**.

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://dev.azure.com/)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Swagger](https://img.shields.io/badge/docs-Swagger%20UI-green)](https://product-api-app.azurewebsites.net/index.html)

---

## 🌐 Live Demo

**Swagger UI:** [https://product-api-app.azurewebsites.net/index.html](https://product-api-app.azurewebsites.net/index.html)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [CI/CD Pipeline](#cicd-pipeline)
- [Pipeline Stages](#pipeline-stages)
- [Environment Variables](#environment-variables)
- [Learnings](#learnings)

---

## 📖 About the Project

A RESTful Product Catalogue API built with ASP.NET Core 8, designed as a hands-on project to learn and implement:

- ASP.NET Core Web API development with C#
- Azure DevOps CI/CD pipelines (YAML)
- SonarCloud code quality scanning
- Azure App Service deployment
- Artifact management in Azure DevOps

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core 8 |
| API Docs | Swagger / Swashbuckle (OAS3) |
| CI/CD | Azure DevOps YAML Pipelines |
| Code Quality | SonarCloud |
| Hosting | Azure App Service |
| Runtime | .NET 8 |

---

## 📁 Project Structure

```
DOTNET-PRODUCT-API/
├── ProductCatalogue.csproj      # build config & NuGet packages
├── ProductCatalogue.sln         # solution file
├── Program.cs                   # app startup & middleware
├── appsettings.json             # base configuration
├── appsettings.Development.json # dev environment config
├── Controllers/                 # API endpoint handlers
│   ├── HealthController.cs      # GET /Health
│   └── ProductsController.cs    # CRUD /Products
├── Properties/
│   └── launchSettings.json      # local dev settings
└── azure-pipelines.yml          # CI/CD pipeline definition
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio / VS Code](https://code.visualstudio.com/)

### Run Locally

```bash
# Clone the repo
git clone https://github.com/SadhanaNS/DOTNET-PRODUCT-API.git
cd DOTNET-PRODUCT-API

# Restore packages
dotnet restore

# Build
dotnet build

# Run
dotnet run

# Swagger UI available at:
# http://localhost:5000/index.html
```

---

## 📡 API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/Health` | Health check |
| GET | `/Products` | Get all products |
| GET | `/Products/{id}` | Get product by ID |
| POST | `/Products` | Create new product |
| PUT | `/Products/{id}` | Update product |
| DELETE | `/Products/{id}` | Delete product |

---

## ⚙️ CI/CD Pipeline

### Pipeline Flow

```
Code push to dev branch
        ↓
┌─────────────────────┐
│   Stage 1: Build    │  restore → build → publish → upload artifact
└─────────────────────┘
        ↓
┌─────────────────────┐
│  Stage 2: Deploy    │  download artifact → deploy to Azure App Service
└─────────────────────┘
        ↓
   App is Live 🚀
```

### Pipeline File

```yaml
trigger:
  - dev

pool:
  vmImage: ubuntu-latest

variables:
  buildConfiguration: 'Release'
  dotnetVersion: '8.0.x'

stages:
  - stage: Build
  - stage: Deploy
    dependsOn: Build
    condition: succeeded()
```

---

## 🔀 Pipeline Stages

### Stage 1 — Build

| Step | Command | Purpose |
|---|---|---|
| Install SDK | `UseDotNet@2` | Pin .NET 8 SDK |
| Restore | `dotnet restore` | Download NuGet packages |
| Build | `dotnet build` | Compile source code |
| Publish | `dotnet publish` | Create deployable zip |
| Upload | `PublishBuildArtifacts` | Store artifact in Azure DevOps |

### Stage 2 — Deploy

| Step | Task | Purpose |
|---|---|---|
| Download | `DownloadBuildArtifacts` | Fetch zip from Azure DevOps |
| Deploy | `AzureWebApp@1` | Push to Azure App Service |

---

## 🔑 Environment Variables

| Variable | Purpose |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | Switches config (Development / Production) |
| `ASPNETCORE_URLS` | Override port binding |
| `ConnectionStrings__DefaultConnection` | Database connection string |

> ⚠️ Never store secrets in `appsettings.json` — use Azure Key Vault or pipeline secret variables.

---

## 📚 Learnings

### .NET Concepts

- **.NET** = runtime (makes code run on the machine)
- **C#** = language (what you write code in)
- **ASP.NET Core** = web framework (handles HTTP, routing, JSON)

### DevOps Concepts Practiced

**Artifact Flow:**
```
dotnet publish → zip → PublishBuildArtifacts (ArtifactName: drop)
                              ↓
                    Azure DevOps artifact store
                              ↓
                    DownloadBuildArtifacts → AzureWebApp deploy
```

**Key Pipeline Variables:**
```
$(Build.ArtifactStagingDirectory)  → staged build outputs
$(System.ArtifactsDirectory)       → downloaded artifacts in deploy stage
$(System.DefaultWorkingDirectory)  → source code (NOT artifacts)
```

**Common Mistakes Fixed:**
- `--no-build` on publish fails when `obj/` folder doesn't exist on fresh agent
- `azureSubscription` is not a valid input on `DotNetCoreCLI@2` tasks
- Each stage runs on a **fresh agent** — files don't carry over unless uploaded as artifact
- Always pin SDK version with `UseDotNet@2` — hosted agents may pick up wrong version
- `publishLocation: Container` stores artifact inside Azure DevOps (no external storage needed)

### SonarCloud Notes

- Free tier supports **main branch only**
- Needs `organization` key in pipeline (not just `projectKey`)
- Must use matching task versions: `SonarCloudPrepare@2`, `SonarCloudAnalyze@2`, `SonarCloudPublish@2`
- `SonarCloudPrepare` must run **before** build, `SonarCloudAnalyze` must run **after** build
- Cannot mix `SonarQube*` and `SonarCloud*` tasks — pick one set

---

*Built for learning CI/CD with Azure DevOps — ProductCatalogue API deployed at [product-api-app.azurewebsites.net](https://product-api-app.azurewebsites.net/index.html)*
