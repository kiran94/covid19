# Create a Container for the Build Process
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder

# Set Build Environment Variables
ENV ASPNETCORE_CONFIGURATION=Release

# Change the containers working directory
WORKDIR app

# Copy the contents of the current host directory into the container's working directory
COPY ./Covid.Api.Common /Covid.Api.Common
COPY ./Covid.Api.Grpc /Covid.Api.Grpc

WORKDIR /Covid.Api.Grpc

# Restore Nuget Packages
RUN dotnet restore --runtime linux-x64

# Publish Executable
RUN dotnet publish -c Release --self-contained --runtime linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=True -o /app/out --no-restore

#################################################################

# Switch to the runtime container
FROM mcr.microsoft.com/dotnet/aspnet:5.0

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

CMD ["./Covid.Api.Grpc"]
