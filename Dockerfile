# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY HotelHRM.sln ./
COPY HotelHRM.Models/HotelHRM.Models.csproj HotelHRM.Models/
COPY HotelHRM.Data/HotelHRM.Data.csproj HotelHRM.Data/
COPY HotelHRM.Services/HotelHRM.Services.csproj HotelHRM.Services/
COPY HotelHRM.Web/HotelHRM.Web.csproj HotelHRM.Web/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY . .

# Build the application
WORKDIR /src/HotelHRM.Web
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "HotelHRM.Web.dll"]
