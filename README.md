# SsrsDeploy

SsrsDeploy is a tool to facilitate the automation of the deployment of reports, data sources and other artefacts to SQL Server Reporting Service (SSRS).

[![Build status](https://ci.appveyor.com/api/projects/status/7k5tda804jbcvlq4?svg=true)](https://ci.appveyor.com/project/CdricLCharlier/ssrsdeploy)
![Still maintained](https://img.shields.io/maintenance/yes/2016.svg)
[![nuget] (https://img.shields.io/nuget/v/SsrsDeploy.svg)](https://www.nuget.org/packages/SsrsDeploy/)
[![nuget pre] (https://img.shields.io/nuget/vpre/SsrsDeploy.svg)](https://www.nuget.org/packages/SsrsDeploy/)

#How-to

## Command line arguments

* ```-u``` or ```-url``` specifies the url of the target server for the deployment
* ```-f``` or ```-folder``` specifies the folder of the target server as the top-level folder for this deployment
* ```-s``` or ```-source``` specifies the path to the file containing the manifest for this deployment
* ```-r``` or ```-root``` specifies the folder containing all the artefacts (rdl, rds and other files)

## Elements in the manifest of deployment

* ```<Project>``` is the top element of the manifest
* ```<Folder>``` lets you define a sub-folder and its content
* ```<Report>``` defines the name of the report and optionaly its filename, description and visibility (```Hidden```)
* ```<DataSource>``` defines the name of data source and optionaly its filename
