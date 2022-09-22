
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

## Image Docker
Notre projet utilise  [Docker](https://www.docker.com), pour des questions de simplicité et de temps. Grâce à Docker, nous pouvons transférer un environnement simplement et l'installer sur une autre machine grâce à une commande.

Nous utilisons **4** images dans notre projet avec chacune des images une particularité.

| Image Docker   	| Description         |  URL |
| ------------------    | --------------- | ----------------|
| `kairos-api`*         | Cette image contient le code utile pour notre API. |[http://localhost:5001/swagger/index.html](http://localhost:5001/swagger/index.html) |
| `kairos-web`*     	  | Dans cette image, il se trouve tout notre code nécessaire pour avoir l'interface frontend avec Angular. | [http://localhost:4200/](http://localhost:4200/)
| [postgres](https://hub.docker.com/_/postgres)| Postgres est l'image docker de notre base de données. | -** |
| [dpage/pgadmin4](https://hub.docker.com/r/dpage/pgadmin4) 	| Cette image permet d'avoir une interface d'utilisation de notre base de données.|  [http://localhost:5050/](http://localhost:5050/) |

\* C'est des images non-publiques nous appartenant.

\**Aucun lien URL pour notre base de données.

*Les connexions pour la base de données, si elles n'ont pas été modifiées dans les variables d'environnement reste celle par défaut.*

## Environment Variables

Pour exécuter ce projet, vous devrez ajouter les variables d'environnement suivantes à votre fichier .env\*

\*Le fichier avec les variables d'environnement est déjà prévu n'est pas encore utilisé par notre code.

**Postgres**

| Environement name   | default         |
| ------------------- | --------------- |
| `POSTGRES_DB`       | kairos          |
| `POSTGRES_USER`     | kairos_user     |
| `POSTGRES_PASSWORD` | kairos_password |

**pgAdmin4** *(Site local qui permet de gérer la postgres)*

| Environement name          | default     |
| -------------------------- | ----------- |
| `PGADMIN_DEFAULT_EMAIL`    | user@kairos.com |
| `PGADMIN_DEFAULT_PASSWORD` | kairos      |

**Kairos-api**

| Environement name        | default       |
| ------------------------ | ------------- |
| `ASPNETCORE_ENVIRONMENT` | Development   |
| `ASPNETCORE_URLS`        | http://+:5001 |

**Kairos-web**

| Environement name        | default       |
| ------------------------ | ------------- |
| `NODE_ENV` | development   |

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
