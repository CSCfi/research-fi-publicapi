This project is generated from the database  model. No manual changes are recommended. You can update the model by using the dotnet ef tool.

Installing dotnet ef: https://learn.microsoft.com/en-us/ef/core/cli/dotnet

To update the model use the following command in the solution root folder:

	dotnet ef dbcontext scaffold "Server=localhost;User Id=<user>;Password=<password>;database=dw_rih;" Microsoft.EntityFrameworkCore.SqlServer --project src/DatabaseContext --context ApiDbContext --context-dir . --output-dir Entities --force