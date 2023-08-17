#!/bin/sh

PUBLISHDIR=../RobloxSetArchive.Web

MACHINE=pizza-server.internal.pizzaboxer.xyz

DOMAIN=pizzaboxer.xyz
SUBDOMAIN=sets
SCOPE=web

scp -r $PUBLISHDIR $MACHINE:/var/www/$DOMAIN/$SUBDOMAIN/$SCOPE.staging
ssh -t $MACHINE "cd /var/www/$DOMAIN/$SUBDOMAIN; rm -r $SCOPE; mv $SCOPE.staging $SCOPE;"