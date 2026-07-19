# Build Tailwind CSS (tailwind.min.css is gitignored)
FROM node:20-alpine AS css
WORKDIR /src
COPY package.json package-lock.json ./
RUN npm ci
COPY tailwind.config.js postcss.config.js ./
COPY wwwroot ./wwwroot
COPY Views ./Views
RUN npm run build:css

# Restore and publish ASP.NET Core
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ItransitionTemplates.csproj global.json ./
RUN dotnet restore
COPY . .
COPY --from=css /src/wwwroot/css/tailwind.min.css ./wwwroot/css/tailwind.min.css
RUN dotnet publish ItransitionTemplates.csproj -c Release -o /app/publish --no-restore

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ItransitionTemplates.dll"]
