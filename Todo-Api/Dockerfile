FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Todo-Api.csproj", "./"]
RUN dotnet restore "./Todo-Api.csproj"

COPY . .
RUN dotnet publish "./Todo-Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Todo-Api.dll"]
EXPOSE 80
