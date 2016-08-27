
![thomoku.jpg][1]

This is very old stuff!

![pelitapa.jpg][2]

![pelityyli.jpg][3]

## TEKOÄLY RISTINOLLASSA

### 1. Tiivistys                

Tämä asiakirja sisältää erittäin lyhyesti selitettynä pääpiirteissään ristinollan säännöt, pelissä käytettävät algoritmit ja algoritmien sovelluksen tietokoneelle.

 

### 2. Ristinollan säännöt 

Ristinollan säännöt vaihtelevat hieman, mutta perussääntönä voidaan pitää, että pelaajat laittavat pelilaudalle ruudukkoon oman pelimerkkinsä vuorotellen ja se joka saa n kpl omia pelimerkkejään peräkkäin suoraan voittaa. Välissä ei saa olla vastustajan pelimerkkejä. Suora voidaan muodostaa joko pystysuunnassa, vaakasuunnassa tai viistoon (sekä vasemmalle, että oikealle). Jos lauta on 3x3 mallinen niin nolla ei voi voittaa. Todistus www-sivulla:

http://tuli.cc.lut.fi/~hietanen/lunchbox/tictac/1.htm

Näin ollen laudan pitää olla suurempi. Yleensä pelilautana käytetään ruutupaperia, joten pelilauta on suuri. Suurella laudalla pelaajan pitää saada 5 pelimerkkiä peräkkäin. Tämä johtuu siitä, että jos pelimerkkejä pitäisi saada alle 5, ei nolla voi voittaa. Ja taas jos pelimerkkejä pitäisi saada 6, ei peli päättyisi välttämättä koskaan. Koska ristillä on aina yksi pelimerkki enemmän kuin nollalla, on risti varsin suuressa etulyöntiasemassa.

### 3. Ristinollan kansainvälisyys

Ristinolla on englanniksi tic-tac-toe, mutta se tarkoittaa vain 3x3-laudalla pelattavaa versiota. Isommalla laudalla tunnetaan toki samanlainen peli, nimellä Go-moku. Go-mokun alkuperä on aasiasta. Laudan koko on siinä kiinteä 15x15 ja pelimerkkeinä pelaajilla on mustat kivet ja valkoiset kivet, jotka sijoitetaan laudan viivojen leikkauspisteisiin. Muuten peli on sama kuin ristinolla, kuitenkin sillä erotuksella, että yli 5 pelimerkkiä peräkkäin ei voita. (Normaalisti ristinollassa katsotaan usein 6 suoran sisältävän 5 suoran.) Koska go-mokukin suosii aloittavaa puolta, eli mustaa kiveä, on luotu lisäksi erilaisia sääntöjä. Esimerkiksi Renju on muuten samanlainen peli, mutta aloittajan saadessa 2 kpl kummastakin päästä torjumattomia 3 suoria tai 2 kpl toisesta päästä torjuttuja 4 suoria hän häviää. Peli on silloin vaikeampi kuin ristinolla, mutta aloittajalla on yhä suuri etu puolellaan. 

### 4. Algoritmi    

Jos pelaajalla on neljän suora jonka jommassakummassa päässä on tyhjä ruutu, eikä vastustaja torju sitä, hän saa seuraavalla vuorolla 5 suoran ja voittaa pelin. Näin ollen 4 suora on pakko torjua, eli se on uhka, josta voittaa 1 siirron päästä. Vastaavasti jos pelaajalla on kummastakin päästä torjumaton 3 suora, eikä vastustaja torju sitä kummastakaan päästä, saa pelaaja seuraavalla siirrolla torjumattoman 4 suoran, jota vastustajan on mahdoton enää estää muodostumasta 5 suoraksi. Näin ollen 3 suora on pakko torjua toisesta päästä. 3 suora on uhka,  josta vastustaja voittaa 2 siirron päästä. Näin ollen pelaaja voi halutessaan pelata ensin oman 4 suoransa (voitto 1 siirron päässä) ennen kuin torjuu vastustajan 3 suoran (vastustajan voitto 2 siirron päässä).


Jos pelaaja onnistuu rakentamaan kaksi uhkaa samaan aikaan, ei vastustaja voi torjua kumpaakin, vaan pelaaja voittaa pelin. Näin ollen mahdolliset voitot ovat:

 - Viiden suora, voitto oitis. ( xxxxx )

 - Torjumaton 4 suora, voitto seuraavalla siirrolla ( _xxxx_ )

 - Kaksi kappaletta 4 suoria, voitto seuraavalla siirrolla. ( oxxxx_ ja oxxxx_ ) 

 - Neljän suora ja 3 suora, voitto viimeistään kahden vuoron päästä. ( oxxxx_ ja _xxx_ )

 - Kaksi 3 suoraa, voitto kahden vuoron päästä ( _xxx_ ja _xxx_ )

Näistä on tärkeää huomata, että kaksi kappaletta torjumattomia 3 suoria ei auta, jos vastustajalla on jo 4 suora ja torjumaton 3 suora.

Jos saadaan pakotettua vastustaja torjumaan uhka kunnes saadaan paikka josta saadaan kaksi uhkaa, on pelin voitto selvä.

### 5. Pelin muunto tietokoneelle 

Pelilauta on jokin kaksiulotteinen taulukko. Pelilaudalla voi olla kolmea eri tyyppiä merkkejä: risti, nolla tai tyhjä. Tietyn rivin hakeminen laudalta (esim. etsitään onko laudalla oxxxx_ ) tapahtuu yksinkertaisena tekstihakuna. Tekstihaku pitää tietysti suorittaa joka ilmansuuntaan, eli 8 kertaa. Näin ollen hakuun tulee 8*(laudankoko_x)*(laudankoko_y) kertaa suoritettava hakuoperaatio, joka sisältää haettavan rivin pituuden verran vertailuja. Tämä joudutaan tekemään jokaista erilaista haettavaa riviä kohden. 

Tekemäni ristinolla käyttää lähinnä Knuthin, Morrisin ja Prattin tapaista tekstihakua. Monimutkaisemmat tekstihaut (esim. Bayerin, Mooren ja Horspoolin algoritmi) perustuvat siihen, että tekstissä on satoja erityylisiä merkkejä. (Tekstihauista erittäin hyvät selostukset Martti Penttosen kirjassa Johdatus Algoritmien Suunnitteluun ja Analysointiin.) Valitettavasti ristinollalaudalla ei ole kuin kolmea tyyppiä merkkejä. Lisäksi samankaltaisia rivejä on kymmeniä (eli myöskään Ahon ja Chorasickin algoritmi ei sovellu).

### 6. Algoritmin muunto tietokoneelle 

Kaikki pelin siirrot voidaan ajatella muodostavan yhdessä pelistä suuren puurakenteen. Jokainen erilainen seuraava siirto johtaa puun solmun eri lapsisolmuun. Puusta tapahtuvat haut tehdään rekursiivisesti.

Paras tilanne saavutetaan kun saadaan vastustaja pakotettua torjumaan. Tällöin vastustajan siirrot voidaan yhdistää omiin siirtoihin ja puusta karsiutuu puolet pois. Silloin voidaan vain etsiä mitkä omat siirrot johtavat voittoon, eli mitä polkua pitkin puusta voittaa.

Algoritmi on yksinkertainen, mutta erittäin raskas, ottaen huomioon, että tekstihaut joka riville joudutaan suorittamaan jokaisessa puun solmussa, koska pelilaudalla olevat rivit muuttuvat joka tilanteessa.

Kun tietokone aloittaa paikkansa laskemisen, haetaan ensin voitot samalla siirrolla:
 
```
		xx_xx 

		xxx_x 

		xxxx_  

		ja

		oo_oo

		ooo_o

		oooo_
```
 
Jos tällainen rivi löytyy, on aivan ilmeistä mihin kohtaan tietokoneen pitää laittaa oma pelinappinsa. Olettaen että kummallakin pelaajalla on tällainen rivi, niin kone laittaa tietysti omaan paikkaansa. 

Seuraavaksi haettavat rivit, joilla muodostuu pelaajalle 4 suora (joka vastustajan on pakko torjua) ja niiden löytyessä korvattava uusi rivi (nyt oletetaan että pelaaja on X, jos hän on 0 niin merkit ovat tietysti päinvastoin):

```
		xx_x_ -> xxoxx

		xx_x_ -> xxxxo

		x_x_x -> xxxox

		xx__x -> xxxox

		xx__x -> xxoxx

		xxx__ -> xxxxo

		xxx__ -> xxxox

		x_xx_ -> xoxxx

		x_xx_ -> xxxox
``` 

Jos tällainen paikka löytyy, mennään hakupuussa seuraavaan solmuun alaspäin. Solmuja käydään läpi, kunnes löytyy haluttu maali tai kunnes puussa ei ole enää solmuja tai kunnes haluttu puun maksimihakusyvyys saavutetaan. (Pitää myös tarkistaa, ettei vastustaja saa voittoa saman tien…) Jos maalia ei löydy, pitää rivi korvata takaisin alkuperäisen laudan rivillä ja peruuttaa yksi solmu ylöspäin. Maalin löytyminen tarkoittaa, että pelaaja on saanut kaksi uhkaa ja voittaa pelin. Maalit joissa lopetetaan puusta hakeminen:

```
		 xx_xx

		 xxx_x

		 xxxx_
```

Tällöin puun maksimihakusyvyydeksi vaihdetaan yhtä pienemmäksi kuin löytyneen maalin syvyys ja talletetaan löytyneen haun ensimmäisen solmun koordinaattit (x,y) ylös, eli paikka, josta laudalla voittaa.

Kun haku on suoritettu loppuun, niin vaihdetaan puolet ja kutsutaan uudestaan äskeistä tarkistusta, jotta saadaan selville, ettei vastustajalla ole voittoa 1. siirron päässä. Jos ei ole, niin voidaan jatkaa hauilla joissa voitto saavutetaan 2 siirron päästä kun laitetaan tiettyyn paikkaan:

```		 
		_xx___ -> oxxxoo

		_xx___ -> oxxoxo

		_x_x__ -> oxxxoo

		_x_x__ -> oxoxxo

		_x__x_ -> oxxoxo
```

Tässä vastustajan pelimerkkejä voi nyt olla useampia, koska vastustaja voi torjua useaan eri paikkaan. Vastustajan liikamerkit eivät haittaa, koska jo edellisessä haussa tarkistettiin, ettei vastustaja voi voittaa suoraa ja tässä haussa ei käsitellä vastustajan merkkejä.

Maalit ovat nyt rivit:

```
		_xxx__

		_xx_x_
```

Maalien löytyminen tarkoittaa, että pelaaja saa jostain kaksi kolmen suoraa, kun on pelannut oikeista kohdista kaikki kahden paikkansa kolmen suoriksi.



### 7. Heurastiset haut 

Koska haku on erittäin raskas, ei pelaaja jaksa odottaa yli n kpl siirron laskentasyvyyttä enempää (nykytietokoneilla n on noin 3-5). Se takaa kuitenkin jo ihan hyvän tuloksen, koska näissä siirroissa on vastustajan siirrot mukana, eli oikeasti siirtomäärä on n*2 ja parhaat ihmispelaajatkin pystyvät laskemaan keskimäärin vain 5 siirtoa eteenpäin.

Kuitenkin on kehitettävä malli joka antaa parhaan paikan kun voittoa ei saavuteta käytetyllä laskentasyvyydellä. Tämä toteutetaan heurastiikalla, joka ennakoi parhaan paikan ilman siirtojen laskemista eteenpäin. Periaatteessa ristinolla pelaa varsin hyvin jo pelkällä heurastisella haulla, mutta valitettavasti se saattaa antaa väärän tuloksen joissain tapauksissa, varsinkin kun hyvä ristinollan pelaaja (tai rekursiivisesti laskeva tietokone) rakentaa ilkeän ansan parin siirron päähän.

Heurastinen malli on myös hyvä pelin aivan alussa. Toinen vaihtoehto on opettaa pelille mallialkuja, joista saadaan erittäin hyvät lähtökohdat. Valmiilla aloituksilla voidaan parantaa varsinkin ristin jo muutenkin hyvää etulyöntiasemaa.

Hyvä heurastinen haku saavutetaan antamalla tärkeimmille riveille pisteytukset, joka annetaan rivin tietylle tyhjälle kohdalle. Kun rivien pisteytykset paikalle lasketaan yhteen, voidaan paikkojen pisteytykset tallentaa prioriteettimatriisiin (eli taulukkoon joka vastaa pelilautaa kooltaan). Näin ollen prioriteettimatriisin suurin luku on suoraa paikka mihin kannattaa pelimerkki pistää. Paikkojen pisteytykset ovat suhteessa toisiinsa ja peliin voidaan haluttaessa lisätä pientä satunnaisuutta lisäämällä jokin pieni satunnaisluku paikan prioriteetin kokonaisarvoon.

Alla on esimerkki prioriteeteista (THO-MOKU 0.995 Agressive Attacker), mutta tätä tarkasteltaessa on hyvä pitää mielessä, että esim. suora voitto tarkistettiin jo rekursiivisen haun alussa, joten sitä ei tässä tarvitse tässä tarkistaa uudestaan. Lisäksi nämä arvot määräävät pelityylin, eivätkä ole mitään absoluuttisesti parhaita vaan suurpiirteisiä arvoja. Paremmat arvot saataisiin esim. rakentamalla pieni silmukka, jossa ohjelma muuttaa hieman arvoja ja pelaa vanhoja arvoja vastaan 15 peliä (sekä ristillä, että nollalla), ja jos uudet arvot voittivat suurimman osan, niin tallennetaan uudet arvot. Sitten muutetaan arvoja ja mennään silmukan alkuun. Koneella menee muutama päivä laskiessa ja saadaan erittäin hyvät prioriteettiarvot. Optimaalisessa tapauksessahan koko heurastisuutta ei kuitenkaan tarvita.

                                             
```
		_ = tyhjä paikka laudalla, mihin lisätään prioriteetti

		* = tyhjä paikka laudalta, mihin ei kuitenkaan lisätä prioriteettia

		Haettava rivi / Prioriteetti


		*XX_X      5000

		XX_X*      5000

		XXX_*      5000

		0XXX*_     5000

		XX*X_      3000

		X*XX_      3000

		XX__X      2900

		X_X_X      2900

		X_000__    2000

		*_000_*    2000

		_0_00_     2000

		**0_00**     30   (=2000+30)

		**00_0**     30   (=2000+30)

		X*000_*      30   (=2000+30)

		*XX_*       500

		*X_X*       500/2 (Symmetriasyistä /2,

		*X__X*      490    nämähän haetaan

		*X*X_*      490    kahteen suuntaan,

		*XX*_*      490    esim. ylös ja alas)

		X000__      350

		X00_0_      350

		X0_00_      350

		X_000_      350

		*OO_*       200

		*O_O*       200

		*O__O*      190

		*O*O_*      190

		*OO*_*      80

		*XX**_*     80

		*X_*        60 

		*X*_*       50

		*O_*        40

		*O*_*       25

		*X*_*X*     16/2

		*X**_X*     10

		*X**_*      10

		*OO**_*     10

		*0*_*0*      6/2

		*0**_0*      2

		0_0_0_0   14000/2
```
 
### 8. THO-MOKU 

Nämä edellä lyhyesti selostetut tekoälyn kannalta mielenkiintoiset algoritmit ovat käytössä THO-MOKU Ristinollaaja v0.995B ohjelmassa. Niillä muodostetaan pääpiirteittäin ristinollan tietokoneen tekoäly, jonka toteutus, c-kielinen muutaman tuhannen rivin koodi (ei sis. käyttöliittymää) on saatavilla THO-MOKU:n kotisivulta. Koodi lukee tiedostosta pelilaudan taulukon (0 = tyhjä, 1 = risti, 2 = nolla) ja ottaa syötteeksi tietokonepelaajan parametrit (puoli, tyyli, yms), laskee parhaan paikan ja palauttaa sen pelilaudan muotoisena prioriteettimatriisina käyttöliittymälle. Jos löytyy voittava paikka, on muiden paikkojen prioriteetti 0. Käyttöliittymän tehtävä on piirtää pelilautaruudukko (40x40) ja antaa pelaajien laittaa pelimerkkinsä ruudukkoon.


Koko pelin saa käännettynä WIN/DOS-versiona kotisivulta.

Lisäksi kotisivulla on ohjelman käyttöohjeet (readme.txt) ja linkkejä aiheesta, yms.

http://www.lut.fi/~hietanen/thomoku/


- Tuomas Hietanen, (c) 1998 - ...

![screensht1.gif][4]
![screensht2.gif][5]
![screensht3.gif][6]
 

   [1]: https://raw.github.com/Thorium/Thomoku/master/thomoku.jpg
   [2]: https://raw.github.com/Thorium/Thomoku/master/pelitapa.jpg
   [3]: https://raw.github.com/Thorium/Thomoku/master/pelityyli.jpg
   [4]: https://raw.github.com/Thorium/Thomoku/master/screensht1.gif
   [5]: https://raw.github.com/Thorium/Thomoku/master/screensht2.gif
   [6]: https://raw.github.com/Thorium/Thomoku/master/screensht3.gif

 
