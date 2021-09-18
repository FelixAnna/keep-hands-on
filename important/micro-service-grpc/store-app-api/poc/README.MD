# a simple java grpc server + client poc
1. download latest JDK (16/11) and install;

2. create parent project (maven, pom)
	- delete all folders/files except pom.xml
3. create proto-buf project (module)
	- create below parent project
	- copy dependency and build configurations
	- create proto folder under src/main
	- remove other folders
	- create a proto file under proto folder created before
	- compile this project(mvn compile), and check code generated under target/generated-sources/protobuf
		
4. create server project, 
	- add reference to protobuf project
	- add dependency
	
5. create client project
	- add reference to protobuf project
	- add dependency
	
6. compile and debug
	-- compile and install proto: mvn package install
	-- swith to parent project, run: mvn package install
	
	
	- compile server project
	- add package and spring boot application code
	- spring-boot start server project (if not detect proto generated code)
	- maven update project (if not detect proto generated code)
	- add service implementation
	- spring-boot start server project
	
	
	
	- compile server project
	- add package and client code
	- spring-boot start server project
	
done.