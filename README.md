## Report: PRDH Coding Exercise

## WebApi

### 1. Design Choices:
- **Features-Based Structure**: The WebAPI adopts a features-based organization rather
  than the traditional layer-based approach. This means that related functionalities or
  domain-related actions are grouped together, making it easier to locate and maintain
  code for a specific feature.

- **Minimal Api**: The WebApi is structure in a way that the endpoint are separated by
  features.

- **WorkerTestLab BackgroundService**: The design introduces a background worker, `WorkerTestLab`, which runs continuously and fetches lab test data at regular intervals specified in `_appSettings.TimeSpan`.

- **Dependency Injection**: Leveraged the built-in DI container of .NET Core to provide necessary services and configurations, ensuring that the components remain loosely coupled, which is crucial for scalability and maintainability.

- **Service Pattern with Repositories**: Both the lab test and cases features use a service pattern backed by a repository. This abstraction allows for separating business logic from data access logic, making the system more maintainable.

- **Asynchronous Programming**: Used `async` and `await` throughout to ensure non-blocking code execution. This is crucial for improving throughput and scalability.

### 2. Challenges:
- **Error Handling**: Multiple checkpoints were introduced to handle potential errors, such as issues with HTTP response, stream reading, or JSON deserialization. 
- **Data Processing**: Extracting the earliest positive tests for patients and further creating cases from these results added complexity. It was addressed by breaking the tasks into smaller methods and using LINQ for effective querying.

### 3. Assumptions:
- **Retry Mechanism**: If the worker fails to retrieve or process data, it logs an error and continues after the delay. It assumes that any temporary issues will get resolved in the subsequent run.
- **HttpClient Lifetime**: A single `HttpClient` instance is used throughout the lifetime
  of the service. This assumes that the worker is primarily communicating with one
  external service and that there's no need to frequently create new HttpClient instances.
- **External Api Date Parameters**: It is assume that the correct date paramters to get the
  proper data are SampleCollectedDates.
- ****

### 4. Steps to Run and Test the WebAPI:

1. **Setup**:
    - Ensure you have the .NET SDK installed.
    - Set up the necessary databases and connection strings in the app settings.

2. **Run**:
    - Navigate to the root of the WebAPI project in the terminal.
    - Use the command `dotnet run` to start the application.
		- Make sure the http://localhost:5000 is available.
		- Wait a minute to make sure the webapi is ready.

3. **Test WorkerTestLab**:
    - Once the application is running, the `WorkerTestLab` will automatically start and log its activities. Monitor the logs to ensure it's fetching and processing data correctly.

4. **Test Endpoints (Assuming they exist)**:
    - Use tools like Postman or Swagger (if integrated) to test the API endpoints for both lab tests and cases features.

I hope this provides a comprehensive overview of the design and structure of the WebAPI. If you have any questions or need further explanations, please let me know.


## WebApp

### 1. Design Choices:

- **Modular Structure**: The Angular application is structured modularly, adhering to best practices. Components, services, and models are appropriately separated.
  
  - The `features` folder indicates a feature-based architecture, where each significant functionality or domain-related action (e.g., `cases`) is treated as a distinct module.

- **Reactive Forms**: The `CasesListComponent` uses Angular's Reactive Forms, offering an immutable approach to handling form state and validations.

- **Lazy Loading**: In the routing module, the use of `loadChildren` indicates that the cases module is lazily loaded. This improves the app's initial load time.

- **Material Components**: The use of Angular Material components like `mat-toolbar`, `mat-card`, and `mat-table` ensures a consistent and responsive UI.

- **Custom Data Source**: The `CasesDataSource` class extends the DataSource class to fetch and manage data for the `mat-table`. It abstracts the fetching and error-handling logic away from the component.

- **RxJS Observables**: Heavy use of RxJS for reactive programming. Observables, Subjects, and various operators (`tap`, `merge`, `distinctUntilChanged`) are used to handle asynchronous data and UI updates seamlessly.

### 2. Challenges:

- **Data Pagination and Sorting**: With the inclusion of `MatPaginator` and `MatSort`, the component had to be designed to reactively update the table based on pagination, sorting, or filtering events.

- **Reactive Form Changes**: Listening to form changes, especially for date filters, and reflecting those changes on the table required a combination of Observables to ensure performance and user experience were not compromised.

### 3. Steps to Run and Test the WebApp:

1. **Setup**:
   - Make sure you have Node.js and Angular CLI installed.
   - Navigate to the WebApp's root directory.
   - Run `npm install` to install all the necessary packages.
	 - Modify environments.ts to the proper webapi url if it has been modified.

2. **Run**:
   - Use the command `ng serve` to start the application.
   - Access the app in a browser at `http://localhost:4200/`.

3. **Test CasesListComponent**:
   - On loading, the `CasesListComponent` should fetch and display a list of cases in the table.
   - The table should allow sorting by columns and pagination.
   - Filtering by date range should refresh the table based on the selected dates.


I hope this provides a detailed overview of the design and structure of the Angular WebApp. Should you have further questions or require additional details, please let me know.