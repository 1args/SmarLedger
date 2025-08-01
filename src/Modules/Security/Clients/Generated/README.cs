### Keycloak usage 

After launching keycloak, you need to create and configure a realm. 
After that, you need to generate the necessary classes and interfaces to work with keycloak.
This is done with the nswag tool, download it like this: `dotnet tool install -g NSwag.ConsoleCore`. 
The data from which the necessary tools are generated is https://www.keycloak.org/docs-api/latest/rest-api/openapi.json.
To decide in detail how exactly this should be generated, you need to create a configuration file `keycloak.nswag` and specify everything in it. 
Then you need to run it with the following command: `nswag run keycloak.nswag`.