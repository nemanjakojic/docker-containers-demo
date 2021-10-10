#!/bin/bash

TIMEOUT=60
DBSTATUS=1
ERRCODE=1
i=0

while [[ $i -lt $TIMEOUT ]] ; do
    echo "Waiting for SQL Server instance to start ($i / $TIMEOUT)"
	i=$i+1
	DBSTATUS=$(/opt/mssql-tools/bin/sqlcmd -h -1 -t 1 -U sa -P TestPwd.098 -Q "SET NOCOUNT ON; Select SUM(state) from sys.databases")
	ERRCODE=$?
	sleep 1

	if [[ $DBSTATUS -eq 0 ]] && [[ $ERRCODE -eq 0 ]]; then
		break
	fi
done

if [[ $DBSTATUS -ne 0 ]] || [[ $ERRCODE -ne 0 ]]; then
	echo "SETUP: SQL Server took more than $TIMEOUT seconds to start up or one or more databases are not in an ONLINE state"
	exit 1
fi

sleep 2

echo "Setting up database schema"
#run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P TestPwd.098 -d master -i db-init.sql
