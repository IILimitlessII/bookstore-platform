FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/BookstorePlatform.Web/BookstorePlatform.Web.csproj", "src/BookstorePlatform.Web/"]
COPY ["src/BookstorePlatform.Application/BookstorePlatform.Application.csproj", "src/BookstorePlatform.Application/"]
COPY ["src/BookstorePlatform.Domain/BookstorePlatform.Domain.csproj", "src/BookstorePlatform.Domain/"]
COPY ["src/BookstorePlatform.Infrastructure/BookstorePlatform.Infrastructure.csproj", "src/BookstorePlatform.Infrastructure/"]
COPY ["tests/BookstorePlatform.Tests/BookstorePlatform.Tests.csproj", "tests/BookstorePlatform.Tests/"]
RUN dotnet restore "src/BookstorePlatform.Web/BookstorePlatform.Web.csproj"
COPY . .
RUN dotnet publish "src/BookstorePlatform.Web/BookstorePlatform.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN mkdir -p /app/Data
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV Database__Provider=Sqlite
ENV ConnectionStrings__Sqlite=Data Source=/app/Data/bookstore.db
ENV Database__EnsureCreated=false
ENV Database__AutoMigrate=true
EXPOSE 8080
ENTRYPOINT ["dotnet", "BookstorePlatform.Web.dll"]
