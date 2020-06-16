# Create a Container for the Build Process
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder

# Set Build Environment Variables
ENV ASPNETCORE_CONFIGURATION=Release

# Change the containers working directory
WORKDIR app

# Copy the contents of the current host directory into the container's working directory


COPY ./Covid.Api.Common /Covid.Api.Common
COPY ./Covid.Api.GraphQL /Covid.Api.GraphQL

WORKDIR /Covid.Api.GraphQL

# Restore Nuget Packages
RUN dotnet restore

# Publish DLL
RUN dotnet publish -c Release -o /app/out --no-restore

#################################################################

# Switch to the runtime container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

# Switch container's working directory
WORKDIR /app

# Copy from the build container into the runtime container
COPY --from=builder /app/out .

# Set Runtime Environment Variables
# TODO: work on HTTPs support
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Development}

# Expose Port
EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "Covid.Api.GraphQL.dll"]