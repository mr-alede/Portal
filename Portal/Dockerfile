FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base-env
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install -y build-essential nodejs

WORKDIR /app

# Build stage
FROM microsoft/dotnet:2.1-sdk AS build-env
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install -y build-essential nodejs
RUN npm install @angular/cli -g
# End setup
WORKDIR /app

# restore
COPY *.csproj .
RUN dotnet restore *.csproj

# copy src
COPY . .

WORKDIR ClientApp
RUN npm install
RUN npm rebuild node-sass
WORKDIR /app

# publish
RUN dotnet publish Portal.csproj -o /app

# Runtime stage
FROM base-env as portal
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "Portal.dll"]