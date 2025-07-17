# ---------- STAGE 1: Build Angular frontend ----------
  FROM node:18-alpine AS frontend-build

  WORKDIR /app/frontend
  COPY book-app/ ./
  RUN npm install
  RUN npm run build -- --configuration production
  
  # ---------- STAGE 2: Build .NET backend ----------
  FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
  
  WORKDIR /app
  
  COPY *.csproj ./
  RUN dotnet restore
  
  COPY . ./
  RUN dotnet publish -c Release -o out
  
  # ---------- STAGE 3: Final image ----------
  FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
  
  WORKDIR /app
  
  # Kopiera backend
  COPY --from=backend-build /app/out ./
  
  # Kopiera Angular-build till wwwroot
  COPY --from=frontend-build /app/frontend/dist/book-app ./wwwroot
  
  # Öppna port
  EXPOSE 80
  
  # Starta din .NET-backend (ändra om din .csproj har annat namn)
  ENTRYPOINT ["dotnet", "TestPraktik.dll"]
  
  