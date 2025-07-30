# Employment System API

An advanced employment management system built with ASP.NET Core Web API.  
The system allows companies to post job vacancies, users to apply for jobs, and provides a full messaging system between users, in addition to skill and experience validation features.

---

## 📌 Features

- 🔐 User registration and authentication (JWT)
- 🧑‍💼 Company & Employee management
- 📄 Job posting by companies
- 📝 Apply to jobs by users
- 💬 Real-time-style chat/messages between users
- 🧠 User skills & experiences with confirmation
- 🔍 Search & filter companies and users
- 📑 Admin and role-based access system
- 🗃️ Clean and structured API with 26+ endpoints

---

## ⚙️ Tech Stack

- ASP.NET Core Web API (.NET 7+)
- Entity Framework Core
- SQL Server
- JWT Authentication
- LINQ & Filtering
- Service Layer Pattern for clean architecture
---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/hadi-dotnet/EmploymentSystem.API.git
```

### 2. Navigate to the project folder
```bash
cd EmploymentSystem.API
```

### 3. Setup the database
- Create SQL Server DB
- Add connection string to `appsettings.json`
- Run migrations:
```bash
dotnet ef database update
```

### 4. Run the API
```bash
dotnet run
```

The API will start at: `https://localhost:5001`

---

## 🧪 API Testing

> ✅ Swagger will be added soon  
> ✅ Postman collection coming soon  

For now, use Postman to manually test endpoints using Bearer token authentication.

---

## 🔮 Future Improvements

- Add Swagger UI
- Add Postman collection with all endpoints
- Implement global exception handler
- Add unit testing

---

## 📂 Folder Structure

```
EmploymentSystem.API/
│
├── Controllers/
├── Data/
├── DTOs/
├── Entities/
├── Interfaces/
├── Services/
├── Helpers/
├── appsettings.json
└── Program.cs
```

---

## 🙌 Author

Built with love by [Hadi](https://github.com/hadi-dotnet) ❤️
