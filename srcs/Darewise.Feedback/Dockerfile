FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Darewise.Feedback/Darewise.Feedback.csproj", "Darewise.Feedback/"]
RUN dotnet restore "Darewise.Feedback/Darewise.Feedback.csproj"
COPY . .
WORKDIR "/src/Darewise.Feedback"
RUN dotnet build "Darewise.Feedback.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Darewise.Feedback.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Darewise.Feedback.dll"]
