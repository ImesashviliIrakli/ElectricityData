# ElectricityData

Hello

This is a web api that returns the last four months of electricity data.

When you run the app, please first run the "update-database" command in the package manager console.

When it is done updating the database you can run the app successfully.

On the screen you will several endpoints. You should go through all of them in the order it is presented.

Each will return their own months of data. after that all for months data will be added to the database.

Then run the last endpoint and you will see that now you have the last four months of electricity data aggregated.

Added loggin using serilog and saved the logs to a file, of which the filepath is specified in the program.cs class.

Added Unit tests that test getting all four months of data.

The application does run on docker but sadly it throws an error when trying to add to the database. Some error of LocalDB not supported.
Due to my lack of knowledge on docker, I decided to not waste time anymore and work on the error on myself rather than use up more time.

Hope you understand.
