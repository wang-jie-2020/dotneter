#! /bin/bash

docker-compose down

#############################################
DOCKERREPOSITORIES=(
	"registry.cn-hangzhou.aliyuncs.com/wxlgzh/identity"
	"registry.cn-hangzhou.aliyuncs.com/wxlgzh/platform"
	"registry.cn-hangzhou.aliyuncs.com/wxlgzh/terminal"
)
index=0
while [ $index -lt ${#DOCKERIMAGES[*]} ]
do
	docker pull ${DOCKERREPOSITORIES[$index]}
	
	let index+=1
done
#############################################

docker-compose up -d