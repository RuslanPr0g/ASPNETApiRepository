FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["YTScrapper/YTScrapper.csproj", "YTScrapper/"]
RUN dotnet restore "YTScrapper/YTScrapper.csproj"
COPY . .
WORKDIR "/src/YTScrapper"
RUN dotnet build "YTScrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YTScrapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YTScrapper.dll"]