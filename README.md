# Permissions App

## Overview

The **Permissions App** is a solution designed to manage permissions in N5 company. This application allows users to get, create, update and remove permissions efficiently. It provides a robust system for handling permissions.

## Features

- **Get Permissions**: Enables retrieval of permissions based on various criteria.
- **Request Permissions**: Allows users to request and add new permissions.
- **Modify Permissions**: Supports modification of existing permissions.
- **Remove Permissions**: Provides functionality to remove permissions and manage the lifecycle of permissions.

## Architecture

- **Frontend**: Built with React and Vite, providing a modern and responsive user interface for interacting with the permissions system.
- **Backend**: Developed using .NET 8, offering a robust API for managing permissions. It includes endpoints for creating, updating, retrieving, and removing permissions.
- **Data Storage**: Employs Elasticsearch for indexing permissions, and uses a SQL Server 2022 relational database for transactional data.
 - **Elasticsearch**: Used for indexing and searching permissions data. The application interacts with Elasticsearch to provide fast search capabilities and real-time data indexing.
- **Kafka**: Apache Kafka is integrated into the architecture for handling real-time data streaming and messaging. Kafka is used to publish events related to permissions, such as permission requests and modifications. This enables asynchronous communication between different services and ensures that events are processed reliably.
- **Testing**: Includes comprehensive unit tests to ensure the reliability and performance of the application.

## Getting Started

To get started with the Permissions App, follow these instructions to set up the development environment, run the application, and interact with it.

### Prerequisites

Before you begin, ensure you have the following installed on your machine:

- **.NET SDK**: Version 8.0 or later
- **Node.js**: Version 18 or later
- **Docker**: Version 20.10 or later
- **Apache Kafka**: For local development, you can use Docker to run Kafka
- **Elasticsearch**: For local development, you can use Docker to run Elasticsearch
- **SQL Server**: For local development, you can use Docker to run SQL Server

## Getting Started with Docker Compose

This section will guide you through setting up the Permissions App using Docker Compose. Docker Compose is used to define and run multi-container Docker applications.

### Prerequisites

- Ensure you have [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/) installed on your machine.

- Clone the Repository on your machine.

   ```bash
   git clone https://github.com/yherediag/PermissionsApp.git
   cd permissions-app

### Docker Compose Setup

1. **Verify `docker-compose.yml` file**:

2. **Build and Start the Containers**:

    To build and start the containers, navigate to the directory containing your `docker-compose.yml` file and run:

    ```sh
    docker-compose build
    docker-compose up
    ```

    This will build the Docker images and start the containers defined in the `docker-compose.yml` file.

3. **Verify the Setup**:

    - **Permissions WebAPI**: Access the API at [https://localhost:5001/swagger](https://localhost:5001/swagger).
    - **Permissions App**: Access the app at [http://localhost:3000](http://localhost:3000).
    - **Kafka-UI**: Access Kafka at [http://localhost:8080/](http://localhost:8080/).
    - **Elasticsearch**: Access the Elasticsearch service at [http://localhost:9200](http://localhost:9200).
    - **SQL Server**: Connect to the SQL Server using `localhost,1433`.

4. **Stopping the Containers**:

    To stop the running containers, use:

    ```sh
    docker-compose down
    ```

    This will stop and remove the containers, networks, and volumes defined in your `docker-compose.yml`.

### Additional Notes

- **Configuration**: Ensure your application’s configuration (such as `appsettings.json`) matches the settings defined in `docker-compose.yml`.
- **Environment Variables**: Adjust the environment variables in `docker-compose.yml` according to your requirements.
- **Volumes**: For persistent data, consider adding volumes to your Docker Compose setup.

For more details on Docker Compose, refer to the [Docker Compose documentation](https://docs.docker.com/compose/).


## Configuration

The Permissions App uses several services that need to be configured properly. Below is the configuration used for local development, which includes settings for logging, database connection, Kafka, and Elasticsearch.

### Configuration File

The configuration file (`appsettings.json`) should be placed in the root of your backend project directory with the following content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=permissionsapp.database,1433;Database=PermissionsApp;User Id=SA;Password=SuperPassword123;TrustServerCertificate=True"
  },
  "Kafka": {
    "BootstrapServers": "kafka:9092"
  },
  "Elasticsearch": {
    "Uri": "http://elasticsearch:9200",
    "Username": "elastic",
    "Password": "SuperPassword123"
  }
}
