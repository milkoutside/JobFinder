# JobFinder

## for successful start bot you need:

### 1. Copy repository : `gh repo clone witchblvd/JobFinder`

### 2. Enter your token for telegram in ***TelegramClient.cs*** such variable ***_botclient***

### 3. Connect your Database in ***DataContext***. I'm using MongoDb, but you can use any other database and you will need to redo all methods related to the database code.

### 4. If you don't have ngrok.exe, then you need to [download it](https://ngrok.com/ "link"). 

### To start the server on a local port, you need to type the following in PowerShell: `./ngrok.exe http [port]`

### Take the link from the ***Forwarding*** line, which looks something like this: `https://aa91-94-45-110-42.eu.ngrok.io` and copy this string to hok in ***TelegramClient.cs***. In the end, you should end up with a string like this: 
`var hook = "https://aa91-94-45-110-42.eu.ngrok.io/api/message/update"; `

## for use you have commands:

### 1. ***/start*** - this command send starting message

### 2. ***/help*** - this command send all list commands 

### 3. ***/switchsettings*** - it's your menu for enter,delete settings

### 4. ***/startsearch*** - this is a command that starts searching  new vacancies that are not in the database

### 5. ***/startsearch*** - this is a command that stop searching  new vacancies

## If you have any questions, please contact my telegram : ***@eboshimo***
