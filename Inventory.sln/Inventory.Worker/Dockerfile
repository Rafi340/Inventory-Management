FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs

COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
COPY ["Inventory.Application/Inventory.Application.csproj", "Inventory.Application/"]
COPY ["Inventory.Infrastructure/Inventory.Infrastructure.csproj", "Inventory.Infrastructure/"]
COPY ["Inventory.Web/Inventory.Web.csproj", "Inventory.Web/"]
COPY ["Inventory.Worker/Inventory.Worker.csproj", "Inventory.Worker/"]

RUN dotnet restore "Inventory.Worker/Inventory.Worker.csproj"

COPY . .

WORKDIR "/src/Inventory.Worker"
RUN dotnet build "Inventory.Worker.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Inventory.Worker.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT [ "dotnet", "Inventory.Worker.dll" ]
