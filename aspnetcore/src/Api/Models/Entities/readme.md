Update EF model with:
	dotnet ef dbcontext scaffold Name=dbconnectionstring Microsoft.EntityFrameworkCore.SqlServer --context ApiDbContext --context-dir DatabaseContext --output-dir Models\Entities --project src\Api --force

Requires that the connection string named "dbconnectionstring" is in the user secrets/env variables/etc.