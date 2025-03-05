# **The Good Samaritan System**  

## **Overview**  
The **Good Samaritan** is a request management system designed to facilitate patient assistance by connecting them with donors through a structured approval and coordination process. It streamlines the workflow for handling patient requests, verifying donor availability, and managing communication between admins, subleaders, and diallers.  

This system is built using **ASP.NET Web API** following the **Repository Pattern** to ensure maintainability, scalability, and clean architecture.  

---

## **Features**  
### **🔹 Request Handling**
- Users can **initiate requests** by entering patient details.  
- The system checks if a **patient already exists** before creating a new entry.  
- Requests are validated and submitted to the appropriate subleaders.  

### **🔹 Patient & Donor Management**
- Subleaders **receive and review requests** to determine available donors.  
- If donors are found, they are **contacted and connected** to the patient.  
- If additional assistance is required, a **Companion Servant** is assigned.  

### **🔹 Approval & Closure**
- Once the donation process is completed, requests are **approved and closed**.  
- If any issues arise, admins and subleaders can intervene.  

---

## **Technology Stack**  
| **Technology**  | **Usage**  |
|---------------|-----------|
| **ASP.NET Web API**  | Backend development |
| **C#**  | Primary programming language |
| **Entity Framework Core**  | ORM for database interactions |
| **SQL Server**  | Database management |
| **Repository Pattern**  | Code maintainability |
| **Swagger**  | API documentation |
| **JWT Authentication**  | Secure user authentication |
| **GitHub Codespaces (Optional)**  | Remote development |

---

## **System Workflow**  
1. **Patient Request Submission**  
   - Admins enter patient details and check if the patient already exists.  
   - Requests are validated and submitted.  

2. **Request Handling by Subleaders**  
   - Subleaders receive requests and verify donor availability.  
   - If donors are available, they are contacted.  
   - If no donors are available, the request is escalated.  

3. **Donor Connection & Finalization**  
   - Donors are connected to the patient.  
   - If needed, a Companion Servant is assigned.  
   - The process is confirmed, and the request is closed.  

---

## **Installation & Setup**  
### **🔹 Prerequisites**  
Ensure you have the following installed:  
- .NET 6+  
- SQL Server  
- Visual Studio / VS Code  
- Git  

### **🔹 Clone the Repository**  
```bash
git clone https://github.com/your-repo/good-samaritan.git
cd good-samaritan
```

### **🔹 Set Up the Database**  
1. Configure the **connection string** in `appsettings.json`.  
2. Apply database migrations:  
   ```bash
   dotnet ef database update
   ```

### **🔹 Run the Application**  
```bash
dotnet run
```
The API will be accessible at `http://localhost:5000` (or as configured).  

---

## **Deployment Options**  
You can deploy the system using:  
- **GitHub Codespaces** for cloud-based development.  
- **Docker** for containerized deployment.  
- **Azure App Service / AWS / DigitalOcean** for production hosting.  

---

## **Contributing**  
1. **Fork the repository**.  
2. **Create a feature branch** (`git checkout -b feature-name`).  
3. **Commit changes** (`git commit -m "Added new feature"`).  
4. **Push to GitHub** (`git push origin feature-name`).  
5. **Submit a Pull Request**.  

---
