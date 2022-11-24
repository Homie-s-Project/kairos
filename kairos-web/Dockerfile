FROM node:lts

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

COPY package.json /usr/src/app/

RUN npm install -g npm@latest
RUN npm install --legacy-peer-deps
RUN npm install -g @angular/cli@15.0.1

COPY  . .
RUN ng analytics off

EXPOSE 4200 49153
CMD ["npm", "start"]
