## Architecture

### Layers

The architecture will be described starting with the layers. The project uses a clean architecture approach with a clear separation of responsibilities. I would like to emphasize that the **Hosts** folders contain any projects that can be run (for example, Web API or console applications). In general, considering all layers, here is their description:

- **Domain** - serves as an independent, central, and key layer. It defines the subject area of the business and describes business rules, aggregates, entities, and value objects
- The next layer - **Applications**, which is divided into 2 sublayers:
    - **AppServices** - acts as an orchestrator between Domain and Infrastructure. It is responsible for managing the execution of business logic
    - **Handlers** - contains handlers for CQRS primitives (commands, queries, events) that work with app services.
The next layer is **Infrastructures**, which contains the implementation of technical details. In general, it is divided into the following layers in the project to avoid monolithism:
    - **DataAccess** - responsible for working with data from external storage
    - **FileStorage** - responsible for storing and retrieving files from external systems (in my case, from MinIo)
    - **BackgroundJobs** - manages background task execution (in my case via Hangfire)
- There is also an auxiliary layer called **Contracts**, which contains components for interacting with external resources or modules. In addition, it may also contain mappers, common constants, and options.
- In **Hosts**, there is an **Api** layer, which is responsible for interacting with external resources, and there may also be a **Migrations** layer, which contains scripts that describe the structure of the database schema, which can be run through a console application, providing automation to the process
- The **Clients** layer is also used to interact with various external APIs