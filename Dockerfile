FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /App

COPY . ./

# NodeJS
RUN apt update
RUN apt install -y nodejs

RUN dotnet restore Website

# Build and publish a release
RUN dotnet build Website
RUN dotnet publish Website -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Website.dll"]
