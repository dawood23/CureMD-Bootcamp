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

