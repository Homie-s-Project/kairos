FROM node:lts-alpine
WORKDIR /usr/src/app
RUN npm install -g @angular/cli@14.2.2
COPY ["package.json", "package-lock.json*", "npm-shrinkwrap.json*", "./"]
RUN npm install && mv node_modules ../
COPY . .
RUN chown -R node /usr/src/app
USER node
RUN ng analytics off
EXPOSE 4200
CMD ["ng", "serve", "--host", "0.0.0.0"]
