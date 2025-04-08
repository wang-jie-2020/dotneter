#! /bin/bash

cd docker-mysql
docker-compose up -d
cd ..

cd docker-rabbitmq
docker-compose up -d
cd ..

cd docker-redis
docker-compose up -d
cd ..

#cd docker-nginx
#docker-compose up -d
#cd ..