## Why start this project

**This is a project for learning asp.net core ,and you can take it as your base project when you want start a new web application in .net platform.**

# 

### What we support now or in the future we will ?

- Mini Profiler based on memory cache
- IP Rate Limit
- Swagger
- SignalR
- Docker-compose
- Hosted Task Service
- Hangfire based on redis
- NLog
- MongoDB
- HttpReports based on mysql 
- SqlSugar based on sqlite/mysql
- Gzip compression
- Autofac
- AutoMapper
- Shouldly
- EFCore  based on mysql 
- Consul
- Nginx
- Jwt token Authentication
- Elastic Search

### How to add dependencies

###### Sqlite

```shell
sqlite3 mydb.db
```

###### Redis

```shell
sudo redis-server /etc/redis/redis.conf
```

###### Mysql 

```shell
mysql -u root -p;
create user 'sa'@'%' identified by '123456';
flush privileges;
create database HttpReports;
grant all privileges on HttpReports.* to 'sa'@'%' with grant option;
flush privileges;
mysql -u sa -p;
123456;
show databases;
```

### How to deploy in docker ?

```shell
# Step1 pull images from docker hub
docker pull redis
docker pull mysql

# Step 2 build our own image from aspnet core web application
# It should locate to dirctory where *.sln is 
docker build -f Dockerfile -t myweb .

# Step3 Run our service from images in containers
# -v mean Persistence
# --networks mean services belong to same network segment by bridge mode
docker network create test_nets
docker volume create data_redis
docker volume create data_mysql
docker run --network test_nets --name myredis -p 6379:6379 -v data_redis:/redis/data -d redis --appendonly yes
docker run --network test_nets --name tmysql -p 3306:3306 -v data_mysql:/var/lib/mysqll -e MYSQL_ROOT_PASSWORD=123456 -d mysql:latest
docker run --network test_nets --name mycoreweb -p 5001:80 -d myweb:latest

```

Enter container

```shell
docker exec -it myredis redis-cli
docker exec -it tmysql bash
..
```

And you can also visit it by shell

```shell
redis-cli
mysql -u -root -p
..
```

### How to deploy with Docker-compose

The docker-compose.yaml

```yaml
# locate to dir where *.sln is

version: '3'
services:
  myredis :
    image: "redis:latest"
    ports:
     - "6379:6379"
    container_name: myredis_demo
    volumes:
    - /docker/redis/data:/redis/data
  tmysql:
    image: "mysql:latest"
    ports:
     - "3306:3306"
    restart: always
    container_name: mysql_demo
    environment:
       MYSQL_ROOT_PASSWORD: 123456
       MYSQL_DATABASE : HttpReports
    volumes:
     - /docker/mysql/data:/var/lib/mysql
     #- /docker/mysql/conf:/etc/mysql/conf.d
  mycoreweb:
    image: myweb:latest
    build:
      context: .
      dockerfile: Dockerfile
    restart: always
    container_name: mycoreweb
    ports:
     - "5001:80"
    depends_on:
     - myredis
     - tmysql
```

```powershell
cd to dir where .yaml is

#build image
docker-compose build

#create container and start it
docker-compose up -d
```

### How to deploy reverse proxy with Nginx in docker

The nginx.conf

```shell
user  nginx;
# the num of process
worker_processes  1;
error_log  /var/log/nginx/error.log warn;
pid        /var/run/nginx.pid;
events {
	# the max num of connection
    worker_connections  1024;
}
# http server config
http {
	server	 {
   		 listen   80;
   		 # server_name   example.com *.example.com;
   		 location / {
			 # host.docker.internal is an ip mapping in hosts in windows
      		 proxy_pass         https://host.docker.internal:5001;
     		 proxy_http_version 1.1;
       		 proxy_set_header   Upgrade $http_upgrade;
       		 proxy_set_header   Connection keep-alive;
       		 proxy_set_header   Host $host;
       		 proxy_cache_bypass $http_upgrade;
       		 proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       		 proxy_set_header   X-Forwarded-Proto $scheme;
    	}
	}
}
```

```shell
docker run --name docker_nginx -d -p 80:80 -v /docker/conf/nginx.conf:/etc/nginx nginx
```

### How to deploy Consul in docker

```shell
# create a cluster named consul1
docker run -d -p 8500:8500 -v /data/consul:/consul/data -e CONSUL_BIND_INTERFACE='eth0' --name=consul1 consul agent -server -bootstrap -ui -client='0.0.0.0'

# get the ip of consul1
docker inspect --format '{{ .NetworkSettings.IPAddress }}' consul1

# if the ip is 172.17.0.2
docker run -d --name=consul2 -e CONSUL_BIND_INTERFACE=eth0 consul agent --server=true --client=0.0.0.0 --join 172.17.0.2;
docker run -d --name=consul3 -e CONSUL_BIND_INTERFACE=eth0 consul agent --server=true --client=0.0.0.0 --join 172.17.0.2;
docker run -d --name=consul4 -e CONSUL_BIND_INTERFACE=eth0 consul agent --server=false --client=0.0.0.0 --join 172.17.0.2;

# check the members of consul1
docker exec -it consul1 consul members

#if you want add more datacenters to this cluster =>
#https://www.jianshu.com/p/df3ef9a4f456

```

### How to deploy elastic search and kibana

```shell
#how to run es:

docker run -d -it --name myes -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" -e "ES_JAVA_OPTS=-Xms256m -Xmx256m" elasticsearch:7.8.0

#elasticsearch.yml:

network.host: 0.0.0.0
port: 9200

#how to run kibana:

docker inspect --format '{{ .NetworkSettings.IPAddress }}' myres
docker run -d --name mykibana -e ELASTICSEARCH_URL=http://172.17.0.2:9200  -p 5601:5601 kibana:7.8.0
```



### Portal

- go check SwaggerUI  => {host}/  OR  {host}/swagger

- go check Hangfire  => {host}/hangfire 

- go check HttpReports  => {host}/httpreports

- go check SignalR => {host}/signal

- go check MiniProfiler => {host}/profiler/results-index


