FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["UsersAPI/UsersAPI.csproj", "UsersAPI/"]
RUN dotnet restore "UsersAPI/UsersAPI.csproj"
COPY . .
WORKDIR "/src/UsersAPI"
RUN dotnet build "UsersAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersAPI.dll"]