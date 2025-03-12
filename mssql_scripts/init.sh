#!/bin/bash

sleep 40s

/opt/mssql-tools/bin/sqlcmd -S mssql -U sa -P VLcaDAge^njD%811 -d master -i /opt/mssql_scripts/init.sql
