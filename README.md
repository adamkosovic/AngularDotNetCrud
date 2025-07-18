# 📚 BookApp - Angular & .NET CRUD Application

A full-stack web application for managing books and quotes, built with Angular frontend and .NET backend.

## 🚀 Features

- **User Authentication** - Login and registration system
- **Book Management** - Create, read, update, and delete books
- **Quote System** - favorite quotes
- **Dark Mode** - Toggle between light and dark themes
- **Responsive Design** - Works on desktop and mobile devices

## 🛠️ Tech Stack

### Frontend

- **Angular 19** - Modern web framework
- **Bootstrap 5** - CSS framework for styling
- **FontAwesome** - Icon library

### Backend

- **.NET 8** - Web API framework
- **Entity Framework Core** - Database ORM
- **PostgreSQL** - Database
- **ASP.NET Core Identity** - Authentication system

## 📁 Project Structure

```
TestPraktik/
├── book-app/                 # Angular frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/   # Reusable UI components
│   │   │   ├── pages/        # Page components
│   │   │   ├── services/     # API services
│   │   │   ├── models/       # Data models
│   │   │   └── guards/       # Route guards
│   │   └── styles.css
│   └── package.json
├── Controllers/              # .NET API controllers
├── Models/                   # .NET data models
├── Services/                 # .NET business logic
├── Data/                     # Database context
└── Program.cs               # .NET startup configuration
```

## 🚀 Getting Started

### Prerequisites

- Node.js (v18 or higher)
- .NET 9 SDK
- PostgreSQL database

### Frontend Setup

```bash
# Navigate to Angular project
cd bin/Debug/net9.0/book-app

# Install dependencies
npm install

# Start development server
ng serve
```

### Backend Setup

```bash
# Navigate to project root
cd /Users/adamkosovic/Desktop/TestPraktik

# Restore .NET packages
dotnet restore

# Run database migrations
dotnet ef database update

# Start the API server
dotnet run
```

### Database Configuration

Update the connection string in `Program.cs`:

```csharp
options.UseNpgsql("Host=localhost;Database=bookappdb;Username=postgres;Password=your_password")
```

## 📱 Usage

1. **Register** a new account or **Login** with existing credentials
2. **Add Books** with title, author, and publish date
3. **Manage Quotes** - save your favorite book quotes
4. **Toggle Dark Mode** using the theme button in the navbar
5. **Edit/Delete** your books and quotes as needed

## 🌐 Deployment

### Frontend (Netlify)

The Angular frontend can be deployed to Netlify for free hosting with automatic deployments.

📖 **See [DEPLOYMENT.md](DEPLOYMENT.md) for detailed deployment instructions**

Quick deployment:

```bash
# Build for production
cd bin/Debug/net9.0/book-app
npm run build

# Deploy to Netlify (drag dist/book-app folder to netlify.com)
```

### Backend (Azure/Railway/Heroku)

Deploy your .NET backend to your preferred hosting service:

- **Azure App Service** - Microsoft's cloud platform
- **Railway** - Simple deployment with database
- **Heroku** - Popular cloud platform

## 🔧 Development

### Adding New Features

- **Frontend**: Create components in `book-app/src/app/`
- **Backend**: Add controllers in `Controllers/` and services in `Services/`
- **Database**: Create models in `Models/` and run migrations

### Code Style

- **Angular**: Follow Angular style guide
- **C#**: Use C# coding conventions
- **Git**: Use conventional commit messages

## 🐛 Troubleshooting

### Common Issues

1. **Angular CLI schema error**: Run `npm install` in the Angular directory
2. **Database connection**: Check PostgreSQL is running and connection string is correct
3. **CORS errors**: Ensure backend is running on port 5020

### Useful Commands

```bash
# Clear Angular cache
ng cache clean

# Reset database
dotnet ef database drop
dotnet ef database update

# Check .NET version
dotnet --version
```

## 📄 License

This project is for educational purposes.

## 👨‍💻 Author

Created as a learning project for Angular and .NET development.

---

**Happy coding! 📚✨**
