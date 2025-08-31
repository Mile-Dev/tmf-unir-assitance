# compilar la imagen
docker build -t VoucherServices-image .
# correr manual la imagen de docker
docker run -d -p 5295:8080 --name VoucherServices-container VoucherServices-image


# crear api minimal
dotnet new web -o microservice-phone-consultation
# Agregarla a la solución 
dotnet sln add microservice-phone-consultation/microservice-phone-consultation.csproj

# crear api 
dotnet new webapi -o MyMicroservice
# Agregarla a la solución 
dotnet sln add PhoneConsultationService/PhoneConsultationService.csproj

-------------------------------
docker compose up 

# crear tabla en dynamo db -- se debe ubicar en la ruta en donde se encuntra el archivo
aws dynamodb create-table --cli-input-json Docs://create-table.json --endpoint-url http://localhost:8000
O  
aws dynamodb create-table  --table-name PhoneConsultations  --attribute-definitions AttributeName=idEvent,AttributeType=N AttributeName=idPhoneRecord,AttributeType=N --key-schema AttributeName=idEvent,KeyType=HASH AttributeName=idPhoneRecord,KeyType=RANGE --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5 --table-class STANDARD


#delete 
aws dynamodb delete-table --table-name PhoneConsultations --endpoint-url http://localhost:8000
aws dynamodb delete-table --table-name PhoneConsultations --endpoint-url http://localhost:8000



# listar tablas en dynamo db 
aws dynamodb list-tables --endpoint-url http://localhost:8000

#Insertar un dato 
aws dynamodb put-item --table-name PhoneConsultations --item file://put-item.json --endpoint-url http://localhost:8000