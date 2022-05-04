# AddressBook

- Project Framework `net6.0`, DB `SQLite`

- The application will create an admin user on Startup if it doesn't exist. The credentials
  of this admin user can be found in the `appsettings.json` file. Additionally it creates the roles
  `Admin and Regular` in order to differenciate the admin user from a regular user. It's set up in this way
  just for this challenge, in a real life scenario that can be done via Migrations or let a super user
  manage the roles.

- The migrations are applied automatically on Startup as well.
  Please clone the repo on [Github] (https://github.com/alvarodiaz889/AddressBook) and
  install [.NetCore](https://dotnet.microsoft.com/en-us/download)
