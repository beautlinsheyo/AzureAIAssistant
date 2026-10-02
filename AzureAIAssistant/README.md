# Azure AI Smart Assistant

A full-stack AI-powered chat assistant built with ASP.NET Core, Entity Framework Core, and Azure OpenAI — deployed live on Azure App Service.

🔗 **Live Demo:** https://smart-assistant-app-a4e8hdctc9c2gxcz.westus3-01.azurewebsites.net

## Overview

This project is a ChatGPT-style assistant that lets users have multi-turn conversations with an AI model. It demonstrates a complete, production-style full-stack flow: a browser-based chat UI, a REST API backend, a persistent database, and a live integration with Azure's OpenAI service — all deployed and running in the cloud.

## Features

- 💬 Real-time chat interface with typing indicator and timestamps
- 🧠 Conversation memory — the AI remembers context within a session
- 🗄️ Persistent chat history stored in a SQLite database
- 🔒 Secrets managed securely (User Secrets locally, Azure App Settings in production)
- ✅ Input validation and graceful error handling (empty messages, network failures, AI service downtime)
- ☁️ Live deployment on Azure App Service with automatic database migrations on startup

## Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 8.0 Web API (C#) |
| Database | SQLite + Entity Framework Core |
| AI | Azure OpenAI (GPT-4.1-mini) |
| Frontend | HTML, CSS, vanilla JavaScript |
| Hosting | Azure App Service (Windows, Free tier) |
| Architecture | Controller → Service → Database pattern with dependency injection |

## Architecture
Browser (HTML/CSS/JS)
│
▼
AssistantController (API endpoints, validation)
│
▼
ChatService (business logic, builds conversation history)
│
├──► AppDbContext (EF Core) ──► SQLite Database
│
└──► AIService (HTTP client) ──► Azure OpenAI API


The backend follows a clean separation of concerns:
- **Controller** — handles HTTP requests/responses only
- **Service layer** — contains business logic, decoupled from both the database and the AI provider
- **DTOs** — `ChatRequest` shapes the API contract separately from the `ChatMessage` database entity

## Key Design Decisions

- **Stateless API, client-held conversation history**: Since LLM APIs have no built-in memory, the frontend maintains the conversation transcript and resends it with each request — the standard pattern used by LLM chat applications.
- **Service layer abstraction**: `IChatService` and `IAIService` interfaces allow the AI provider or database to be swapped without touching the controller.
- **Automatic migrations on startup**: `Database.Migrate()` runs on app launch, ensuring the database schema is always correct in any environment (local or cloud) without manual steps.
- **Layered validation**: Input is validated at the HTML layer, the JavaScript layer, and the backend (`[Required]`, `[StringLength]`) — since client-side validation alone can be bypassed.

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- An Azure account with an Azure OpenAI resource deployed

### Setup
1. Clone the repository
2. Add your Azure OpenAI credentials using .NET User Secrets:
```bash
   dotnet user-secrets set "AzureOpenAI:Endpoint" "your-endpoint-url"
   dotnet user-secrets set "AzureOpenAI:ApiKey" "your-api-key"
   dotnet user-secrets set "AzureOpenAI:DeploymentName" "your-deployment-name"
```
3. Run the app:
```bash
   dotnet run
```
4. Open the URL shown in the console — the database and tables are created automatically on first run.

## What I Learned

Building this project involved hands-on work with:
- Designing a layered backend architecture (Controller/Service/Data) with dependency injection
- Integrating a third-party AI API (Azure OpenAI) over HTTP, including authentication and JSON request/response handling
- Managing application secrets securely across local and cloud environments
- Debugging a live production issue (a missing database table in the deployed environment) using Azure's Log Stream
- Deploying and configuring a real cloud-hosted web application end-to-end

## Future Improvements

- [ ] Persist conversation history across page refreshes
- [ ] Add user authentication for multiple separate conversations
- [ ] Rate limiting to prevent API abuse
- [ ] Unit tests for service layer logic