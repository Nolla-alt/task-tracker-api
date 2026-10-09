# Repository Patterns Reference

One reusable rule set, apply this to every entity.

## Rule 1 — No foreign keys (e.g. Category, User)

**ADO.NET:**
- SELECT: plain query on the one table, read columns by position
- INSERT/UPDATE/DELETE: plain query, parameters for every field

**EF Core:**
- SELECT: `_context.Entities.ToListAsync()` / `.FindAsync(id)`
- INSERT/UPDATE/DELETE: `.Add()` / `.Update()` / `.Remove()` + `.SaveChangesAsync()`

## Rule 2 — Has foreign keys (e.g. TaskItem → User, Category)

**ADO.NET:**
- SELECT: `JOIN` every related table, alias their columns, map each manually
- INSERT/UPDATE/DELETE: only touch the entity's own table, the FK is just an `int`, no JOIN needed

**EF Core:**
- SELECT: add `.Include(x => x.RelatedEntity)` once per foreign key
- INSERT/UPDATE/DELETE: identical to Rule 1, EF resolves the FK automatically

## The one sentence to remember

> **Reading data with foreign keys needs extra work (JOIN or Include). Writing data with foreign keys doesn't, it's just a number.**

## Generalized Template — ADO.NET repository with 2+ foreign keys

### SELECT (read) — always needs a JOIN per foreign key

```sql
SELECT 
    t.Id, t.[Field1], t.[Field2],
    t.[ForeignKey1Id], r1.[DisplayField] AS [Related1Name],
    t.[ForeignKey2Id], r2.[DisplayField] AS [Related2Name]
FROM [MainTable] t
JOIN [RelatedTable1] r1 ON t.[ForeignKey1Id] = r1.Id
JOIN [RelatedTable2] r2 ON t.[ForeignKey2Id] = r2.Id
```

### INSERT (create) — no JOIN, foreign keys are just numbers

```sql
INSERT INTO [MainTable] ([Field1], [Field2], [ForeignKey1Id], [ForeignKey2Id])
OUTPUT INSERTED.Id
VALUES (@Field1, @Field2, @ForeignKey1Id, @ForeignKey2Id)
```

### UPDATE / DELETE — never touch foreign keys beyond their own int column

## Applied so far

| Entity | Foreign Keys | Pattern |
|---|---|---|
| Category | none | Rule 1 |
| User | none | Rule 1 |
| TaskItem | UserId, CategoryId | Rule 2 (see TaskItemRepositoryAdo.cs for the real example) |