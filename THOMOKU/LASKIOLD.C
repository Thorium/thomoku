
/*  Laskin for THO-MOKU  -              Version 4.0B OLD BETA.  */
/*                                                              */
/*  Ristinollaengine laskemaan THO-MOKU:un                      */
/*  tietokoneelle parhaat paikat.                               */

/*  By: Tuomas Hietanen (tuomashi@freenet.hut.fi)               */
/*  Tekijä ei vastaa mistään. I'm responsible of nothing.       */

/*  This code is FREEWARE.                                      */
/*  If you use this, please mention where you got it.           */
/*  This code (for C) is a bit old, but this one has comments   */
/*  and the new on is harder to understand.                     */

/*  Koodia saa kopioida vapaasti.                               */
/*  Jos käytät koodia johonkin, mainitse lähde.                 */
/*  Tämä koodi on jo pahasti vanhentunut, koska uusi engine     */
/*  on 8 kertaa nopeampi, sisältää tehokkaampia kaavoja, jne.   */
/*  Mutta tästä koodista näkee hyvin ohjelman                   */
/*  toimintaperiaatteen, sillä uudempi koodi on vielä hieman    */
/*  vaikeaselkoinen, eikä sisällä kommentteja.                  */



/* Toimintaperiaate:                                            */
/* Ohjelma lukee 40*40 syötematriisin tiedostosta.              */
/* Syötematriisi sisältää ristinollan pelilaudan. (board matrix)*/
/* Pelilaudalla 0 = tyhjä (empty), 1 = risti (X), 2 = nolla (0).*/
/* Ohjelma laskee parhaan paikan mihin tietokone laittaa        */
/* seuraavan napin. (The best place for the next move.)         */
/* Paikkojen pisteytykset tallennetaan toiseen                  */
/* tiedostoon 40*40 matriisiin. Suurin pistemäärä on            */
/* paras paikka.                                                */



#include <stdio.h>

/* Kaavat sijoitetaan taulukkoon riveille 0 - 75. */
/* Tästä puuttuu ainakin yksi kaava... (25)       */
/* Nämä ovat siis tunnetut vektorit, joita etsitään taulukosta       */
/* (known vectors searched from matrix)                              */
/*  0 = Tyhjä paikka johon laitetaan. (empty)                        */
/*  3 = Tyhjä paikka, mutta ei laiteta. (empty, but not _the place_) */
/* V1 = Tietokoneen nappula. (computer's own)                        */
/* V2 = Vastustajan nappula. (opponent's own)                        */
/*  5 = Mikä tahansa merkki  (anything)                              */

void taulukko(unsigned int AA[75][8], unsigned int V1, unsigned int V2) {
 AA[0][0]=V2;AA[0][1]=V2;AA[0][2]=0;AA[0][3]=V2;AA[0][4]=V2;AA[0][5]=5;AA[0][6]=5;AA[0][7]=5;
 AA[1][0]=V2;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;AA[1][5]=5;AA[1][6]=5;AA[1][7]=5;
 AA[2][0]=V2;AA[2][1]=0;AA[2][2]=V2;AA[2][3]=V2;AA[2][4]=V2;AA[2][5]=5;AA[2][6]=5;AA[2][7]=5;
 AA[3][0]=V2;AA[3][1]=V2;AA[3][2]=V2;AA[3][3]=V2;AA[3][4]=0;AA[3][5]=5;AA[3][6]=5;AA[3][7]=5;
 AA[4][0]=0;AA[4][1]=V2;AA[4][2]=V2;AA[4][3]=V2;AA[4][4]=V2;AA[4][5]=5;AA[4][6]=5;AA[4][7]=5;
 AA[5][0]=V1;AA[5][1]=V1;AA[5][2]=0;AA[5][3]=V1;AA[5][4]=V1;AA[5][5]=5;AA[5][6]=5;AA[5][7]=5;
 AA[6][0]=V1;AA[6][1]=0;AA[6][2]=V1;AA[6][3]=V1;AA[6][4]=V1;AA[6][5]=5;AA[6][6]=5;AA[6][7]=5;
 AA[7][0]=V1;AA[7][1]=V1;AA[7][2]=V1;AA[7][3]=0;AA[7][4]=V1;AA[7][5]=5;AA[7][6]=5;AA[7][7]=5;
 AA[8][0]=V1;AA[8][1]=V1;AA[8][2]=V1;AA[8][3]=V1;AA[8][4]=0;AA[8][5]=5;AA[8][6]=5;AA[8][7]=5;
 AA[9][0]=0;AA[9][1]=V1;AA[9][2]=V1;AA[9][3]=V1;AA[9][4]=V1;AA[9][5]=5;AA[9][6]=5;AA[9][7]=5;
 AA[10][0]=3;AA[10][1]=V2;AA[10][2]=V2;AA[10][3]=0;AA[10][4]=V2;AA[10][5]=5;AA[10][6]=5;AA[10][7]=5;
 AA[11][0]=3;AA[11][1]=V2;AA[11][2]=0;AA[11][3]=V2;AA[11][4]=V2;AA[11][5]=5;AA[11][6]=5;AA[11][7]=5;
 AA[12][0]=V2;AA[12][1]=V2;AA[12][2]=0;AA[12][3]=V2;AA[12][4]=3;AA[12][5]=5;AA[12][6]=5;AA[12][7]=5;
 AA[13][0]=V2;AA[13][1]=0;AA[13][2]=V2;AA[13][3]=V2;AA[13][4]=3;AA[13][5]=5;AA[13][6]=5;AA[13][7]=5;
 AA[14][0]=V2;AA[14][1]=V2;AA[14][2]=V2;AA[14][3]=0;AA[14][4]=3;AA[14][5]=5;AA[14][6]=5;AA[14][7]=5;
 AA[15][0]=3;AA[15][1]=0;AA[15][2]=V2;AA[15][3]=V2;AA[15][4]=V2;AA[15][5]=5;AA[15][6]=5;AA[15][7]=5;
 AA[16][0]=V1;AA[16][1]=V2;AA[16][2]=V2;AA[16][3]=V2;AA[16][4]=3;AA[16][5]=0;AA[16][6]=5;AA[16][7]=5;
 AA[17][0]=0;AA[17][1]=3;AA[17][2]=V2;AA[17][3]=V2;AA[17][4]=V2;AA[17][5]=V1;AA[17][6]=5;AA[17][7]=5;
 AA[18][0]=V2;AA[18][1]=V2;AA[18][2]=3;AA[18][3]=V2;AA[18][4]=0;AA[18][5]=5;AA[18][6]=5;AA[18][7]=5;
 AA[19][0]=0;AA[19][1]=V2;AA[19][2]=V2;AA[19][3]=3;AA[19][4]=V2;AA[19][5]=5;AA[19][6]=5;AA[19][7]=5;
 AA[20][0]=V2;AA[20][1]=3;AA[20][2]=V2;AA[20][3]=V2;AA[20][4]=0;AA[20][5]=5;AA[20][6]=5;AA[20][7]=5;
 AA[21][0]=0;AA[21][1]=V2;AA[21][2]=3;AA[21][3]=V2;AA[21][4]=V2;AA[21][5]=5;AA[21][6]=5;AA[21][7]=5;
 AA[22][0]=V2;AA[22][1]=0;AA[22][2]=0;AA[22][3]=V2;AA[22][4]=V2;AA[22][5]=5;AA[22][6]=5;AA[22][7]=5;
 AA[23][0]=V2;AA[23][1]=V2;AA[23][2]=0;AA[23][3]=0;AA[23][4]=V2;AA[23][5]=5;AA[23][6]=5;AA[23][7]=5;
 AA[24][0]=V2;AA[24][1]=0;AA[24][2]=V2;AA[24][3]=0;AA[24][4]=V2;AA[24][5]=5;AA[24][6]=5;AA[24][7]=5;
 AA[25][0]=V1;AA[25][1]=0;AA[25][2]=V1;AA[25][3]=0;AA[25][4]=V1;AA[25][5]=0;AA[25][6]=V1;AA[25][7]=5;
 AA[26][0]=0;AA[26][1]=0;AA[26][2]=V1;AA[26][3]=V1;AA[26][4]=V1;AA[26][5]=0;AA[26][6]=V2;AA[26][7]=5;
 AA[27][0]=V2;AA[27][1]=0;AA[27][2]=V1;AA[27][3]=V1;AA[27][4]=V1;AA[27][5]=0;AA[27][6]=0;AA[27][7]=5;
 AA[28][0]=3;AA[28][1]=0;AA[28][2]=V1;AA[28][3]=V1;AA[28][4]=V1;AA[28][5]=0;AA[28][6]=3;AA[28][7]=5;
 AA[29][0]=0;AA[29][1]=V1;AA[29][2]=0;AA[29][3]=V1;AA[29][4]=V1;AA[29][5]=0;AA[29][6]=5;AA[29][7]=5;
 AA[30][0]=3;AA[30][1]=3;AA[30][2]=V1;AA[30][3]=0;AA[30][4]=V1;AA[30][5]=V1;AA[30][6]=3;AA[30][7]=3;
 AA[31][0]=3;AA[31][1]=3;AA[31][2]=V1;AA[31][3]=V1;AA[31][4]=0;AA[31][5]=V1;AA[31][6]=3;AA[31][7]=3;
 AA[32][0]=3;AA[32][1]=0;AA[32][2]=V1;AA[32][3]=V1;AA[32][4]=V1;AA[32][5]=3;AA[32][6]=V2;AA[32][7]=5;
 AA[33][0]=V2;AA[33][1]=3;AA[33][2]=V1;AA[33][3]=V1;AA[33][4]=V1;AA[33][5]=0;AA[33][6]=V2;AA[33][7]=3;
 AA[34][0]=0;AA[34][1]=V1;AA[34][2]=V1;AA[34][3]=0;AA[34][4]=V1;AA[34][5]=0;AA[34][6]=5;AA[34][7]=5;
 AA[35][0]=3;AA[35][1]=0;AA[35][2]=V2;AA[35][3]=V2;AA[35][4]=3;AA[35][5]=5;AA[35][6]=5;AA[35][7]=5;
 AA[36][0]=3;AA[36][1]=V2;AA[36][2]=V2;AA[36][3]=0;AA[36][4]=3;AA[36][5]=5;AA[36][6]=5;AA[36][7]=5;
 AA[37][0]=3;AA[37][1]=V2;AA[37][2]=0;AA[37][3]=V2;AA[37][4]=3;AA[37][5]=5;AA[37][6]=5;AA[37][7]=5;
 AA[38][0]=3;AA[38][1]=V2;AA[38][2]=0;AA[38][3]=0;AA[38][4]=V2;AA[38][5]=3;AA[38][6]=5;AA[38][7]=5;
 AA[39][0]=3;AA[39][1]=0;AA[39][2]=V2;AA[39][3]=3;AA[39][4]=V2;AA[39][5]=3;AA[39][6]=5;AA[39][7]=5;
 AA[40][0]=3;AA[40][1]=V2;AA[40][2]=3;AA[40][3]=V2;AA[40][4]=0;AA[40][5]=3;AA[40][6]=5;AA[40][7]=5;
 AA[41][0]=3;AA[41][1]=V2;AA[41][2]=V2;AA[41][3]=3;AA[41][4]=0;AA[41][5]=3;AA[41][6]=5;AA[41][7]=5;
 AA[42][0]=3;AA[42][1]=0;AA[42][2]=3;AA[42][3]=V2;AA[42][4]=V2;AA[42][5]=3;AA[42][6]=5;AA[42][7]=5;
 AA[43][0]=V2;AA[43][1]=V1;AA[43][2]=V1;AA[43][3]=V1;AA[43][4]=0;AA[43][5]=0;AA[43][6]=5;AA[43][7]=5;
 AA[44][0]=V2;AA[44][1]=V1;AA[44][2]=V1;AA[44][3]=0;AA[44][4]=V1;AA[44][5]=0;AA[44][6]=5;AA[44][7]=5;
 AA[45][0]=V2;AA[45][1]=V1;AA[45][2]=0;AA[45][3]=V1;AA[45][4]=V1;AA[45][5]=0;AA[45][6]=5;AA[45][7]=5;
 AA[46][0]=V2;AA[46][1]=0;AA[46][2]=V1;AA[46][3]=V1;AA[46][4]=V1;AA[46][5]=0;AA[46][6]=5;AA[46][7]=5;
 AA[47][0]=3;AA[47][1]=0;AA[47][2]=V1;AA[47][3]=V1;AA[47][4]=3;AA[47][5]=5;AA[47][6]=5;AA[47][7]=5;
 AA[48][0]=3;AA[48][1]=V1;AA[48][2]=V1;AA[48][3]=0;AA[48][4]=3;AA[48][5]=5;AA[48][6]=5;AA[48][7]=5;
 AA[49][0]=3;AA[49][1]=V1;AA[49][2]=0;AA[49][3]=V1;AA[49][4]=3;AA[49][5]=5;AA[49][6]=5;AA[49][7]=5;
 AA[50][0]=3;AA[50][1]=V1;AA[50][2]=0;AA[50][3]=0;AA[50][4]=V1;AA[50][5]=3;AA[50][6]=5;AA[50][7]=5;
 AA[51][0]=3;AA[51][1]=0;AA[51][2]=V1;AA[51][3]=3;AA[51][4]=V1;AA[51][5]=3;AA[51][6]=5;AA[51][7]=5;
 AA[52][0]=3;AA[52][1]=V1;AA[52][2]=3;AA[52][3]=V1;AA[52][4]=0;AA[52][5]=3;AA[52][6]=5;AA[52][7]=5;
 AA[53][0]=3;AA[53][1]=V1;AA[53][2]=V1;AA[53][3]=3;AA[53][4]=0;AA[53][5]=3;AA[53][6]=5;AA[53][7]=5;
 AA[54][0]=3;AA[54][1]=0;AA[54][2]=3;AA[54][3]=V1;AA[54][4]=V1;AA[54][5]=3;AA[54][6]=5;AA[54][7]=5;
 AA[55][0]=3;AA[55][1]=0;AA[55][2]=3;AA[55][3]=3;AA[55][4]=V2;AA[55][5]=V2;AA[55][6]=3;AA[55][7]=5;
 AA[56][0]=3;AA[56][1]=V2;AA[56][2]=V2;AA[56][3]=3;AA[56][4]=3;AA[56][5]=0;AA[56][6]=3;AA[56][7]=5;
 AA[57][0]=3;AA[57][1]=V2;AA[57][2]=0;AA[57][3]=3;AA[57][4]=5;AA[57][5]=5;AA[57][6]=5;AA[57][7]=5;
 AA[58][0]=3;AA[58][1]=0;AA[58][2]=V2;AA[58][3]=3;AA[58][4]=5;AA[58][5]=5;AA[58][6]=5;AA[58][7]=5;
 AA[59][0]=3;AA[59][1]=V2;AA[59][2]=3;AA[59][3]=0;AA[59][4]=3;AA[59][5]=5;AA[59][6]=5;AA[59][7]=5;
 AA[60][0]=3;AA[60][1]=0;AA[60][2]=3;AA[60][3]=V2;AA[60][4]=3;AA[60][5]=5;AA[60][6]=5;AA[60][7]=5;
 AA[61][0]=3;AA[61][1]=V1;AA[61][2]=0;AA[61][3]=3;AA[61][4]=5;AA[61][5]=5;AA[61][6]=5;AA[61][7]=5;
 AA[62][0]=3;AA[62][1]=0;AA[62][2]=V1;AA[62][3]=3;AA[62][4]=5;AA[62][5]=5;AA[62][6]=5;AA[62][7]=5;
 AA[63][0]=3;AA[63][1]=V1;AA[63][2]=3;AA[63][3]=0;AA[63][4]=3;AA[63][5]=5;AA[63][6]=5;AA[63][7]=5;
 AA[64][0]=3;AA[64][1]=0;AA[64][2]=3;AA[64][3]=V1;AA[64][4]=3;AA[64][5]=5;AA[64][6]=5;AA[64][7]=5;
 AA[65][0]=3;AA[65][1]=V2;AA[65][2]=3;AA[65][3]=0;AA[65][4]=3;AA[65][5]=V2;AA[65][6]=3;AA[65][7]=5;
 AA[66][0]=3;AA[66][1]=V2;AA[66][2]=3;AA[66][3]=3;AA[66][4]=0;AA[66][5]=V2;AA[66][6]=3;AA[66][7]=5;
 AA[67][0]=3;AA[67][1]=V2;AA[67][2]=0;AA[67][3]=3;AA[67][4]=3;AA[67][5]=V2;AA[67][6]=3;AA[67][7]=5;
 AA[68][0]=3;AA[68][1]=V2;AA[68][2]=3;AA[68][3]=3;AA[68][4]=0;AA[68][5]=3;AA[68][6]=5;AA[68][7]=5;
 AA[69][0]=3;AA[69][1]=0;AA[69][2]=3;AA[69][3]=3;AA[69][4]=V2;AA[69][5]=3;AA[69][6]=5;AA[69][7]=5;
 AA[70][0]=3;AA[70][1]=0;AA[70][2]=3;AA[70][3]=3;AA[70][4]=V1;AA[70][5]=V1;AA[70][6]=3;AA[70][7]=5;
 AA[71][0]=3;AA[71][1]=V1;AA[71][2]=V1;AA[71][3]=3;AA[71][4]=3;AA[71][5]=0;AA[71][6]=3;AA[71][7]=5;
 AA[72][0]=3;AA[72][1]=V1;AA[72][2]=3;AA[72][3]=0;AA[72][4]=3;AA[72][5]=V1;AA[72][6]=3;AA[72][7]=5;
 AA[73][0]=3;AA[73][1]=V1;AA[73][2]=3;AA[73][3]=3;AA[73][4]=0;AA[73][5]=V1;AA[73][6]=3;AA[73][7]=5;
 AA[74][0]=3;AA[74][1]=V1;AA[74][2]=0;AA[74][3]=3;AA[74][4]=3;AA[74][5]=V1;AA[74][6]=3;AA[74][7]=5;
}


int doku(void) {
 printf("Old THO-MOKU beta engine to count best places.\n");
 printf("THO-MOKU Ristinollaajan vanhentunut beta engine.\n");
 printf("By: Tuomas Hietanen\n");
 return 0;
}
      /* Eri tietokone pelaajille on omat aliohjelmansa ja omat pisteensä */

 		/* LAAJENNETUT KAAVAT: Näillä kaavoilla korvattiin yli 130.    */
 		/*	kaavaa. Tulos pitäisi olla nopeampi ja parempi tietokone. 	*/
      /*   _ = Tyhjä paikka johon laitetaan.                         */
      /*   * = Tyhjä paikka, mutta ei laiteta.                       */
      /*   X = Tietokoneen nappula.                                  */
      /*   0 = Pelaajan nappula.                                     */
void attacker(long PISTEET[75]) {
                                   /* Pisteytys: (priority) */
 PISTEET[0]=35000; 	/*0	  XX_XX	    35000	*/
 PISTEET[1]=35000; 	/*1	  XXX_X     35000	*/
 PISTEET[2]=35000; 	/*2	  X_XXX     35000	*/
 PISTEET[3]=35000; 	/*3	  XXXX_     35000	*/
 PISTEET[4]=35000; 	/*4	  _XXXX     35000	*/
 PISTEET[5]=15000; 	/*5	  00_00     15000	*/
 PISTEET[6]=15000; 	/*6	  0_000     15000	*/
 PISTEET[7]=15000; 	/*7	  000_0     15000	*/
 PISTEET[8]=15000; 	/*8	  0000_     15000	*/
 PISTEET[9]=15000; 	/*9	  _0000     15000	*/
 PISTEET[10]=5000; 	/*10	  *XX_X      5000	*/
 PISTEET[11]=5000; 	/*11	  *X_XX      5000	*/
 PISTEET[12]=5000; 	/*12	  XX_X*      5000	*/
 PISTEET[13]=5000; 	/*13	  X_XX*      5000	*/
 PISTEET[14]=5000; 	/*14	  XXX_*      5000	*/
 PISTEET[15]=5000; 	/*15	  *_XXX      5000	*/
 PISTEET[16]=5000; 	/*16	  0XXX*_     5000	*/
 PISTEET[17]=5000; 	/*17	  _*XXX0     5000	*/
 PISTEET[18]=3000; 	/*18	  XX*X_      3000	*/
 PISTEET[19]=3000; 	/*19	  _XX*X      3000	*/
 PISTEET[20]=3000; 	/*20	  X*XX_      3000	*/
 PISTEET[21]=3000; 	/*21	  _X*XX      3000	*/
 PISTEET[22]=2900; 	/*22	  X__XX      2900	*/
 PISTEET[23]=2900; 	/*23      XX__X      2900	*/
 PISTEET[24]=2900; 	/*24	  X_X_X      2900	*/
 PISTEET[25]=14000; 	/*25	  0_0_0_0   14000	*/
 PISTEET[26]=2000; 	/*26	  __000_X    2000	*/
 PISTEET[27]=2000; 	/*27	  X_000__    2000	*/
 PISTEET[28]=2000; 	/*28	  *_000_*    2000	*/
 PISTEET[29]=2000; 	/*29	  _0_00_     2000	*/
 PISTEET[30]=30;   	/*30	  **0_00**     30	*/
 PISTEET[31]=30;	/*31	  **00_0**     30	*/
 PISTEET[32]=30;	/*32	  *_000*X      30	*/
 PISTEET[33]=30;	/*33	  X*000_*      30	*/
 PISTEET[34]=2000; 	/*34	  _00_0_     2000	*/
 PISTEET[35]=500;  	/*35	  *_XX*       500	*/
 PISTEET[36]=500;    	/*36	  *XX_*       500 	*/
 PISTEET[37]=500;    	/*37	  *X_X*       500 	*/
 PISTEET[38]=490;	/*38	  *X__X*      490	*/
 PISTEET[39]=490;	/*39	  *_X*X*      490	*/
 PISTEET[40]=490;	/*40	  *X*X_*      490	*/
 PISTEET[41]=490;	/*41	  *XX*_*      490	*/
 PISTEET[42]=490;	/*42	  *_*XX*      490	*/
 PISTEET[43]=350;	/*43	  X000__      350	*/
 PISTEET[44]=350;	/*44	  X00_0_      350	*/
 PISTEET[45]=350;	/*45	  X0_00_      350 	*/
 PISTEET[46]=350;	/*46	  X_000_      350	*/
 PISTEET[47]=200;	/*47	  *_OO*       200 	*/
 PISTEET[48]=200;	/*48	  *OO_*       200	*/
 PISTEET[49]=200;	/*49	  *O_O*       200	*/
 PISTEET[50]=190;	/*50	  *O__O*      190	*/
 PISTEET[51]=190;	/*51	  *_O*O*      190	*/
 PISTEET[52]=190;	/*52	  *O*O_*      190	*/
 PISTEET[53]=80;	/*53	  *OO*_*      80	*/
 PISTEET[54]=80;	/*54	  *_*OO*      80 	*/
 PISTEET[55]=80;	/*55	  *_**XX*     80 	*/
 PISTEET[56]=80;	/*56	  *XX**_*     80 	*/
 PISTEET[57]=60;	/*57	  *X_*        60	*/
 PISTEET[58]=60;	/*58	  *_X*        60 	*/
 PISTEET[59]=50;	/*59	  *X*_*       50 	*/
 PISTEET[60]=50;	/*60	  *_*X*       50 	*/
 PISTEET[61]=40;	/*61	  *O_*        40	*/
 PISTEET[62]=40;	/*62	  *_O*        40 	*/
 PISTEET[63]=25;	/*63	  *O*_*       25 	*/
 PISTEET[64]=25;	/*64	  *_*O*       25 	*/
 PISTEET[65]=15;	/*65	  *X*_*X*     15 	*/
 PISTEET[66]=10;	/*66	  *X**_X*     10 	*/
 PISTEET[67]=10;	/*67	  *X_**X*     10 	*/
 PISTEET[68]=10;	/*68	  *X**_*      10 	*/
 PISTEET[69]=10;	/*69	  *_**X*      10 	*/
 PISTEET[70]=10;	/*70	  *_**OO*     10 	*/
 PISTEET[71]=10;	/*71	  *OO**_*     10 	*/
 PISTEET[72]=5;		/*72	  *0*_*0*      5 	*/
 PISTEET[73]=2;		/*73	  *0**_0*      2 	*/
 PISTEET[74]=2;		/*74	  *0_**0*      2 	*/
}


void defender(long PISTEET[75]) {
 PISTEET[0]=35000; 	/*0	  XX_XX	    35000	*/
 PISTEET[1]=35000; 	/*1	  XXX_X     35000	*/
 PISTEET[2]=35000; 	/*2	  X_XXX     35000	*/
 PISTEET[3]=35000; 	/*3	  XXXX_     35000	*/
 PISTEET[4]=35000; 	/*4	  _XXXX     35000	*/
 PISTEET[5]=15000; 	/*5	  00_00     15000	*/
 PISTEET[6]=15000; 	/*6	  0_000     15000	*/
 PISTEET[7]=15000; 	/*7	  000_0     15000	*/
 PISTEET[8]=15000; 	/*8	  0000_     15000	*/
 PISTEET[9]=15000; 	/*9	  _0000     15000	*/
 PISTEET[10]=2500; 	/*10	  *XX_X      2500	*/
 PISTEET[11]=2500; 	/*11	  *X_XX      2500	*/
 PISTEET[12]=2500; 	/*12	  XX_X*      2500	*/
 PISTEET[13]=2500; 	/*13	  X_XX*      2500	*/
 PISTEET[14]=2500; 	/*14	  XXX_*      2500	*/
 PISTEET[15]=2500; 	/*15	  *_XXX      2500	*/
 PISTEET[16]=2500; 	/*16	  0XXX*_     2500	*/
 PISTEET[17]=2500; 	/*17	  _*XXX0     2500	*/
 PISTEET[18]=2400; 	/*18	  XX*X_      2400	*/
 PISTEET[19]=2400; 	/*19	  _XX*X      2400	*/
 PISTEET[20]=2400; 	/*20	  X*XX_      2400	*/
 PISTEET[21]=2400; 	/*21	  _X*XX      2400	*/
 PISTEET[22]=2000; 	/*22	  X__XX      2000	*/
 PISTEET[23]=2000; 	/*23      XX__X      2000	*/
 PISTEET[24]=2000; 	/*24	  X_X_X      2000	*/
 PISTEET[25]=14000; 	/*25	  0_0_0_0   14000	*/
 PISTEET[26]=2900; 	/*26	  __000_X    2900	*/
 PISTEET[27]=2900; 	/*27	  X_000__    2900	*/
 PISTEET[28]=2900; 	/*28	  *_000_*    2900	*/
 PISTEET[29]=2900; 	/*29	  _0_00_     2900	*/
 PISTEET[30]=50;   	/*30	  **0_00**     50	*/
 PISTEET[31]=50;	/*31	  **00_0**     50	*/
 PISTEET[32]=50;	/*32	  *_000*X      50	*/
 PISTEET[33]=50;	/*33	  X*000_*      50	*/
 PISTEET[34]=2900; 	/*34	  _00_0_     2900	*/
 PISTEET[35]=400;  	/*35	  *_XX*       400	*/
 PISTEET[36]=400;	/*36	  *XX_*       400 	*/
 PISTEET[37]=400;	/*37	  *X_X*       400 	*/
 PISTEET[38]=390;	/*38	  *X__X*      390	*/
 PISTEET[39]=390;	/*39	  *_X*X*      390	*/
 PISTEET[40]=390;	/*40	  *X*X_*      390	*/
 PISTEET[41]=390;	/*41	  *XX*_*      390	*/
 PISTEET[42]=390;	/*42	  *_*XX*      390	*/
 PISTEET[43]=350;	/*43	  X000__      350	*/
 PISTEET[44]=350;	/*44	  X00_0_      350	*/
 PISTEET[45]=350;	/*45	  X0_00_      350 	*/
 PISTEET[46]=350;	/*46	  X_000_      350	*/
 PISTEET[47]=400;	/*47	  *_OO*       400 	*/
 PISTEET[48]=400;	/*48	  *OO_*       400	*/
 PISTEET[49]=400;	/*49	  *O_O*       400	*/
 PISTEET[50]=380;	/*50	  *O__O*      380	*/
 PISTEET[51]=380;	/*51	  *_O*O*      380	*/
 PISTEET[52]=380;	/*52	  *O*O_*      380	*/
 PISTEET[53]=90;	/*53	  *OO*_*      90	*/
 PISTEET[54]=90;	/*54	  *_*OO*      90 	*/
 PISTEET[55]=90;	/*55	  *_**XX*     90 	*/
 PISTEET[56]=90;	/*56	  *XX**_*     90 	*/
 PISTEET[57]=30;	/*57	  *X_*        30	*/
 PISTEET[58]=30;	/*58	  *_X*        30 	*/
 PISTEET[59]=15;	/*59	  *X*_*       15 	*/
 PISTEET[60]=50;	/*60	  *_*X*       50 	*/
 PISTEET[61]=45;	/*61	  *O_*        45	*/
 PISTEET[62]=45;	/*62	  *_O*        45 	*/
 PISTEET[63]=25;	/*63	  *O*_*       25 	*/
 PISTEET[64]=25;	/*64	  *_*O*       25 	*/
 PISTEET[65]=15;	/*65	  *X*_*X*     15 	*/
 PISTEET[66]=10;	/*66	  *X**_X*     10 	*/
 PISTEET[67]=10;	/*67	  *X_**X*     10 	*/
 PISTEET[68]=10;	/*68	  *X**_*      10 	*/
 PISTEET[69]=10;	/*69	  *_**X*      10 	*/
 PISTEET[70]=10;	/*70	  *_**OO*     10 	*/
 PISTEET[71]=10;	/*71	  *OO**_*     10 	*/
 PISTEET[72]=5;		/*72	  *0*_*0*      5 	*/
 PISTEET[73]=2;		/*73	  *0**_0*      2 	*/
 PISTEET[74]=2;		/*74	  *0_**0*      2 	*/
}


void preasure(long PISTEET[75]) {
 PISTEET[0]=35000; 	/*0	  XX_XX	    35000	*/
 PISTEET[1]=35000; 	/*1	  XXX_X     35000	*/
 PISTEET[2]=35000; 	/*2	  X_XXX     35000	*/
 PISTEET[3]=35000; 	/*3	  XXXX_     35000	*/
 PISTEET[4]=35000; 	/*4	  _XXXX     35000	*/
 PISTEET[5]=15000; 	/*5	  00_00     15000	*/
 PISTEET[6]=15000; 	/*6	  0_000     15000	*/
 PISTEET[7]=15000; 	/*7	  000_0     15000	*/
 PISTEET[8]=15000; 	/*8	  0000_     15000	*/
 PISTEET[9]=15000; 	/*9	  _0000     15000	*/
 PISTEET[10]=2700; 	/*10	  *XX_X      2700	*/
 PISTEET[11]=2700; 	/*11	  *X_XX      2700	*/
 PISTEET[12]=2700; 	/*12	  XX_X*      2700	*/
 PISTEET[13]=2700; 	/*13	  X_XX*      2700	*/
 PISTEET[14]=2700; 	/*14	  XXX_*      2700	*/
 PISTEET[15]=2700; 	/*15	  *_XXX      2700	*/
 PISTEET[16]=2700; 	/*16	  0XXX*_     2700	*/
 PISTEET[17]=2700; 	/*17	  _*XXX0     2700	*/
 PISTEET[18]=3000; 	/*18	  XX*X_      3000	*/
 PISTEET[19]=3000; 	/*19	  _XX*X      3000	*/
 PISTEET[20]=3000; 	/*20	  X*XX_      3000	*/
 PISTEET[21]=3000; 	/*21	  _X*XX      3000	*/
 PISTEET[22]=2900; 	/*22	  X__XX      2900	*/
 PISTEET[23]=2900; 	/*23      XX__X      2900	*/
 PISTEET[24]=2900; 	/*24	  X_X_X      2900	*/
 PISTEET[25]=14000; 	/*25	  0_0_0_0   14000	*/
 PISTEET[26]=2000; 	/*26	  __000_X    2000	*/
 PISTEET[27]=2000; 	/*27	  X_000__    2000	*/
 PISTEET[28]=2000; 	/*28	  *_000_*    2000	*/
 PISTEET[29]=2000; 	/*29	  _0_00_     2000	*/
 PISTEET[30]=30;   	/*30	  **0_00**     30	*/
 PISTEET[31]=30;	/*31	  **00_0**     30	*/
 PISTEET[32]=30;	/*32	  *_000*X      30	*/
 PISTEET[33]=30;	/*33	  X*000_*      30	*/
 PISTEET[34]=2000; 	/*34	  _00_0_     2000	*/
 PISTEET[35]=500;  	/*35	  *_XX*       500	*/
 PISTEET[36]=500;	/*36	  *XX_*       500 	*/
 PISTEET[37]=500;	/*37	  *X_X*       500 	*/
 PISTEET[38]=490;	/*38	  *X__X*      490	*/
 PISTEET[39]=490;	/*39	  *_X*X*      490	*/
 PISTEET[40]=490;	/*40	  *X*X_*      490	*/
 PISTEET[41]=490;	/*41	  *XX*_*      490	*/
 PISTEET[42]=490;	/*42	  *_*XX*      490	*/
 PISTEET[43]=420;	/*43	  X000__      420	*/
 PISTEET[44]=400;	/*44	  X00_0_      400	*/
 PISTEET[45]=400;	/*45	  X0_00_      400 	*/
 PISTEET[46]=400;	/*46	  X_000_      400	*/
 PISTEET[47]=200;	/*47	  *_OO*       200 	*/
 PISTEET[48]=200;	/*48	  *OO_*       200	*/
 PISTEET[49]=200;	/*49	  *O_O*       200	*/
 PISTEET[50]=220;	/*50	  *O__O*      220	*/
 PISTEET[51]=220;	/*51	  *_O*O*      220	*/
 PISTEET[52]=220;	/*52	  *O*O_*      220	*/
 PISTEET[53]=90;	/*53	  *OO*_*      90	*/
 PISTEET[54]=90;	/*54	  *_*OO*      90 	*/
 PISTEET[55]=90;	/*55	  *_**XX*     90 	*/
 PISTEET[56]=90;	/*56	  *XX**_*     90 	*/
 PISTEET[57]=60;	/*57	  *X_*        60	*/
 PISTEET[58]=60;	/*58	  *_X*        60 	*/
 PISTEET[59]=50;	/*59	  *X*_*       50 	*/
 PISTEET[60]=50;	/*60	  *_*X*       50 	*/
 PISTEET[61]=40;	/*61	  *O_*        40	*/
 PISTEET[62]=40;	/*62	  *_O*        40 	*/
 PISTEET[63]=25;	/*63	  *O*_*       25 	*/
 PISTEET[64]=25;	/*64	  *_*O*       25 	*/
 PISTEET[65]=15;	/*65	  *X*_*X*     15 	*/
 PISTEET[66]=10;	/*66	  *X**_X*     10 	*/
 PISTEET[67]=10;	/*67	  *X_**X*     10 	*/
 PISTEET[68]=10;	/*68	  *X**_*      10 	*/
 PISTEET[69]=10;	/*69	  *_**X*      10 	*/
 PISTEET[70]=10;	/*70	  *_**OO*     10 	*/
 PISTEET[71]=10;	/*71	  *OO**_*     10 	*/
 PISTEET[72]=5;		/*72	  *0*_*0*      5 	*/
 PISTEET[73]=2;		/*73	  *0**_0*      2 	*/
 PISTEET[74]=2;		/*74	  *0_**0*      2 	*/
}


void tbuilder(long PISTEET[75]) {
 PISTEET[0]=35000; 	/*0	  XX_XX	    35000	*/
 PISTEET[1]=35000; 	/*1	  XXX_X     35000	*/
 PISTEET[2]=35000; 	/*2	  X_XXX     35000	*/
 PISTEET[3]=35000; 	/*3	  XXXX_     35000	*/
 PISTEET[4]=35000; 	/*4	  _XXXX     35000	*/
 PISTEET[5]=15000; 	/*5	  00_00     15000	*/
 PISTEET[6]=15000; 	/*6	  0_000     15000	*/
 PISTEET[7]=15000; 	/*7	  000_0     15000	*/
 PISTEET[8]=15000; 	/*8	  0000_     15000	*/
 PISTEET[9]=15000; 	/*9	  _0000     15000	*/
 PISTEET[10]=5000; 	/*10	  *XX_X      5000	*/
 PISTEET[11]=5000; 	/*11	  *X_XX      5000	*/
 PISTEET[12]=5000; 	/*12	  XX_X*      5000	*/
 PISTEET[13]=5000; 	/*13	  X_XX*      5000	*/
 PISTEET[14]=4950; 	/*14	  XXX_*      4950	*/
 PISTEET[15]=4950; 	/*15	  *_XXX      4950	*/
 PISTEET[16]=5000; 	/*16	  0XXX*_     5000	*/
 PISTEET[17]=5000; 	/*17	  _*XXX0     5000	*/
 PISTEET[18]=3000; 	/*18	  XX*X_      3000	*/
 PISTEET[19]=3000; 	/*19	  _XX*X      3000	*/
 PISTEET[20]=3000; 	/*20	  X*XX_      3000	*/
 PISTEET[21]=3000; 	/*21	  _X*XX      3000	*/
 PISTEET[22]=2900; 	/*22	  X__XX      2900	*/
 PISTEET[23]=2900; 	/*23      XX__X      2900	*/
 PISTEET[24]=2900; 	/*24	  X_X_X      2900	*/
 PISTEET[25]=14000; 	/*25	  0_0_0_0   14000	*/
 PISTEET[26]=2000; 	/*26	  __000_X    2000	*/
 PISTEET[27]=2000; 	/*27	  X_000__    2000	*/
 PISTEET[28]=2000; 	/*28	  *_000_*    2000	*/
 PISTEET[29]=2000; 	/*29	  _0_00_     2000	*/
 PISTEET[30]=20;   	/*30	  **0_00**     20	*/
 PISTEET[31]=20;	/*31	  **00_0**     20	*/
 PISTEET[32]=20;	/*32	  *_000*X      20	*/
 PISTEET[33]=20;	/*33	  X*000_*      20	*/
 PISTEET[34]=2000; 	/*34	  _00_0_     2000	*/
 PISTEET[35]=360;  	/*35	  *_XX*       360	*/
 PISTEET[36]=360;	/*36	  *XX_*       360 	*/
 PISTEET[37]=370;	/*37	  *X_X*       370 	*/
 PISTEET[38]=490;	/*38	  *X__X*      490	*/
 PISTEET[39]=490;	/*39	  *_X*X*      490	*/
 PISTEET[40]=490;	/*40	  *X*X_*      490	*/
 PISTEET[41]=490;	/*41	  *XX*_*      490	*/
 PISTEET[42]=490;	/*42	  *_*XX*      490	*/
 PISTEET[43]=350;	/*43	  X000__      350	*/
 PISTEET[44]=350;	/*44	  X00_0_      350	*/
 PISTEET[45]=350;	/*45	  X0_00_      350 	*/
 PISTEET[46]=350;	/*46	  X_000_      350	*/
 PISTEET[47]=200;	/*47	  *_OO*       200 	*/
 PISTEET[48]=200;	/*48	  *OO_*       200	*/
 PISTEET[49]=200;	/*49	  *O_O*       200	*/
 PISTEET[50]=110;	/*50	  *O__O*      110	*/
 PISTEET[51]=140;	/*51	  *_O*O*      140	*/
 PISTEET[52]=140;	/*52	  *O*O_*      140	*/
 PISTEET[53]=80;	/*53	  *OO*_*      80	*/
 PISTEET[54]=80;	/*54	  *_*OO*      80 	*/
 PISTEET[55]=90;	/*55	  *_**XX*     90 	*/
 PISTEET[56]=90;	/*56	  *XX**_*     90 	*/
 PISTEET[57]=50;	/*57	  *X_*        50	*/
 PISTEET[58]=50;	/*58	  *_X*        50 	*/
 PISTEET[59]=35;	/*59	  *X*_*       35 	*/
 PISTEET[60]=35;	/*60	  *_*X*       35 	*/
 PISTEET[61]=40;	/*61	  *O_*        40	*/
 PISTEET[62]=40;	/*62	  *_O*        40 	*/
 PISTEET[63]=25;	/*63	  *O*_*       25 	*/
 PISTEET[64]=25;	/*64	  *_*O*       25 	*/
 PISTEET[65]=20;	/*65	  *X*_*X*     20 	*/
 PISTEET[66]=15;	/*66	  *X**_X*     15 	*/
 PISTEET[67]=15;	/*67	  *X_**X*     15 	*/
 PISTEET[68]=15;	/*68	  *X**_*      15 	*/
 PISTEET[69]=15;	/*69	  *_**X*      15 	*/
 PISTEET[70]=15;	/*70	  *_**OO*     15 	*/
 PISTEET[71]=15;	/*71	  *OO**_*     15 	*/
 PISTEET[72]=7;		/*72	  *0*_*0*      7 	*/
 PISTEET[73]=3;		/*73	  *0**_0*      3 	*/
 PISTEET[74]=3;		/*74	  *0_**0*      3 	*/
}



/* AA on kaavataulukko, PISTEET on pistetaulukko kaavoille              */
/* A on pelilaudan taulukko, PRIORITY on pisteytys pelilaudalle taulukko */
int main(int argc, char *argv[]) {
 unsigned int A[55][55];
 unsigned int LUUPPI, C, D, AA[75][8];
 unsigned int KERTO, P1, P2, LIIKA, TAPAUS, KOVA;
 long PRIORITY[55][55];
 long PISTEET[75];
 FILE *sisaan;
 FILE *ulos;
 if(argc==1) {doku(); return 0;}



/* Komentorivin argumentista luetaan onko tietokone hyökkääjä vai puolustaja. */
/* (Is computer X or 0  and  the style of computer)                           */
 if(*argv[1]=='5'||*argv[1]=='1'||*argv[1]=='3'||*argv[1]=='7') taulukko(AA, 1, 2);
 if(*argv[1]=='6'||*argv[1]=='2'||*argv[1]=='4'||*argv[1]=='8') taulukko(AA, 2, 1);
/* Komentorivin argumentista luetaan tietokoneen pelityyli. */
 if (*argv[1]=='5'||*argv[1]=='6') attacker(PISTEET);
 if (*argv[1]=='1'||*argv[1]=='2') defender(PISTEET);
 if (*argv[1]=='3'||*argv[1]=='4') preasure(PISTEET);
 if (*argv[1]=='7'||*argv[1]=='8') tbuilder(PISTEET);



/* Taulukon nollaus */
 for(P2=0;P2<54;P2++){
  for(P1=0;P1<54;P1++){
   PRIORITY[P1][P2]=0;
   A[P1][P2]=0;
  };
 };



/* Luetaan pelilauta tiedostosta taulukkoon A */
/* Read the 40*40 input matrix, image from the board */
 sisaan=fopen("THOUT.DAT","r");
 for(D=7;D<47;D++){
  for(C=7;C<47;C++){
   fscanf(sisaan,"%d ",&A[C][D]);
  };
 };
 fclose(sisaan);





/* TÄMÄ ON OHJELMAN YDIN, (engine) joka laskee parhaan paikan laittaa  */
/* tietokoneen pelimerkki.                                             */
/* This program looks the best place for computer's move               */
/* for THO-MOKU, a Go-Moku like game.                                  */
 for(D=7;D<=47;D++){
  for(C=7;C<=47;C++){
   for(TAPAUS=0;TAPAUS<=75;TAPAUS++){
    for(LUUPPI=0;LUUPPI<=3;LUUPPI++){
     for(KOVA=0;KOVA<=7;KOVA++){
      LIIKA=0;
      for(KERTO=0;KERTO<=7;KERTO++){
       if ((LUUPPI == 0) && (AA[TAPAUS][KERTO]==A[C+KERTO][D])) LIIKA+=1;
       if ((LUUPPI == 1) && (AA[TAPAUS][KERTO]==A[C][D+KERTO])) LIIKA+=1;
       if ((LUUPPI == 2) && (AA[TAPAUS][KERTO]==A[C+KERTO][D-KERTO])) LIIKA+=1;
       if ((LUUPPI == 3) && (AA[TAPAUS][KERTO]==A[C+KERTO][D+KERTO])) LIIKA+=1;
       if (AA[TAPAUS][KERTO]==5) LIIKA+=1;
       if (AA[TAPAUS][KERTO]==3) {
        if ((LUUPPI == 0) && (A[C+KERTO][D]==0)) LIIKA+=1;
        if ((LUUPPI == 1) && (A[C][D+KERTO]==0)) LIIKA+=1;
        if ((LUUPPI == 2) && (A[C+KERTO][D-KERTO]==0)) LIIKA+=1;
        if ((LUUPPI == 3) && (A[C+KERTO][D+KERTO]==0)) LIIKA+=1;
       };
      };
      if ((LUUPPI == 0) && (A[C+KOVA][D]==0)) LIIKA+=1;
      if ((LUUPPI == 1) && (A[C][D+KOVA]==0)) LIIKA+=1;
      if ((LUUPPI == 2) && (A[C+KOVA][D-KOVA]==0)) LIIKA+=1;
      if ((LUUPPI == 3) && (A[C+KOVA][D+KOVA]==0)) LIIKA+=1;
      if ((LIIKA==9) && (AA[TAPAUS][KOVA]==0)) {
       if (LUUPPI == 0) PRIORITY[C+KOVA][D]+=PISTEET[TAPAUS];
       if (LUUPPI == 1) PRIORITY[C][D+KOVA]+=PISTEET[TAPAUS];
       if (LUUPPI == 2) PRIORITY[C+KOVA][D-KOVA]+=PISTEET[TAPAUS];
       if (LUUPPI == 3) PRIORITY[C+KOVA][D+KOVA]+=PISTEET[TAPAUS];
      };
     };
    };
   };
  };
 };
/* Uudemmassa versiossa kone käyttää hieman Knuthin, Morrisin ja Prattin  */
/* algoritmin (merkkijonon haku taulukosta) tyylistä algoritmiä.          */
/* Syyn voi lukea README.TXT:stä.                                         */

/* Ei negatiivisia arvoja taulukkoon */
 for(P2=7;P2<47;P2++){
  for(P1=7;P1<47;P1++){
   if (PRIORITY[P1][P2]<=0) PRIORITY[P1][P2]=0;
  };
 };




/* Kirjoitetaan taulukko tiedostoon.      */
/* Suurin arvo on paras paikka.           */
/* Write the 40*40 output matrix to file. */
/* Highest priority is the best place.    */
 ulos=fopen("THIN.DAT","w");
 for(P2=7;P2<48;P2++){
  for(P1=7;P1<47;P1++){
   fprintf(ulos,"%d ",PRIORITY[P1][P2]);
  };
 };
 fclose(ulos);






 /* Siinä se.                                           */

 /* Estimated time on Pentium 166MHz about 16 sec.      */
 /* (My new engine counts the same in < 2 sec. :)   */
 return 0;
}

/* - Thorium - tuomashi@freenet.hut.fi */
