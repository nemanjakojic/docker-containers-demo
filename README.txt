-- =========================================== --
   Solution: kojic-backend-test-1
-- =========================================== --

This folder contains all necessary artifacts for deploying and running the solution on docker containers.

----------------------------------------------------------------------
Solution Folder Structure
----------------------------------------------------------------------
 * account-api: 
   Project folder for the REST API implemented using ASP.NET Core 5.0.
    
 * database:
   Contains necessary bash and SQL scripts for setting up the database.
   
 * postman: 
   Contains Postman artifacts for testing the API.
   NOTE: make sure both the request collection and environment are imported prior to running the tests.
   
 * cert:
   Contains a sample SSL certificate for setting up HTTPS.
   
 * docker-compose.yml:

-----------------------------------------------------------------------
Software architecture / Deployment Model  
-----------------------------------------------------------------------
 The application is comprised of the following services:
 * account-api: 
   REST API for account management accessible over HTTPS. 
   Runs on .NET Core and ASP.NET Core 5.0 in a Docker container.
   
 * mssql-db: 
   An SQL Server instance that stores accounts.
   Runs in a Docker container.
   Note: the database files reside inside the container for the sake of simplicity. 
 
 * session-store:
   A Redis cache instance that keeps active user sessions.
   Runs in a Docker container.

-----------------------------------------------------------------------
Account Management REST API Summary
-----------------------------------------------------------------------
 Communication with the endpoints is encrypted. Only HTTPS is supported/allowed. 

 1) Create a new account (email address MUST BE unique)
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
    
 2) Log in with an existing account (email address and password MUST exist and match)
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
    Additionally: a session cookie is sent back to the client 
    
 3) Log out and terminate the session
    POST /account/logout
    Request Payload: none (the session cookie will be sent to the server)
    Response Payload: 
    {
      "success": "true/false"
      "message": "..."
    }

---------------------------------------------------------------------    
Instructions for Running the Solution
---------------------------------------------------------------------
 Run 'sudo docker-compose up -d' to build, deploy and run the containers.
 
 NOTE: The script that initializes the database schema will wait until 
       the SQL Server instance is ready (up and running). 
       If, for any reason, this script fails to initialize the database automatically, 
       please run 'sudo docker-compose up -d' again.
       
---------------------------------------------------------------------
Instructions for Testing the Solution
---------------------------------------------------------------------
 The solution comes with a Postman request collection and environment file. 
 Step 1: Run Postman;
 Step 2: Import a request collection from 'postman/AccountApiTests.postman_collection.json';
 Step 3: Import a test environment from 'postman/Test.postman_environment.json';
 Step 4: Set the Test enviroment as Active.
 Step 5: The sample SSL certificate is self-signed. 
         Disable SSL certificate verification to prevent errors.
         Postman: File > Settings > General > SSL Certificate Verification - set to OFF.
 Step 6: Run the imported request collection from within Postman.
         Make sure the Test environment is set as active. 

---------------------------------------------------------------------
Design Notes and Assumptions
---------------------------------------------------------------------
 - The database stores hashed passwords. 
 - Passwords are hashed using the BCrypt algorithm with the SHA-384 hash function and the cost of 10 (2^10 rounds).
 - The account-api service does the password hashing. No plain passwords traveling between the api and the database. 
 - A custom password policy has been implemented to prevent weak passwords from entering the platform.
 - A special database login/user has been set up that can only access accounts and nothing else.
 - The account-api service employs Redis for distributed session management. 
 - The account-api endpoints are accessible exclusively over HTTPS. 
 - The attached sample SSL certificate is self-signed. 
 - No additional tokens are used for securing access to the API.
 - The session cookie is protected/encrypted using ASP.NET's built-in encryption keys.
 - The session cookie is removed from the client on logout.
 - For the sake of simplicity, the passwords for the SSL certificate and SQL Server are kept in app config files (no key vaults).
 - A session will be terminated after N minutes of inactivity. 
 
For any additional information, please reach out at nemanja.kojic@gmail.com.

