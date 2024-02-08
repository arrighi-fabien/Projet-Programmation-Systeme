# Utilisez la dernière image officielle Ubuntu
FROM ubuntu:latest

# Éviter les interactions utilisateur avec tzdata
ENV DEBIAN_FRONTEND=noninteractive

# Installer wget, curl, zip, jq et les paquets nécessaires pour l'ajout de dépôts externes
RUN apt-get update && \
    apt-get install -y wget gnupg software-properties-common curl zip jq nano && \
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* 

# Définir le répertoire de travail
WORKDIR /app

# Commande pour maintenir le conteneur actif
CMD ["tail", "-f", "/dev/null"]
