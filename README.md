# <img src="https://sets.pizzaboxer.xyz/img/logo.svg" width="48"> Roblox Set Archive
An archive of user-created sets on Roblox from 2010 to 2018.

This is the source code for the [Roblox Set Archive Website](https://sets.pizzaboxer.xyz). Not much else to it. 

RobloxSetArchive.Api contains the backend, and RobloxSetArchive.Web contains the frontend. 

The frontend folder is supposed to contain a Vue project, but unfortunately that's currently unavailable due to [unexpected circumstances](https://twitter.com/boxerpizza/status/1604933187838554114). Sorry (don't look too carefully at the comments at the top of every .cs file lmao). So, it contains the compiled frontend instead, which is good enough as it can still be modified.

If you're setting this up, do not use database migrations. [Download and import the archive database instead](https://mega.nz/file/RLQCwKwY#5mAGC92jyjYCyv_GSTcYMCfDaq9IqKg2EGV81uHjWxs).

Uses ASP.NET Core 6.0 with PostgreSQL for the API and Vue with Typescript for the frontend (though i'm thinking about doing it in nuxt or svelte if i ever need to do it again, which i likely will).
