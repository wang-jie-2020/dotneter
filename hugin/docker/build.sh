#! /bin/bash

############################################################################################################
############################################################################################################

DOCKERFILES=(
	"../src/identity/Hugin.IdentityServer.Host/Dockerfile"
	"../src/platform/Hugin.Platform.Host/Dockerfile"
	"../src/terminal/Hugin.Terminal.Host/Dockerfile"
)

DOCKERIMAGES=(
	"hugin.identity"
	"hugin.platform"
	"hugin.terminal"
)

DOCKERREPOSITORIES=(
	"registry.cn-hangzhou.aliyuncs.com/wangjie0303/identity"
	"registry.cn-hangzhou.aliyuncs.com/wangjie0303/platform"
	"registry.cn-hangzhou.aliyuncs.com/wangjie0303/ternimal"
)

DOCKER_IMAGE_VERSION="1.0"
DOCKER_REPOSITORY="registry.cn-hangzhou.aliyuncs.com"
DOCKER_REPOSITORY_USER=""
DOCKER_REPOSITORY_PASSWORD=""

HELPINFO="args: [-i , -p , -h] or [--image= , --push , --help]" 

############################################################################################################
############################################################################################################

GETOPT_ARGS=`getopt -o i:ph -al image:,push,help -- "$@"`
eval set -- "$GETOPT_ARGS"

while [ -n "$1" ]
do
        case "$1" in
                -p|--push)  PUSH="true"; shift 1;;
                -i|--image) IMAGE=$2; shift 2;;
                -h|--help)  echo -e $HELPINFO; exit 0 ;;
                --) break ;;
                *) echo $HELPINFO; break ;;
        esac
done

#echo "PUSH: $PUSH , IMAGE: $IMAGE"

#if [[ -z $PUSH || -z $IMAGE ]]; then
#	echo "PUSH: $PUSH , IMAGE: $IMAGE"
#	exit 0
#fi

############################################################################################################
############################################################################################################

function getArrItemIdx(){
	local arr=$1
	local item=$2
	local index=0
	
	for i in ${arr[*]}
	do
		if [[ $item == $i ]];then
			echo $index
			return
		fi
		index=$(( $index + 1 ))
	done
}

if [[ ! -z $IMAGE ]]; then
	index=$(getArrItemIdx "${DOCKERIMAGES[*]}" $IMAGE)
	if [[ -z $index ]]; then
		echo "IMAGE NOT FOUND"
		exit -1
	fi
	DOCKERIMAGES=(${DOCKERIMAGES[$index]})
	DOCKERFILES=(${DOCKERFILES[$index]})
	DOCKERREPOSITORIES=(${DOCKERREPOSITORIES[$index]})
fi

#echo ${DOCKERIMAGES[*]}
#echo ${DOCKERFILES[*]}
	
#for var in ${DOCKERFILES[@]};do
#echo $var
#done

############################################################################################################
############################################################################################################

function buildImage(){
	local dockerfile=$1
	local image=$2
	local version=$3

	docker build -t $image -f $dockerfile ../
	docker tag $image $image:$version
}

function pushImage(){

	local repository=$1
	local image=$2
	local version=$3

	docker tag $image $repository
	docker push $repository
	docker rmi $repository
	
	docker tag $image $repository:$version
	docker push $repository:$version
	docker rmi $repository:$version
}

let index=0
while [ $index -lt ${#DOCKERIMAGES[*]} ]
do
    echo "---------building image ${DOCKERIMAGES[$index]}---------"
	buildImage ${DOCKERFILES[$index]} ${DOCKERIMAGES[$index]} $DOCKER_IMAGE_VERSION
	echo "---------building done---------"
	
    let index+=1
done


if [[ ! -z $PUSH ]]; then
	
	docker login --username=$DOCKER_REPOSITORY_USER --password=$DOCKER_REPOSITORY_PASSWORD $DOCKER_REPOSITORY
	
	let index=0
	while [ $index -lt ${#DOCKERIMAGES[*]} ]
	do
		echo "---------pushing image ${DOCKERIMAGES[$index]}---------"
		pushImage ${DOCKERREPOSITORIES[$index]} ${DOCKERIMAGES[$index]} $DOCKER_IMAGE_VERSION
		echo "---------pushing done---------"
		
		let index+=1
	done
fi

############################################################################################################
############################################################################################################

if [[ ! -z $(docker images -q -f dangling=true) ]]; then
	docker rmi $(docker images -q -f dangling=true)
fi

