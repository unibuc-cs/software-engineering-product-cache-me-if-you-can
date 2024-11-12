# Docker Commands Guide
Follow the steps below to run the application using Docker:
1. Build the Docker image:
```
docker build -t backend-server .
```
2. Run the Docker container:
```
docker run -d -p 8000:8000 backend-server
```
The server will be accessible at http://localhost:8000.