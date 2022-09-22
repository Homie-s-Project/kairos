# Kairos

kairos est une application WEB principalement qui pourrait être aussi développé plus tard pour devenir disponible sur les mobiles.

Elle a pour but de venir en aide aux personnes qui ont du mal à se lancer dans des révisions et à rester concentré.

Vous pourrez lancer un timer avec le temps que vous vous fixez pour réviser. L'application vous informera quand il faut prendre une pause et vous donner quelques conseilles/astuces lorsque les pauses se feront.

Plus de temps vous révisez plus de crédit\*\* vous recevrez pour vous occuper de votre compagnon virtuel.

\*\* Ce sont des crédits virtuel qui appartiennent à l'application, aucun échange avec une monnaie réelle n'est prévu.

## Installation

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
$ git clone https://github.com/Homie-s-Project/kairos.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode debug (Mode de développement)

```bash
$ docker compose -f "docker-compose.debug.yml" up -d --build
```

## Deployment

Se projet est fait pour fonctionner avec [Docker](https://www.docker.com).

Une fois docker installer est fonctionnel, il faudra clonner ce répository.

```bash
$ git clone https://github.com/Homie-s-Project/kairos.git
```

Une fois le répository clonner il faudra lancer docker pour qu'il installer tous les modules/packages nécessaires.

Avant de pouvoir lancer les commandes docker, il vous faut vous assurer que vous vous trouvez dans le ficher racines du projet.

Vous pouvez le lancer en mode de production

```bash
$ docker compose -f "docker-compose.yml" up -d --build
```

## Environment Variables

Pour exécuter ce projet, vous devrez ajouter les variables d'environnement suivantes à votre fichier .env\*

\*Le fichier avec les variables d'environnement est déjà prévu n'est pas encore utilisé par notre code.

Utilisation de la base de données

| Environement name   | default         |
| ------------------- | --------------- |
| `POSTGRES_DB`       | kairos          |
| `POSTGRES_USER`     | kairos_user     |
| `POSTGRES_PASSWORD` | kairos_password |

Utilisation de pgAdmin4 (Site local qui permet de gérer la postgres)

| Environement name          | default     |
| -------------------------- | ----------- |
| `PGADMIN_DEFAULT_EMAIL`    | Development |
| `PGADMIN_DEFAULT_PASSWORD` | kairos      |

Utilisation du backend

| Environement name        | default       |
| ------------------------ | ------------- |
| `ASPNETCORE_ENVIRONMENT` | Development   |
| `ASPNETCORE_URLS`        | http://+:5001 |

## Color Reference

| Couleur             | Hex                                                                |
| ----------------- | ------------------------------------------------------------------ |
| Couleur avec propriété non définie* | ![#eff7f7](https://via.placeholder.com/15/eff7f7/eff7f7.png) #eff7f7 |
| Couleur avec propriété non définie* | ![#d7e6eb](https://via.placeholder.com/15/d7e6eb/d7e6eb.png) #d7e6eb |
| Couleur avec propriété non définie* | ![#33576c](https://via.placeholder.com/15/33576c/33576c.png) #33576c |
| Couleur avec propriété non définie* | ![#162741](https://via.placeholder.com/15/162741/162741.png) #162741 |

\* Les couleurs n'ont pas une utilisation précise définie.

## Authors

-   [Christopher Andrade](https://github.com/Chriss052)
-   [Romain Antunes](https://github.com/Flasssh)
-   [Alexandre Botta](https://github.com/bottaalexandre)
-   [William Pasquier](https://github.com/WilliamDevv)
