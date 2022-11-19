FROM node:lts

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY package.json /usr/src/app/
RUN npm install
RUN npm install -g @angular/cli@15.0.0

COPY  . .
RUN ng analytics off

EXPOSE 4200 49153
CMD ["npm", "start"]
