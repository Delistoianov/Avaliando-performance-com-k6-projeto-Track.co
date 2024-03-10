# README.md

## Running the Web API

This guide will help you run the Web API on your local machine.

### Prerequisites

- Docker
- .NET SDK

### Steps

1. **Start the database**

   Run the following command to start the database using Docker Compose:

   ```bash
   docker compose up -d
   ```

2. **Update the database**

   Navigate to the `Web.Api.Track.co` directory and run the following command to update the database:

   ```bash
   cd Web.Api.Track.co
   dotnet ef database update
   ```

3. **Run the API**

   Still in the `Web.Api.Track.co` directory, run the following command to start the API:

   ```bash
   dotnet run
   ```

After following these steps, the API should be running on your local machine. You can access it by navigating to `http://localhost:5000` in your web browser (or whatever port is specified in your `launchSettings.json` file).

Please note that any changes you make to the database will persist as long as the Docker container is not removed. If you wish to start with a fresh database, you can bring down the Docker container and repeat the steps above.