# Dockerized Angular + .NET Web API

**Building images**  
docker-backend  
docker-frontend  

**Docker Compose**

The project is running with docker compose.  

In the root folder which is named **Docker** run the following commands:  

```bash
docker compose build
docker-compose up -d
````

This will create the images and then run them on their following port.

**Assigned Ports**
Frontend: [http://localhost:4200](http://localhost:4200)
API: [http://localhost:5000](http://localhost:5000)
API Health: [http://localhost:5000/health](http://localhost:5000/health)

**Project Structure**
```
Docker/
├── pvm/         
├── WebAPIAssignment/           
├── docker-compose.yml
├── docker-compose.prod.yml
├── .env
└── README.md
```

I have tested the connectivity between the Angular app and the .NET API and it is working completely fine.


Both the Images are uploaded on the Docker Hub
If u want to download those images and run them these are the commands,

```
docker pull dawoodnadeem213/docker-backend:1.0.0
docker pull dawoodnadeem213/docker-frontend:1.0.0

docker run -d \
  --name dotnet-api \
  -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  dawoodnadeem213/docker-backend:1.0.0


docker run -d \
  --name angular-app \
  -p 4200:80 \
  -e API_BASE_URL=http://localhost:5000 \
  dawoodnadeem213/docker-frontend:1.0.0

```

Or just use the docker-compose.prod.yml that i have uploaded on this repo and run this command.

```
docker compose -f docker-compose.prod.yml --env-file .env up -d
```