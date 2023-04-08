How to Run

Database
1. Create a new database "Hashes" in your sql server
2. set the server and credentials of the connection string in 
		a. WebApi appsettings.json - DefaultConnection 
		b. WorkerService appsettings.json - DefaultConnection
3. Open Package Manage Console
	- select "Shared" as default project
	- run "enable-migrations"
	
RabbitMq
1. Set RabbitMq server, this uses the default guest/guest account
	a. WebApi appsettings.json - RabbitMQ_Server 
	b. WorkerService appsettings.json - RabbitMQ_Server

Run the projects
1. Select Multiple startup projects in the Solution Properties
	- set WebApi and WorkerService Action to Start