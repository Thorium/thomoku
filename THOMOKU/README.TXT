



                        THO-MOKU Ristinollaaja         (Beta 0.995)



Erityisesti kiitoksia kaikille palautetta l‰hett‰neille!

Uusi versio, beta 0.996 tulossa pian,
http://www.lut.fi/~hietanen/thomoku 
Kannattaa kokeilla myˆs pelin OpenGL versiota,
jos omistaa uuden multimediahirmukoneen.


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






2. NéPPéIMET


 2.1 Peliss‰:

   Nuolet    -  Siirt‰‰ kursoria
   Enter     -  Sijoittaa pelinapin
   Space     -  Sijoittaa pelinapin
   Tab       -  Moninpeliin tekstiviesti muille pelaajille
   , . M J   -  Siirt‰‰ pelilautaa, jos Unlimited (Scroll) p‰‰ll‰.
                Piste ja pilkku sivuille, M ja J (vaihtoehto K) alas/ylˆs.
   Esc       -  Quit. Jos tietokone miettii paraikaa, voi
                pelaaja ensin painaa kerran Ctrl-C ja sitten Esc.


   Beta-testaajille:
   R         -  P‰ivitt‰‰ ruudun
   S         -  N‰ytt‰‰ tietokoneen edellisi‰ harkintoja v‰rikoodeilla.
                T‰m‰n k‰yttˆn voi katsoa huijaamiseksi.

 2.2 Valikoissa:

   Nuoli ylˆs & alas  - siirt‰‰ valittua kohtaa
   Nuolet sivuille    - vaihtaa valittua jotain
   Enter & Space      - Valitsee jonkun tai siirt‰‰ jotain
   Esc                - Peruutus & Quit
   N‰pp‰imet ovat loppujen lopuksi ihan selv‰t, kun n‰kee valikot...






3. VALIKOT

 3.1 MAIN MENU:

    - Yksinpeli niin ett‰, tietokone aloittaa.
    - Yksinpeli niin ett‰, pelaaja aloittaa.
    - Moninpeli, pelaaja vs. pelaaja.
    - Tietokone vs. Tietokone. Tulokset n‰kyv‰t myˆs TULOS.LOG:issa.
    - Scenario menu
    - Options menu
    - Ohje, Pikaohje alkuun
    - Quit, eli pelin lopetus

 3.2 OPTIONS MENU:

  3.2.2 Toimivat:

    * Computer's Thinking Time
      - Tietokone voi laskea useita siirtoja eteenp‰in,
        mutta se kasvattaa miettimisaikaa dramaattisesti.
      - Toisaalta taktikointi paranee. Laskeehan ihminenkin useita
        siirtoja eteenp‰in. Tosin paras paikka tietyll‰ hetkell‰ on
        arviolta 90% varmuudelta sama, vaikka laskettaisiin eteenp‰inkin,
        joten yli 5 siirron miettimisess‰ ei juuri ole mielt‰.

   * Computer's Style:t
     - Katso alempaa kohta 4. eli Tietokonepelaajat

    * Talking Computer
      - Tietokone heittelee kommentteja pelitilanteesta, 
        mik‰ toisaalta helpottaa peli‰ ja toisaalta ‰rsytt‰‰ 
        k‰ytt‰j‰‰.

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
       satunnaisemmin, joka toisaalta aiheuttaa virheit‰
       sen peliin. Asetus kannattaa ehdottomasti pit‰‰
       normaalissa parhaan teko‰lyn saavuttamiseksi.

   * Loop-Mode
     - Tietokone vs. Tietokone -peliss‰ aloittaa aina
       uuden pelin, kun edellinen on p‰‰ttynyt. Katsojan
       ei tarvitse painaa mit‰‰n nappia. Tietokone pit‰‰
       kirjaa asetuksista, pelien voitoista yms. tiedostoon
       TULOS.LOG joista niit‰ voi katsella mill‰ esim. tahansa
       teksti‰ ymm‰rt‰v‰ll‰ ohjelmalla, kuten NotePad, Edit,
       WordPad, WordPerfect, Word, jne.
     - Laajennettu toimimaan myˆs pelaaja vs. pelaaja -peliss‰
       niin, ettei pelist‰ tarvitse poistua valikkoon.
       (Kaksinpelej‰ kun pelataan yleens‰ monta, h‰vi‰j‰h‰n
       haluaa uusintaa :)

   * Your Name
     - T‰h‰n voi pelaaja kirjoittaa oman nimens‰, jota
       tarvitaan salasanan kanssa mm. E-mail peleiss‰ sek‰
       jos Count Ratings on p‰‰ll‰.

   * Scroll Board in Start
     - Rullaa ensimm‰isen rastin keskelle peliruutua.
       Tavallisesti p‰‰ll‰. T‰m‰ takaa 40*40 ruudun riitt‰v‰n
       isoihinkin peleihin ja toisaalta v‰hent‰‰ vapaasti
       rullattavassa laudassa helposti tapahtvia peli-virheit‰,
       kun pelaaja hukkaa pelinappinsa ruudun ulkopuolelle.
     - Fast-Mode:n ollessa p‰‰ll‰ on Scroll Board in Start
       myˆs aina p‰‰ll‰.

  3.2.2 Ei toimivat / tulevaisuudessa toimivat:

    * Learning Computer - pois t‰st‰ betasta
      - Takaa, ettei tietokone pelaa samanlaista
        peli‰ heti uudestaan, mik‰li h‰visi edellisen.

    * Fast Mode
      - Pienempi lauta, nopeampi miettiminen.

    * Count Ratings
      - Pit‰‰ kirjaa pelaajista, niiden voitoista, h‰viˆist‰,
        pelien m‰‰r‰st‰, yms.
      - Laskee pelaajille jotain prosentteja / suhdelukuja, jotka
        ovat vertailukelpoisia esim. kahden ihmispelaajan vertaamiseen.
      - Sek‰ oma ett‰ Vastustajan taso vaikuttaa pisteiden
        m‰‰r‰‰n per ottelu.
      - Taulukot ja tilastot parhaista pelaajista.

    * Board Size
      - 40*40 on normaali lauta,
        peli poikki samanlaisista 5 per‰kk‰isest‰ merkist‰.
      - 3*3 on pieni lauta,
        peli poikki kolmen suorasta.
      - Unlimited (Scroll)
        Rajaton pelilauta, periaatteessa helppo toteuttaa,
        mutta aiheuttaa ep‰mukavuutta k‰ytt‰j‰lle, jos peli menee yli
        laudan ja ruutua joutuu rullailemaan etsiess‰‰n t‰rkeit‰ paikkoja.
        Esimerkki: jos tietokone p‰‰tt‰‰ laittaa pelimerkkins‰ 5 metri‰
        oikealle ja 3 metri‰ alasp‰in edellisest‰ pelimerkist‰ niin miten
        ihmispelaaja saa mukavasti tietoonsa miss‰ uusin
        pelimerkki on sek‰ miss‰ ovat edelliset pelimerkit.
      - 16*16 - Fast-Mode
        Vain Fast-Moden ollessa p‰‰ll‰, pienehkˆ 16*16 lauta,
        josta peli alkaa keskelt‰. Tarkoitettu tietokoneen nopeutukseen,
        mutta ei ole t‰ll‰ hetkell‰ juuri nopeampi kuin normaali tietokone.

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

    * Muut ovat kehityksen alla.

 3.3 SCENARIO MENU

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



 4.2 Vanhentuneita tilastoja - Eiv‰t p‰de en‰‰

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
        - 99 on optimaalinen voittoprosentti X:lle.
        - Teoriassa kaikkien pit‰isi olla > 50.
        - Yhteens‰ laskut eiv‰t ole suhteutettu pelien m‰‰r‰‰n.
        - Nasty Trap Builder kaatuu itse omiin ansoihinsa.



 4.3 Oman tietokonepelaajan tekeminen

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
    - Pit‰nee kehitt‰‰ joku levitys systeemi ja systeemi, jolla voi
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
     Scenarioiden editori lˆytyy scenario-hakemistosta,
     (valitettavasti ei OpenGL versiossa)
   - Pit‰nee kehitt‰‰ joku e-mail levitys systeemi...

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

 - Kaikki versiot ovat t‰h‰n menness‰ Beta-versioita.

 6.1 Vuosi 1998
  0.1 - 0.5   Basic-pohjainen, tietokoneen algoritmit paranivat
              ja paranivat. Ei graafista k‰yttˆliittym‰‰.
              if-lausepohjaisesta enginest‰ siirrytt‰v‰ matriisi-
              pohjaiseen.

  0.5x - 0.8x Graafinen k‰yttˆliittym‰, laudan koko 25*60 -> 40*40,
              lis‰‰ valikkoja ja ominaisuuksia, bugi-korjauksia,
              algoritmit suurenivat kunnes yhden siirron pituus
              kesti 2 minuuttia 150MHz Pentiumilla. Basic:illa
              erilaisten fonttien saamiseksi kovaa kikkailua, yms.
              Algoritmit k‰‰nnettiin C-kielelle, jolloin sama aika
              supistui 2 sekunttiin, mutta uusi nopeus mahdollisti
              raskaammat algoritmit. Basic ja C-kielen yhteistyˆ
              tiedostojen v‰lityksell‰ toimii, vaikka kuulostaa
              mahdottomalta... Testiss‰ teko‰ly p‰ihitti er‰‰n
              uuden hyvin tunnetun Go-Moku:n.

        0.97  Tieokoneen algoritmit viritetty huippuunsa ja ohjelman
              muuttaminen miten vain saatu helpoksi. Scenariot, mutta
              niiss‰ pari pahaa bugia. Pari pient‰ bugia tietokoneen
              pelitavassa. Basic ei sallinut en‰‰
              isompia ohjelmia -> C-kielen osuus kasvaa.
              Siirron pituus Pentium 150MHz koneella noin 5 sekunttia,
              joka on viel‰ hidas. Options menu ja verkko puuttuvat.
              Laskimen versio 4.01 ymm‰rt‰‰ tyhj‰n kohdan,
              johon ei pid‰ laittaa, joten kaavojen m‰‰r‰ saatu tiputettua
              131:st‰ 74:j‰‰n ja suorituskyky parantunut. Laskin suorittaa
              noin 345 miljoonaa if-lausetta/siirto, jos kaikki kahdeksan
              sis‰kk‰ist‰ for-lausetta purettaan.

 6.2 Vuosi 1999

        0.991 Options-menu toimii jo osittain, irrallisena ohjelmasta.
              Edellisen version Bugit korjattu. Lis‰ksi korjattu
              tietokonepelaajan kaksi pelikuviota, jotka aiheuttivat
              sille h‰viˆn. Laskimen versio 4.03,
              miettimisaika 16 sekunttia/siirto, verratessaan 534.9 miljoonaa
              if-lausetta (!). Fast-mode:lla noin 4 sekunttia/siirto.

        0.992 Lis‰‰ optioneita options menussa, eri tietokone pelaajat, yms.
              Laskimen engine rukattu melkein kokonaan uusiksi,
              jolloin siit‰ saatu ratkaisevasti nopeampi, jopa niin paljon,
              ett‰ Fast-Mode on turha. Tehoa syˆv‰ alle sivunmittaista
              sis‰kk‰isten for-lauseiden koodia korvattu suurin osa
              asettamalla if-lauseiden sis‰kk‰isyyksi‰ paljon niin, ett‰
              koodi olisi optimaalisen nopea. Koodista tuli 8 kertaa pidempi,
              mutta if-vertailuja joudutaan suorittamaan vain noin 30.4
              miljoonaa ja koko laskurin aikakompleksisuus pieneni noin 1/8
              osaan edellisest‰ versiosta (2 sekunttia P150:sella).
              Algoritmit ovat itse kehitettyj‰, mutta muistuttavat hieman
              Knuthin, Morrisin ja Prattin algoritmi‰. Lis‰ksi
              laskin ottaa automaattisesti huomioon peilikuvat, jolloin
              kaavojen m‰‰r‰ saatu tiputettua 46:teen. Testiss‰ er‰it‰ muita
              tietokoneen ristinollia vastaan Attack Master ja Passive
              Preasurer voitti, Nasty Trap Builder h‰visi pelins‰ ja
              Defence Masterilla pelatessa vastustajan ristinollalta
              loppui lauta kesken.


        0.993 Sis‰lt‰‰ laskimen versio 5.13, joka on sek‰ pelillisesti
              parempi, ett‰ nyt koneen pit‰isi toimia myˆs Windows NT:ss‰
              ja DOS:issa. Yhteensopivuuden takia jouduttiin lis‰‰m‰‰n
              koodinp‰tk‰, joka saattaa aiheuttaa tietokoneen laittavan
              ihan tyhm‰‰n kohtaan... T‰ss‰ versiossa tietokone saattaa
              lopettaa pelaamisen kesken pelin, onneksi se on harvinaista.

        0.995 Sis‰lt‰‰ laskimen versio 6.30, joka laskee peli‰ eteenp‰in.
              T‰m‰ toimii siten, ett‰ siirroista muodostetaan puurakenne,
              jota k‰yd‰‰n l‰pi ja etsit‰‰n haaraumaa josta voittaa.
              Vaatii varsin paljon konetehoa ja on hidasta.
              Uudet ja monimutkaisemmat aloitustyylit, eiv‰t tosin viel‰k‰‰n
              t‰ydelliset.
              Pieni‰ uudistuksia k‰yttˆliittym‰‰n: voittosuoran merkkaus,
              puhuva tietokone, yms.

 6.3 Tulevaisuus (0.996-...)

              Huomattavasti paremmat tietokonetyylit.
              Hiotummat aloitustyylit.
              Parempi dokumentointi/ohjeet.
              Scenarioiden korjaukset/parannukset.
              Paremmat www-sivut pelille.
              Verkkopeli, laudan koko ja oppiva tietokone eiv‰t ole tulossa
              ihan seuraavaan versioon. Jonkinlaisia tilastosysteemeit‰
              on luvassa. 
              Tulevassa versiossa kaikki toimii nopeasti ja
              bugeja ei ole...  toivottavasti.






7. TIEDOSTOT


        THOMOKU.EXE       - Tiedosto, josta ohjelma l‰htee k‰yntiin.
                            Sis‰lt‰‰ mm. graafisen k‰yttˆliittym‰n.
        LASKIN.EXE        - Laskin, joka laskee parhaat paikat tietokoneen
                            siirroille. Sis‰lt‰‰ myˆs pikaohjeen.
        LASKIN.C          - Laskimen julkinen c-koodi. T‰ll‰ hetkell‰
                            vanhentunut versio.
        THOUT.DAT         - THOMOKU.EXE:n kirjoittama matriisi pelilaudasta
                            syˆtteen‰ laskimelle.
        THIN.DAT          - Laskimen kirjoittama matriisi syˆtteen‰ takaisin
                            THOMOKU.EXE:lle.
        README.TXT        - T‰m‰ tekstitiedosto.
        THOMOKU.PIF       - Sis‰lt‰‰ Windowsin asetukset THO-MOKU:lle.
        OPTIONS.EXE       - Options valikko. Voidaan k‰ytt‰‰ myˆs ohjelman
                            sis‰lt‰.
        THOPT.DAT         - Optionsvalikon tallentamat asetukset.
        FILE_ID.DIZ       - Lyhyt kuvaus ohjelmasta BBS:i‰ varten.
        TULOS.LOG         - Logi tietokoneen peleien voitoista.

        END.GIF           - Pari kuvankaappausta
        MENU.GIF            pelist‰.
        START.GIF 

        SCENARIO\
           SCENNULL.TSC   - Tyhj‰n scenarion sis‰ltˆ, joka pit‰‰ kopioida
                            kaikkien niiden scenario tiedostojen sis‰llˆksi,
                            joita ei viel‰ ole (SCEN009.SCN - ...)
           SCEN*.TSC      - Valmiita scenarioita.
           OWNSCEN*.TSC   - Pelaajien tekemi‰ omia scenarioita.
           SCENEDIT.EXE   - Scenarioiden teko ohjelma, ei toimi viel‰.

        COMPUTER\
           *.CMP          - Tietokonepelaajia, ei k‰ytˆss‰ viel‰.
           COMPEDIT.EXE   - Tietokonepelaajien tekoon tarkoitettu ohjelma,
                            joka ole viel‰ valmis.






8. TEKIJéNOIKEUDET:

      - Copyrightit sun muut kuuluvat tekij‰lle. Ohjelman on tehnyt
        Tuomas Hietanen. Ohjelma on suunniteltu opetustarkoitukseen.
        Tekij‰ ei vastaa mist‰‰n mit‰ ohjelma aiheuttaa kenellekk‰‰n
        tai millekk‰‰n. Tekij‰ ei vastaa mist‰‰n muustakaan. Ohjelma
        saattaa sis‰lt‰‰ bugeja tai kirjoitusvirheit‰. Niist‰ saa
        ilmoittaa tekij‰lle. Ohjelman l‰hdekoodia ei saa (toistaiseksi)
        muuttaa ilman tekij‰n lupaa. Ohjelmassa ei ole mink‰‰nlaisia
        kaupallisia intressej‰. Oikeus kaiken muuttamiseen pid‰tet‰‰n.
        Oikeus n‰iden tekstien muuttamiseen pid‰tet‰‰n. Jne, jne.
        Muutettua l‰hdekoodia ei saa levitt‰‰.

      - Blaa blaa, suurin piirtein kaikki samat jutut,
        mit‰ muidenkin ohjelmien teksteiss‰.

      - THO-MOKU on ainakin toistaiseksi FREEWARE:a, eli sit‰ saa
        kopioida t‰ysin vapaasti, mutta ei muuttaa. Itseasiassa
        THO-MOKU on viel‰ Beta-asteella.






9. YLEISIMMIN KYSYTYT KYSYMYKSET (FAQ)

 K: Scenariot eiv‰t toimi?
 V: Jos purit THO-MOKU:n ZIP paketin PkZipill‰, niin k‰ytith‰n -d optiota
    purkamaan myˆs hakemistot? Mik‰li et, voit viel‰ tehd‰ Scenario
    hakemiston ja siirt‰‰ sinne *.tsc tiedostot ja scenedit.exe:n.

 K: H‰vi‰n aina scenariossa?
 V: Heh, olet huono. ;) Nyt k‰vi niin huonosti, ett‰ tietokoneen parantuessa
    parista vaikeasta scenariosta taisi tulla mahdottomia.

 K: THO-MOKU pist‰‰ miettiess‰‰n kovalevyni raksuttelemaan kovaa
    ja miettii hitaasti DOS-k‰yttˆj‰rjestelm‰ss‰.
 V: Jos k‰yt‰t THO-MOKU:a puhtaasta DOS-tilasta, aja ennen ohjelman
    k‰ynnistyst‰ SMARTDRV. Sen pit‰isi nopeuttaa ohjelmaa reilusti
    ja kovalevyn rouskuttelut tippuvat todella paljon.

 K: THO-MOKU ei l‰hde k‰yntiin tai tietokoneen peli ei toimi
    vaan tietokoneen yritt‰ess‰ mietti‰ tulee joku Win32-virheilmoitus.
 V: T‰m‰n pit‰isi olla poistunut uusimmassa versiossa.
    Ohjelman laskin vaati 32-bittisen k‰yttˆj‰rjestelm‰n, kuten
    Windows 95 tai Windows 98. Joskus asetukset olivat v‰‰rin ja Windows
    yritti ajaa THO-MOKU:a 16-bittisess‰ DOS-tilassa. Tarkista ettei
    asetus "Est‰ DOS-pohjaista ohjelmaa huomaamasta Windows" ole p‰‰ll‰.

 K: Onnistuin jumiuttamaan tietokoneen jollain uudella kikkailulla.
    Mill‰ saan lopetettua tietokoneen miettimisen?
 V: V‰hint‰‰n sarjan Esc -> Esc -> Ctrl-C -> Esc -> Esc pit‰isi
    her‰tt‰‰ pahastikkin kuukahtanut tietokone. Tietysti vanhat
    windows-n‰pp‰imet, esim. Alt-TAB ja Ctrl-Alt-Del toimivat,
    jos niiden k‰yttˆ‰ ei ole estetty.

 K: Voiko tietokoneen j‰tt‰‰ pelailemaan taustalle?
 V: Voi. Sen voi j‰tt‰‰ miettim‰‰n yht‰ siirtoa, jos ei ole laittanut
    ohjelman tausta-ajon keskeytyst‰ p‰‰lle. Tai sitten sen voi j‰tt‰‰
    pelaamaan tietokone vs. tietokone pelej‰, jolloin kannattaa pit‰‰
    Loop-Mode p‰‰ll‰ ja tulokset kirjautuvat TULOS.LOG tiedostoon.

 K: Miksi aloitit Basic:illa ohjelman teon? C-kielih‰n on ainoa oikea
    ohjelmointikieli.
 V: Koulussani (Lappeenrannan Teknillinen Korkeakoulu) meille on opetettu
    c-kielest‰ kaikken maailman algoritmej‰, mutta ei grafiikkak‰skyj‰.
    Sen sijaan Basic:ia olen k‰ytt‰nyt jo yli 10 vuotta ja osaan siit‰
    grafiikkak‰skyt. Lis‰ksi sen kirjoittaminen on minulle nopeampaa kuin
    suomen kielen ja yht‰ virheetˆnt‰. Joten p‰‰tin vain valita helpon tavan.
    Aloittaessani en viel‰ tiennyt, ett‰ sovelluksesta tulee n‰inkin raskas.
    Basic-koodia piti sitten k‰‰nt‰‰ C-kielelle, joka onnistui yll‰tt‰v‰n
    helposti... Ja sitten viel‰ optimointi... Ohjelmasta on OpenGL-versio,
    joka on kokonaan C-kielt‰.

 K: Miksi kone on niin hidas?
 V: Ohjelma on tehty parhaan mahdollisen teko‰lyn saamiseksi,
    ei nopeimman tietokoneen saamiseksi teko‰lyn kustannuksella.
    Noin 5 sekunttia ei ole paljon ihmiselt‰ ristinollan miettimiselt‰,
    jos jaksaa mietti‰ yli 3 siirtoa eteenp‰in ja katsoa kaikki paikat
    huolellisesti. Niin sitten kun on ehtinyt katsoa ne ja j‰nnitt‰‰,
    ett‰ laittaako kone ihmisen mielest‰ parhaaseen paikkaan, niin
    kyll‰h‰n se jo siirt‰‰kin.

 K: Miksi kone sitten on hidas?
 V: C-kielell‰ tehty pieni koodinp‰tk‰ laskee yli 30 miljoonaa (!)
    if-lausetta. Koodi on tietenkin optimoitu, niin ett‰ enginen koodi mahtuu
    onneksi yhdelle ruudulle. Julkistan sen ehk‰ myˆhemmin.
    T‰ll‰ hetkell‰ ohjelma toimii etsim‰ll‰ taulukosta erilaisia tunnettuja
    vektoreita ja vertaillen niiden prioriteettej‰. Siksi ohjelman
    muokkaus uusia pelityylej‰ ja ansoja varten on eritt‰in helppoa.
    C-kielen koodia ei voi en‰‰ optimoida, mutta algoritmej‰ saattaisi
    voida hieman. Kehitteill‰ onkin Fast-Mode, jonka pit‰isi olla melkein
    yht‰ hyv‰ kuin normaali. Kuitenkin painottaisin, ett‰ teko‰ly on t‰rkein,
    eik‰ muutama sekuntti. Huom. hitailla koneilla tiputa laskettavien
    siirtojen m‰‰r‰‰ options-valikosta.


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
    laskettu). Loppujen lopuksi voisi saavuttaa noin 2 sekunttia nopeamman
    enginen kovan tyˆn tuloksena. Mutta mieluummin suuntaan kehityksen t‰ll‰
    hetkell‰ muualle (kuten parempi tietokoneen teko‰ly hidastamatta
    ohjelmaa) ja odotan sen 2 sekunttia saadakseni paremman tuloksen.
    Mukana oli myˆs kokeilu siit‰, ett‰ laskisi parhaat paikat vain niiden
    kohtien avulla, joiden l‰hell‰ on jokin merkki. T‰m‰ ei kuitenkaan
    nopeuttanut k‰yt‰nnˆn kokeiluissa tietokonetta juuri lainkaan.
    (V‰hemm‰n kuin 0.1 sekunttia.) Pentium 150MHz koneella yksi tietokoneen
    siirto kest‰‰ noin 5 sekunttia riippumatta kuinka t‰ysi pelilauta on.
    Miettimisajan pit‰isi olla aina suurin piirtein vakio. Jos haluaa
    nopeammin tuloksen, voi ostaa nopeamman koneen, mutta yleens‰ tekee
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
 V: Mahdollisesti koodi vaihtelee eri versioissa.
    Ohjelman hitauden aiheuttaa laskin. Testaappa laittaa Defender scenario
    ja suuri laskentasyvyys niin johan hyytyy kone kuin kone.

 K: Olen kuullut jotain jostain dr. L. Victor Alliksesta.
 V: Dr. Victor Allis on teeseill‰‰n ratkaissut ristinollan,
    kuinka risti voittaa aina, nollan siirroista huolimatta.
    Kysyin s‰hkˆpostitse Dr. L.V. Allikselta h‰nen ristinolla
    ohjelmasta. Se ei ole julkisesti saatavilla (ei kaupallisesti
    eik‰ ilmaisena). Lis‰ksi sen kone osaa pelata hyvin vain
    ristill‰, nolla "tiet‰‰ h‰vi‰v‰ns‰". Kiitokset dr. Victor
    Allikselle teeseist‰‰n, mutta toisaalta se asettaa ristinollan
    "pelin‰" eritt‰in kyseenalaiseen arvoon. En sis‰llytt‰nyt THO-MOKU:un
    "Allis-Mode":a, koska se ei ole mielest‰ni vaivan arvoista.
    Ohjelma vain miettisi satoja kertoja pidemp‰‰n. THO-MOKU:n
    poikkeuksellisen suuri pelilauta aiheuttaa ohjelmalle paljon
    hitautta ja suuresti ep‰ilen toimisivatko dr. L.V. Alliksen
    algoritmit suuremmalla laudalla, koska risti ei voikkaan
    turvautua laudan loppumiseen.
    Asiassta enemm‰n kiinnostuneille THO-MOKU:n kotisivulla on
    linkki Victor Alliksen sivuille, joista teesit voi hakea
    omalle koneelleen.

 K: Mist‰ t‰m‰ ohjelma l‰htee k‰yntiin?
 V: THOMOKU.EXE

 K: Miss‰ on t‰m‰n ohjelman kotisivu/mist‰ t‰m‰n voi kopioida?
 V: Kotisivutkin ovat viel‰ hieman kesken er‰iset. T‰ss‰ kuitenkin osoite:
    http://www.lut.fi/~hietanen/thomoku/

 K: Mist‰ tavoittaa tekij‰n, ett‰ voi valitta bugeista?
 V: IRC Nick:   Thorium
    S‰hkˆposti: tuomashi@freenet.hut.fi






10. SYSTEM REQUIREMENTS

10.1 Needed: (Not tested)
    * 486 or better... 
    * VGA 640*480*16

10.2 Recommended:
    * Pentium 150MHz or better.
    * Windows 95 or 98.
    * 4MB of physical memory or more.
    * Faster processor & hard disk means
      faster computer player.
    * Good monitor & refresh rate for your eyes...

    * Pentium3 800MHz or better for counting 4 moves forward...





 - Thorium - tuomashi@freenet.hut.fi

