# Base image for runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0@sha256:e6b552fd7a0302e4db30661b16537f7efcdc0b67790a47dbf67a5e798582d3a5 AS base
WORKDIR /app


# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build
WORKDIR /src

# Update this COPY command to reflect the correct path
COPY ["reciever.csproj", "./"]
RUN dotnet restore "reciever.csproj"

# Copy the rest of the application files
COPY . .

# Set the working directory to where your project files are
WORKDIR "/src"

# Build the application
RUN dotnet build "reciever.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "reciever.csproj" -c Release -o /app/publish

# Final image for runtime
FROM base AS final
WORKDIR /app

# Copy the published output to the final stage
COPY --from=publish /app/publish .


# Entry point
ENTRYPOINT ["dotnet", "reciever.dll"]

