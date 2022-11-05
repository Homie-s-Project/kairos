## Routes

Dans cette section, vous retrouvez toutes les routes.

### Auth

**Version simplifier*

#### Login [GET]
Cette endpoint permet de rediriger l'utilisateur sur le site de Microsoft pour qu'il puisse s'y connecter.


#### CallBack [GET]

*Cette endpoint est utilisé par Microsoft pour ne retourner le "code" qui nous permettra ensuite de faire une requête à leur service pour récupérer les informations de l'utilisateur.

#### Me [GET]

Cette endpoint nous permet de récupérer les informations de l'utilisateur qu'on a à stocker en base de données sur Kairos. Elle sert aussi à permettre de s'assurer qu'il soit connecté depuis le front.
