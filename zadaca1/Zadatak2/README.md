# Calculator

Za pokretanje projekta potreno je izvrsiti sljedece komande

>
> fable app/ build/
>

Nakon toga potrebno je preko npm-a dohvatiti sve potrebne fajlove (dependencies):

>
> npm install
>

Te sa komandom ispod otvara se server na portu 5173 na lokalnoj masini, gdje se preko browsera moze pristupiti

>
> npm run dev
>
> firefox localhost:5173
>

Kada odlucite zatvoriti projekat, potrebno je takodjer ubiti node i fable procese koji se izvrsavaju u pozadini

>
> pkill node
> pkill fable
>
