# multistage builds.

# 1. Build image called "build-image" that is used to build your project
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-image

WORKDIR /home/app

# Copy everything from host to the workdir in the container
#COPY . .
# Copy only the project and solution so that source code changes don't trigger `dotnet restore`
COPY ./dotnetcore-jwt-auth.csproj .
COPY ./dotnetcore-jwt-auth.sln .

# Restores any dependencies needed
RUN dotnet restore

COPY . .

RUN dotnet publish ./dotnetcore-jwt-auth.csproj -o /publish/

# 2. Build a 'runtime' image that just has the stuff you need
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /publish

# Copy stuff from the build-image's publish directory into your new runtime image
COPY --from=build-image /publish .

ENV ASPNETCORE_URLS="http://0.0.0.0:5000"

ENTRYPOINT ["dotnet", "dotnetcore-jwt-auth.dll"]

# Build Image
# docker build -t dotnetcore-jwt-auth:build .
# docker build -t dotnetcore-jwt-auth:runtime .

# Run commands:
# docker run --rm -it -p 8080:5000 dotnetcore-jwt-auth:build
# docker run --rm -it -p 8080:5000 --name dotnetcore-container dotnetcore-jwt-auth:runtime