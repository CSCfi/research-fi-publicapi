---
name: scaffold-dbcontext-cleanup
description: "Clean up and verify the EF Core DbContext and entities in aspnetcore/src/DatabaseContext after the user has run dotnet ef dbcontext scaffold against the dw_rih SQL Server database. Normalizes CRLF line endings, hides line-ending noise, checks for credentials, builds, and reports real schema changes. USE FOR: post-scaffold cleanup; reviewing what a scaffold run changed; checking the DatabaseContext build after regeneration. DO NOT USE FOR: running the scaffold itself (the user runs it); handling or asking for the connection string; hand-editing entities (they are generated); changing the Interface or Indexer code that consumes them."
---

# Scaffold DbContext Cleanup

Cleans up `aspnetcore/src/DatabaseContext` after the user has regenerated it, so only real schema changes remain in the diff.

Model suitability: the Steps are mechanical and can run on a Haiku-class model. The schema review in Reporting needs a stronger model.

## Responsibilities

- The user runs the `dotnet ef dbcontext scaffold` command in their own terminal. The agent never runs it.
- The agent never asks for, prints, or stores the connection string or any password.
- If a password has appeared in a chat or terminal history, ask the user to rotate it before continuing.

## User step (for reference)

The user runs this from `aspnetcore/` in their own terminal, after exporting `DW_RIH_CONNECTION_STRING` in that same shell:

```
dotnet ef dbcontext scaffold "$DW_RIH_CONNECTION_STRING" Microsoft.EntityFrameworkCore.SqlServer --project src/DatabaseContext --context ApiDbContext --context-dir . --output-dir Entities --no-onconfiguring --force
```

`--no-onconfiguring` keeps the connection string out of `ApiDbContext.cs`.

## Steps

Run every command from `aspnetcore/`.

0. Ask the user to confirm the scaffold command has finished successfully. If they have not run it, stop and wait. Do not run the scaffold.

1. Normalize line endings to LF. The scaffold writes CRLF on macOS, and the repo uses LF (see `.gitattributes` and `.editorconfig`). This command is portable across macOS and Windows.

   ```
   perl -pi -e 's/\r\n/\n/' src/DatabaseContext/*.cs src/DatabaseContext/Entities/*.cs
   ```

2. Check the diff. Use `--ignore-cr-at-eol` so any remaining line-ending noise is hidden.

   ```
   git status --short
   git diff --ignore-cr-at-eol --stat
   ```

3. Confirm no credentials were written. Expect no output.

   ```
   grep -rn -i "password\|OnConfiguring" src/DatabaseContext || true
   ```

   If anything is printed, stop, report the file and line without repeating the matched value, and ask the user how to proceed.

4. Build.

   ```
   dotnet build src/DatabaseContext/DatabaseContext.csproj /consoleloggerparameters:NoSummary
   ```

## Reporting

- List the files with real changes from step 2. Do not list files that only changed line endings.
- Call out schema changes that were not expected, such as new columns, indexes, or default constraints. Ask the user to confirm them before they are committed.
- Report the build result.
- Do not commit until the user confirms the schema changes.
