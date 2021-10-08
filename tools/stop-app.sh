#!/bin/bash

# Load the environment variables
source set-env.sh

# Tear down the docker containers
sudo docker-compose down -v
