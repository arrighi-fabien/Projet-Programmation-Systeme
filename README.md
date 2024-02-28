# EasySave By ProSoft

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

This project is a simple backup software that allows you to create backups of your files and folders.

# Summary

 - [Built With](#Built-With)
 - [Usage guide](#Usage-Guide)
 - [Download and use](#Download-and-use)
 - [CI/CD pipeline for EasySave](#CI-CD-pipeline-for-EasySave)
 - [Authors](#Authors)
	
 # Built With

- .NET
- C#

# Languages
- English
- French
- Spanish
- German
- Italian
- Russian
- Arabic

# Usage Guide

EasySave now offers an enhanced interface for managing your backup jobs. Below is a comprehensive guide to navigate through the application's features. <br>

## Main Menu

Upon launching the application, the main menu presents you with the following options: <br>

- **Execute Save Job :** Run configured backup jobs. <br>
- **Create Save Job :** Set up a new backup job. <br>
- **Update Save Job :** Modify existing backup jobs. <br>
- **Delete Save Job :** Remove backup jobs that are no longer needed. <br>
- **Settings :** Adjust application settings such as language, log format, encryption extensions and professional app. <br>

## Execute or Delete Save Job

To execute or delete a save job: <br>
- Navigate to the corresponding option from the main menu. <br>
- Select the desired job from the list displayed on the right side of the interface. <br>

## Create or Update Save Job

Both creating a new save job and updating an existing one share a similar process: <br>
- **For creation :** Choose `Create Save Job` from the main menu. A new page opens. <br>
- **For update :** Select a save job to modify, then choose `Update Save Job`. You'll be directed to a similar page as for creation. <br>

On the configuration page: <br>
- **Save Job Name :** Enter a unique name for the job. <br>
- **Source Folder :** Specify the folder you wish to back up. <br>
- **Destination Folder :** Define where the backup should be stored. <br>
- **Save Type :** Select between a full or differential backup. <br>

## Settings

Accessing `Settings` from the main menu opens a new page where you can configure: <br>
- **Language :** Choose between English and French. <br>
- **Log Format :** Select your preferred format for logs, either JSON or XML. <br>
- **Extensions to Encrypt :** Manually input file extensions you want to encrypt during the backup process. <br>
- **Professional App :** Block the transfer if the application(s) in Professional App are open
- **Priority Extension :** No backup of a non-priority file can be made as long as there are priority extensions pending on at least one job.
- **Limited weight(Ko) :** To avoid overloading the bandwidth, no two files larger than n KB may be transferred at the same time.
- **Server Status :** Start or shut down the server, so you can monitor the progress of backups in real time on a remote workstation
- **Server Port :** Port to set up the TCP connection (client-server)

## Closing the Application

To exit the application safely: <br>
- Navigate to `Close Application` from any page. <br>
- Confirm to ensure all your settings and configurations are saved. <br>

By adhering to this guide, you'll be able to efficiently manage your backup jobs with EasySave. Whether it's performing routine backups or adjusting settings, EasySave's intuitive interface caters to all your data protection needs. <br>

# Download and use

Depending on your operating system, download the appropriate zip file from the releases on our GitHub page.

### Windows 64-bit

- Download [EasySaveGUI.zip](https://github.com/arrighi-fabien/Projet-Programmation-Systeme/releases). <br>
- Extract the zip file. <br>
- Launch `EasySaveGUI.exe`. <br>

### Linux 64-bit

- Download [EasySaveConsole_Linux64.tar.gz](https://github.com/arrighi-fabien/Projet-Programmation-Systeme/releases/download/JENKINS-v2/EasySaveConsole_Linux.tar.gz) <br>
- Extract the tar file with `tar -xzvf EasySaveConsole_Linux.tar.gz`. <br>
- Open a terminal and navigate to the extracted folder: `cd /path/to/linux-x64`. <br>
- Make the file executable with `chmod +x EasySaveConsole`. <br>
- Launch the application with `./EasySaveConsole`. <br>

## Use the console application with Docker

A Docker container is available to easily test the application on Linux. You can pull it from Docker Hub: <br>
`docker pull airg213/easysaveconsole-v1.1:latest` <br>

To launch the container and access the application: <br>
`docker run -it --name easysaveconsole-v1.1 airg213/easysaveconsole-v1.1:latest` <br>

Once in the container, navigate to the application directory: <br>
`cd /opt/linux-x64` <br>

Launch the application with: <br>
`./EasySaveConsole` <br>


# CI/CD pipeline for EasySave with GitHub Actions

The CI/CD pipeline for EasySave is now powered by GitHub Actions, enabling automated workflows for building, testing, and deploying the EasySave application directly from GitHub. This pipeline ensures that every change pushed to the main branch goes through a rigorous process of validation before being made available for release. <br>

## Workflow Stages

### 1. Build and Test
- **Trigger :** Any push or pull request to the `main` branch. <br>
- **Actions :** The .NET application is built and tested using the latest version of the .NET framework. This ensures that all changes are verified and meet the project's quality standards. <br>

### 2. Publish
- **Trigger :** Successful completion of the build and test stage on the `main` branch. <br>
- **Actions :** The application is published, and the artifacts (executable files) are prepared for deployment. These artifacts are then archived and uploaded as workflow artifacts, ensuring they are available for deployment. <br>

### 3. Release
- **Trigger :** Manual trigger by the repository maintainers or automated trigger upon successful completion of the publish stage. <br>
- **Actions :** A new release is created on GitHub, and the archived artifacts from the publish stage are attached to the release. This makes the latest version of the application readily available for users to download directly from the GitHub releases page. <br>

## Getting Started with GitHub Actions

To view and manage the CI/CD pipeline: <br>
1. Navigate to the GitHub repository. <br>
2. Click on the "Actions" tab to see the list of workflows. <br>
3. You can see the status of recent workflows, view logs, and manage workflow runs. <br>

## Download and Use

To download the latest version of EasySave: <br>
- Visit the [Releases](https://github.com/arrighi-fabien/Projet-Programmation-Systeme/releases) page of the EasySave GitHub repository. <br>
- Download the appropriate artifact for your operating system (Windows/Linux). <br>
- Follow the usage guide for installation and operation instructions. <br>

The GitHub Actions CI/CD pipeline ensures that EasySave is continuously integrated and delivered with the highest standards of quality and reliability. <br>

For more details on GitHub Actions and workflows, visit the [GitHub Actions documentation](https://docs.github.com/en/actions). <br>

# Authors 

* **FODIL Nel** _alias_ [@nel34](https://github.com/nel34)
* **ARRIGHI Fabien** _alias_ [@arrighi-fabien](https://github.com/arrighi-fabien)
* **GOUADFEL Rayan** _alias_ [@AirG213](https://github.com/AirG213)
* **MARCELLI Enzo** _alias_ [@EnzoMrcli](https://github.com/EnzoMrcli)

Project Link: [https://github.com/arrighi-fabien/Projet-Programmation-Systeme](https://github.com/arrighi-fabien/Projet-Programmation-Systeme)
