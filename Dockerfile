
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["AspDotNetCore3/AspDotNetCore3.csproj", "AspDotNetCore3/"]
RUN dotnet restore "AspDotNetCore3/AspDotNetCore3.csproj"
COPY . .
WORKDIR "/src/AspDotNetCore3"
RUN dotnet build "AspDotNetCore3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspDotNetCore3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspDotNetCore3.dll"]

LABEL version="v1.0.0"
LABEL description="aspnetcore3.1 demo"
LABEL author="hal tan"