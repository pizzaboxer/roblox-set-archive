#!/bin/sh

PROJECTDIR=../RobloxSetArchive.Api
PUBLISHDIR=bin/Release/net6.0/linux-x64/publish

MACHINE=pizza-server.internal.pizzaboxer.xyz
SERVICE=sets.pizzaboxer.xyz.service

DOMAIN=pizzaboxer.xyz
SUBDOMAIN=sets
SCOPE=api

rm -r $PROJECTDIR/$PUBLISHDIR
dotnet publish $PROJECTDIR -c Release -r linux-x64 --no-self-contained
scp -r $PROJECTDIR/$PUBLISHDIR $MACHINE:/var/www/$DOMAIN/$SUBDOMAIN/$SCOPE.staging
echo "sudo password required for server"
ssh -t $MACHINE "cd /var/www/$DOMAIN/$SUBDOMAIN; sudo systemctl stop $SERVICE; cp -p $SCOPE/appsettings.json $SCOPE.staging/appsettings.json; rm -r $SCOPE; mv $SCOPE.staging $SCOPE; sudo systemctl start $SERVICE; systemctl status $SERVICE;"