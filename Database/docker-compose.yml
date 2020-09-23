version: '3.7'
volumes:
    seq-storage:
services:
  seq:
    container_name: seq
    image: datalust/seq:latest
    volumes:
      - type: volume
        source: seq-storage
        target: /data
    ports:
      - "5342:80"
      - "5341:5341"
    expose: 
      - 44390
      - 2525
      - 587
    environment:
      ACCEPT_EULA: 'Y'
      TZ:  Asia/Bangkok
    restart: unless-stopped
