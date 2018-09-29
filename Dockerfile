FROM microsoft/dotnet:2.1.402-sdk-alpine AS build

# Add Node From https://github.com/nodejs/docker-node
RUN apk add --no-cache bash 
RUN apk add --update nodejs nodejs-npm git

WORKDIR /app

# Restore Nugets to cache layer
COPY src/OverwatchAnalyser.Jeff.fsproj  src/
COPY tests/OverwatchAnalyser.Jeff.Tests.fsproj  tests/
COPY *.sln ./
RUN dotnet restore

# Copy everything for build
COPY . .
RUN ./build.sh Publish -configuration Release


FROM microsoft/dotnet:2.1.4-aspnetcore-runtime-alpine AS base

EXPOSE 80

ENV Neo4j.Bolt "bolt://neo4j:7687"
ENV Neo4j.User neo4j
ENV Neo4j.Pass neo4j

WORKDIR /app

COPY --from=build /app/out/ .
ENTRYPOINT ["dotnet", "jeff.dll"]