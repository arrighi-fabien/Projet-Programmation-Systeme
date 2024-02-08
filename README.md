# EasySave By ProSoft

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

This project is a simple backup software that allows you to create backups of your files and folders.

# Summary

 - [User guide](#User-Guide)
 - [Available languages](#Available-languages)
 - [Interface](#Interface)
 - [Built With](#Built-With)
 - [Download and use](#Download-and-use)
 - [Use with Docker](#Use-with-Docker)
 - [CI/CD pipeline for EasySave](#CI/CD-pipeline-for-EasySave)
 - [Using Docker Hub](#Using-Docker-Hub)
 - [Authors](#Authors)


# User guide

## Available languages

- English
- French
- Corsican

## Interface 

Firstly, after launching the app we arrive at this interface menu where we can carry out these 6 different tasks:

_1. Change language_ <br>
_2. Execute one or more save jobs_ <br>
_3. Create save job_ <br>
_4. Update save job_ <br>
_5. Delete save job_ <br>
_6. Close application_ <br>

### Change language 
_Choose "1" and select the language._

### Execute one or more save jobs
_Choose "2" to launch one or more backup jobs._

### Create save job
_Choose "3" to create a job._ <br>
_Then enter the name of the job._ <br>
_Enter the path of the source folder and the path of the destination folder._ <br>
_Then choose between a full save or a differential save._ <br>

### Update save job
_Choose "4" to update a job._

### Delete save job
_Choose "5" to delete a job._

### Close application
_Choose "6" to close the application._

# Built With

- .NET
- C#

# Download and use

Depending on your operating system, download the appropriate zip file from the releases on our GitHub page.

### Windows 64-bit

- Download `EasySaveConsole_Win64.zip`. <br>
- Extract the zip file. <br>
- Launch `EasySaveConsole.exe`. <br>

### Linux 64-bit

- Download `EasySaveConsole_Linux64.zip`. <br>
- Extract the zip file. <br>
- Open a terminal and navigate to the extracted folder: `cd /path/to/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64`. <br>
- Make the file executable with `chmod +x EasySaveConsole`. <br>
- Launch the application with `./EasySaveConsole`. <br>

# Use with Docker

A Docker container is available to easily test the application on Linux. You can pull it from Docker Hub: <br>
`docker pull airg213/easysaveconsole:1.0` <br>

To launch the container and access the application: <br>
`docker run -it airg213/easysaveconsole:1.0` <br>

Once in the container, navigate to the application directory: <br>
`cd /etc/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64` <br>

Launch the application with: <br>
`./EasySaveConsole` <br>


# CI/CD pipeline for EasySave

The CI/CD pipeline for the EasySave project automates the process of compiling, testing, packaging, and deploying the EasySave application. Every change pushed to the Git repository triggers this pipeline, ensuring continuous integration and deployment.

### Pipeline Stages

#### 1. SCM Polling
**Description:** Monitors the specified branch of the Git repository for any new commits and triggers the pipeline automatically. <br>
**Configuration:** Configured to monitor the "main" branch. <br>

#### 2. Build
**Description :** Compiles the source code using `dotnet publish` and generates the necessary binaries.  

#### 3. Run (Test)
**Description :** Runs the application to ensure it works as expected in a test environment.  

#### 4. Packaging (Zip)
**Description :** Package the application in separate ZIP files for Windows and Linux. <br>
**Orders :** <br>
- For Windows: `zip -r EasySaveConsole_Win.zip ./EasySaveConsole/publish/win-x64/` <br>
- For Linux: `zip -r EasySaveConsole_Linux.zip ./EasySaveConsole/publish/linux-x64/` <br>

#### 5. Release
**Description :** Publishes packages to GitHub Releases, making it easier to deploy new versions of the application. <br>
**Command:** Script to create a release on GitHub and upload the ZIP archives. <br>

#### 6. Tagging
**Description :** Marks each successful build with a unique tag in GitHub, allowing easy version tracking. <br>
**Configuration:** Uses `$BUILD_ID` to create a unique tag, for example `v${BUILD_ID}`. <br>

#### 7. Clean-Up
**Description :** Cleans up the post-build Jenkins workspace to avoid contamination between builds. <br>
**Jenkins Action:** "Delete workspace when build is done." <br>

# Using Docker Hub

Jenkins server is available on the Docker Hub for easy recovery. You can pull the preconfigured Jenkins server with the CI/CD pipeline for EasySave: <br>
`docker pull airg213/easysavejenkins:latest` <br>

To launch the Jenkins server in a Docker container: <br>
`docker run -p 8080:8080 -p 50000:50000 airg213/easysavejenkins:latest` <br>

# Authors 

* **FODIL Nel** _alias_ [@nel34](https://github.com/nel34)
* **ARRIGHI Fabien** _alias_ [@arrighi-fabien](https://github.com/arrighi-fabien)
* **GOUADFEL Rayan** _alias_ [@AirG213](https://github.com/AirG213)
* **MARCELLI Enzo** _alias_ [@EnzoMrcli](https://github.com/EnzoMrcli)

Project Link: [https://github.com/arrighi-fabien/Projet-Programmation-Systeme](https://github.com/arrighi-fabien/Projet-Programmation-Systeme)
