# simple-opencap
A simple, json file-based implementation of [OpenCAP](https://github.com/opencap/protocol).

## data.json

This file lists the aliases you want to serve as well as all of the addresses for them.
There is no validation done against the address types and formats provided by OpenCAP,
so there is no need to update the container image if new supported formats are released.
In addition, this allows you to use unsupported types, if you desire.

The format of the file is simple:

```
{
  "xno$kga.earth": [
    {
      "address_type": "300",
      "address": "nano_1234...432"
    },
    {
      "address_type": "300",
      "address": "nano_1567...765",
      "extensions": {}
    },
    {
      "address_type": "300",
      "address": "nano_1890...098",
      "extensions": {
        "nickname": "Kevin"
      }
    }
  ]
}
```

Each alias you want to serve provides a list of the addresses associated with it.
Set up each address how you want it to be served.  Extensions are optional; if they
are not provided, simple-opencap will fill in an empty extensions object.

simple-opencap will handle matching aliases as well as filtering on address_type.
You can also enable a swagger interface at the root of the site (see below).  If
you leave the swagger interface disabled, the base of the site will provide a 404,
as there is nothing required to be there.

## Docker-Compose Example

```
version: '3.8'

services:

  opencap:
    image: 'cinderblockgames/simple-opencap:1'
    environment:
      # establishes the port under which it listen; default value provided
      - ENV ASPNETCORE_URLS=http://+:2022
      # determines whether to disable CORS for GET requests; default value provided
      - ENV DISABLE_CORS=true
      # determines whether to serve swagger at the root of the site; default value provided
      - ENABLE_SWAGGER=false
    volumes:
      # either the individual file or the whole folder can be mapped, but this file is required
      #- '/run/opencap/data.json:/app/config/data.json'
      # prefer the directory so that the containers pick up changes
      - '/run/opencap/config:/app/config'
    networks:
      - traefik
    deploy:
      mode: replicated
      replicas: 2
      labels:
        - 'traefik.enable=true'
        - 'traefik.docker.network=traefik'
        - 'traefik.http.routers.opencap.rule=Host(`cap.example.com`)'
        - 'traefik.http.routers.opencap.entrypoints=web-secure'
        - 'traefik.http.routers.opencap.tls'
        - 'traefik.http.services.opencap.loadbalancer.server.port=2022'

networks:
  traefik:
    external: true
```
