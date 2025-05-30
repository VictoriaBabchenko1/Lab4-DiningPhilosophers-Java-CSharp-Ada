FROM debian:bookworm
RUN apt-get update && \
    apt-get install -y gnat && \
    apt-get clean
WORKDIR /app
COPY dining_philosophers.adb .
RUN gnatmake dining_philosophers.adb
CMD ["./dining_philosophers"]
