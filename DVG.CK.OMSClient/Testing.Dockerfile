FROM nginx:1.13-alpine

ENV APP_PATH /app
ENV PATH $APP_PATH/node_modules/@angular/cli/bin/:$PATH

RUN apk add --update --no-cache nodejs && mkdir $APP_PATH && rm -rf /etc/nginx/conf.d/*
WORKDIR $APP_PATH

COPY . .

COPY DVG.CK.OMSClient/nginx/default.conf /etc/nginx/conf.d/

RUN rm -rf /usr/share/nginx/html/*

COPY DVG.CK.OMSClient/dist/ /usr/share/nginx/html/

CMD ["nginx", "-g", "daemon off;"]
