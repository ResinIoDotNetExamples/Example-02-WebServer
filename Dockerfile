FROM resin/rpi-raspbian:wheezy
MAINTAINER martin@prometheusengineering.net

RUN apt-get update
RUN apt-get install -y libmono2.0-cil mono-runtime

ADD layout/* /app/SimpleWebServer/

CMD ["mono", "/app/SimpleWebServer/SimpleWebServer.exe"]
