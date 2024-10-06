FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV DOTNET_URLS=http://+:5000
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["./Bookstore.Api/Bookstore.Api.csproj", "src/Bookstore.Api/"]
COPY ["./Bookstore.Application/Bookstore.Application.csproj", "src/Bookstore.Application/"]
COPY ["./Bookstore.Domain/Bookstore.Domain.csproj", "src/Bookstore.Domain/"]
COPY ["./Bookstore.Persistence/Bookstore.Persistence.csproj", "src/Bookstore.Persistence/"]
COPY ["./Bookstore.Infrastructure/Bookstore.Infrastructure.csproj", "src/Bookstore.Infrastructure/"]

RUN dotnet restore "src/Bookstore.Api/Bookstore.Api.csproj"

COPY . .

WORKDIR "/src/Bookstore.Api/"
RUN dotnet build -c Release  -o /app/build

FROM build AS publish
RUN dotnet publish -c Release  -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Bookstore.Api.dll"]