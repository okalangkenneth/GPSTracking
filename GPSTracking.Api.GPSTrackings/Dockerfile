#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GPSTracking.Api.GPSTrackings/GPSTracking.Api.GPSTrackings.csproj", "GPSTracking.Api.GPSTrackings/"]
RUN dotnet restore "GPSTracking.Api.GPSTrackings/GPSTracking.Api.GPSTrackings.csproj"
COPY . .
WORKDIR "/src/GPSTracking.Api.GPSTrackings"
RUN dotnet build "GPSTracking.Api.GPSTrackings.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GPSTracking.Api.GPSTrackings.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GPSTracking.Api.GPSTrackings.dll"]