#!/bin/bash

source set-env.sh

# Create and run application containers as specified in docker-compose.yml
echo "Creating and running the containers"
sudo docker-compose up -d

# Wait until the SQL Server container is up and running
mssql_up=""
while [ "$mssql_up" != "Up" ]
do
    echo "Waiting until $MSSQL_CONTAINER_NAME gets up and running"
    sleep 5s
    mssql_up=$(sudo docker ps --format "{{.Names}} {{.Status}}" | grep "$MSSQL_CONTAINER_NAME" | awk '{ print $2 }')
done

# Copy the database initialization script to the container instance
sudo docker cp create-db.sql $MSSQL_CONTAINER_NAME:/home/create-db.sql

# Initialize application database
echo "Setting up the database schema"
sudo docker exec -it $MSSQL_CONTAINER_NAME /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $DB_ADMIN_PASS -d master -i /home/create-db.sql
echo "Application database is ready."
