# TestMQ

A test project to explore the possibilities RabbitMQ, ActiveMQ, ZeroMQ (NetMQ) and MassTransit for RabbitMQ and ActiveMQ.

Install Docker with RabbitMQ
docker run -d --hostname my-rabbit-host --name my-rabbit -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password rabbitmq:3-management
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

WebUI http://localhost:15672/
login: guest
password: guest

Install Docker with ActiveMQ
docker pull rmohr/activemq
docker run -p 61616:61616 -p 8161:8161 rmohr/activemq

WebUI http://localhost:8161/admin/
login: admin
login: admin