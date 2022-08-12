FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

WORKDIR /app
COPY ./ ./

WORKDIR /app/SimpleOpenCap
RUN dotnet restore
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
COPY --from=build-env /app/SimpleOpenCap/out .

RUN apt-get update && apt-get install -y curl

ENV ASPNETCORE_URLS=http://+:2022
ENV DISABLE_CORS=true
ENV ENABLE_SWAGGER=false

CMD [ "dotnet", "SimpleOpenCap.dll" ]
