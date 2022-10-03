# **Creativity Report Generator**
## About app:

Application for automated generation of creativity reports from Git repositories

## How to run the app:

### _Desktop app:_

0) Install [ASP.NET Core 3.1 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)

1) Download the project repository from latest [release](https://github.com/CL-Kyiv/creativity-report-generator/releases/tag/v1.0.0) to your computer

2) Run the _./CreativityReportGenerator/run.bat_ file.

This file is a script that runs the web api and electron application in different windows.

## How to use the app:

1) Select the folder with the repository by clicking on the "Directory" button.

2) Select the month and year for which you want to receive a report of creativity. After that click on the "Enter" button.

3) Enter your working hours.

4) Select the author for which you want to receive the report of creativity. After that click on the "Generate" button.

5) Select the tasks that will be included in the report of creativity.

6) Click on the "Export file" button to download the csv file.

## File structure

```
   Name                                       Description
├──api                                      * web api build files
├──electron                                 * electron application build files
├──run.bat                                  * script to run the application
```
## Technologies
* Electron
* Angular
* ASP.NET Core web API