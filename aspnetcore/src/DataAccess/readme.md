Update EF model with:
	dotnet ef dbcontext scaffold Name=dbconnectionstring Microsoft.EntityFrameworkCore.SqlServer --context ApiDbContext --context-dir DatabaseContext --output-dir Entities --project src\Api.DataAccess --force

Requires that the connection string named "dbconnectionstring" is in the user secrets/env variables/etc.