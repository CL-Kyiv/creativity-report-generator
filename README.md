# **Creativity Report Generator**
## About app:

Application for automated generation of creativity reports from Git repositories.


The app works with bitbucket (as a web app and as an electron app) and with local repositories (only as an electron app).


## How to run the app:

0) Install [ASP.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1).

1) Download the project repository from latest [release](https://github.com/CL-Kyiv/creativity-report-generator/releases/tag/v2.0.0) to your computer.

   ### Application structure:
```
   Name                                   Description
├──bitbucketApp                         * application for working with bitbucket repositories
├─────app                               * build files
├─────run.bat                           * script to run the application

├──localApp                             * application for working with local repositories
├─────api                               * web api build files
├─────electron                          * electron application build files
├─────run.bat                           * script to run the application
```
2) Choose which repositories you will work with and run the appropriate run.bat file.

      If you chose to work with bitbucket repositories, then the run.bat file starts the web interface and the application in the browser.

      If you chose to work with local repositories then run.bat file runs the web api and electron application in different windows.

## How to work with the project
1) Download the project repository to your computer.

2) Open the settings.json file and change the isBitbucket field according to which application you want to work with.
```bash
{
    "currentService": {
      "isBitbucket": true ## to work with bitbucket repositories
    }
}
```
```bash
{
    "currentService": {
      "isBitbucket": false ## to work with local repositories
    }
}
```
3) Go to the backend/ directory, open the project solution (CreativityReportGenerator.sln) and run it.

4) Open a command prompt, go to the frontend/ directory and write the command data:
```bash
npm install
```

```bash
npm run ng:serve ## if you want to run the application as a web app
```
```bash
npm run start ## if you want to run the application as an electron app
```

> Remind you that application works with local repositories only as an electron app

## How to use the application to work with bitbucket repositories:

1) Enter the consumer key and consumer secret key. To find or create them, follow the instructions in the ["Create a consumer" section](https://support.atlassian.com/bitbucket-cloud/docs/use-oauth-on-bitbucket-cloud/#:~:text=make%20API%20calls.-,Create%20a%20consumer,-OAuth%20needs%20a).

2) Select a repository from the drop-down list.

3) Select the month and year for which you want to receive a report of creativity. After that click on the "Enter" button.

4) Enter your working hours.

5) Select the author for which you want to receive the report of creativity. After that click on the "Generate" button.

6) Select the tasks that will be included in the report of creativity.

7) Click on the "Export file" button to download the .csv file.

## How to use the application to work with local repositories:

1) Select the folder with the repository by clicking on the "Directory" button.

2) Select the month and year for which you want to receive a report of creativity. After that click on the "Enter" button.

3) Enter your working hours.

4) Select the author for which you want to receive the report of creativity. After that click on the "Generate" button.

5) Select the tasks that will be included in the report of creativity.

6) Click on the "Export file" button to download the .csv file.

## Technologies
* Electron
* Angular
* ASP.NET Core web API