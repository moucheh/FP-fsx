# FP-fsx repozitorij

Moguce je preuzeti citav repozitorij unosenjem komande ispod u terminal:

     git clone https://github.com/moucheh/FP-fsx.git

Za koristenje prethodne komande, potreban je alat git, koji je moguce preuzeti pomocu nekog od package managera:

>Ubuntu, Mint, Debian
>
>     sudo apt install git
>

>Fedora, CentOS, RHEL
>
>     sudo dnf install git
>

>Windows (PowerShell i winget)
>
>     winget install --id Git.Git -e --source winget
>

Azuriranje repozitorija je moguce komandom (terminal mora biti otvoren u direktoriju u kojem je repozitorij sacuvan):

    git pull

Pokretanje repl okruzenja za F# moguce je komandom
    
    dotnet fsi

Takodjer je moguce u istoj komandi proslijediti fajl s ekstenzijom .fsx ili .fs, npr.

    dotnet fsi p1.fsx
