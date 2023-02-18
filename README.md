# My Recipes

A personal project that allows users to manage their favorite recipes. Users will also be able to look up their recipes based on the ingredients they have available.

I'm using this project for learning purposes and to get comfortable with C# development and some good practices and technologies such as:

* Azure DevOps
* ~~[Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/)~~ (Replaced with something resembling [Vertical Architecture](https://code-maze.com/vertical-slice-architecture-aspnet-core/))
* MediatR & CQRS
* ~~AutoMapper~~ (Replaced with manual mapping)
* Dapper
* RESTFUL API

## UPDATE 18.2.2023

After returning to the project I realized that the architecture I had initially chosen was overly complex for a simple CRUD application. 
I liked the idea of separating concerns using the [Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture/), but in the end I felt like I was making things needlessly complicated.

I've refactored the architecture to resemble something akin to Vertical Architecture <[Inspiration 1](https://code-maze.com/vertical-slice-architecture-aspnet-core/), [Inspiration 2](https://github.com/nadirbad/VerticalSliceArchitecture)>
where I try to organize things by features.
I'm still using some fragments of the past, 'Entities' and 'Infrastructure'. They could be incorporated into the 'Features' as well, but I don't see a problem with them being the way they are for now.

I feel like handling project architecture is HARD! I could spend (and have spent) way too much time adjusting the architecture and folder structure, but it feels like there's always something that could be improved.