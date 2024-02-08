# EasySave By ProSoft

[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)  [![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)

This project is a simple backup software that allows you to create backups of your files and folders.

# Sommaire

 - [User guide](##User-Guide)
 - [Available languages](#Available-languages)
 - [Interface](#Interface)
 - [Installation](#Installation)
 - [Autors](#Autors)
 - [Contact](#Contact)


# User guide

## Available languages

- English
- French
- Corsican

## Interface 

Firstly, after launching the app we arrive at this interface menu where we can carry out these 6 different tasks:

_1. Change language_
_2. Execute one or more save jobs_
_3. Create save job_
_4. Update save job_
_5. Delete save job_
_6. Close application_

### Change language 
Choose "1" and select the language

### Execute one or more save jobs
Write here

### Create save job
Write here

### Update save job
Write here

### Delete save job
Write here

### Close application
Choose "6" to close the application

## Built With
.Net
C#



## Téléchargement et utilisation

Selon votre système d'exploitation, téléchargez le fichier zip approprié depuis les releases sur notre page GitHub.

### Windows 64-bit

- Téléchargez `EasySaveConsole_Win64.zip`.
- Extrayez le fichier zip.
- Lancez `EasySaveConsole.exe`.

### Linux 64-bit

- Téléchargez `EasySaveConsole_Linux64.zip`.
- Extrayez le fichier zip.
- Ouvrez un terminal et naviguez vers le dossier extrait : `cd /path/to/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64`.
- Rendez le fichier exécutable avec `chmod +x EasySaveConsole`.
- Lancez l'application avec `./EasySaveConsole`.

### Utilisation avec Docker

Un conteneur Docker est disponible pour tester facilement l'application sous Linux. Vous pouvez le pull depuis Docker Hub :
`docker pull airg213/easysaveconsole:1.0`

Pour lancer le conteneur et accéder à l'application :
`docker run -it airg213/easysaveconsole:1.0`

Une fois dans le conteneur, naviguez vers le répertoire de l'application :
`cd /etc/EasySaveConsole_Linux64/EasySaveConsole/publish/linux-x64`

Lancez l'application avec :
`./EasySaveConsole`


## Pipeline CI/CD pour EasySave

Le pipeline CI/CD pour le projet EasySave automatise le processus de compilation, de test, de packaging, et de déploiement de l'application EasySave. Chaque changement poussé au référentiel Git déclenche ce pipeline, assurant une intégration et un déploiement continus.

### Étapes du Pipeline

#### 1. SCM Polling
**Description :** Surveille la branche spécifiée du référentiel Git pour tout nouveau commit et déclenche le pipeline automatiquement.  
**Configuration :** Configuré pour surveiller la branche "main".

#### 2. Build
**Description :** Compile le code source en utilisant `dotnet publish` et génère les binaires nécessaires.  

#### 3. Run (Test)
**Description :** Exécute l'application pour s'assurer qu'elle fonctionne comme prévu dans un environnement de test.  

#### 4. Packaging (Zip)
**Description :** Package l'application dans des fichiers ZIP séparés pour Windows et Linux.  
**Commandes :**
- Pour Windows : `zip -r EasySaveConsole_Win.zip ./EasySaveConsole/publish/win-x64/`
- Pour Linux : `zip -r EasySaveConsole_Linux.zip ./EasySaveConsole/publish/linux-x64/`

#### 5. Release
**Description :** Publie les packages sur GitHub Releases, facilitant le déploiement de nouvelles versions de l'application.  
**Commande :** Script pour créer un release sur GitHub et uploader les archives ZIP.

#### 6. Tagging
**Description :** Marque chaque build réussi avec un tag unique dans GitHub, permettant un suivi facile des versions.  
**Configuration :** Utilise `$BUILD_ID` pour créer un tag unique, par exemple `v${BUILD_ID}`.

#### 7. Clean-Up
**Description :** Nettoie l'espace de travail Jenkins post-build pour éviter toute contamination entre les builds.  
**Action Jenkins :** "Delete workspace when build is done."

### Utilisation du Docker Hub

Le serveur Jenkins est disponible sur le Docker Hub pour une récupération facile. Vous pouvez pull le serveur Jenkins préconfiguré avec le pipeline CI/CD pour EasySave :
`docker pull airg213/easysavejenkins:latest`

Pour lancer le serveur Jenkins dans un conteneur Docker :
`docker run -p 8080:8080 -p 50000:50000 airg213/easysavejenkins:latest`

# Authors 

* **FODIL Nel** _alias_ [@nel34](https://github.com/nel34)
* **ARRIGHI Fabien** _alias_ [@arrighi-fabien](https://github.com/arrighi-fabien)
* **GOUADFEL Rayan** _alias_ [@AirG213](https://github.com/AirG213)
* **MARCELLI Enzo** _alias_ [@EnzoMrcli](https://github.com/EnzoMrcli)

# Contact

FODIL Nel - `nel.fodil@viacesi.fr`
ARRIGHI Fabien - `fabien2a.arrighi@protonmail.com` 
GOUADFEL Rayan -`rayan.gouadfel@viacesi.fr` 
MARCELLI Enzo -`marcelli.enzo@gmail.com` 

Project Link: [https://github.com/arrighi-fabien/Projet-Programmation-Systeme](https://github.com/arrighi-fabien/Projet-Programmation-Systeme)
