# Docker Commands Guide
Follow the steps below to run the application using Docker:
1. Create .env file in root project Backend-Server following structure of ```.env.example```
2. Build the Docker image:
```
docker build -t backend-server .
```
3. Run the Docker container:
```
docker run -d -p 8000:8000 backend-server
```
The server will be accessible at http://localhost:8000.

# For the update with token exchange between applications
1. In .Net root project ```Developer-Toolbox```, create a .env file, the file structure should be like ```.env.example```.
2. Do not forget to install .env package via Nugget console, run this command:
   ```
   dotnet add package DotNetEnv
   ```
3. The keys from Backend-Server app and Developer-Toolbox have to be the same, otherwise the server will block the client app.
4. You can set the keys with a string, generate a random API key using openssl or directly via terminal
 ```
 openssl rand -base64 32
 ```
