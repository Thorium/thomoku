



                        THO-MOKU Ristinollaaja         (Versio 1.48)
			Open-GL



Valikossa t‰hdell‰ (*) merkityill‰
kohdilla ei ole viel‰ vaikutusta...

http://www.lut.fi/~hietanen/thomokugl/




1. THO-MOKU


 1.1 Mik‰ THO-MOKU ON?

   - THO-MOKU on versio kuuluisasta kiinalaisesta Go-Moku
     pelist‰, jossa pelataan mustilla ja valkoisilla kivill‰.
   - THO-MOKU on "j‰tk‰nshakki" eli ristinolla jossa
     yritet‰‰n saada 5 omaa pelimerkki‰ per‰kk‰in laudalle
     ennenkuin vastustaja saa 5 vastustajan omaa pelimerkki‰
     per‰kk‰in laudalle.


 1.2 MIKSI SE ON TEHTY?

   - THO-MOKU:n kehitys alkoi alunperin, koska muiden tietokone-
     ristinollien ominaisuuksissa oli parantamisen varaa.
     (Toivottavasti tuleva Renjun World Championship III ratkaisee
     asian, mutta Renju ei pelin‰ vastaa aivan Go-Mokua ja yleisesti
     ottaen Renjun suosio on j‰‰nyt hieman v‰hemm‰lle johtuen mm.
     vaikeammista s‰‰nnˆist‰). Kiitokset Renjun World Championship II
     voittajalle nimelt‰ NOESIS, kone miettii kymmeni‰ minuutteja,
     ja k‰yttˆliittym‰ on surkea, mutta teko‰ly on mainio.
   - THO-MOKU:n hyv‰n‰ puolena on sen monipuolisuus. Teko‰lyn peli‰
     voi helposti muokata ihan millaiseksi vain. T‰h‰n menness‰
     THO-MOKU:n teko‰ly voittaa yleens‰ aloittelijat, mutta ei
     viel‰ niit‰ jotka harrastavat ristinollan pelaamista p‰ivitt‰in.
   - THO-MOKU ei laita v‰ltt‰m‰tt‰ "parhaaseen" paikkaan vaan yritt‰‰
     etsi‰ paikkoja, jotka j‰‰v‰t helposti huomaamatta.
   - THO-MOKU sopii erinomaisesti ristinollan opetteluun ja taitojen
     parantamiseen. Kokemus on paras opettaja. Mukana on myˆs valmiita
     scenarioita, jotka ovat kaikki mahdollisia voittaa ja viel‰
     suhteellisen helppoja. Lis‰‰ skenarioita tulee, kun ohjelma
     on muuten valmis.
   - THO-MOKU:n hyviin puoliin kuuluu se, ett‰ kaikkea voi s‰‰t‰‰...
     T‰ll‰ hetkell‰ pelin mukana tuleva koodi on hieman vanhempaa
     versiota, koska uusi ei sis‰ll‰ kommentointia.



2. Grafiikat:
     Osa pelin grafiikoista on tekij‰n itse piirt‰mi‰...
     Osa textureista on napattu jostain ilmaisjutuista. (Pettymys... 
     Pit‰‰ v‰‰nt‰‰ itse lis‰‰ kun ehtii.)
     Peli toimii sek‰ ristinollan ett‰ Go-Mokun grafiikoilla. 

2.1 Go-moku:
     Toinen pelaaja on mustat kivet, toinen valkoiset.
     Kivi‰ asetellaan laudan viivoituksen risteyksiin.

2.2 Ristinolla:
     Toinen pelaaja on risti, toinen nolla.
     Palikat asetellaan ruutujen rajaamiin laatikoihin.

     Kannattaa kokeilla pelata myˆs toisella ruudukolla,
     kuin on tottunut, se antaa mukavasti lis‰ haastetta.


3. VALIKOT


 3.1 Asetusten talletus:

    - Asetukset tarkoittavat muiden valikkojen valittuna olevia kohtia.
      Esim. Random-laudan siirtojen m‰‰r‰, tietokoneen miettimisaika,
      grafiikan ulkoasu ja tietokoneen miettimisen n‰kyminen.
    - K‰ynnistyksess‰ ladataan aina oletusasetukset, mutta t‰st‰ valikosta voi 
      ladata aikaisemmin talletetut asetukset.
    - Asetukset voi myˆs tallettaa t‰st‰ valikosta. Toinen tapa on
      poistua ohjelmasta ja samalla tallettaa asetukset: Quit (Save settings)
    - T‰st‰ valikosta voi myˆs ladata oletusasetukset.

 3.2 Grafiikkamenu:

    - Teemoilla voi vaihdella laudan ulkon‰kˆ‰.
      (Vartavasten valituista hienoista textureista jotka sopivat yhteen)
    - Pelin ulkon‰kˆ (Layout) on selostettu yll‰, kohdassa 2.

    - Enable/Disable Textures
      TƒMƒ VAIKUTTAA PELIN NOPEUTEEN. Disabloi texturet, jos peli on
      yleisesti hidas, eik‰ vain koneen siirrolla.
      Disable textures n‰ytt‰‰ kaiken paljon nopeammin, mutta ei piirr‰
      taustakuvia. Kuitenkin asetukset muuttuvat siten, ett‰ peli muistuttaa
      sen DOS-versiota, ja on hyvin pelattava.
      Jos n‰ytˆnohjaimessasi on t‰ysi OpenGL tuki, 
      eiv‰t tekstuurit juuri hidasta peli‰.

 3.3 Pelaajamenu:

    - Yksinpeli niin ett‰, tietokone aloittaa.
    - Yksinpeli niin ett‰, pelaaja aloittaa.
    - Moninpeli, pelaaja vs. pelaaja.
    - Tietokone vs. Tietokone. Tulokset n‰kyv‰t myˆs TULOS.LOG:issa.


 3.4 Muita menuja:

  3.4.2 Toimivat:

  3.4.2.1: Tietokone:

    * Computer's Thinking Time
      - Tietokone voi laskea useita siirtoja eteenp‰in,
        mutta se kasvattaa miettimisaikaa dramaattisesti.
      - Toisaalta taktikointi paranee. Laskeehan ihminenkin useita
        siirtoja eteenp‰in. Tosin paras paikka tietyll‰ hetkell‰ on
        arviolta 90% varmuudelta sama, vaikka laskettaisiin eteenp‰inkin,
        joten yli 5 siirron miettimisess‰ ei juuri ole mielt‰.

   * Computer's Style:t
     - Katso alempaa kohta 4. eli Tietokonepelaajat

   * Starting Style
     + Basic antaa tietokoneen oman enginen laskea
       raa'asti "parhaat" paikat alusta asti.
       Paras, jos tietokone vs. tietokone
     + Breadfan's special, emuloi Breadfanin k‰ytt‰m‰‰ aloitusta,
       joka kilpailee THO-MOKU:n aloituksen kanssa varsin tiukasti.
     + THO-MOKU Special on ihmispelaajia vastaan suunniteltu paras (?) aloitus,
       joka yritt‰‰ emuloida pelin tekij‰n kehitt‰m‰‰ & k‰ytt‰m‰‰ tyyli‰.
       Paras aloitus tyyli ja oikein k‰ytett‰ess‰ takaa 
       voiton ristille l‰hes varmasti.
     + Playsite Big X on hieman uudempi aloitusmalli, joka on antanut
       varsin lupaavia tuloksia.
     - Special-aloitustyylit ovat hieman kesken, eiv‰tk‰
       viel‰ emuloi t‰ysin esikuviensa peli‰, joten tulokset
       saattavat olla huonompia kuin normaalin aloituksen.


   * Computer's Randomness
     - M‰‰ritt‰‰ kuinka paljon tietokone arpoo paikan
       mihin laittaa. Normaalisti "Normal" tietokone
       pelaa jonkin verran satunnaisesti. Asennossa
       "High" saattaa tulla muutoksia tietokoneen peliin,
       eik‰ se laita nappiaan aina parhaaseen paikkaan.
       Asennossa "Very High" tietokone pelaa jo paljon
       satunnaisemmin, joka aiheuttaa selvi‰ virheit‰
       sen peliin. Asetus kannattaa ehdottomasti pit‰‰
       normaalissa parhaan teko‰lyn saavuttamiseksi.

   * Talking Computer
     - Tietokone heittelee kommentteja pelitilanteesta, 
       mik‰ toisaalta helpottaa peli‰ ja toisaalta ‰rsytt‰‰ 
       k‰ytt‰j‰‰.

   * Loop-Mode
     - Tietokone vs. Tietokone -peliss‰ aloittaa aina
       uuden pelin, kun edellinen on p‰‰ttynyt. Katsojan
       ei tarvitse painaa mit‰‰n nappia. 

       (Tietokone pit‰‰
       kirjaa asetuksista, pelien voitoista yms. tiedostoon
       TULOS.LOG joista niit‰ voi katsella mill‰ esim. tahansa
       teksti‰ ymm‰rt‰v‰ll‰ ohjelmalla, kuten NotePad, Edit,
       WordPad, WordPerfect, Word, jne. Tosin se ei ole 
       viel‰ t‰ss‰ betassa) 

     - Laajennetaan jatkossa toimimaan myˆs pelaaja vs. pelaaja -peliss‰
       niin, ettei pelist‰ tarvitse poistua valikkoon.
       (Kaksinpelej‰ kun pelataan yleens‰ monta, h‰vi‰j‰h‰n
       haluaa uusintaa :)

  3.4.2.2: Muut:

    * Board Size
      - 40*40 on normaali lauta,
        peli poikki samanlaisista 5 per‰kk‰isest‰ merkist‰.
      - 15*15 on standardi Go-Moku lauta.
      - 16*16 ja 20*20 laudat toimivat myˆs.
      - 3*3 on pieni lauta, peli poikki kolmen suorasta, poistettu betasta,
        koska nurkasta alottaja voittaa aina.
      - Unlimited (Scroll) tulossa.
        Rajaton pelilauta, periaatteessa helppo toteuttaa,
        mutta aiheuttaa ep‰mukavuutta k‰ytt‰j‰lle, jos peli menee yli
        laudan ja ruutua joutuu rullailemaan etsiess‰‰n t‰rkeit‰ paikkoja.
        Esimerkki: jos tietokone p‰‰tt‰‰ laittaa pelimerkkins‰ 5 metri‰
        oikealle ja 3 metri‰ alasp‰in edellisest‰ pelimerkist‰ niin miten
        ihmispelaaja saa mukavasti tietoonsa miss‰ uusin
        pelimerkki on sek‰ miss‰ ovat edelliset pelimerkit.


    * Cheat Menu
        T‰st‰ saa p‰‰lle huijausfunktiot:
      - Peruuttaa _yhden_ edellisen siirron. Saman voi toistaa,
        mutta ei montaa kertaa per‰kk‰in. (Sellainen pelaaminen
        ei ole hauskaa, paljon kivempaa ottaa uusi peli ja mietti‰
        2 sekunttia kauemmin _ennenkuin_ siirt‰‰.)

      - N‰ytt‰‰ tietokoneen miettimi‰ paikkoja (ihan hepreaa
        tavalliselle k‰ytt‰j‰lle)

    * Help menu
        Pika ohjeet... (sumealla tekstill‰) :(
      - Eri pelityylien s‰‰nnˆt
      - Yhden sivun pikakurssi, kuinka voittaa ristinollassa/go-mokussa,
        ohjeita niin alkajalle kuin kokeneellekin pelaajalle.


  3.4.2 Ei toimivat / tulevaisuudessa toimivat:

    * Learning Computer
      - Takaa, ettei tietokone pelaa samanlaista
        peli‰ heti uudestaan, mik‰li h‰visi edellisen.

    * Count Ratings
      - Pit‰‰ kirjaa pelaajista, niiden voitoista, h‰viˆist‰,
        pelien m‰‰r‰st‰, yms.
      - Laskee pelaajille jotain prosentteja / suhdelukuja, jotka
        ovat vertailukelpoisia esim. kahden ihmispelaajan vertaamiseen.
      - Sek‰ oma ett‰ Vastustajan taso vaikuttaa pisteiden
        m‰‰r‰‰n per ottelu.
      - Taulukot ja tilastot parhaista pelaajista.

    * Network Protocol
      - Jos kaksi ihmist‰ haluavat pelata vastakkain, vaikka eiv‰t
        olisi samalla koneella.
      - No Network, jos koneella ei ole mahdollisuuksia p‰‰st‰
        mitenk‰‰n verkkoon. Samalla koneella onnistuu toki kahden
        ihmisen v‰liset pelit ilman verkkoakin, ja jos pit‰‰ paperilla
        kirjaa, voi j‰rjest‰‰ erilaisia turnauksia, cuppeja, liigoja, jne.
        joissa on useita pelaajia vastakkain.
      - E-mail on s‰hkˆpostin v‰lityksell‰ tapahtuvia pelej‰ varten,
        jolloin liitteen‰ l‰hetet‰‰n koodattu eritt‰in pieni tiedosto,
        josta peli osaa lukea pelitilanteet, yms. Haittapuolena on
        s‰hkˆpostin v‰lityksell‰ pelattujen pelien hitaus.
        E-mail peli‰ voi pelata yht‰ hyvin disketin v‰lityksell‰ kahdessa
        koneessa ilman s‰hkˆpostia, yms, yms.
      - TCP/IP toimii vasta kaukana tulevaisuudessa...
        Mahdollista toteuttaa niin, ett‰ er‰‰lle koneelle asennetaan
        THO-MOKU - Server ohjelma tausta-ajoon ja sitten siell‰ voi pelata
        turnauksia, liigoja, cup:peja, yksitt‰isi‰ otteluita, yms.
        kaikkien paikalla olijoiden kanssa, sek‰ katsella muiden pelej‰
        ja pit‰‰ kirjaa pelaajien rating:eista. Ainoa haittapuoli on,
        etten ole ehtinyt tehd‰ THO-MOKU - Server ohjelmaa viel‰,
        ja teen ensin koko pelin muuten valmiiksi.
      - TCP/IP (Via Mapped Network Drive) on oikotie onneen.
        Itseasiassa se taitaa k‰ytt‰‰ NetBeuia, tms. mutta idea
        on kuitenkin, ett‰ verkkoon on jaettu tietty hakemisto/tiedosto,
        jota kumpikin verkon asiakas voi lukea ja kirjoittaa.
        Peli verkon l‰pi sujuu helposti ja ongelmitta, mik‰li
        jaettu hakemisto/tiedosto on kummankin k‰ytt‰j‰n tiedossa.
        Haittapuolena, ett‰ tiedosto pit‰‰ jakaa verkkoon.
      - Skill Compare on er‰‰nlainen systeemi,
        jolla saa selville kuka on l‰hipiirin paras pelaaja.

    * S‰‰nnˆt eri pelityypeille:
      - Selostettu pelin online-helpiss‰, joka on harmillisen samea.

    * Muut ovat kehityksen alla.

 3.5 SCENARIO MENU

    - Scenario valikosta valitaan pelattava scenario.
    - Scenarioista n‰kyy nimi sek‰ muita oleellisia tietoja.
    - Tarkemmin voi katsoa alempaa kohdasta 5. eli Scenariot.
    - Scenario valikosta voi valita Random Board:in, jolloin
      esiin tulee uusi Random Board valikko, josta saa m‰‰ritell‰
      millaisen pelin haluaa.





4. TIETOKONEPELAAJAT


 4.1 Valmiit tietokonepelaajat

   + Tietokonepelaajien tyyliin on tulossa muutoksia.

   * Aggressive Attacker
     + Ahne pelaaja, joka hyˆkk‰‰ viimeiseen siirtoon asti.
     + Yleisesti ottaen paras pelaaja.
     + Nopeita pelej‰.
     - Joskus ahnehtii liikaa ja tekee itsemurhasiirtoja.

   * Defence Master
     + Ei yrit‰ hyˆk‰t‰ vaan puolustaa ahkerasti.
     + Rakentaa omia paikkoja taustalla.
     - Antaa toisen pelaajan pilata hyv‰t paikkansa.
     - Pitki‰ ja tuskallisia pelej‰, joissa pit‰‰ mietti‰ paljon.

   * Passive Presurer
     + Jonkinlainen keskiverto pelaaja.
     + Parhaimmillaan pelatessaan muita tietokoneita vastaan.
     - V‰ltt‰‰ agressiivisen hyˆkk‰yksen edut ja pikavoitot.
     - Muistuttaa eniten normaalia ihmispelaajaa ja pelaa aika
       tavallisesti.

   * Nasty Trap Builder
     + Tekee ansoja, joihin varsinkin alottelevat pelaajat sortuvat.
     + Hyv‰ pelaaja ihmisi‰ vastaan, huono pelaaja tietokoneita vastaan.
     - Ei laita aina parhaaseen paikkaan.
     - Ansat helppo pilata.

   * Pelaajien itsevirittelem‰t custom-tietokoneet pois k‰ytˆst‰...


 4.2 Tilastoja (Vanhentuneet)

    * Tietokone vs. tietokone pelien tilastoja normal randomnessilla.
    * Pelien tilastot laskettu TULOS.LOG:in pohjalta liian pienill‰
      otoksilla.
    * AA = Aggressive Attacker, DM = Defence Master,
      PP = Passive Presurer, TB = Nasty Trap Builder

                 Evil-aloitus  Normal-aloitus  Weird-aloitus
                 AA DM PP TB   AA DM PP TB     AA DM PP TB        
   Voittoprosentti:                                               Yhteens‰:
              AA 71             0              67                    46
              DM    80             0              99                 60
              PP       75            99              99              91
              TB          99            99              67           88
   Keskim‰‰r‰inen pelien pituus vuoroina:                         Yhteens‰:
              AA 40             37             49                    42
              DM    88            58              41                 62
              PP       34            47              35              39
              TB          43            53             121           72

      * Mit‰ t‰st‰ voimme p‰‰tell‰?
        + Emme mit‰‰n.
        - Kaikki n‰ist‰ tuloksista poikkeavat ovat tilastopoikkeuksia. (???)
        - Passive Preasurer on huono. (???)
        - Otokset ovat liian pieni‰.
        - Teoriassa kaikkien pit‰isi olla > 50.
        - 99 on optimaalinen voittoprosentti X:lle. Johon p‰‰st‰‰nkin
          Victor Alliksen teorialla. Toisaalta olen tutkinut sen v‰itˆskirjan
          l‰pi ja se ei perjaatteessa oikeasti takaa yht‰‰n mit‰‰n, vaan
          sis‰lt‰‰ kasan algoritmeja... En ole n‰hnyt teoriaa k‰ytt‰v‰‰
          ristinollaa. Kyselin asiasta Victor Allikselta s‰hkˆpostilla, mutta
          vastaus oli, ett‰ h‰nen kehitt‰m‰ ohjelma ei ole saatavilla
          kaupallisesti eik‰ muutenkaan mitenk‰‰n. Toivottavasti THO-MOKU
          ei mene koskaan siihen pisteeseen. :) T‰ll‰ hetkell‰ t‰m‰ on FreeWarea,
          eli saa kopioida vapaasti.
        - Yhteens‰ laskut eiv‰t ole suhteutettu pelien m‰‰r‰‰n.
        - Nasty Trap Builder kaatuu itse omiin ansoihinsa.



 4.3 Oman tietokonepelaajan tekeminen (Tulossa pian)

    - Jos valmiista tietokonepelaajista riitt‰‰ vastusta, niin
      ei kannata tehd‰ omia tietokoneita. Kuitenkin jos kiinnostusta
      ja kokemusta ristinollan tahkoamisesta riitt‰‰ ja valmiiden
      pelaajien tuoma vastus alkaa olla l‰hinn‰ naurettava, kannattaa
      v‰‰nt‰‰ oma tietokonepelaaja. Teoriassa on t‰ysin mahdollista
      luoda THO-MOKU:un tietokonepelaaja, joka pelaa aina parhaalla
      mahdollisella tavalla, eli voittaa aina kun on X ja antaa
      eritt‰in hyv‰n vastuksen myˆs itse‰‰n vastaan pelatessa kun on 0.
    - THO-MOKU:un voi tehd‰ oman tietokone pelaajan luettelemalla
      noin 45 lukua (eli prioriteetit vektoreille), jotka tallennetaan
      tiedostoon PLAYER1.CMP, PLAYER2.CMP, PLAYER3.CMP, tms.
      Kokeneelle ristinollan pelaajalle tietokoneiden teko on suhteellisen
      pienell‰ s‰‰t‰misell‰ ja matematiikan hallinnalla.
    - Tietokonepelaajista saa periaatteessa ihan millaisia vain.
      Kukaan ei kuitenkaan halua huonoja tietokonepelaajia.
      Hyv‰‰ tietokonepelaajaa ei saa tehty‰ pelk‰ll‰ tuurilla,
      vaikka sit‰kin tarvitaan.
    - Editori ei ole viel‰ valmis.
    - PLAYERx.CMP sis‰lt‰‰ jo t‰h‰n menness‰ jotain tietoja,
      mutta ei ole viel‰ k‰ytˆss‰.
    - Merkinn‰t:
      X = tietokoneen nappula, 0 = Vastustajan nappula,
      _ = tyhj‰ paikka, johon seuraavalla vuorolla pit‰isi laittaa,
      - = tyhj‰ paikka, johon seuraavalla vuorolla ei saisi laittaa,
      . = mik‰ tahansa merkki, eli t‰yte ett‰ 8 merkin pituus saataisiin
          t‰yteen.
    - Rivej‰ on t‰ll‰ hetkell‰ kiinte‰t 46 (tai 79), eik‰ niit‰ voi muuttaa
      (tai tietysti voi, mutta se ei ole suotavaa). Rivien peilikuvat
      otetan automattisesti huomioon, joten niit‰ ei tarvitse tehd‰.
    - Jokaiselle riville annetaan suhdeluku, joka m‰‰r‰‰ kuinka t‰rke‰ ko.
      rivi on. Suurin suhdeluku on (t‰ll‰ hetkell‰) 35000 ja pienin on 1.
      Jos suhdeluku on nolla, sill‰ ei ole mit‰‰n merkityst‰. Negatiivisi‰
      suhdelukuja ei voi k‰ytt‰‰ ja mieluiten ei yli 40000 menevi‰
      suhdelukuja.
    - Eri kuvioiden suhdeluvut lasketaan yhteen jokaisessa paikassa
      erikseen.
    - Esimerkki‰ voi katsoa tietokonepelaajista (mutta mallia ei).
    - Rivin maksimipituus (=minimipituus) on 8 merkki‰. Sitten tulee
      v‰lilyˆnti ja sitten prioriteettiluku, jonka j‰lkeen tulee rivivaihto.
    - Testaa tekemi‰si tietokonepelaajia ainakin 100 peli‰.
    - Pelin tekij‰n‰ otan mielell‰ni vastaan tietokonepelaajia
      jotka voittavat valmiit pelaajat ja ovat muutenkin todella
      hyvi‰. Sitten ne voi lis‰t‰ viralliseenkin peliversioon uusiin
      versioihin (ja tietysti paljon kiitoksia tekij‰lle dokuihin... ;)
      Mutta en todellakaan halua testata 10000 erilaista tietokonetta,
      ja koska valmiiseen versioon ei voi sis‰llytt‰‰ kovin montaa,
      niin haluan vain parhaan pelaajan... Mutta mik‰ on paras,
      kas siin‰ pulma.
    - Pit‰‰ kehitt‰‰ joku e-mail levitys systeemi ja systeemi, jolla voi
      ottaa matsia omatekoisella tietokoneella muiden koneita vastaan...






5. SCENARIOT


 5.1 Scenariot - Mit‰ ja miksi?

   - Sceanriot ovat pelej‰, joista joku on "pelannut alun".
     Pelaaja astuu kuvaan kun on aika tehd‰ t‰rkeimm‰t p‰‰tˆkset
     ja valinnat. Oikein k‰ytt‰m‰ll‰ scenarioista saa pelin suolan,
     sill‰ niit‰ ei ole ollut aikaisemmin miss‰‰n ristinollassa
     (koska t‰m‰n tekij‰ keksi koko idean itse). Mik‰li ne ovat
     tarpeeksi hyvi‰, tulee idea varmaan kopioitua ja niit‰
     ilmestyy muihinkin ristinolliin ;)

   - Scenario voi olla normaalin pelin tulos, jolloin siit‰ voi oppia
     jotain ja lis‰ksi oppia hahmottamaan pelej‰ niin, ettei tarvitse
     mietti‰, vaan "n‰kee suoraan" parhaat paikat.

   - Scenario voi olla myˆs mielenkiintoisen n‰kˆinen kuvio tai eritt‰in
     taktinen ja haastava ongelma.

   - Tavoite on, ett‰ scenariot olisivat a) vaikeita, b) mahdollisia.
     Lis‰ksi niiden pit‰‰ olla mielenkiintoisempia kuin mit‰ pelaaja
     saisi omalla pelill‰‰n tavallisesti aikaan.

   - Scenarion pit‰‰ olla mahdollinen pelattava koneen
     default-asetuksilla.

 5.2 Scenario Editori

   - Omat scenariot ovat nyt tiedostonimill‰
     ownscen1.tsc, ownscen2.tsc, ownscen3.tsc, ownscen4.tsc.
     Lis‰‰ vaihtoehtoja kun scenarioecitoria parannetaan.
   - Scenarioeditori on kehitetty, ett‰ peliin voisi tehd‰ omia
     scenarioita. Parhaat voi sitten l‰hett‰‰ pelin tekij‰lle
     ja saada yleiseen levitykseen pelin uudempien versioiden mukana
     ja tietysti scenarion tekij‰lle paljon mainetta ja kunniaa... ;)
     Mutta scenarioitakin mahtuu toistaiseksi vain rajallinen m‰‰r‰...
   - Pit‰‰ kehitt‰‰ joku e-mail levitys systeemi...
   - Scenarioeditori toimii toistaiseksi vain dossista ja sen tekemi‰
     tiedostoja on hiukan muokattava ennenkuin ne suostuvat scenarioiksi
     (Tekstirivit leikata pois ja delli‰ pari rivivaihtoa, mallia voi katsoa
     valmiista skenarioista.) Scenarioedit tekee t‰ysin DOS-veriso yhteensopivia
     scenarioita, joita ei tarvitse muokata mitenk‰‰n.


 5.3 Random Board

   - Random Board on satunnainen asetelma laudalla. Tietokone arpoo
     paikat pelaajien pelinapeille. Joskus kone saattaa arpoa
     liiankin vaikeita tai helppoja tilanteita, joskus taas juuri
     sopivia, jos haluaa v‰h‰n lis‰‰ vaihtelua ja haastetta
     tavallisien pelien rinnalle.
   - Jos vaikeus on s‰‰detty vaikealle, niin tietokone saa
     todenn‰kˆisemmin parempia keskityksi‰ ja
     asetelmia kuin ihmispelaaja.
   - Lis‰ksi voidaan s‰‰t‰‰ onko X vai 0 ja paljonko vuoroja
     on ollut ennenkuin pelaaja tulee peliin, eli kuinka paljon
     suurinpiirtein rasteja ja nollia on laudallla.





6. PELIN HISTORIA

  1998:

 * THO-MOKU Ristinollaaja for DOS
   + DOS-versioita suuri m‰‰r‰, OpenGL ensimm‰inen versio vasta
     DOS-versiosta 0.993, vuoden 1999 lopulla. Tietokonepelaaja
     oli alkuversioissa l‰hinn‰ vitsi.

  1999:

 * THO-MOKU Ristinollaaja OpenGL versio 1.25

   + Perustuu Dos-versioon 0.993B, ei rekursiivista eteenp‰in laskevaa
     teko‰ly‰. Muuten kaikki suhteellisen hyvin ok.

  2001:

 * THO-MOKU Ristinollaaja OpenGL versio 1.48

   + Perustuu DOS-versioon 0.995B. Paljon pieni‰ bugikorjauksia versioon 1.25,
     esim. Go-Moku s‰‰nnˆill‰ 6 suora ei en‰‰ ole voittava suora ja koneen
     ei pit‰isi saada tasottavaa vuoroa miss‰‰n tapauksessa... Sis‰lt‰‰ 
     eteenp‰in laskevan tietokoneen. Koneen miettimisajat hitaita, 
     riippuen eteenp‰in laskettavien siirtojen m‰‰r‰st‰. 


 6.3 Tulevaisuus

     Jokseenkin DOS-version mukaan, tietokoneiden aloituksissa
     ja pelityyleiss‰ on viel‰ parantamisen varaa.
     Parempi dokumentointi, toimivat s‰‰nnˆt muihinkin kuin kahteen ja 
     skenariot uusiksi (jotkut nyt mahdottomia tietyill‰ tietokoneilla).
     Skenario-editori myˆs OpenGL:lle. Verkkopeli‰ tuskin tullaan n‰kem‰‰n pian.
     Tulevassa versiossa kaikki toimii nopeasti ja bugeja ei ole...  toivottavasti.


7. TIEDOSTOT


        README.TXT        - T‰m‰ tekstitiedosto.
        ALOKUV.THP        - Aloituskuva.
        HRULESI.THP       - On-line helppi s‰‰ntˆihin.
        HHOWTO2.THP       - On-line helppi pelaamiseen.
        *.THP             - Textuuri tiedosto.
        FILE_ID.DIZ       - Lyhyt kuvaus ohjelmasta BBS:i‰ varten.
        THOMOKU.ICO       - Ikoni Windowsiin.
        TULOS.LOG         - Logi tietokoneen peleien voitoista.
        SCEN*.TSC         - Valmiita scenarioita.
        OWNSCEN*.TSC      - Pelaajien tekemi‰ omia scenarioita.
        SCENNULL.TSC      - Tyhj‰n scenarion sis‰ltˆ.

        *.CMP             - Tietokonepelaajia, ei k‰ytˆss‰ viel‰.

Mahdollisesti samassa hakemistossa:

        THOMOKU.EXE       - Tiedosto, josta ohjelma l‰htee k‰yntiin.
                            Windows-versio.
        RISTINOLLA        - Unix-versio edelliseen.
        SCENEDIT.EXE      - Scenarioiden teko ohjelma (DOS)
        COMPEDIT.EXE      - Tietokonepelaajien tekoon tarkoitettu ohjelma,
                            joka ole viel‰ valmis (ei jaossa).


        GLUT32.DLL        - Glutin DLL Windowsiin. Ei levitet‰ ohjelman mukana.
                            T‰m‰n saa pikaisella etsinn‰ll‰ netist‰.

        OPENGL32.DLL      - OpenGL:‰n DLL Windowsiin. Ei levitet‰ ohjelman mukana.
                            Jos n‰ytˆnohjain tukee OpenGL:‰‰, t‰t‰ ei tarvita,
                            koska t‰m‰ lˆytyy Windows:sin system-hakemistosta.
                            Jos ei, niin software-version saa netist‰, 
                            mutta se on hidas ja ei v‰ltt‰m‰tt‰ toimi.

        ristinolla.c      - L‰hdekoodi sek‰ Unixille, ett‰ Windows:sille.
 
        makefile          - Unixille l‰hdekoodin k‰‰ntˆˆn. Tapahtuu k‰skyll‰
                            make ristinolla

8. TEKIJéNOIKEUDET:

      - Copyrightit sun muut kuuluvat tekij‰lle. Ohjelman on tehnyt
        Tuomas Hietanen. Ohjelma on suunniteltu opettamaan muille ristinollaa
        (opetustarkoitus) ja toimimaan harjoitustyˆn‰ (oppimistarkoitus).
        Tekij‰ ei vastaa mist‰‰n mit‰ ohjelma aiheuttaa kenellekk‰‰n
        tai millekk‰‰n. Tekij‰ ei vastaa mist‰‰n muustakaan. Ohjelma
        saattaa sis‰lt‰‰ bugeja tai kirjoitusvirheit‰. Niist‰ saa
        ilmoittaa tekij‰lle. Ohjelman l‰hdekoodia ei saa (toistaiseksi)
        muuttaa ilman tekij‰n lupaa. Ohjelmassa ei ole mink‰‰nlaisia
        kaupallisia intressej‰. Oikeus kaiken muuttamiseen pid‰tet‰‰n.
        Oikeus n‰iden tekstien muuttamiseen pid‰tet‰‰n. Jne, jne.
        Muutettua l‰hdekoodia ei saa levitt‰‰.

      - Blaa blaa, suurin piirtein kaikki samat jutut,
        mit‰ kaikkien muidenkin ohjelmien teksteiss‰.

      - THO-MOKU on ainakin toistaiseksi FREEWARE:a, eli sit‰ saa
        kopioida t‰ysin vapaasti, mutta ei muuttaa. Itseasiassa
        THO-MOKU on viel‰ Beta-asteella.

      - OpenGL on joku rekisterˆity merkki, katso lis‰‰ http://www.opengl.com
        ja sitten Windows myˆs (http://www.microsoft.com)




9. YLEISIMMIN KYSYTYT KYSYMYKSET (FAQ)

 K: Ohjelma valittaa ett‰ jotain tiedostoja puuttuu?
 V: THO-MOKU k‰ytt‰‰ OpenGL-kirjastoa  (ja Glut). Tarvitset
    OpenGL32.DLL:‰n, joka on toivottavasti tullut uuden nopean
    n‰ytˆnohjaimesi mukana. Jos ei, niin netist‰ (Http://www.opengl.com/)
    voi hakea hitaan software OpenGL:‰n (jolla peli saattaa toimia).

 K: Pist‰‰ mustan ruudun ja ei sano mit‰‰n?
 V: 3Dfx:n voodoo ja voodoo2 piirit pit‰isiv‰t toimia, mutta
    ne eiv‰t taida tyk‰t‰ hiiren kursorista. Jos 3Dfx:n OpenGL ei
    toimi, niin kopioi www:st‰ software OpenGL-ajuri THO-MOKU:n hakemistoon.

 K: Menu on sekava?
 V: Se on niin selke‰ ja hieno kuin vain Glut:illa pystyi tekem‰‰n.
    Jos et tied‰ mit‰ jokin kohta tekee, niin kokeile.

 K: Minulla ei ole Windowsia.
 V: Ei h‰t‰‰, THO-MOKU:n pit‰isi toimia myˆs Linuxilla, Unixilla, yms...
    Tai voit hakea netist‰ vanhan DOS-verion, joka k‰ytt‰‰ samaa
    teko‰ly‰. Http://www.lut.fi/~hietanen/thomoku/

 K: Ruutu ei p‰ivity/kaikki on kamalan hidasta?
    K‰ytˆss‰si on ilmeisesti software OpenGL ajuri (opengl32.dll)

 K: Voiko tietokoneen j‰tt‰‰ pelailemaan taustalle?
 V: Voi. Sen voi j‰tt‰‰ miettim‰‰n yht‰ siirtoa (omaa siirtoaan) tai
    pelailemaan kesken‰‰n. Loopmoden ollessa p‰‰ll‰ tulokset tulevat
    kirjautumaan TULOS.LOG:iin pelin uudemmassa versiossa kuin t‰m‰.

 K: Miksi kone on niin hidas tietokoneen siirt‰ess‰?
 V: Saat tietokoneen siirt‰m‰‰n nopeammin (ja huonommin) kun
    v‰henn‰t "Computer - Thinking time" -valikosta eteenp‰in laskettavien
    siirtojen m‰‰r‰‰.
    Ohjelma on tehty parhaan mahdollisen teko‰lyn saamiseksi,
    ei nopeimman tietokoneen saamiseksi teko‰lyn kustannuksella.


 K: Miksi kone sitten on hidas?
 V: C-kielell‰ tehty pieni koodinp‰tk‰ laskee yli 30 miljoonaa (!)
    if-lausetta pelk‰st‰‰n yhdelle siirrolle. Sitten kun niit‰
    ruvetaan laskemaan eteenp‰in niin huh huh... 
    Koodi on aikas optimoitua (5000 rivi‰!), mutta laskettavaa on liikaa.

 K: No miten saan koneen nopeaksi?
 V: Pist‰ menusta Disable Textures, jolloin kuvat katoavat.
    Jos ei auttanut, niin V‰henn‰t "Computer - Thinking time" -valikosta 
    eteenp‰in laskettavien siirtojen m‰‰r‰‰ (voit kokeilla laittaa texturet 
    takaisin).

 K: Miksi pit‰‰ laskea jokainen ruutu, vaikka vain viimeinen siirto, eli
    5 ruutua siit‰ joka suuntaan riitt‰isi? (=10*10 matriisi)
 V: T‰t‰ harkitsin kauan. Tulin kuitenkin siihen tulokseen, ett‰
    on parempi laskea jokainen ruutu, vaikka se onkin (40*40)/(10*10)=
    16 kertaa hitaampaa. T‰ss‰ pari syyt‰: 1. Jotkin kuviot saattavat
    olla jopa 8 merkki‰ pitki‰, eli 5 ruutua yhteen suuntaan ei riit‰.
    2. Mik‰li viimeinen rasti on laitettu ison pelin toiselle puolelle,
    ei kone en‰‰ osaisi siirt‰‰ pelin painopistett‰. T‰m‰n voisi tietysti
    korvata sill‰, ett‰ tallentaa muistiin aikaisemmin lasketut ruudut
    alueen 10*10 tai itseasiassa 16*16 ulkopuolella, jolloin periaatteessa
    pit‰isi saada yht‰ hyv‰ tulos. Lis‰ksi siirto-operaatioon tulisi
    tietysti aika, joka menee kun 16*16 matriisi leikataan oikealta kohdalta
    40*40 matriisia (ja muunnetaan takaisin matriisiin kun paras paikka on
    laskettu). Loppujen lopuksi voisi saavuttaa noin puoli sekunttia nopeamman
    enginen kovan tyˆn tuloksena (koneesta riippuen). Mutta mieluummin suuntaan 
    kehityksen t‰ll‰ hetkell‰ muualle (kuten parempi tietokoneen teko‰ly 
    hidastamatta ohjelmaa) ja odotan sen puoli sekunttia saadakseni paremman tuloksen.
    Mukana oli myˆs kokeilu siit‰, ett‰ laskisi parhaat paikat vain niiden
    kohtien avulla, joiden l‰hell‰ on jokin merkki. T‰m‰ ei kuitenkaan
    nopeuttanut k‰yt‰nnˆn kokeiluissa tietokonetta juuri lainkaan.
    (V‰hemm‰n kuin 0.1 sekunttia.) Pentium 150MHz koneella yksi tietokoneen
    siirto kest‰‰ noin pari sekunttia riippumatta kuinka t‰ysi pelilauta on.
    Pentium 2 450MHz laskee sen jo selv‰sti alle sekunttiin.
    Jos haluaa nopeammin tuloksen, voi ostaa nopeamman koneen
    (tai OpenGL n‰ytˆnohjaimen!), mutta yleens‰ tekee
    pelaajallekin hyv‰‰ hieman mietti‰ mihin kone rastinsa
    (ja pelaaja seuraavan rastin) laittaa.

 K: Miksi THO-MOKU:ssa on k‰ytetty Knuthin, Morrisin ja Prattin
    algoritmin kaltaista algoritmi‰ (merkkijonon haku tekstist‰),
    etk‰ esim. Boyerin, Mooren ja Horspoolin algoritmi‰ tai Ahon ja
    Corasickin algoritmi‰?
 V: Koska ristinollassa esiintyvi‰ merkkej‰ ei ole useita olisi
    Bayerin, Mooren ja Horspoolin algoritmin k‰yttˆ hidasta.
    Ahon ja Chorasickin algoritmi ei oikein sovellu siksi, ett‰
    l‰hes samanlaisia rivej‰ on kymmeni‰. Lis‰ksi rivisysteemin
    on tarkoitus pysy‰ sellaisena, ett‰ mahdollisesti k‰ytt‰j‰n
    tulisi voida halutessaan lis‰t‰ rivej‰ ja jos jokaisesta rivist‰
    teht‰isiin ‰‰rellinen automaatti, niiden teko olisi liian hankalaa
    normaali k‰ytt‰j‰lle ja rivej‰ on niin monta, ett‰ ‰‰rellisten
    automaattien tekemiseen menisi paljon aikaa.

 K: Miksei t‰m‰ ohjelma ole kovin hidas vaikka teksteiss‰ niin v‰itet‰‰n?
 V: Rukkasin laskimen koodia v‰h‰n uusiksi. Nyt se on ruma l‰hdekoodina,
    mutta ei kovin hidas. Mahdollisesti koodi vaihtelee eri versioissa.
    Jos peli on nopea, voit lis‰t‰ eteenp‰in laskettavien siirtojen m‰‰r‰‰,
    jossain 4 kohdalla se viel‰ n‰kyy peliss‰. Siirtom‰‰r‰‰ nostamalla
    kone hidastuu ja reippaasti.

 K: Olen kuullut jotain jostain dr. L. Victor Alliksesta.
 V: Dr. Victor Allis on teeseill‰‰n (v‰itˆskirja) ratkaissut ristinollan,
    kuinka risti voittaa aina, nollan siirroista huolimatta.
    Kysyin s‰hkˆpostitse Dr. L.V. Allikselta h‰nen ristinolla
    ohjelmasta. Se ei ole julkisesti saatavilla (ei kaupallisesti
    eik‰ ilmaisena). Lis‰ksi sen kone osaa pelata hyvin vain
    ristill‰, nolla "tiet‰‰ h‰vi‰v‰ns‰". Kiitokset dr. Victor
    Allikselle teeseist‰‰n, mutta toisaalta se asettaa ristinollan
    "pelin‰" eritt‰in kyseenalaiseen arvoon. THO-MOKU:n teko‰lyll‰
    ei ainaista voittoa viel‰ saavuteta, joten ei h‰t‰‰. :) 
    Asiassta enemm‰n kiinnostuneille THO-MOKU:n kotisivulla on
    linkki Victor Alliksen sivuille, joista teesit voi hakea
    omalle koneelleen.

 K: Mist‰ t‰m‰ ohjelma l‰htee k‰yntiin?
 V: Windows: THOMOKU.EXE
    Unix: ./ristinolla (kunhan on ensin laittanut make ristinolla)

 K: Miss‰ on t‰m‰n ohjelman kotisivu/mist‰ t‰m‰n voi kopioida?
 V: OHJELMAN KOTISIVU: http://www.lut.fi/~hietanen/thomokugl/
    Yll‰tt‰en myˆs ohjelman vanhalle DOS-versiolle (beta myˆs)
    on pieni kotisivu http://www.lut.fi/~hietanen/thomoku/

 K: Mist‰ tavoittaa tekij‰n, ett‰ voi valitta bugeista?
 V: IRC Nick:   Thorium
    S‰hkˆposti: tuomashi@freenet.hut.fi






10. SYSTEM REQUIREMENTS

10.1 Needed: (Not tested)

    - DOS-version:
    * 486 or better...
    * VGA 640*480*16

    - OpenGL-version:
    * OpenGL support...
    * SVGA 640*480


10.2 Recommended:
    DOS:
    * Pentium 150MHz or better. Pentium 3 800MHz for counting 4 moves forward!
    * Windows 98, ME or 2000.
    * 4MB of physical memory or more.
    * Faster processor & hard disk means
      faster computer player.
    * Good monitor & refresh rate for your eyes...

    OpenGL:
    * Pentium 3 or better. Actually nothing is enough for counting 4 moves forward!
    * Unix / Windows 95 or 98.
    * Fast OpenGL display adapter
    * 16MB of physical memory or more.
    * Resolutions up to 1600*1200 supported.



 - Thorium - tuomashi@freenet.hut.fi

