FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base-env
WORKDIR /app

# Build frontend stage
# Linux
FROM node:10 AS build-node
# Windows
#FROM stefanscherer/node-windows:10.15 AS build-node
WORKDIR /ClientApp
COPY ClientApp/package.json .
COPY ClientApp/package-lock.json .
RUN npm install
RUN npm rebuild node-sass
COPY ClientApp/ . 
RUN npm run build --prod

# Build stage
FROM microsoft/dotnet:2.2-sdk AS build-env
ENV BuildingDocker=true
WORKDIR /app
COPY *.csproj .
RUN dotnet restore Portal.csproj

# copy src
COPY . .

# publish
RUN dotnet publish Portal.csproj -o /app -c Release

# Runtime stage
FROM base-env as portal
WORKDIR /app
COPY --from=build-env /app .
COPY --from=build-node /ClientApp/dist ./ClientApp/dist
ENTRYPOINT ["dotnet", "Portal.dll"]