# docker-containers-demo
This repository includes all the necessary artifacts to deploy and run a set of demo application services in a small Docker cluster.

## Solution Folder Structure
 - `account-api`: 
   Project folder for the REST API implemented using ASP.NET Core 5.0.
 - `database`:
   Contains necessary bash and SQL scripts for setting up the database.
 - `postman`: 
   Contains Postman artifacts for testing the API.
   NOTE: make sure both the request collection and environment are imported prior to running the tests.
 - `cert`:
   Contains a sample SSL certificate for setting up HTTPS.
 - `docker-compose.yml`:
   A YAML file for creating and running the application Docker containers.
   
## Software architecture / Deployment Model  
The demo application is comprised of the following services:
 * `account-api`: 
   REST API for account management accessible over HTTPS. 
   Runs on .NET Core and ASP.NET Core 5.0 in a Docker container.
 * `mssql-db`: 
   An SQL Server instance that stores accounts.
   Runs in a Docker container.
   Note: the database files reside inside the container for the sake of simplicity. 
 * `session-store`:
   A Redis cache instance that keeps active user sessions.
   Runs in a Docker container.

## Account Management REST API Summary
The API endpoints require HTTPS.

 1) Create a new account (email address MUST BE unique)
    ```
    POST /account/signup
    Request Payload: 
    {
      "username": "...",
      "password": "..."
    }
    Response Payload: 
    {
      "success": "true/false"
      "message": "..."
    }
    ```
 2) Log in with an existing account (email address and password MUST exist and match)
    ```
    POST /account/login 
    Request Payload: 
    {
      "username": "...",
      "password": "..."
    }
    Response Payload: 
    {
      "success": "true/false"
      "message": "..."
    }
    ```
    Additionally, a session cookie is sent back to the client 
    
 4) Log out and terminate the session
    ```
    POST /account/logout
    Request Payload: none (the session cookie will be sent to the server)
    Response Payload: 
    {
      "success": "true/false"
      "message": "..."
    }
    ```
     
## Instructions for Running the Solution
PREREQUISITES: `docker` and `docker-compose` are installed on the system where the app will run.
(This setup has been tested on Ubuntu 20.04.)

Run `docker-compose up -d` command from the folder that contains the `docker-compose.yml` file to build, deploy and run the containers.
 
Additional useful commands: 
 * Run `docker-compose down` to tear down the Docker containers cluster.
 * Run `docker-compose build` to rebuild the account-api Docker container image.
 
 > NOTE: The script that initializes the database schema will wait until 
         the SQL Server instance is ready (up and running). 
         If, for any reason, this script fails to initialize the database automatically, 
         please run `sudo docker-compose up -d` again.

## Instructions for Testing the Solution
 The solution comes with a Postman request collection and an environment file. 
 1) Run Postman;
 1) Import a request collection from 'postman/AccountApiTests.postman_collection.json';
 1) Import a test environment from 'postman/Test.postman_environment.json';
 1) Set the Test environment as Active.
 1) The sample SSL certificate is self-signed. 
         Disable SSL certificate verification to prevent errors.
         Postman: File > Settings > General > SSL Certificate Verification - set to OFF.
 1) Run the imported request collection from within Postman.
         Make sure the Test environment is set as active. 

## Design Notes and Assumptions
 - The database stores hashed passwords. 
 - Passwords are hashed using the BCrypt algorithm with the SHA-384 hash function and the cost of 10 (2^10 rounds).
 - The account-api service does the password hashing. No plain passwords are traveling between the api and the database. 
 - A custom password policy has been implemented to prevent weak passwords from entering the platform.
 - A special database login/user has been set up that can only access accounts and nothing else.
 - The account-api service relies on Redis for distributed session management. 
 - The account-api endpoints are accessible exclusively over HTTPS. 
 - The attached sample SSL certificate is self-signed. 
 - No additional tokens are used for securing access to the API.
 - The session cookie is protected/encrypted using ASP.NET's built-in encryption keys.
 - The session cookie is removed from the client on logout.
 - For simplicity, the passwords for the SSL certificate and SQL Server are kept in app config files (no key vaults).
 - A session will be terminated after N minutes of inactivity. 
 
For any additional information, please feel free to reach out at nemanja.kojic@gmail.com.
