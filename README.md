# This is an extension framework for Native AspNetCore 3.x

### What we support now or in the future we will ?

- Mini Profiler
- IP Rate Limit
- Swagger
- SignalR
- Docker CI
- Hosted Task Service
- Hangfire
- Log4net/Nlog
- Redis
- MongoDB/SqlLite
- SqlSugar/EFCore
- IdentityServer4
- Files Manage Plugin
- Email Service
- Jwt token
- Autofac
- AutoMapper
- Shouldly

### How To Run in debugger mode

###### Sqlite

if you wanna do CURD with SqlSugar , sqlite3 must be installed.

```shell
sqlite3 mydb.db
```

###### Redis

if  you wanna run Hangfire to set jobs, then redis service should be started .

```shell
sudo redis-server /etc/redis/redis.conf
```

###### Mysql 

if you wanna active HttpReports , I choose mysql as its source in this project.

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

### How To Run in docker ?

```shell
# Step1 pull images from docker hub
docker pull redis
docker pull mysql

# Step 2 build our own image from aspnet core web application
# It should locate to dirctory where *.sln is 
docker build -f AspDotNetCore3/Dockerfile -t myweb .

# Step3 Run our service from images in containers
# -v mean Persistence
# --networks mean services belong to same network segment by bridge mode
docker run --network test_nets --name myredis -p 6379:6379 -v /redis/data:/redis/data -d redis
docker run --network test_nets --name tmysql -p 3306:3306 -v /home/mysql:/var/lib/mysqll -e MYSQL_ROOT_PASSWORD=123456 -d mysql:latest
docker run --network test_nets --name mycoreweb -p 5001:80 -d myweb:latest

```

PS: if you want enter someone service you can follow these orders

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



### Portal

goto SwaggerUI：{host}/  OR  {host}/index.html

goto Hangfire ：{host}/hangfire 

goto HttpReports ：{host}/httpreports



