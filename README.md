# Developer Toolbox :rocket:
## Documenting the existing application 
### Completed user stories:
* As an unregistred user, I want to see others' questions and answers.
* As user, I want to login.
* As user, I want to ask technical questions and receive answers.
* As user, I want to answer others' questions.
* As user, I want to have a profile.
* As user, I want to rate questions and answers.
* As user, I want to save questions and answers.
* As user, I want to search specific questions or a tag.
* As user, I want to browse a variety of coding exercises
* As user, I want to be able to select and attempt coding exercises directly within the app, within a code editor.
* As a moderator, I want to manage the content and exercises available on the platform
* As a moderator, I want to delete a post or a comment if it violates community guidelines. 
* As an administrator, I want to manage the content and exercises available on the platform
* As an administrator, I want to delete a post or a comment if it violates community guidelines. 
* As an administrator, I want to manage user roles and account details.
* As an administrator, I want the capability to delete user accounts.

### Unachieved user stories
* As user, I want to track my progress and performance over time. 

   **Analysis of Unachieved User Story:**

   - **Reason for Incompletion:**  
     The progress-tracking feature was partially implemented but faced challenges due to the complexity of tracking and visualizing user metrics, as well as time constraints during the development phase.
  
   - **Future Development Recommendation:**  
     Yes, this user story should be included in the next development phase, as it would add significant value for users who wish to monitor their learning journey and improvement.

   - **Tactics to Ensure Completion:**  
     - **Scope Reduction:** Begin with a minimal approach, focusing initially on simple metrics like the number of completed exercises or questions answered.
     - **Clear Specifications:** Define specific progress metrics early on to avoid ambiguity and align development efforts.
     - **Incremental Development:** Break down the feature into smaller tasks, such as tracking completion status and quiz scores, allowing for gradual deployment.
     - **Regular Testing and Feedback:** Engage users for feedback during development to fine-tune metrics and display preferences.
---

## Team Description

Our team collaborated closely on the development of the MDS project, with each member contributing specific skills and focusing on various components. We implemented the Model-View-Controller (MVC) architecture by dividing tasks based on project components, ensuring efficient and organized development.

### Team Members

- **Ciurescu Irina Alexandra**
  - **Role:** Development & QA
  - **Responsibilities:** Led the development of specific application components, like User Roles & Authetication. Later focused on creating and executing tests to ensure code quality and reliability.

- **Stoinea Maria Miruna**
  - **Role:** Development & QA
  - **Responsibilities:** Focused on developing  components, like Questions & Answers forum, and joined Ciurescu in testing to validate the application’s functionality.

- **Toma Alexandra**
  - **Role:** Development
  - **Responsibilities:** Focused on developing  components, like Exercises and Categories. Worked on backend server development for the compiler functionality and code editor integration.

- **Macovei Cătălina**
  - **Role:** Development
  - **Responsibilities:** Focused on developing  components, like Tags, Bookmarks and Solutions for Exercises. Then, primarly worked on the backend server for the compiler functionality and integrating the code editor.

- **Predicted Changes:** No changes predicted in team members roles.

Each team member’s contributions were integral to completing the project, starting from creating diagrams and reaching the development process. As the project progresses, roles may naturally evolve, but no immediate changes in responsibilities are anticipated.


# Software Architecture Report

## Overview
This document outlines the architectural decisions made in the previous iteration of our project, along with a discussion of their effectiveness. Diagrams created during the design phase are referenced here to explain key decisions.


## a. Technologies Used

Our project leverages a set of technologies chosen for their compatibility, performance, and suitability for the application’s requirements:

- **ASP.NET Core**: A high-performance framework for building web applications with support for MVC architecture and built-in dependency injection.
- **Entity Framework Core**: An ORM tool for data access in .NET applications, making it easier to interact with the database using LINQ.
- **SQL Server**: A robust relational database management system to handle data persistence.
- **JavaScript, HTML, CSS**: For front-end development, providing a responsive and interactive user interface.
- **xUnit**: Used for unit tests, allowing us to test individual components in isolation.
- **Moq**: Enabled us to mock dependencies, making it easier to test controllers and services without relying on external data sources.


These technologies helped answering to project’s requirements.


## b. Architectural Patterns Implemented

### 1. Model-View-Controller (MVC) Architecture
- **Overview**: The application follows the MVC architectural pattern, which organizes the project into three main components:
  - **Model**: Contains the core application data and business logic, represented by classes such as `Answer`, `ApplicationUser`, `Bookmark`, `Question`, etc.
  - **View**: Manages the user interface and presentation logic, including views like `Index`, `Edit`, and `Show`.
  - **Controller**: Handles user requests, updates the model, and renders views in response to user actions.
- **Effectiveness**: The MVC pattern enhanced separation of concerns, code readability, and maintainability. It allowed for a well-organized codebase, making testing and debugging more efficient.

### 2. Client-Server Architecture
- **Overview**: The application adheres to a client-server model with a separation between client-side components (views) and server-side components (controllers, models, and data access).
- **Effectiveness**: This architecture enhanced scalability by allowing independent management of client and server resources. It also provided flexibility for future integration with additional front-end or back-end services.


## c. Coding Standards and Principles
We established and enforced several coding principles, such as:
- **Code Modularity**: Each function handles a single responsibility, and reusable code is organized into separate modules.
- **Naming Conventions**: Descriptive and consistent naming for variables, functions, and classes.
- **Code Documentation**: Inline comments and method descriptions were added to enhance readability and maintainability.
- **Exception Handling**: Implemented robust exception handling to manage errors gracefully.


## d. Faults Discovered During Development
During the development process, the following issues were identified:
- **Data Retrieval Issues**: Some database queries did not retrieve the data properly, leading to inaccuracies in the results.
- **Dependency Management Issues**: Unresolved dependencies between in certain areas led to errors.
- **Local Environment Security Risks**: Executing exercise files in the local environment poses security risks. While we did not use Docker containers during development, implementing Dockerization would enhance code versioning and provide a more secure environment for executing these files.

These issues have been partially addressed, and optimization efforts are ongoing.


## e. Areas for Refactoring
Based on our observations, the following areas would benefit from refactoring:
- **Database Query Optimization**: Some queries need further optimization to enhance performance.
- **Dockerization**: Transitioning to a Docker-based architecture would improve security and improve the execution of exercise files, mitigating the risks associated with running them in a local environment.


## Diagrams

### Entity Relationship Diagram
<p align="center">
  <img src="Diagrams/MDS-Diagrams/ERD.svg" alt="Entity Relationship Diagram">
</p>

### UML Diagram
<p align="center">
  <img src="Diagrams/MDS-Diagrams/UML.jpg" alt="UML Diagram" style="max-height: 500px;">
</p>

### Workflow Diagram
<p align="center">
  <img src="Diagrams/MDS-Diagrams/Workflow.svg" alt="Workflow Diagram">
</p>

### Gantt Chart - development process in time
<p align="center">
  <img src="Diagrams/MDS-Diagrams/Gantt.svg" alt="Gantt Chart">
</p>

---


## Conclusion
The architectural patterns chosen provided a solid foundation for our application, with MVC and Client-Server proving especially effective in maintaining a modular, testable codebase. Future iterations will focus on optimizing database performance and exploring more advanced state management solutions.


