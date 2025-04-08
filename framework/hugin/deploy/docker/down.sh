#! /bin/bash

cd docker-mysql
docker-compose down
cd ..

cd docker-rabbitmq
docker-compose down
cd ..

cd docker-redis
docker-compose down
cd ..

#cd docker-nginx
#docker-compose down
#cd ..