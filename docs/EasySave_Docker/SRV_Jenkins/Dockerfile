# Utilisez la dernière image officielle Ubuntu
FROM ubuntu:latest

# Éviter les interactions utilisateur avec tzdata
ENV DEBIAN_FRONTEND=noninteractive

# Installer wget et les paquets nécessaires pour l'ajout de dépôts externes
RUN apt-get update && \
    apt-get install -y wget gnupg software-properties-common curl zip jq 

# Ajouter le dépôt Microsoft et installer .NET SDK
RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0

# Installer OpenJDK pour Jenkins, ainsi que libicu-dev et d'autres outils utiles
RUN apt-get update && \
    apt-get install -y openjdk-11-jdk libicu-dev git nano sudo wget unzip

# Nettoyer le cache apt pour réduire la taille de l'image
RUN apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Réinitialiser la variable d'environnement DEBIAN_FRONTEND
ENV DEBIAN_FRONTEND=

# Exposer le port 8080 pour l'accès web et le port 50000 pour les agents Jenkins
EXPOSE 8080
EXPOSE 50000

# Définir le répertoire de travail
WORKDIR /app

# Télécharger et installer Jenkins
RUN wget https://get.jenkins.io/war-stable/latest/jenkins.war -O jenkins.war

# Commande par défaut pour démarrer Jenkins
CMD ["java", "-jar", "jenkins.war"]
