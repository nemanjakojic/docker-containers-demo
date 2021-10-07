#!/bin/bash

# Load the environment variables
source set-env.sh

# Tear down the docker containers
sudo docker-compose down -v

# Clean up database files
rm -f $DB_DATA_PATH/*
rm -f $DB_LOG_PATH/*
