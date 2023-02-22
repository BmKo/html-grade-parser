FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HtmlGradeParser/HtmlGradeParser.csproj", "HtmlGradeParser/"]
RUN dotnet restore "HtmlGradeParser/HtmlGradeParser.csproj"
COPY . .
WORKDIR "/src/HtmlGradeParser"
RUN dotnet build "HtmlGradeParser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HtmlGradeParser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HtmlGradeParser.dll"]
