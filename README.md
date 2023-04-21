# My Recipes

This personal project is a full-stack web application that enables users to manage and organize their favorite recipes. The application also features a recipe search functionality based on the ingredients that users have available. Developed with React and .NET, this project serves as a learning experience for implementing good practices and technologies in web development.

* Azure DevOps + GitHub for CI/CD
* [Vertical Architecture](https://code-maze.com/vertical-slice-architecture-aspnet-core/) for the structure
* JWTs & Identity for authentication
* Restful API Controllers
* MediatR & CQRS for separation of commands and queries
* Repository pattern for handling data access and persistence
* Dapper to communicate with the database

## Front-end (React)
The front-end of the application is developed using React with TypeScript and MUI. I enjoy working with React's component-based architecture, and creating reusable UI components that can be integrated into various parts of the application. The decision to use MUI was made based on its extensive library of ready-to-use components, which facilitated faster development times. TypeScript comes as a default for me when working with JavaScript, as it provides invaluable type safety and confidence. In addition, the project takes advantage of caching using 'React-Query'. For example the API sends the user's recipes in the response after a successful authenticaton, allowing them to be cached and reducing the number of necessary calls. Using the React-Query has allowed me to separate the server-side and client-side state from eachother.

## Back-end (.NET)
The back-end of the application is developed using .NET. I've tried to follow vertical architecture by categorizing the code by 'features' instead of arbititary folders like 'services' and 'interfaces'. This approach provides a clearer understanding of the application's purpose and allows for easier maintenance and scalability. Dependency injection was utilized to manage dependencies and ensure loose coupling between components. Dapper was used to communicate with the database, providing a lightweight and performant ORM solution. Authentication was implemented using JWTs and Identity. The application also uses MediatR and CQRS pattern to better seperate commands and queries, which in hindsight might've been an overkill, but I enjoy the pattern and take it as a learning experience.

### Integration testing
Something that I'm pretty excited about is the integration testing that I've managed to accomplish. The tests set up an in-memory version of the API, enabling testing of individual endpoints as if interacting with the API as a user. This has allowed for me to create tests for the whole flow of the application, ensuring expected responses were returned. I've also taken advantage of containerization, as the tests create a contained database that is cleared after each test run to avoid leakage and ensure test reliability.

## Design
My idea for the design of the project was a mobile-first approach with the aim of creating a simplistic and clear user interface. Before starting to code, I took some time to sketch out the layout using Figma. The following pictures showcase some of the initial design concepts. I've tried to keep the styling consistent by using a primary color for elements that the user can interact with, in addition to consistent white-space amount and uniform font styles.

The following pictures showcase some of the initial design concepts.

<img src="https://user-images.githubusercontent.com/89644326/233549279-afa8a914-a9cc-448e-ad3f-d54f1a3ffe6c.png" width="700" height="320" />
<img src="https://user-images.githubusercontent.com/89644326/233550979-6963dbfb-2aa1-4f7f-be38-707f905d27a4.png" width="700" height="320" />
<img src="https://user-images.githubusercontent.com/89644326/233550179-fd3ca273-f74d-4a90-8591-5691696f2d43.png" width="600" height="350" />
<img src="https://user-images.githubusercontent.com/89644326/233551922-9e6c1c12-0cfe-442a-accf-4ffb4f8b1c26.png" width="700" height="370" />

## Azure DevOps CI/CD
I have established a pipeline in Azure DevOps, ensuring that any changes made to the codebase are automatically tested, built, and published to GitHub. Additionally, I've also created a Heroku account to enable automatic deployment of those published changes. I can definetely see the benefit of adopting a CI/CD approach. By automating the testing and deployment process, the likelihood of introducing errors or inconsistencies is greatly reduced.

![kuva](https://user-images.githubusercontent.com/89644326/233547338-8fa795f8-779b-4dec-8f54-cbc44dcb134c.png)
