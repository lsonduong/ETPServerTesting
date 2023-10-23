# Introduction 
[HAL HDS] ETP Automated Testing Framework is an automation test framework based on WITSMLStudio Desktop and MSTest written in C#. It includes ETP.Devkit and WITSMLStudio source code.

It contains the following projects.

##### HDS.iETP
Provides the main application user interface for PDS WITSMLstudio Desktop and Save Inputs Parameters Tool that allow user to save their inputs to file.

##### HDS.iETP.Core
A collection of reusable components and plug-in framework of WITSMLStudio.

##### HDS.iETP.IntegrationTest
Contains automation test framework based on MSTest Unit Testing that includes the headless ETP Client App and Messages Comparing Validation Tool.

##### HDS.iETP.Plugins.DataReplay
Data Producer plug-in that simulates streaming data in and out of a WITSML server.

##### HDS.iETP.Plugins.EtpBrowser
ETP Browser plug-in to communicate with a WITSML server via ETP protocol.

##### HDS.iETP.Plugins.ObjectInspector
Object Inspector plug-in that displays WITSML data objects with corresponding Energistics schema information.

##### HDS.iETP.Plugins.WitsmlBrowser
WITSML Browser plug-in to communicate with a WITSML server via SOAP.

##### HDS.iETP.UnitTest
Unit tests for the WITSML Browser and core functionality.


# Build and Test

##### Restore and build the solution
> nuget restore HAL.HDS.iETP.sln

> msbuild HAL.HDS.iETP.sln

##### Run Tests
> MSTest.exe /testcontainer:%WORKSPACE%\\src\\Desktop.IntegrationTest\\bin\\Debug\\HAL.HDS.iETP.IntegrationTest.dll /test:HAL.HDS.iETP.IntegrationTestCases.%TestClassToRun% /resultsfile:%WORKSPACE%\\Results.trx

# References
Azure repos path:
> https://dev.azure.com/HAL-HDS/Real-Time/_git/rts-iEtp-AutomatedTests

The WITSMLStudio Desktop source code is published here:
> https://github.com/pds-technology/witsml-studio