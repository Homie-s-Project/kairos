## Routes

Dans cette section, vous retrouvez toutes les routes.

### Authentification


| HTTP Method | Path                     | Description                                                                                                                                                                            |
| ----------- | ------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET         | /auth/login/microsoft    | Cette endpoint permet de rediriger l'utilisateur sur le site de Microsoft pour qu'il puisse s'y connecter.                                                                             |
| GET         | /auth/login/google       | Cette endpoint permet de rediriger l'utilisateur sur le site de google pour qu'il puisse s'y connecter.                                                                                |
| GET         | /auth/callback/microsoft | *Cette endpoint est utilisé par Microsoft pour ne retourner le "code" qui nous permettra ensuite de faire une requête à leur service pour récupérer les informations de l'utilisateur. |
| GET         | /auth/callback/google    | *Cette endpoint est utilisé par Google pour ne retourner le "code" qui nous permettra ensuite de faire une requête à leur service pour récupérer les informations de l'utilisateur.    |

### Group

| HTTP Method | Path            | Description                                                                                                                                                 |
| ----------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET         | /group/me       | Cette route permet de retourner tous les groupes ou l'utilisateur en fait partie.                                                                           |
| GET         | /group/personal | Cette route permet de retourner tous les groupes personnels que l'utilisateur possède, c'est-à-dire tous les groupes privés avec lui en tant que fondateur. |
| POST         | /group/create?groupName=`<nom du groupe>` | Cette route permet de créer un group. |

## Event

| HTTP Method | Path            | Description                                                                                                                                                 |
| ----------- | --------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| GET         | /event/`{groupId}`       | Cette route permet d'avoir l'event d'un group. |
| CREATE         | /create/       | Cette route permet de crée un event. |
| DELETE         | /delete/`{eventId}`       | Cette route permet de pouvoir supprimer un event, seleument si elle nous appartient. |
| PUT         | /update/`{eventId}`       | Cette route permet de pouvoir modifier un event, seleument si elle nous appartient. |


### Label

| HTTP Method | Path                                     | Description                                                                |
| ----------- | ---------------------------------------- | -------------------------------------------------------------------------- |
| GET         | /label/me                                | Cette route permet de récupérer tous les labels que l'utilisateur à créer. |
| POST        | /label/create?labelName=`<nom du label>` | Cette route permet de créer un label.                                      |
| GET        | /label/event/`{eventId}` | Cette route permet de recevoir les labels utilisés par un event en particulier.  |
| GET        | /label/group/`{groupId}` | Cette route permet de recevoir les labels utilisés par un group en particulier.  |

### Studies

| HTTP Method | Path               | Description                                                                                                                          |
| ----------- | ------------------ | ------------------------------------------------------------------------------------------------------------------------------------ |
| POST         | /studies/heartbeat | Cette route permet d'envoyer un message au backend pour informer que l'utilisateur est toujours en train de faire sa séance d'étude. |
| POST         | /studies/start`?time={temps en seconde}&labelsId={les ids des labels utilisé séparer par ',' EX: =23,42,64}` | Cette route permet de créer une session de travail avec le temps désiré et les labels.. |
| POST        | /studies/stop | Cette route permet d'arrêter la session de travail en cours. |
| GET         | /studies/`{studiesId}` | Cette route permet de recevoir des informations sur une studies en particulier. |
| GET         | /studies/lastWeek/hoursStudied | Cette route permet de recevoir le nombre d'heures étudié par jour durant la semaine dernière. |
| GET         | /studies/lastWeek/hoursPerLabel | Cette route permet de recevoir le nombre d'heures étudié par label durant la semaine dernière. |
| GET         | /studies/lastWeek/rate | Cette route permet de recevoir le taux d'étude des derniers 7 jours comparant la semaine d'avant. |

### User

| HTTP Method | Path     | Description                                                                |
| ----------- | -------- | -------------------------------------------------------------------------- |
| GET         | /user/me | Cette route permet de renvoyer les informations de l'utilisateur connecté. |


