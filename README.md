# EasySave By ProSoft

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

This project is a simple backup software that allows you to create backups of your files and folders.

# Summary

 - [Usage guide](#Usage-Guide)
 - [Built With](#Built-With)
 - [Download and use](#Download-and-use)
 - [CI/CD pipeline for EasySave](#CI-CD-pipeline-for-EasySave)
 - [Authors](#Authors)


# Usage Guide

EasySave offers a straightforward interface for managing your backup jobs. Here's a step-by-step guide to using each feature with additional details and examples.

## 1. Change Language

Upon starting the application, you're greeted with the option to select a language. This changes the application's interface to your preferred language.

- **To change the language:**
  - Type `1` and press Enter.
  - You'll see `Select a language:` followed by the options `1. English` or `2. Français`.
  - Type `1` for English or `2` for French and press Enter.

## 2. Execute One or More Save Jobs

You can execute previously configured save jobs individually or in bulk.

- **To execute save jobs:**
  - Choose `2` from the main menu.
  - If you have multiple save jobs, you'll be prompted to select which ones to execute. Enter the job number(s), separated by `-` or `;` if executing multiple.

## 3. Create Save Job

Creating a save job involves specifying a source and destination folder, as well as choosing between a full or differential save.

- **To create a new save job:**
  - Select `3` and press Enter.
  - Follow the prompts to enter a unique job name, source folder, and destination folder.
  - Choose `1` for a full save, which backs up all files in the source folder, or `2` for a differential save. Differential saves only back up files that have been added or modified since the last save, saving time and storage space.

## 4. Update Save Job

Update existing save jobs to modify their settings or backup type.

- **To update a save job:**
  - Choose `4` from the main menu.
  - Select the save job you wish to update by entering its number.
  - You can then update the job's name, source folder, destination folder, and whether it should perform a full or differential save.

## 5. Delete Save Job

Remove save jobs that are no longer needed.

- **To delete a save job:**
  - Select `5` from the main menu.
  - Enter the number of the save job you wish to delete.

## 6. Log Format Choice

EasySave allows you to choose the format of your logs between JSON and XML, enabling flexibility in how you store and view backup information.

- **To select the log format:**
  - Choose `6` from the main menu after launching the app.
  - You will be prompted to select the log format: `1. /JSON` or `2. /XML`.
  - Type `1` for JSON or `2` for XML and press Enter.
  - Your choice will apply to all future logs until you decide to change it again.

## 7. Close Application

This option safely exits the application, ensuring all your settings and configurations are preserved.

- **To close the application:**
  - Navigate to `7` on the main menu.
  - Press Enter to confirm and safely exit EasySave.

By following this guide, you can efficiently manage your backup jobs with EasySave, ensuring your data is backed up according to your needs and preferences. Whether you're performing routine backups or need to update your backup configurations, EasySave provides a user-friendly interface to accomplish your goals.

# Built With

- .NET
- C#

# Download and use

Depending on your operating system, download the appropriate zip file from the releases on our GitHub page.

### Windows 64-bit

- Download [EasySaveConsole_Win64.zip](https://github.com/arrighi-fabien/Projet-Programmation-Systeme/releases/download/JENKINS-v1/EasySaveConsole_Win64.zip). <br>
- Extract the zip file. <br>
- Launch `EasySaveConsole.exe`. <br>

### Linux 64-bit

- Download [EasySaveConsole_Linux64.zip](https://github.com/arrighi-fabien/Projet-Programmation-Systeme/releases/download/JENKINS-v1/EasySaveConsole_Linux64.zip) <br>
- Extract the zip file. <br>
- Open a terminal and navigate to the extracted folder: `cd /path/to/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64`. <br>
- Make the file executable with `chmod +x EasySaveConsole`. <br>
- Launch the application with `./EasySaveConsole`. <br>

## Use the application with Docker

A Docker container is available to easily test the application on Linux. You can pull it from Docker Hub: <br>
`docker pull airg213/easysaveconsole:latest` <br>

To launch the container and access the application: <br>
`docker run -it --name easysaveconsole airg213/easysaveconsole:latest` <br>

Once in the container, navigate to the application directory: <br>
`cd /opt/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64` <br>

Launch the application with: <br>
`./EasySaveConsole` <br>


# CI CD pipeline for EasySave

The CI/CD pipeline for the EasySave project automates the process of compiling, testing, packaging, and deploying the EasySave application. Every change pushed to the Git repository triggers this pipeline, ensuring continuous integration and deployment.

### Pipeline Stages

#### 1. SCM Polling
**Description:** Monitors the specified branch of the Git repository for any new commits and triggers the pipeline automatically. <br>
**Configuration:** Configured to monitor the "main" branch. <br>

#### 2. Build
**Description :** Compiles the source code using `dotnet publish` and generates the necessary binaries.  <br>

#### 3. Run (Test)
**Description :** Runs the application to ensure it works as expected in a test environment.  <br>

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

## Import Jenkins server with Docker Hub

Jenkins server is available on the Docker Hub for easy recovery. You can pull the preconfigured Jenkins server with the CI/CD pipeline for EasySave: <br>
`docker pull airg213/easysavejenkins:latest` <br>

To launch the Jenkins server in a Docker container: <br>
`docker run -d -p 8080:8080 -p 50000:50000 --name jenkins airg213/easysavejenkins:latest` <br>

Jenkins is accessible at http://127.0.0.1:8080. You can log in with the preconfigured credentials to manage the CI/CD pipelines for EasySave. <br>

# Authors 

* **FODIL Nel** _alias_ [@nel34](https://github.com/nel34)
* **ARRIGHI Fabien** _alias_ [@arrighi-fabien](https://github.com/arrighi-fabien)
* **GOUADFEL Rayan** _alias_ [@AirG213](https://github.com/AirG213)
* **MARCELLI Enzo** _alias_ [@EnzoMrcli](https://github.com/EnzoMrcli)

Project Link: [https://github.com/arrighi-fabien/Projet-Programmation-Systeme](https://github.com/arrighi-fabien/Projet-Programmation-Systeme)
