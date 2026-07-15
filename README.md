# ItransitionTemplates

A web application for creating, managing, and sharing customizable templates and questionnaires. Users can build templates with various question types (single-line, multiline, numbers, checkboxes, multiple choice), control visibility, and collect responses.

## Features

- **Template Builder** — Create templates with drag-and-drop question types
- **Response Collection** — Share templates and collect structured responses
- **Access Control** — Public/private templates with admin management
- **Search & Discovery** — Find templates by title, topic, or creator
- **Like System** — Like and favorite templates
- **Real-time Validation** — Inline form validation with toast notifications
- **Responsive Design** — Mobile-first UI with Tailwind CSS

## Tech Stack

| Layer | Technology | Version |
|-------|-----------|---------|
| **Runtime** | .NET | 8.0 |
| **Framework** | ASP.NET Core MVC | 8.0 |
| **ORM** | Entity Framework Core | 8.0.10 |
| **Database** | MySQL (Pomelo Provider) | 8.0.2 |
| **CSS** | Tailwind CSS | 3.4 (CDN) |
| **JS** | Alpine.js | 3.x (CDN) |
| **Toasts** | Sonner.js | latest (CDN) |
| **Font** | Inter | Google Fonts |
| **Testing** | xUnit + Moq | latest |
| **Test DB** | EF Core InMemory | 8.0.x |

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL Server 8.x
- Node.js (optional, for Tailwind CLI builds)

## Getting Started

### 1. Clone the repository

```bash
git clone <repository-url>
cd itransitionProjectDefenseTemplateGenerator
```

### 2. Configure database connection

Using .NET User Secrets (recommended):

```bash
dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=itransition_template_manager;User=root;Password=yourpassword"
```

Or edit `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=itransition_template_manager;User=root;Password=yourpassword"
  }
}
```

### 3. Create the database

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet watch run
```

The app will start at `http://localhost:5148`.

## Testing

### Run unit tests

```bash
dotnet test tests/ItransitionTemplates.Tests.Unit/
```

### Run integration tests

```bash
dotnet test tests/ItransitionTemplates.Tests.Integration/
```

### Run all tests

```bash
dotnet test
```

## Project Structure

```
├── Controllers/          # MVC controllers (Home, User, Template, Question, Response)
├── Services/             # Business logic layer
│   ├── User/             # User authentication & management
│   ├── Template/         # Template CRUD & search
│   ├── Question/         # Question management
│   ├── Response/         # Response collection
│   ├── Topic/            # Topic management
│   └── Admin/            # Admin role management
├── Models/               # Data models & exceptions
├── Data/                 # EF Core DbContext
├── Migrations/           # Database migrations
├── Middleware/            # Exception handling middleware
├── Utils/                # Utilities (JsonResponse, HashText, Session)
├── Views/                # Razor views
│   ├── Shared/           # Layout, partials, error page
│   ├── Home/             # Home page
│   ├── User/             # Login, SignUp
│   └── Template/         # Create, View templates
├── wwwroot/              # Static files (JS, CSS, images)
│   ├── js/               # JavaScript modules
│   └── css/              # Tailwind CSS
├── tests/
│   ├── ItransitionTemplates.Tests.Unit/        # Unit tests (xUnit + InMemory)
│   └── ItransitionTemplates.Tests.Integration/ # Integration tests (WebApplicationFactory)
└── .planning/            # Project planning docs (GSD workflow)
```

## Error Handling

The application uses a structured error handling system:

- **Global middleware** catches unhandled exceptions and returns consistent JSON responses
- **Service exceptions** (`ServiceException`) carry error codes and user-friendly messages
- **Frontend** displays errors via toast notifications (Sonner.js)
- **Error format**: `{ error: { code, message, details } }`

## License

This project is for educational purposes.
