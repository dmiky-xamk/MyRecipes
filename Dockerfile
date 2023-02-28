#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
COPY ["src/MyRecipes.API/MyRecipes.API.csproj", "src/MyRecipes.API/"]
COPY ["src/MyRecipes.Application/MyRecipes.Application.csproj", "src/MyRecipes.Application/"]
RUN dotnet restore "src/MyRecipes.API/MyRecipes.API.csproj"
COPY . .
WORKDIR "/src/MyRecipes.API"
RUN dotnet build "MyRecipes.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyRecipes.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyRecipes.API.dll"]