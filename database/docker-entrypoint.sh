#!/bin/bash

# Make db-init.sql executable
chmod +x db-init.sh

#start SQL Server, start the script to create/setup the DB
 /db-init.sh & /opt/mssql/bin/sqlservr
