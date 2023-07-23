#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Discount.API/Discount.API.csproj", "Discount.API/"]
COPY ["Discount.Application/Discount.Application.csproj", "Discount.Application/"]
COPY ["Discount.Domain/Discount.Domain.csproj", "Discount.Domain/"]
COPY ["Discount.Infrasturcture/Discount.Infrasturcture.csproj", "Discount.Infrastructure/"]
RUN dotnet restore "Discount.API/Discount.API.csproj"
COPY . .
WORKDIR "/src/Discount.API"
RUN dotnet build "Discount.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.API.dll"]