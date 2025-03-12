## Start db + create initial database called TestDb
`docker-compose up -d`

## Execute migrations 
dotnet ef database update --project DbContextPooling.csproj --startup-project DbContextPooling.csproj --context DbContextPooling.ApplicationDbContext 

## Seed records
call the `/api/test/seed` endpoint to seed 150_000 records

## Run load test
`k6 run loadtest.js`

## check open connections
```sql
SELECT
    DB_NAME(dbid) as DBName,
    COUNT(dbid) as NumberOfConnections,
    loginame as LoginName
FROM
    sys.sysprocesses
WHERE
    dbid > 0
GROUP BY
    dbid, loginame
;
```