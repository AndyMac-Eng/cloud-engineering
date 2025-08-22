# Multi-stage Dockerfile for .NET 8 Azure Functions Isolated Worker

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet8 AS runtime
WORKDIR /home/site/wwwroot
COPY --from=build /out .

# Expose default Azure Functions port
EXPOSE 80

# ENTRYPOINT is set by the base image
