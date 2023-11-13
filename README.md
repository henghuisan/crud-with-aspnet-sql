<h1 align="center" id="title">(2) CRUD Operations using ASP NET Web API and SQL Server | Database Connection Using ODBC</h1>

## Tutorial
[![CRUD Operations using ASP NET Web API and SQL Server | Database Connection Using ODBC](https://img.youtube.com/vi/cVz5oWRGbYA/0.jpg)](https://www.youtube.com/watch?v=cVz5oWRGbYA)

## Getting Started
### 1. Inititalize Project
- Create a <b>ASP.NET Core Web API</b> project
- Delete unused files
  - WeatherForecastController.cs
  - WeatherForecast.cs

### 2. Connect Database
- Select "Tools" -> "Connect to Database..." -> Enter server name <b>LAPTOP-JP6QO1OQ\SQLEXPRESS</b> -> Select database (make sure the database is created in SQL Server)
- Create table
  ```
  CREATE TABLE (
  properties...
  )
  ```
- Open appsettings.json, append the code
    ```
    "ConnectionStrings": {
      "SqlServerDb": "the connection string"
    }
    ```

### 3. Create Controller
- Right click <b>Controller</b> folder -> Select <b>Add</b>, then <b>Controller</b> -> Select <b>API</b> from Comman directory -> Select <b>API Controller - Empty</b> -> Enter file name
- Create constructor (shortkey: ctor)
  ```
  public NameController(IConfiguration configuration)
  {

  }
  ```
- Connect database
  - Create ``` private readonly string connectionString; ```
  - Configure
      ```
      public NameController(IConfiguration configuration)
      {
        connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
      }
      ```

### 4. Create Model
- Create <b>Model</b> folder in root directory
- Create <b>Dto</b> class first by selecting <b>Add</b>, then <b>Class</b>
  ```
  // For example
  namespace WebApp.Models
  {
    public class ProductDto
    {
      [Required]
      public string Name { get; set; } = ""; // or public string? Name { get; set; } ( define NULL value if field is not required)
      ...
    }
  }
  ```
- Create <b>Class</b> for the model
  ```
  // For example
  namespace WebApp.Models
  {
    public class Product
    {
      public int Id { get; set; };
      public string Name { get; set; } = ""; // or public string? Name { get; set; } ( define NULL value if field is not required)
      ...
    }
  }
  ```

### 5. Write API
You can refer to the project..
