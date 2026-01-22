# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and projects
COPY Evento.sln .
COPY Evento.Application/ Evento.Application/
COPY Evento.Domain/ Evento.Domain/
COPY Evento.Infrastructure/ Evento.Infrastructure/
COPY Evento.Email/ Evento.Email/
COPY Evento.Jobs/ Evento.Jobs/
COPY Evento.Payments/ Evento.Payments/
COPY Evento.Endpoints/ Evento.Endpoints/

# Restore & build
RUN dotnet restore
RUN dotnet publish Evento.Endpoints/Evento.Endpoints.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Evento.Endpoints.dll"]
