/* THO-MOKU OpenGL versio 1.48 */


/* Noniin, tässä THO-MOKU:n koodi, katso ja kauhistu...         */
/* Copyright  Tuomas Hietanen                                   */


/* Ensin ladataan kaikki mahdolliset kirjastot...               */
#include <stdio.h>
#include <math.h>  
#include <stdlib.h>
#include <time.h>  
#include <float.h> 
#include <GL/glut.h>


/* Tässä kasa vakioita. */


/* Viewporttijutut */
#define X_MINI  -1.75f
#define X_MAXI   1.75f
#define Y_MAXI   1.75f
#define Y_MINI  -1.75f
#define Z_MINI  -4.0f
#define Z_MAXI   4.0f

#define tex_width     4  /* texturejutskaa...turha. */
#define tex_height    4

#define PI 3.14159265358979323846 /* ihana luonnonvakio */
#define OTSIKKO  "THO-MOKU  Open GL Beta"



/* Tähän kameran kiertokulmaa */
GLfloat kulma1 = 0;
GLfloat kulma2  = 0;
GLfloat kulma3 = 0;
GLfloat etaisz = 1;
int taustanum=0;

int korkeus, leveys; /* monitorin & viewportin resoluutiot */

int paaikkuna; /* ikkunointinumerojuttu (3dfx-piirejä varten)*/


/* THE LAUTA */
unsigned int A[64][64], puoli=1, tyyli=1;

/* Laudan koko ja pelityyli */
int laudankoko=40, pelvscomp=0, rulet=1;

int maxsyvyys=3;
int puhe=0;

int kokovuoro=0, arpamaara=0;
int alkux, alkuy, loppux, loppuy;

/* Laudan ulkonäkö... */
int viivaon=0, leiaut=0;
char taustakv3[13], taustakv2[13], taustakv[13];
int peliloppu=0, loopmode=0, distext=0;
int akuva=1, helppi=0;




/* Ruudulle kirjoitettavat merkkijonot */
static char tekstijutska[100];
static char tekstijutska2[100];
static char tekstijutska3[100];
static char tekstijutska4[100];
static char tekstijutska5[100];
int tekstit=0;


/* Undojuttuja */
int undo1x=0, undo1y=0, undo2x=0, undo2y=0;
/* Tietokoneen juttuja */
int edellx=0, edelly=0, edell2x=0, edell2y=0;
int isoinx=0, isoiny=0, alotyyli=2, alotyyli2=5;
int primatriisi=0;
long int PRIORITY[64][64];
int neliox, nelioy, tietokone=0;

/* Laudan arpomisen muuttujat*/
int randpuoli=1, randmaara=1, randvaik=1;
int arvottu2, arvottu3;

/* Menut */
int teema, lautakoko, pelaajat, kuittaus, kone, leiautti, maisemat, settinki;
int ktyyli, kstyyli, karvonta, kajatus, koppi, klooppi, muitten, vakioscen;
int vuoroja, taso, pelaajapuol, randomi, skenariot, huijaus, saannot;
int menuvalinta[32];








/* Tekstin kirjoitus paikalleen... */

void merkkijonoruudulle(char *s){
  unsigned int i;
  for(i=0;i<strlen(s);i++)glutBitmapCharacter (GLUT_BITMAP_HELVETICA_12, s[i]);
}

void isojonoruudulle(char *s){
  unsigned int i;
  for(i=0;i<strlen(s);i++)glutBitmapCharacter (GLUT_BITMAP_HELVETICA_18, s[i]);
}


/* Näillä satunnaisuus */
int arpa3(void){
	time_t aika2;
	struct tm aika;
	aika2=time(&aika2);
	aika=*localtime(&aika2);
	/* Satunnaisuus otetaan kellosta */
	srand(((aika.tm_sec+1)*(aika.tm_hour+1)*(aika.tm_min+1)));
	return ((rand()%(50))+1);
}

int arpa2(void){
	time_t aika2;
	struct tm aika;
	aika2=time(&aika2);
	aika=*localtime(&aika2);
	srand(((aika.tm_sec+1)*(aika.tm_hour+1)*(aika.tm_min+1))+arvottu3);
	return ((rand()%(100))+1);
}

int arpa(void){
	time_t aika2;
	struct tm aika;
	aika2=time(&aika2);
	aika=*localtime(&aika2);
	srand(((aika.tm_sec+1)*(aika.tm_hour+1)*(aika.tm_min+1))+arvottu2+arvottu3);
	return ((rand()%(4))+1);
}


int arpa4(void){
	time_t aika2;
	struct tm aika;
	aika2=time(&aika2);
	aika=*localtime(&aika2);
	srand(aika.tm_sec+1);
	return ((rand()%(4))+1);
}



/* Tekstuuri kuvien lataus */
typedef struct{unsigned int xkoko, ykoko;GLubyte * data;} PPMkuva;

static void Skaalaakuva(PPMkuva * kuva, int xkoko, int ykoko){
    GLubyte * bufferi;
    bufferi = (GLubyte *)malloc(3 * xkoko * ykoko);
    if (gluScaleImage(GL_RGB, kuva->xkoko, kuva->ykoko, GL_UNSIGNED_BYTE,
                      kuva->data, xkoko, ykoko, GL_UNSIGNED_BYTE, bufferi) != 0){
        fprintf(stderr, "Error...\n");exit(1);
    }
    free(kuva->data);
    kuva->xkoko = xkoko;
    kuva->ykoko = ykoko;
    kuva->data = bufferi;
}

static PPMkuva * HaePPM(const char * tiednim){
    char kuvarivi[80];
    FILE * tiedostokuv;
    PPMkuva * kuva;
    int kuvmaxkoko;
    tiedostokuv = fopen(tiednim, "rb");
    fgets(kuvarivi, sizeof(kuvarivi), tiedostokuv);
    if ((kuvarivi[0] != 'P') || (kuvarivi[1] != '6')){
        fprintf(stderr, "Wrong image-format, should be `P6'\n");exit(1);
    }
    kuva = malloc(sizeof(PPMkuva));
    for(;;){
        if(!fgets(kuvarivi, sizeof(kuvarivi), tiedostokuv)){perror(tiednim);exit(1);}
        if((kuvarivi[0] == '#') || (kuvarivi[0] == '\n')) continue;
        if(sscanf(kuvarivi, "%d %d", &kuva->xkoko, &kuva->ykoko) != 2){
            fprintf(stderr, "Error: can't load kuva `%s'\n", tiednim);exit(1);
        }
        break;
    }
    for(;;){
        if (!fgets(kuvarivi, sizeof(kuvarivi), tiedostokuv)) {perror(tiednim);exit(1);}
        if ((kuvarivi[0] == '#') || (kuvarivi[0] == '\n')) continue;
        if (sscanf(kuvarivi, "%d", &kuvmaxkoko) != 1){
            fprintf(stderr, "Error: can't load image `%s'\n", tiednim);exit(1);
        }
        break;
    }
    kuva->data = malloc(3 * kuva->xkoko * kuva->ykoko);
    if (fread(kuva->data, 3 * kuva->xkoko, kuva->ykoko, tiedostokuv) != kuva->ykoko){
        perror(tiednim);exit(1);
    }
    if(fclose(tiedostokuv) != 0){perror(tiednim);exit(1);}
    glPixelStorei(GL_UNPACK_ALIGNMENT, 1); glPixelStorei(GL_PACK_ALIGNMENT, 1);
    Skaalaakuva(kuva, (int)pow(2.0, (float)((int)(log(kuva->xkoko)/log(2.0)))),
               (int)pow(2.0, (float)((int)(log(kuva->ykoko)/log(2.0)))));
    return kuva;
}

/* tekstuurit */
PPMkuva * kuva;
PPMkuva * kuva1;
PPMkuva * kuva2;
PPMkuva * kuva3;
PPMkuva * kuva4;
PPMkuva * kuva5;
PPMkuva * kuva6;
PPMkuva * kuva7;
static GLint tausta, alunen, lauta;
















/* <<<<<<  OPEN-GL Piirto juttuja   >>>>>> */


/* Apu muutokseen pelilaudan koordinaateista viewportin koordinaatteihin.         */
/* Eli esim. 40*40 -> (-1.75)*(1.75) (tai itseasiaassa lauta on vain (-1.2)*(1.2) */
float getzeta(void){
	float z=0;
	if(laudankoko==40) z=0.051f;
	if(laudankoko==20) z=0.105f;
	if(laudankoko==16) z=0.13f;
	if(laudankoko==15) z=0.14f;
/*	if(laudankoko==3) z=0.8f; */
	return z;
}

/* piirtää pisteen (josta näkee mitä kone ajattelee) */
void piste(int x, int y, long int suurin){
	float z, vari1;
	vari1=PRIORITY[x+8][y+8]/(suurin+1.0f);
	z=getzeta();
	glPushMatrix(); glLineWidth(4.0);
	glColor3f(1.0f, 1.0f-vari1, 0.1f);
  glBegin(GL_LINE_LOOP);
	  glVertex3f(x*z-0.999f, y*z-1.001f, 0.01f);
	  glVertex3f(x*z-1.001f, y*z-0.999f, 0.01f);
	  glVertex3f(x*z-1.001f, y*z-1.001f, 0.01f);
	  glVertex3f(x*z-0.999f, y*z-0.999f, 0.01f);
	glEnd();
	glPopMatrix();
}

/* piirtää viivan (voittajasuoran päälle) */
void pviiva(void){
	float z;
	z=getzeta();
	glPushMatrix();
    glLineWidth(2.0);
    glBegin(GL_LINES);
	glVertex3f(alkux*z-1.0f, alkuy*z-1.0f, 0.1f);
	glVertex3f(loppux*z-1.0f, loppuy*z-1.0f, 0.1f);
	glEnd();
	glPopMatrix();
}						



/* the kamera, tosin ihan turhaa pyöritellä ristinollan lautaa */
void kamera(void){
  glMatrixMode(GL_MODELVIEW);
  glLoadIdentity();
  glRotatef((kulma2), 0.0f, 1.0f, 0.0f);
  glRotatef((kulma3), 1.0f, 0.0f, 0.0f);
  glTranslatef(0.0, 0.0, etaisz);
}


/* Piirtää ristin */
void risti(float x, float y){
	float z;
  int i;
  float kulmatus, xx, yy;
	z=getzeta();

  glPushMatrix();
	if(leiaut==0){
	  glLineWidth(3.0);
	  glBegin(GL_LINES);
		  glVertex3f( (0.2857f+x)*z-1,  (0.2857f+y)*z-1, 0);
		  glVertex3f( (-0.2857f+x)*z-1, (-0.2857f+y)*z-1, 0);
		  glVertex3f( (0.2857f+x)*z-1,  (-0.2857f+y)*z-1, 0);
		  glVertex3f( (-0.2857f+x)*z-1, (0.2857f+y)*z-1, 0);
		glEnd();
	}

/* Tai mustan kiven */
	if(leiaut==1){
		if(distext==0)glColor3f (0.1F, 0.1F, 0.1F);
		if(distext==1)glColor3f (0.3F, 0.3F, 0.3F);
      glBegin(GL_TRIANGLE_FAN);
      glVertex2f(x*z-1, y*z-1);
      for (i = 0; i <= 360; i += 10) {
        kulmatus = i / 180.0 * PI;
        yy = sin(kulmatus)*z*0.4 + y*z-1;
        xx = cos(kulmatus)*z*0.4 + x*z-1;
        glVertex2f(xx, yy);
      }
		glEnd();
	}
	glPopMatrix();
}


/* Piirtää nollan */
void nolla(float x, float y){
	float z;
  int i;
  float kulmatus, xx, yy;
	z=getzeta();

	glPushMatrix();
	if(leiaut==0){
		glLineWidth(3.0);
		glBegin(GL_LINE_LOOP);
  		glVertex3f( (-0.29143f+x)*z-1,       y*z-1,       0);
	  	glVertex3f( (-0.20607f+x)*z-1,  (0.20607f+y)*z-1, 0);
		  glVertex3f(   x*z-1,            (0.29143f+y)*z-1, 0);
		  glVertex3f(  (0.20607f+x)*z-1,  (0.20607f+y)*z-1, 0);
		  glVertex3f(  (0.29143f+x)*z-1,       y*z-1,       0);
		  glVertex3f(  (0.20607f+x)*z-1, (-0.20607f+y)*z-1, 0);
		  glVertex3f(   x*z-1,           (-0.29143f+y)*z-1, 0);
		  glVertex3f( (-0.20607f+x)*z-1, (-0.20607f+y)*z-1, 0);
		glEnd();
	}
/* Tai valkoisen kiven */
	if(leiaut==1){
		glColor3f (0.9F, 0.9F, 0.9F);
      glBegin(GL_TRIANGLE_FAN);
      glVertex2f(x*z-1, y*z-1);
      for (i = 0; i <= 360; i += 10) {
        kulmatus = i / 180.0 * PI;
        yy = sin(kulmatus)*z*0.4 + y*z-1;
        xx = cos(kulmatus)*z*0.4 + x*z-1;
        glVertex2f(xx, yy);
      }
		glEnd();
	}
	glPopMatrix();

}



/* piirtää ruudukon... */
void piirraruudukko(void){
	float x, y, z;
  glLineWidth(1.0);
	z=getzeta();

	glPushMatrix();
	if(leiaut==0){
		for(y=-1-z*0.05f;y<1;y+=z){
			for(x=-1-z*0.05f;x<1;x+=z){
				glBegin(GL_LINE_LOOP);
		 		glVertex3f( x-z*0.5f, y-z*0.5f, 0);
				glVertex3f( x+z*0.5f, y-z*0.5f, 0);
				glVertex3f( x+z*0.5f, y+z*0.5f, 0);
				glVertex3f( x-z*0.5f, y+z*0.5f, 0);
				glEnd();
			}
		}
	}
	if(leiaut==1){
		for(y=-1+z;y<1-z;y+=z){
			for(x=-1+z;x<1-z;x+=z){
				glBegin(GL_LINE_LOOP);
		 		glVertex3f( x-z, y-z, 0);
				glVertex3f( x+z, y-z, 0);
				glVertex3f( x+z, y+z, 0);
				glVertex3f( x-z, y+z, 0);
				glEnd();
			}
		}
	}
	glPopMatrix();
}


/* näyttää viimeisen siirron (piirtää laatikon ympärille)*/
void kirkastar(int k1, int k2){
	float z;
  glLineWidth(1.0f);
	z=getzeta();

	glPushMatrix();
	glColor3f(1.0f, 0.5f, 0.4f);
  glBegin(GL_LINE_LOOP);
  	glVertex3f( k1*z-z*0.49f-1, k2*z-z*0.49f-1, 0.01f);
	  glVertex3f( k1*z-z*0.49f-1, k2*z+z*0.49f-1, 0.01f);
	  glVertex3f( k1*z+z*0.49f-1, k2*z+z*0.49f-1, 0.01f);
	  glVertex3f( k1*z+z*0.49f-1, k2*z-z*0.49f-1, 0.01f);
	glEnd();
	glColor3f(0.0f, 0.0f, 0.0f);
	glPopMatrix();

}





/* piirtää taustan textuurin */
void piirratausta(void){
	glPushMatrix();
	glBegin(GL_POLYGON);
	  glNormal3f(0.0f, 0.0f, 0.1f);
	  glTexCoord2f(0.0f, 0.0f); glVertex3f(  -1.8f, -1.8f,-3);
	  glTexCoord2f(2.0f, 0.0f); glVertex3f(  1.8f, -1.8f,-3);
	  glTexCoord2f(2.0f, 2.0f); glVertex3f( 1.8f, 1.8f,-3);
	  glTexCoord2f(0.0f, 2.0f); glVertex3f( -1.8f, 1.8f,-3);
	glEnd();
	glPopMatrix();

    /* Tausta! */
}

/* Piirtää laudan alusen tekstuurin*/
void piirraalunen(int lautatyyli){
	glPushMatrix();
  if(lautatyyli!=1){
    glBegin(GL_POLYGON);
	    glNormal3f(0.0f, 0.0f, 1.0f);
  	  glTexCoord2f(0.0f, 0.0f); glVertex3f(  -1.4f, -1.4f,-2);
	    glTexCoord2f(2.0f, 0.0f); glVertex3f(  1.4f, -1.4f,-2);
	    glTexCoord2f(2.0f, 2.0f); glVertex3f( 1.4f, 1.4f,-2);
	    glTexCoord2f(0.0f, 2.0f); glVertex3f( -1.4f, 1.4f,-2);
	  glEnd();
  }
	glPopMatrix();
    /* Tausta! */
}

/* Piirtää laudan tekstuurin */
void piirralauta(int lautatyyli){
	glPushMatrix();
  if(lautatyyli!=1){
    glBegin(GL_POLYGON);
 	    glNormal3f(0.0f, 0.0f, 1.0f);
	    glTexCoord2f(0.0f, 0.0f); glVertex3f(  -1.2f, -1.2f,-1);
  	  glTexCoord2f(2.0f, 0.0f); glVertex3f(  1.2f, -1.2f,-1);
	    glTexCoord2f(2.0f, 2.0f); glVertex3f( 1.2f, 1.2f,-1); 
	    glTexCoord2f(0.0f, 2.0f); glVertex3f( -1.2f, 1.2f,-1);
	  glEnd();
  }
	glPopMatrix();
    /* Tausta! */
}


/* valoa kansalle! */
void Valotpaalle(void){
     GLfloat light_ambient[4] = { 0.5f, 0.5f, 0.5f, 0.5f };
     GLfloat light_specular[4] = { 0.6f, 0.6f, 0.6f, 0.5f };
     GLfloat light_position[4] = { 0.8f, 0.8f, 0.8f, 0.5f };

     /* valon 0 parametrit */
     glLightfv (GL_LIGHT0, GL_AMBIENT, light_ambient);
     glLightfv (GL_LIGHT0, GL_SPECULAR, light_specular);
     glLightfv (GL_LIGHT0, GL_POSITION, light_position);
     /*  ja valot päälle */
     glEnable (GL_LIGHTING);
     glEnable (GL_LIGHT0);
     glEnable(GL_DEPTH_TEST);

     /* materiaalia */
     glEnable(GL_COLOR_MATERIAL);

}


/* Tekee upean alkukuvan PPM-tiedostosta jonka piirsin */
void alkukuva(char tkuvatus[13]){
	viivaon=0;
	glScalef(1.0, 1.0, 1.0);
	glMatrixMode(GL_MODELVIEW);
	kuva1 = HaePPM(tkuvatus);
	tausta = glGenLists(1);
	glNewList(tausta, GL_COMPILE);
	kuva=kuva1;
	gluBuild2DMipmaps(GL_TEXTURE_2D, 3, kuva->xkoko, kuva->ykoko, GL_RGB, GL_UNSIGNED_BYTE, kuva->data);

	glPushMatrix();
	glBegin(GL_POLYGON);
   	glNormal3f(0.0f, 0.0f, 1.0f);
    glTexCoord2f(0.0f, 1.0f); glVertex3f( -1.7f, -1.7f,0);
	  glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.7f, -1.7f,0);
	  glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.7f, 1.7f,0);
	  glTexCoord2f(0.0f, 0.0f); glVertex3f( -1.7f, 1.7f,0);
  glEnd();
	glPopMatrix();
	free(kuva->data);
	free(kuva);
	glEndList();
}




/* Alustaa laudan tekstuurit PPM kuva tiedostoista */
void ruudunpiirto(char taustakuva3[13], char taustakuva2[13], char taustakuva[13], int lautatyyli){
	int laskuri;

	glMatrixMode(GL_TEXTURE);
	glScalef(1.0, 1.0, 1.0);
	glMatrixMode(GL_MODELVIEW);

	if (taustakuva3[0]!='9'){
		for(laskuri=0;laskuri<13;laskuri++){
			taustakv3[laskuri]=taustakuva3[laskuri];
	  	taustakv2[laskuri]=taustakuva2[laskuri];
	  	taustakv[laskuri]=taustakuva[laskuri];
		}
	}
	
	kuva1 = HaePPM(taustakv3);
  kuva2 = HaePPM(taustakv2);kuva3 = HaePPM(taustakv);
	tausta = glGenLists(1);
	glNewList(tausta, GL_COMPILE);
	kuva=kuva1;
	gluBuild2DMipmaps(GL_TEXTURE_2D, 3, kuva->xkoko, kuva->ykoko, GL_RGB, GL_UNSIGNED_BYTE, kuva->data);
	piirratausta();
	free(kuva->data);
	free(kuva);
	glEndList();
	alunen = glGenLists(1);
  glNewList(alunen, GL_COMPILE);
  kuva=kuva2;
  gluBuild2DMipmaps(GL_TEXTURE_2D, 3, kuva->xkoko, kuva->ykoko, GL_RGB, GL_UNSIGNED_BYTE, kuva->data);
  piirraalunen(lautatyyli);
  free(kuva->data);
  free(kuva);
  glEndList();

 	lauta = glGenLists(1);
 	glNewList(lauta, GL_COMPILE);
  kuva=kuva3;
  gluBuild2DMipmaps(GL_TEXTURE_2D, 3, kuva->xkoko, kuva->ykoko, GL_RGB, GL_UNSIGNED_BYTE, kuva->data);
  piirralauta(lautatyyli);
  free(kuva->data);
  free(kuva);
  glEndList();

}















/* <<<<<  Sekalaista,  taulukoiden tyhjennyksiä yms >>>>> */


void nollaus(unsigned int unsignedi[64][64], long int longgi[64][64]){
 unsigned int P1, P2;
 for(P2=0;P2<64;P2++){
  for(P1=0;P1<64;P1++){
   longgi[P1][P2]=0;
   unsignedi[P1][P2]=0;
  }
 }
}

void prionollaus(void){
 unsigned int P1, P2;
 for(P2=0;P2<64;P2++){
  for(P1=0;P1<64;P1++){
	  PRIORITY[P1][P2]=0;
  }
 }
}


void turhatarkistus(void){
 unsigned int P1, P2;
 for(P2=7;P2<47;P2++){
  for(P1=7;P1<47;P1++){
   if (PRIORITY[P1][P2]<=0) PRIORITY[P1][P2]=0;
  }
 }
}


/* Lukee skenarion .TSC (Thomoku Scenario File) tiedostosta. */
void skenaarionluku(char tiedosto[13]){
	unsigned int P1, P2;

	FILE *sisaan;
	sisaan=fopen(tiedosto,"r");

	fscanf(sisaan,"%d ",&pelvscomp);
	pelvscomp--;
	fscanf(sisaan,"%d ",&puoli);
	fscanf(sisaan,"%d ",&kokovuoro);
	for(P2=0;P2<40;P2++){
		for(P1=0;P1<40;P1++){
			fscanf(sisaan,"%d ",&A[P1][39-P2]);
		}
	}
	if(puoli==0){
		nollaus(A, PRIORITY);
		kokovuoro=0;
		pelvscomp=2;
		puoli=1;
		glutPostRedisplay();
	}
	fclose(sisaan);
}


/* Tällä kirjoitetaan tulokset logiin...ei käytössä vielä */
void tiedostoonkirjoitus(){
 unsigned int P1, P2;
 FILE *ulos;
 ulos=fopen("tulos.txt","w");
 for(P2=7;P2<48;P2++){
  for(P1=7;P1<47;P1++){
   fprintf(ulos,"%d ",PRIORITY[P1][P2]);
  }
 }
 fclose(ulos);
}







/* <<<<<<  Napin laitto laudalle ja voiton tarkistus >>>>>>> */



int tarkistavoitto(void){
	int x, y, n;
	unsigned int B[64][64];

	for(y=0;y<64;y++){
		for(x=0;x<64;x++)B[x][y]=0;
	}
	/* Sijoitetaan lauta isompaan taulukkoon, ettei humpsahda    */
	/* taulukon ali/yli for-luuppien arvot                       */
	for(y=0;y<42;y++){
		for(x=0;x<42;x++)B[x+8][y+8]=A[x][y];
	}

	for(y=8;y<(laudankoko+8);y++){
		for(x=8;x<(laudankoko+8);x++){
			for(n=0;B[x+n][y]==puoli;n++);
			if(n>4){
				if((rulet==1)||((n==5)&&(B[x-1][y]!=puoli))){
					alkux=x-8;
					alkuy=y-8;
					loppux=x+n-9;
					loppuy=y-8;
					viivaon=1;
					return puoli;
				}
			}
		}
	}
	for(y=8;y<(laudankoko+8);y++){
		for(x=8;x<(laudankoko+8);x++){
			for(n=0;B[x][y+n]==puoli;n++);
			if(n>4){
				if((rulet==1)||((n==5)&&(B[x][y-1]!=puoli))){
					alkux=x-8;
					alkuy=y-8;
					loppux=x-8;
					loppuy=y+n-9;
					viivaon=1;
					return puoli;
				}
			}
		}
	}
	for(y=8;y<(laudankoko+8);y++){
		for(x=8;x<(laudankoko+8);x++){
			for(n=0;B[x+n][y+n]==puoli;n++);
			if(n>4){
				if((rulet==1)||((n==5)&&(B[x-1][y-1]!=puoli))){
					alkux=x-8;
					alkuy=y-8;
					loppux=x+n-9;
					loppuy=y+n-9;
					viivaon=1;
					return puoli;
				}
			}
		}
	}
	for(y=8;y<(laudankoko+8);y++){
		for(x=8;x<(laudankoko+8);x++){
			for(n=0;B[x-n][y+n]==puoli;n++);
			if(n>4){
				if((rulet==1)||((n==5)&&(B[x+1][y-1]!=puoli))){
					alkux=x-8;
					alkuy=y-8;
					loppux=x-n-7;
					loppuy=y+n-9;
					viivaon=1;
					return puoli;
				}
			}
		}
	}
	return 0;
}

int voitontarkistus(void){
	int voittaja=0;
	voittaja=tarkistavoitto();
	if(voittaja!=0){
		peliloppu=1;
		glDisable(GL_TEXTURE_2D);
		if(voittaja==1)tekstit=1;
		if(voittaja==2)tekstit=2;
		glutPostRedisplay();
	}
	return puoli;
}


int sijoitanappi(int x, int y){
	int apu=0, apu2=1;
	apu=puoli;
	if((pelvscomp==3)&&(kokovuoro==0)){x=laudankoko/2;y=laudankoko/2;}

	if((A[x][y]==0)&&(x>=0)&&(x<=39)&&(y>=0)&&(y<=39)){
		undo2x=undo1x;undo2y=undo1y;
		undo1x=x;undo1y=y;
		A[x][y]=puoli;
		if(puoli==1)apu=2;
		if(puoli==2)apu=1;
		kokovuoro++;
		apu2=0;
	}
	voitontarkistus();
	puoli=apu;
	edell2x=edellx=x;edell2y=edelly=y;
	return apu2;
}







/*   <<<<<    TEKOÄLY     >>>>>>                               */
















/*  Laskin for THO-MOKU  -    Versio 6.30 Beta                  */
/*                                                              */
/*  Ristinollaengine laskemaan THo-Mokuun                       */
/*  tietokoneelle parhaat paikat.                               */
/*  (c) Tuomas Hietanen                                         */
/*                                                              */
/*  Tekijä ei vastaa mistään. I'm responsible of nothing.       */
/*  Jne, jne...                                                 */
/*  Tämä on uudempi versio koodista, mutta tätä ei ole vielä    */
/*  ohjelmapaketissa... Laitan kun jaksan...                    */




/* Toimintaperiaate:                                            */
/* Syötematriisi sisältää ristinollan pelilaudan. (board matrix)*/
/* Pelilaudalla 0 = tyhjä (empty), 1 = risti (X), 2 = nolla (0).*/
/* Ohjelma laskee parhaan paikan mihin tietokone laittaa        */
/* seuraavan napin. (The best place for the next move.)         */
/* Paikkojen pisteytykset tallennetaan toiseen                  */
/* tiedostoon 40*40 matriisiin. Suurin pistemäärä on            */
/* paras paikka.                                                */








/* Suora voitto seuraavalla siirrolla */
void taulukko1(int AA[64][8], int V1, int V2) {
  AA[0][0]=V2;AA[0][1]=V2;AA[0][2]=0;AA[0][3]=V2;AA[0][4]=V2;AA[0][5]=5;AA[0][6]=5;AA[0][7]=5;
  AA[1][0]=V2;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;AA[1][5]=5;AA[1][6]=5;AA[1][7]=5;
  AA[2][0]=V2;AA[2][1]=V2;AA[2][2]=V2;AA[2][3]=V2;AA[2][4]=0;AA[2][5]=5;AA[2][6]=5;AA[2][7]=5;

  AA[3][0]=V1;AA[3][1]=V1;AA[3][2]=0;AA[3][3]=V1;AA[3][4]=V1;AA[3][5]=5;AA[3][6]=5;AA[3][7]=5;
  AA[4][0]=V1;AA[4][1]=V1;AA[4][2]=V1;AA[4][3]=0;AA[4][4]=V1;AA[4][5]=5;AA[4][6]=5;AA[4][7]=5;
  AA[5][0]=V1;AA[5][1]=V1;AA[5][2]=V1;AA[5][3]=V1;AA[5][4]=0;AA[5][5]=5;AA[5][6]=5;AA[5][7]=5;
}

void fastend(long int PISTEET[64]) {
  PISTEET[0]=2000;   /*0   XX_XX     4000/2  */
  PISTEET[1]=4000;   /*1	  XXX_X     4000    */
  PISTEET[2]=4000;   /*2	  XXXX_     4000    */
  PISTEET[3]=200;    /*3   00_00     400/2   */
  PISTEET[4]=400;    /*4	  000_0     400	    */
  PISTEET[5]=400;    /*5	  0000_     400	    */
}




 /* Osa 1: Suora uhka */

void taulukko2(int AA[16][8], int BB[16][8], int V1,  int V2) {
 int i, j;
 for(i=0;i<16;i++){
   for(j=0;j<8;j++){
     BB[i][j]=5;
   }
 }
 /* maalit: */
 /* xxxx_   */
 /* xxx_x   */
 /* xx_xx   */
 
 AA[0][0]=V2;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=V2;AA[0][4]=0;
 AA[1][0]=V2;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;
 AA[2][0]=V2;AA[2][1]=V2;AA[2][2]=0;AA[2][3]=V2;AA[2][4]=V2;

 /* xx_x_ -> xxoxx */
 /* xx_x_ -> xxxxo */
 /* x_x_x -> xxxox */
 /* xx__x -> xxxox */
 /* xx__x -> xxoxx */
 /* xxx__ -> xxxxo */
 /* xxx__ -> xxxox */
 /* x_xx_ -> xoxxx */
 /* x_xx_ -> xxxox */

 AA[3][0]=V2;AA[3][1]=V2;AA[3][2]=0; AA[3][3]=V2;AA[3][4]=0;
 AA[4][0]=V2;AA[4][1]=V2;AA[4][2]=0; AA[4][3]=V2;AA[4][4]=0;
 AA[5][0]=V2;AA[5][1]=0; AA[5][2]=V2;AA[5][3]=0; AA[5][4]=V2;
 AA[6][0]=V2;AA[6][1]=V2;AA[6][2]=0; AA[6][3]=0; AA[6][4]=V2;
 AA[7][0]=V2;AA[7][1]=V2;AA[7][2]=0; AA[7][3]=0; AA[7][4]=V2;
 AA[8][0]=V2;AA[8][1]=V2;AA[8][2]=V2; AA[8][3]=0; AA[8][4]=0;
 AA[9][0]=V2;AA[9][1]=V2;AA[9][2]=V2; AA[9][3]=0; AA[9][4]=0;

 AA[10][0]=V2;AA[10][1]=0;AA[10][2]=V2; AA[10][3]=V2; AA[10][4]=0;
 AA[11][0]=V2;AA[11][1]=0;AA[11][2]=V2; AA[11][3]=V2; AA[11][4]=0;
 
 BB[3][2]=V1;BB[3][4]=V2;
 BB[4][2]=V2;BB[4][4]=V1;
 BB[5][1]=V2;BB[5][3]=V1;
 BB[6][2]=V2;BB[6][3]=V1;
 BB[7][2]=V1;BB[7][3]=V2;
 BB[8][3]=V2;BB[8][4]=V1;
 BB[9][3]=V1;BB[9][4]=V2;
 BB[10][1]=V1;BB[10][4]=V2;
 BB[11][1]=V2;BB[11][4]=V1;

}



/* Osa 2: Jos vastustajallakaan ei ole suoraa uhkaa */


void taulukko3(int AA[16][8], int BB[16][8], int V1,  int V2) {
 int i, j;
 for(i=0;i<16;i++){
   for(j=0;j<8;j++){
     BB[i][j]=5;
   }
 }
/* maalit(toimiikomuka???): */
/* _xxx__ */
/* _xx_x_ */
 
 AA[0][0]=0;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=V2;AA[0][4]=0;AA[0][5]=0;
 AA[1][0]=0;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;AA[1][5]=0;

/* _xx___ -> oxxxoo  */
/* _xx___ -> oxxoxo  */
/* _x_x__ -> oxxxoo  */
/* _x_x__ -> oxoxxo  */
/* _x__x_ -> oxxoxo  */


 AA[2][0]=0;AA[2][1]=V2;AA[2][2]=V2;AA[2][3]=0; AA[2][4]=0;AA[2][5]=0;
 AA[3][0]=0;AA[3][1]=V2;AA[3][2]=V2;AA[3][3]=0; AA[3][4]=0;AA[3][5]=0;
 AA[4][0]=0;AA[4][1]=V2;AA[4][2]=0; AA[4][3]=V2;AA[4][4]=0;AA[4][5]=0;
 AA[5][0]=0;AA[5][1]=V2;AA[5][2]=0; AA[5][3]=V2;AA[5][4]=0;AA[5][5]=0;
 AA[6][0]=0;AA[6][1]=V2;AA[6][2]=0; AA[6][3]=0; AA[6][4]=V2;AA[6][5]=0;
 
 BB[2][0]=V1;BB[2][3]=V2;BB[2][4]=V1;BB[2][5]=V1;
 BB[3][0]=V1;BB[3][3]=V1;BB[3][4]=V2;BB[3][5]=V1;
 BB[4][0]=V1;BB[4][2]=V2;BB[4][4]=V1;BB[4][5]=V1;
 BB[5][0]=V1;BB[5][2]=V1;BB[5][4]=V2;BB[5][5]=V1;
 BB[6][0]=V1;BB[6][2]=V2;BB[6][3]=V1;BB[6][5]=V1;

}







/* heurastinen haku */
void taulukko4( int AA[64][8],  int V1,  int V2) {
 AA[0][0]=3;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=0;AA[0][4]=V2;AA[0][5]=5;AA[0][6]=5;AA[0][7]=5;
 AA[1][0]=V2;AA[1][1]=V2;AA[1][2]=0;AA[1][3]=V2;AA[1][4]=3;AA[1][5]=5;AA[1][6]=5;AA[1][7]=5;
 AA[2][0]=V2;AA[2][1]=V2;AA[2][2]=V2;AA[2][3]=0;AA[2][4]=3;AA[2][5]=5;AA[2][6]=5;AA[2][7]=5;
 AA[3][0]=V1;AA[3][1]=V2;AA[3][2]=V2;AA[3][3]=V2;AA[3][4]=3;AA[3][5]=0;AA[3][6]=5;AA[3][7]=5;
 AA[4][0]=V2;AA[4][1]=V2;AA[4][2]=3;AA[4][3]=V2;AA[4][4]=0;AA[4][5]=5;AA[4][6]=5;AA[4][7]=5;
 AA[5][0]=V2;AA[5][1]=3;AA[5][2]=V2;AA[5][3]=V2;AA[5][4]=0;AA[5][5]=5;AA[5][6]=5;AA[5][7]=5;
 AA[6][0]=V2;AA[6][1]=V2;AA[6][2]=0;AA[6][3]=0;AA[6][4]=V2;AA[6][5]=5;AA[6][6]=5;AA[6][7]=5;
 AA[7][0]=V2;AA[7][1]=0;AA[7][2]=V2;AA[7][3]=0;AA[7][4]=V2;AA[7][5]=5;AA[7][6]=5;AA[7][7]=5;
 AA[8][0]=V2;AA[8][1]=0;AA[8][2]=V1;AA[8][3]=V1;AA[8][4]=V1;AA[8][5]=0;AA[8][6]=0;AA[8][7]=5;
 AA[9][0]=3;AA[9][1]=0;AA[9][2]=V1;AA[9][3]=V1;AA[9][4]=V1;AA[9][5]=0;AA[9][6]=3;AA[9][7]=5;
 AA[10][0]=0;AA[10][1]=V1;AA[10][2]=0;AA[10][3]=V1;AA[10][4]=V1;AA[10][5]=0;AA[10][6]=5;AA[10][7]=5;
 AA[11][0]=3;AA[11][1]=3;AA[11][2]=V1;AA[11][3]=0;AA[11][4]=V1;AA[11][5]=V1;AA[11][6]=3;AA[11][7]=3;
 AA[12][0]=3;AA[12][1]=3;AA[12][2]=V1;AA[12][3]=V1;AA[12][4]=0;AA[12][5]=V1;AA[12][6]=3;AA[12][7]=3;
 AA[13][0]=V2;AA[13][1]=3;AA[13][2]=V1;AA[13][3]=V1;AA[13][4]=V1;AA[13][5]=0;AA[13][6]=V2;AA[13][7]=3;
 AA[14][0]=3;AA[14][1]=V2;AA[14][2]=V2;AA[14][3]=0;AA[14][4]=3;AA[14][5]=5;AA[14][6]=5;AA[14][7]=5;
 AA[15][0]=3;AA[15][1]=V2;AA[15][2]=0;AA[15][3]=V2;AA[15][4]=3;AA[15][5]=5;AA[15][6]=5;AA[15][7]=5;
 AA[16][0]=3;AA[16][1]=V2;AA[16][2]=0;AA[16][3]=0;AA[16][4]=V2;AA[16][5]=3;AA[16][6]=5;AA[16][7]=5;
 AA[17][0]=3;AA[17][1]=V2;AA[17][2]=3;AA[17][3]=V2;AA[17][4]=0;AA[17][5]=3;AA[17][6]=5;AA[17][7]=5;
 AA[18][0]=3;AA[18][1]=V2;AA[18][2]=V2;AA[18][3]=3;AA[18][4]=0;AA[18][5]=3;AA[18][6]=5;AA[18][7]=5;
 AA[19][0]=V2;AA[19][1]=V1;AA[19][2]=V1;AA[19][3]=V1;AA[19][4]=0;AA[19][5]=0;AA[19][6]=5;AA[19][7]=5;
 AA[20][0]=V2;AA[20][1]=V1;AA[20][2]=V1;AA[20][3]=0;AA[20][4]=V1;AA[20][5]=0;AA[20][6]=5;AA[20][7]=5;
 AA[21][0]=V2;AA[21][1]=V1;AA[21][2]=0;AA[21][3]=V1;AA[21][4]=V1;AA[21][5]=0;AA[21][6]=5;AA[21][7]=5;
 AA[22][0]=V2;AA[22][1]=0;AA[22][2]=V1;AA[22][3]=V1;AA[22][4]=V1;AA[22][5]=0;AA[22][6]=5;AA[22][7]=5;
 AA[23][0]=3;AA[23][1]=V1;AA[23][2]=V1;AA[23][3]=0;AA[23][4]=3;AA[23][5]=5;AA[23][6]=5;AA[23][7]=5;
 AA[24][0]=3;AA[24][1]=V1;AA[24][2]=0;AA[24][3]=V1;AA[24][4]=3;AA[24][5]=5;AA[24][6]=5;AA[24][7]=5;
 AA[25][0]=3;AA[25][1]=V1;AA[25][2]=0;AA[25][3]=0;AA[25][4]=V1;AA[25][5]=3;AA[25][6]=5;AA[25][7]=5;
 AA[26][0]=3;AA[26][1]=V1;AA[26][2]=3;AA[26][3]=V1;AA[26][4]=0;AA[26][5]=3;AA[26][6]=5;AA[26][7]=5;
 AA[27][0]=3;AA[27][1]=V1;AA[27][2]=V1;AA[27][3]=3;AA[27][4]=0;AA[27][5]=3;AA[27][6]=5;AA[27][7]=5;
 AA[28][0]=3;AA[28][1]=V2;AA[28][2]=V2;AA[28][3]=3;AA[28][4]=3;AA[28][5]=0;AA[28][6]=3;AA[28][7]=5;
 AA[29][0]=3;AA[29][1]=V2;AA[29][2]=0;AA[29][3]=3;AA[29][4]=5;AA[29][5]=5;AA[29][6]=5;AA[29][7]=5;
 AA[30][0]=3;AA[30][1]=V2;AA[30][2]=3;AA[30][3]=0;AA[30][4]=3;AA[30][5]=5;AA[30][6]=5;AA[30][7]=5;
 AA[31][0]=3;AA[31][1]=V1;AA[31][2]=0;AA[31][3]=3;AA[31][4]=5;AA[31][5]=5;AA[31][6]=5;AA[31][7]=5;
 AA[32][0]=3;AA[32][1]=V1;AA[32][2]=3;AA[32][3]=0;AA[32][4]=3;AA[32][5]=5;AA[32][6]=5;AA[32][7]=5;
 AA[33][0]=3;AA[33][1]=V2;AA[33][2]=3;AA[33][3]=0;AA[33][4]=3;AA[33][5]=V2;AA[33][6]=3;AA[33][7]=5;
 AA[34][0]=3;AA[34][1]=V2;AA[34][2]=3;AA[34][3]=3;AA[34][4]=0;AA[34][5]=V2;AA[34][6]=3;AA[34][7]=5;
 AA[35][0]=3;AA[35][1]=V2;AA[35][2]=3;AA[35][3]=3;AA[35][4]=0;AA[35][5]=3;AA[35][6]=5;AA[35][7]=5;
 AA[36][0]=3;AA[36][1]=V1;AA[36][2]=V1;AA[36][3]=3;AA[36][4]=3;AA[36][5]=0;AA[36][6]=3;AA[36][7]=5;
 AA[37][0]=3;AA[37][1]=V1;AA[37][2]=3;AA[37][3]=0;AA[37][4]=3;AA[37][5]=V1;AA[37][6]=3;AA[37][7]=5;
 AA[38][0]=3;AA[38][1]=V1;AA[38][2]=3;AA[38][3]=3;AA[38][4]=0;AA[38][5]=V1;AA[38][6]=3;AA[38][7]=5;
 AA[39][0]=V1;AA[39][1]=0;AA[39][2]=V1;AA[39][3]=0;AA[39][4]=V1;AA[39][5]=0;AA[39][6]=V1;AA[39][7]=5;
}

 		/* LAAJENNETUT KAAVAT: */
      /* Nämä kertovat kuinka tärkeä on kukin paikka.                */
      /*   _ = Tyhjä paikka johon laitetaan.                         */
      /*   * = Tyhjä paikka, mutta ei laiteta.                       */
      /*   X = Tietokoneen nappula.                                  */
      /*   0 = Pelaajan nappula.                                     */
      /* HUOM! Edellisessä koodissa käytettiin 75 eri kaavaa.        */
      /* Tässä on 45, mutta engine osaa nyt myös peilikuvat,         */
      /* oikeasti kaavoja on 84.                                     */


void attacker(long int PISTEET[64]) {
 PISTEET[0]=2000; 	/*6 	  *XX_X      5000		*/
 PISTEET[1]=2000; 	/*7 	  XX_X*      5000		*/
 PISTEET[2]=2000; 	/*8 	  XXX_*      5000		*/
 PISTEET[3]=2000; 	/*9 	  0XXX*_     5000		*/
 PISTEET[4]=2000; 	/*10	  XX*X_      3000		*/
 PISTEET[5]=2000; 	/*11	  X*XX_      3000		*/
 PISTEET[6]=1900; 	/*12	  XX__X      2900		*/
 PISTEET[7]=1900;      /*13      X_X_X      2900               */

 PISTEET[8]=4000; 	/*14	  X_000__    2000		*/
 PISTEET[9]=4000;      /*15      *_000_*    2000               */
 PISTEET[10]=4000; 	/*16	  _0_00_     2000		*/
 PISTEET[11]=30;        /*17      **0_00**    30                */
 PISTEET[12]=30;        /*18      **00_0**    30                */
 PISTEET[13]=30;        /*19      X*000_*     30                */
 
 PISTEET[14]=500;  	/*20	  *XX_*       500		*/
 PISTEET[15]=270;       /*21      *X_X*       500/2             */
 PISTEET[16]=490; 	/*22	  *X__X*      490		*/
 PISTEET[17]=490;       /*23      *X*X_*      490               */
 PISTEET[18]=490;       /*24      *XX*_*      490               */
 PISTEET[19]=350;       /*25      X000__      350               */
 PISTEET[20]=350;       /*26      X00_0_      350               */
 PISTEET[21]=350;       /*27      X0_00_      350               */
 PISTEET[22]=350;       /*28      X_000_      350               */
 PISTEET[23]=200;       /*29      *OO_*       200               */
 PISTEET[24]=200;       /*30      *O_O*       200               */
 PISTEET[25]=190;       /*31      *O__O*      190               */
 PISTEET[26]=190;       /*32      *O*O_*      190               */
 PISTEET[27]=80;        /*33      *OO*_*      80                */
 PISTEET[28]=80;        /*34      *XX**_*     80                */
 PISTEET[29]=60;        /*35      *X_*        60                */
 PISTEET[30]=50;        /*36      *X*_*       50                */
 PISTEET[31]=40;        /*37      *O_*        40                */
 PISTEET[32]=25;        /*38      *O*_*       25                */
 PISTEET[33]=9;         /*39      *X*_*X*     16/2              */
 PISTEET[34]=10;        /*40      *X**_X*     10                */
 PISTEET[35]=10;        /*41      *X**_*      10                */
 PISTEET[36]=10;        /*42      *OO**_*     10                */
 PISTEET[37]=4;         /*43      *0*_*0*      6/2              */
 PISTEET[38]=2;		/*44	  *0**_0*      2 		*/
 PISTEET[39]=2000;      /*45      0_0_O_O   14000/2             */
}

      /* Tallennan noi tiedostoihin ja sit lataus sitä kautta tms...     */
      /* Niin antaa käyttäjän muuttaa arvoja.                            */
      /* Ja koko tän pitkän jutun voi korvata yhdellä pienellä for:illa. */
void defender(long int PISTEET[64]) {
 PISTEET[0]=1500; 	/*6 	  *XX_X      2500		*/
 PISTEET[1]=1500; 	/*7 	  XX_X*      2500		*/
 PISTEET[2]=1500; 	/*8 	  XXX_*      2500		*/
 PISTEET[3]=1450; 	/*9 	  0XXX*_     2450		*/
 PISTEET[4]=1400; 	/*10	  XX*X_      2400		*/
 PISTEET[5]=1400; 	/*11	  X*XX_      2400		*/
 PISTEET[6]=1000;      /*12      XX__X      2000               */
 PISTEET[7]=1000;      /*13      X_X_X      2000               */
 PISTEET[8]=2200; 	/*14	  X_000__    2200		*/
 PISTEET[9]=2200;	/*15	  *_000_*    2200		*/
 PISTEET[10]=2200; 	/*16	  _0_00_     2200		*/
 PISTEET[11]=80;        /*17      **0_00**    80                */
 PISTEET[12]=80;        /*18      **00_0**    80                */
 PISTEET[13]=80;        /*19      X*000_*     80                */
 PISTEET[14]=400;       /*20      *XX_*       400               */
 PISTEET[15]=220;       /*21      *X_X*       400/2             */
 PISTEET[16]=385; 	/*22	  *X__X*      385		*/
 PISTEET[17]=385;       /*23      *X*X_*      385               */
 PISTEET[18]=385;       /*24      *XX*_*      385               */
 PISTEET[19]=410;       /*25      X000__      410               */
 PISTEET[20]=400;       /*26      X00_0_      400               */
 PISTEET[21]=400;       /*27      X0_00_      400               */
 PISTEET[22]=400;       /*28      X_000_      400               */
 PISTEET[23]=385;       /*29      *OO_*       385               */
 PISTEET[24]=198;       /*30      *O_O*       390/2             */
 PISTEET[25]=380; 	/*31	  *O__O*      380		*/
 PISTEET[26]=380;       /*32      *O*O_*      380               */
 PISTEET[27]=90;        /*33      *OO*_*      90                */
 PISTEET[28]=80;        /*34      *XX**_*     80                */
 PISTEET[29]=30;        /*35      *X_*        30                */
 PISTEET[30]=15;        /*36      *X*_*       15                */
 PISTEET[31]=45;        /*37      *O_*        45                */
 PISTEET[32]=35;        /*38      *O*_*       35                */
 PISTEET[33]=8;         /*39      *X*_*X*     14/2              */
 PISTEET[34]=10;        /*40      *X**_X*     10                */
 PISTEET[35]=10;        /*41      *X**_*      10                */
 PISTEET[36]=10;        /*42      *OO**_*     10                */
 PISTEET[37]=4;         /*43      *0*_*0*      6/2              */
 PISTEET[38]=4;		/*44	  *0**_0*      4 		*/
 PISTEET[39]=2000;      /*45      0_0_O_O   14000/2             */
}

void preasure(long int PISTEET[64]) {
 PISTEET[0]=1700; 	/*6 	  *XX_X      2700		*/
 PISTEET[1]=1700; 	/*7 	  XX_X*      2700		*/
 PISTEET[2]=1700; 	/*8 	  XXX_*      2700		*/
 PISTEET[3]=1600; 	/*9 	  0XXX*_     2600		*/
 PISTEET[4]=1700; 	/*10	  XX*X_      2700		*/
 PISTEET[5]=1700; 	/*11	  X*XX_      2700		*/
 PISTEET[6]=1700;      /*12      XX__X      2700               */
 PISTEET[7]=1700;      /*13      X_X_X      2700               */
 PISTEET[8]=2000; 	/*14	  X_000__    2000		*/
 PISTEET[9]=2000;      /*15      *_000_*    2000               */
 PISTEET[10]=2000; 	/*16	  _0_00_     2000		*/
 PISTEET[11]=50;        /*17      **0_00**    50                */
 PISTEET[12]=50;        /*18      **00_0**    50                */
 PISTEET[13]=50;        /*19      X*000_*     50                */
 PISTEET[14]=500;       /*20      *XX_*       500               */
 PISTEET[15]=270;       /*21      *X_X*       500/2             */
 PISTEET[16]=490;       /*22      *X__X*      490               */
 PISTEET[17]=490;       /*23      *X*X_*      490               */
 PISTEET[18]=490;       /*24      *XX*_*      490               */
 PISTEET[19]=480;       /*25      X000__      480               */
 PISTEET[20]=380;       /*26      X00_0_      380               */
 PISTEET[21]=380;       /*27      X0_00_      380               */
 PISTEET[22]=380;       /*28      X_000_      380               */
 PISTEET[23]=240;       /*29      *OO_*       240               */
 PISTEET[24]=130;       /*30      *O_O*       240/2             */
 PISTEET[25]=220;       /*31      *O__O*      220               */
 PISTEET[26]=210;       /*32      *O*O_*      210               */
 PISTEET[27]=90;        /*33      *OO*_*      90                */
 PISTEET[28]=100;       /*34      *XX**_*     100               */
 PISTEET[29]=80;        /*35      *X_*        80                */
 PISTEET[30]=50;        /*36      *X*_*       50                */
 PISTEET[31]=30;        /*37      *O_*        30                */
 PISTEET[32]=25;        /*38      *O*_*       25                */
 PISTEET[33]=9;         /*39      *X*_*X*     16/2              */
 PISTEET[34]=10;        /*40      *X**_X*     10                */
 PISTEET[35]=10;        /*41      *X**_*      10                */
 PISTEET[36]=10;        /*42      *OO**_*     10                */
 PISTEET[37]=3;         /*43      *0*_*0*      4/2              */
 PISTEET[38]=2;		/*44	  *0**_0*      2 		*/
 PISTEET[39]=2000;      /*45      0_0_O_O   14000/2             */
}

/* 2 siirron laskeminen eteenpäin olisi välttämätöntä                  */
/* ansojen rakentamiseen ja torjumiseen. Eräässä väliversiossa         */
/* koodi laski 2 eri matriisia, Priority ja toinen, joka lisättiin     */
/* priorityyn. Se oli oikotie, mutta poistin sen ja toteutan sen       */
/* joskus oikeasti, pyörittämällä tämän koodin 2 kertaa peräkkäin...   */

void tbuilder(long int PISTEET[64]) {
 PISTEET[0]=1000; 	/*6 	  *XX_X      5000		*/
 PISTEET[1]=1000; 	/*7 	  XX_X*      5000		*/
 PISTEET[2]=950; 	/*8 	  XXX_*      4950		*/
 PISTEET[3]=1000; 	/*9 	  0XXX*_     5000		*/
 PISTEET[4]=1000; 	/*10	  XX*X_      3000		*/
 PISTEET[5]=1000; 	/*11	  X*XX_      4000		*/
 PISTEET[6]=900;      /*12      XX__X      2900               */
 PISTEET[7]=900; 	/*13	  X_X_X      2900		*/
 PISTEET[8]=2020; 	/*14	  X_000__    2020		*/
 PISTEET[9]=2000; 	/*15	  *_000_*    2000		*/
 PISTEET[10]=2000; 	/*16	  _0_00_     2000		*/
 PISTEET[11]=10;        /*17      **0_00**    10                */
 PISTEET[12]=10;        /*18      **00_0**    10                */
 PISTEET[13]=10;        /*19      X*000_*     10                */
 PISTEET[14]=420;       /*20      *XX_*       362               */
 PISTEET[15]=211;       /*21      *X_X*       360/2             */
 PISTEET[16]=490;       /*22      *X__X*      490               */
 PISTEET[17]=490;       /*23      *X*X_*      490               */
 PISTEET[18]=490;       /*24      *XX*_*      490               */
 PISTEET[19]=350;       /*25      X000__      350               */
 PISTEET[20]=350;       /*26      X00_0_      350               */
 PISTEET[21]=350;       /*27      X0_00_      350               */
 PISTEET[22]=350;       /*28      X_000_      350               */
 PISTEET[23]=200;       /*29      *OO_*       200               */
 PISTEET[24]=102;       /*30      *O_O*       200/2             */
 PISTEET[25]=110;       /*31      *O__O*      110               */
 PISTEET[26]=140;       /*32      *O*O_*      140               */
 PISTEET[27]=80;        /*33      *OO*_*      80                */
 PISTEET[28]=90;        /*34      *XX**_*     90                */
 PISTEET[29]=50;        /*35      *X_*        50                */
 PISTEET[30]=35;        /*36      *X*_*       35                */
 PISTEET[31]=34;        /*37      *O_*        34                */
 PISTEET[32]=26;        /*38      *O*_*       26                */
 PISTEET[33]=14;        /*39      *X*_*X*     16/2              */
 PISTEET[34]=23;        /*40      *X**_X*     23                */
 PISTEET[35]=23;        /*41      *X**_*      23                */
 PISTEET[36]=12;        /*42      *OO**_*     12                */
 PISTEET[37]=4;         /*43      *0*_*0*      6/2              */
 PISTEET[38]=3;		/*44	  *0**_0*      3 		*/
 PISTEET[39]=2000;      /*45      0_0_O_O   14000/2             */
}


void haepriority(int syvyys, int rivi0, int suunta0, int X, int Y, long int PRIORITY[64][64]){
  if(suunta0==0){
    if(rivi0==3)if(PRIORITY[X+4][Y]<(9500-syvyys*1000))PRIORITY[X+4][Y]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y]<(9500-syvyys*1000))PRIORITY[X+2][Y]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+1][Y]<(9500-syvyys*1000))PRIORITY[X+1][Y]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y]<(9500-syvyys*1000))PRIORITY[X+2][Y]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X+3][Y]<(9500-syvyys*1000))PRIORITY[X+3][Y]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X+3][Y]<(9500-syvyys*1000))PRIORITY[X+3][Y]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X+4][Y]<(9500-syvyys*1000))PRIORITY[X+4][Y]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X+4][Y]<(9500-syvyys*1000))PRIORITY[X+4][Y]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X+1][Y]<(9500-syvyys*1000))PRIORITY[X+1][Y]=9500-syvyys*1000;
  }
  if(suunta0==1){
    if(rivi0==3)if(PRIORITY[X][Y+4]<(9500-syvyys*1000))PRIORITY[X][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X][Y+2]<(9500-syvyys*1000))PRIORITY[X][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X][Y+1]<(9500-syvyys*1000))PRIORITY[X][Y+1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X][Y+2]<(9500-syvyys*1000))PRIORITY[X][Y+2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X][Y+3]<(9500-syvyys*1000))PRIORITY[X][Y+3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X][Y+3]<(9500-syvyys*1000))PRIORITY[X][Y+3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X][Y+4]<(9500-syvyys*1000))PRIORITY[X][Y+4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X][Y+4]<(9500-syvyys*1000))PRIORITY[X][Y+4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X][Y+1]<(9500-syvyys*1000))PRIORITY[X][Y+1]=9500-syvyys*1000;
  }
  if(suunta0==2){
    if(rivi0==3)if(PRIORITY[X+4][Y+4]<(9500-syvyys*1000))PRIORITY[X+4][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y+2]<(9500-syvyys*1000))PRIORITY[X+2][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+1][Y+1]<(9500-syvyys*1000))PRIORITY[X+1][Y+1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y+2]<(9500-syvyys*1000))PRIORITY[X+2][Y+2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X+3][Y+3]<(9500-syvyys*1000))PRIORITY[X+3][Y+3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X+3][Y+3]<(9500-syvyys*1000))PRIORITY[X+3][Y+3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X+4][Y+4]<(9500-syvyys*1000))PRIORITY[X+4][Y+4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X+4][Y+4]<(9500-syvyys*1000))PRIORITY[X+4][Y+4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X+1][Y+1]<(9500-syvyys*1000))PRIORITY[X+1][Y+1]=9500-syvyys*1000;
  }
  if(suunta0==3){
    if(rivi0==3)if(PRIORITY[X+4][Y-4]<(9500-syvyys*1000))PRIORITY[X+4][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y-2]<(9500-syvyys*1000))PRIORITY[X+2][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+1][Y-1]<(9500-syvyys*1000))PRIORITY[X+1][Y-1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y-2]<(9500-syvyys*1000))PRIORITY[X+2][Y-2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X+3][Y-3]<(9500-syvyys*1000))PRIORITY[X+3][Y-3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X+3][Y-3]<(9500-syvyys*1000))PRIORITY[X+3][Y-3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X+4][Y-4]<(9500-syvyys*1000))PRIORITY[X+4][Y-4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X+4][Y-4]<(9500-syvyys*1000))PRIORITY[X+4][Y-4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X+1][Y-1]<(9500-syvyys*1000))PRIORITY[X+1][Y-1]=9500-syvyys*1000;
  }
  if(suunta0==4){
    if(rivi0==3)if(PRIORITY[X-4][Y]<(9500-syvyys*1000))PRIORITY[X-4][Y]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y]<(9500-syvyys*1000))PRIORITY[X-2][Y]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-1][Y]<(9500-syvyys*1000))PRIORITY[X-1][Y]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y]<(9500-syvyys*1000))PRIORITY[X-2][Y]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X-3][Y]<(9500-syvyys*1000))PRIORITY[X-3][Y]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X-3][Y]<(9500-syvyys*1000))PRIORITY[X-3][Y]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X-4][Y]<(9500-syvyys*1000))PRIORITY[X-4][Y]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X-4][Y]<(9500-syvyys*1000))PRIORITY[X-4][Y]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X-1][Y]<(9500-syvyys*1000))PRIORITY[X-1][Y]=9500-syvyys*1000;
  }
  if(suunta0==5){
    if(rivi0==3)if(PRIORITY[X][Y-4]<(9500-syvyys*1000))PRIORITY[X][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X][Y-2]<(9500-syvyys*1000))PRIORITY[X][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X][Y-1]<(9500-syvyys*1000))PRIORITY[X][Y-1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X][Y-2]<(9500-syvyys*1000))PRIORITY[X][Y-2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X][Y-3]<(9500-syvyys*1000))PRIORITY[X][Y-3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X][Y-3]<(9500-syvyys*1000))PRIORITY[X][Y-3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X][Y-4]<(9500-syvyys*1000))PRIORITY[X][Y-4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X][Y-4]<(9500-syvyys*1000))PRIORITY[X][Y-4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X][Y-1]<(9500-syvyys*1000))PRIORITY[X][Y-1]=9500-syvyys*1000;
  }
  if(suunta0==6){
    if(rivi0==3)if(PRIORITY[X-4][Y-4]<(9500-syvyys*1000))PRIORITY[X-4][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y-2]<(9500-syvyys*1000))PRIORITY[X-2][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-1][Y-1]<(9500-syvyys*1000))PRIORITY[X-1][Y-1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y-2]<(9500-syvyys*1000))PRIORITY[X-2][Y-2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X-3][Y-3]<(9500-syvyys*1000))PRIORITY[X-3][Y-3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X-3][Y-3]<(9500-syvyys*1000))PRIORITY[X-3][Y-3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X-4][Y-4]<(9500-syvyys*1000))PRIORITY[X-4][Y-4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X-4][Y-4]<(9500-syvyys*1000))PRIORITY[X-4][Y-4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X-1][Y-1]<(9500-syvyys*1000))PRIORITY[X-1][Y-1]=9500-syvyys*1000;
  }
  if(suunta0==7){
    if(rivi0==3)if(PRIORITY[X-4][Y+4]<(9500-syvyys*1000))PRIORITY[X-4][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y+2]<(9500-syvyys*1000))PRIORITY[X-2][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-1][Y+1]<(9500-syvyys*1000))PRIORITY[X-1][Y+1]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y+2]<(9500-syvyys*1000))PRIORITY[X-2][Y+2]=9500-syvyys*1000;
    if(rivi0==7)if(PRIORITY[X-3][Y+3]<(9500-syvyys*1000))PRIORITY[X-3][Y+3]=9500-syvyys*1000;
    if(rivi0==8)if(PRIORITY[X-3][Y+3]<(9500-syvyys*1000))PRIORITY[X-3][Y+3]=9500-syvyys*1000;
    if(rivi0==9)if(PRIORITY[X-4][Y+4]<(9500-syvyys*1000))PRIORITY[X-4][Y+4]=9500-syvyys*1000;
    if(rivi0==10)if(PRIORITY[X-4][Y+4]<(9500-syvyys*1000))PRIORITY[X-4][Y+4]=9500-syvyys*1000;
    if(rivi0==11)if(PRIORITY[X-1][Y+1]<(9500-syvyys*1000))PRIORITY[X-1][Y+1]=9500-syvyys*1000;
  }
}


void haepriority2(int syvyys, int rivi0, int suunta0, int X, int Y, long int PRIORITY[64][64]){
  if(suunta0==0){
    if(rivi0==2)if(PRIORITY[X+3][Y]<(9500-syvyys*1000))PRIORITY[X+3][Y]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X+4][Y]<(9500-syvyys*1000))PRIORITY[X+4][Y]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y]<(9500-syvyys*1000))PRIORITY[X+2][Y]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+4][Y]<(9500-syvyys*1000))PRIORITY[X+4][Y]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y]<(9500-syvyys*1000))PRIORITY[X+2][Y]=9500-syvyys*1000;
  }
  if(suunta0==1){
    if(rivi0==2)if(PRIORITY[X][Y+3]<(9500-syvyys*1000))PRIORITY[X][Y+3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X][Y+4]<(9500-syvyys*1000))PRIORITY[X][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X][Y+2]<(9500-syvyys*1000))PRIORITY[X][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X][Y+4]<(9500-syvyys*1000))PRIORITY[X][Y+4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X][Y+2]<(9500-syvyys*1000))PRIORITY[X][Y+2]=9500-syvyys*1000;
  }
  if(suunta0==2){
    if(rivi0==2)if(PRIORITY[X+3][Y+3]<(9500-syvyys*1000))PRIORITY[X+3][Y+3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X+4][Y+4]<(9500-syvyys*1000))PRIORITY[X+4][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y+2]<(9500-syvyys*1000))PRIORITY[X+2][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+4][Y+4]<(9500-syvyys*1000))PRIORITY[X+4][Y+4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y+2]<(9500-syvyys*1000))PRIORITY[X+2][Y+2]=9500-syvyys*1000;
  }
  if(suunta0==3){
    if(rivi0==2)if(PRIORITY[X+3][Y-3]<(9500-syvyys*1000))PRIORITY[X+3][Y-3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X+4][Y-4]<(9500-syvyys*1000))PRIORITY[X+4][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X+2][Y-2]<(9500-syvyys*1000))PRIORITY[X+2][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X+4][Y-4]<(9500-syvyys*1000))PRIORITY[X+4][Y-4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X+2][Y-2]<(9500-syvyys*1000))PRIORITY[X+2][Y-2]=9500-syvyys*1000;
  }
  if(suunta0==4){
    if(rivi0==2)if(PRIORITY[X-3][Y]<(9500-syvyys*1000))PRIORITY[X-3][Y]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X-4][Y]<(9500-syvyys*1000))PRIORITY[X-4][Y]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y]<(9500-syvyys*1000))PRIORITY[X-2][Y]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-4][Y]<(9500-syvyys*1000))PRIORITY[X-4][Y]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y]<(9500-syvyys*1000))PRIORITY[X-2][Y]=9500-syvyys*1000;
  }
  if(suunta0==5){
    if(rivi0==2)if(PRIORITY[X][Y-3]<(9500-syvyys*1000))PRIORITY[X][Y-3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X][Y-4]<(9500-syvyys*1000))PRIORITY[X][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X][Y-2]<(9500-syvyys*1000))PRIORITY[X][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X][Y-4]<(9500-syvyys*1000))PRIORITY[X][Y-4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X][Y-2]<(9500-syvyys*1000))PRIORITY[X][Y-2]=9500-syvyys*1000;
  }
  if(suunta0==6){
    if(rivi0==2)if(PRIORITY[X-3][Y-3]<(9500-syvyys*1000))PRIORITY[X-3][Y-3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X-4][Y-4]<(9500-syvyys*1000))PRIORITY[X-4][Y-4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y-2]<(9500-syvyys*1000))PRIORITY[X-2][Y-2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-4][Y-4]<(9500-syvyys*1000))PRIORITY[X-4][Y-4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y-2]<(9500-syvyys*1000))PRIORITY[X-2][Y-2]=9500-syvyys*1000;
  }
  if(suunta0==7){
    if(rivi0==2)if(PRIORITY[X-3][Y+3]<(9500-syvyys*1000))PRIORITY[X-3][Y+3]=9500-syvyys*1000;
    if(rivi0==3)if(PRIORITY[X-4][Y+4]<(9500-syvyys*1000))PRIORITY[X-4][Y+4]=9500-syvyys*1000;
    if(rivi0==4)if(PRIORITY[X-2][Y+2]<(9500-syvyys*1000))PRIORITY[X-2][Y+2]=9500-syvyys*1000;
    if(rivi0==5)if(PRIORITY[X-4][Y+4]<(9500-syvyys*1000))PRIORITY[X-4][Y+4]=9500-syvyys*1000;
    if(rivi0==6)if(PRIORITY[X-2][Y+2]<(9500-syvyys*1000))PRIORITY[X-2][Y+2]=9500-syvyys*1000;
  }
}





int pikahaku(int B[64][64],int V2, int haettava){
  int AA[4][8], rivi, C, D, lkm=0, pituus=5;
  if(haettava==1){
	lkm=3;pituus=5;
    AA[0][0]=V2;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=V2;AA[0][4]=0;
    AA[1][0]=V2;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;
    AA[2][0]=V2;AA[2][1]=V2;AA[2][2]=0;AA[2][3]=V2;AA[2][4]=V2;
  }
  if(haettava==2){
    lkm=2;pituus=6;
	AA[0][0]=0;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=V2;AA[0][4]=0;AA[0][5]=0;
    AA[1][0]=0;AA[1][1]=V2;AA[1][2]=V2;AA[1][3]=0;AA[1][4]=V2;AA[1][5]=0;
/*    AA[0][0]=V2;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=0;AA[0][4]=0; */
  }
  for(rivi=0;rivi<lkm;rivi++){
    for(D=8;D<laudankoko+8;D++){
      for(C=8;C<laudankoko+8;C++){
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D]==AA[rivi][1]){
            if (B[C+2][D]==AA[rivi][2]){
              if (B[C+3][D]==AA[rivi][3]){
                if (B[C+4][D]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C+5][D]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D+1]==AA[rivi][1]){
            if (B[C][D+2]==AA[rivi][2]){
              if (B[C][D+3]==AA[rivi][3]){
                if (B[C][D+4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C][D+5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D+1]==AA[rivi][1]){
            if (B[C+2][D+2]==AA[rivi][2]){
              if (B[C+3][D+3]==AA[rivi][3]){
                if (B[C+4][D+4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C+5][D+5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D-1]==AA[rivi][1]){
            if (B[C+2][D-2]==AA[rivi][2]){
              if (B[C+3][D-3]==AA[rivi][3]){
                if (B[C+4][D-4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C+5][D-5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D]==AA[rivi][1]){
            if (B[C-2][D]==AA[rivi][2]){
              if (B[C-3][D]==AA[rivi][3]){
                if (B[C-4][D]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C-5][D]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D-1]==AA[rivi][1]){
            if (B[C][D-2]==AA[rivi][2]){
              if (B[C][D-3]==AA[rivi][3]){
                if (B[C][D-4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C][D-5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D-1]==AA[rivi][1]){
            if (B[C-2][D-2]==AA[rivi][2]){
              if (B[C-3][D-3]==AA[rivi][3]){
                if (B[C-4][D-4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C-5][D-5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D+1]==AA[rivi][1]){
            if (B[C-2][D+2]==AA[rivi][2]){
              if (B[C-3][D+3]==AA[rivi][3]){
                if (B[C-4][D+4]==AA[rivi][4]){
                  if(pituus==5)return 9;
                  if (B[C-5][D+5]==AA[rivi][5])return 9;
                }
              }
            }
          }
        }

      }
    }
  }
  return 0;
}


int torjuntahaku(int B[64][64],int V2, long int PRIORITY[64][64]){
  int AA[4][8], rivi, C, D, lkm, uhka; /*, pituus; */
  lkm=1;/*pituus=6;*/
	AA[0][0]=0;AA[0][1]=V2;AA[0][2]=V2;AA[0][3]=V2;AA[0][4]=0;AA[0][5]=0;
  for(rivi=0;rivi<lkm;rivi++){
    for(D=8;D<laudankoko+8;D++){
      for(C=8;C<laudankoko+8;C++){
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D]==AA[rivi][1]){
            if (B[C+2][D]==AA[rivi][2]){
              if (B[C+3][D]==AA[rivi][3]){
                if (B[C+4][D]==AA[rivi][4]){
				        	if (B[C+5][D]==AA[rivi][5]){
                    B[C+4][D]=V2;
                    B[C+5][D]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C+4][D]=0;
                    B[C+5][D]=0;
                    if(uhka==9)PRIORITY[C+4][D]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D+1]==AA[rivi][1]){
            if (B[C][D+2]==AA[rivi][2]){
              if (B[C][D+3]==AA[rivi][3]){
                if (B[C][D+4]==AA[rivi][4]){
			         		if (B[C][D+5]==AA[rivi][5]){
                    B[C][D+4]=V2;
                    B[C][D+5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C][D+4]=0;
                    B[C][D+5]=0;
                    if(uhka==9)PRIORITY[C][D+4]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D+1]==AA[rivi][1]){
            if (B[C+2][D+2]==AA[rivi][2]){
              if (B[C+3][D+3]==AA[rivi][3]){
                if (B[C+4][D+4]==AA[rivi][4]){
			         		if (B[C+5][D+5]==AA[rivi][5]){
                    B[C+4][D+4]=V2;
                    B[C+5][D+5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C+4][D+4]=0;
                    B[C+5][D+5]=0;
                    if(uhka==9)PRIORITY[C+4][D+4]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D-1]==AA[rivi][1]){
            if (B[C+2][D-2]==AA[rivi][2]){
              if (B[C+3][D-3]==AA[rivi][3]){
                if (B[C+4][D-4]==AA[rivi][4]){
			        		if (B[C+5][D-5]==AA[rivi][5]){
                    B[C+4][D-4]=V2;
                    B[C+5][D-5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C+4][D-4]=0;
                    B[C+5][D-5]=0;
                    if(uhka==9)PRIORITY[C+4][D-4]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D]==AA[rivi][1]){
            if (B[C-2][D]==AA[rivi][2]){
              if (B[C-3][D]==AA[rivi][3]){
                if (B[C-4][D]==AA[rivi][4]){
		         			if (B[C-5][D]==AA[rivi][5]){
                    B[C-4][D]=V2;
                    B[C-5][D]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C-4][D]=0;
                    B[C-5][D]=0;
                    if(uhka==9)PRIORITY[C-4][D]=1000;
                  
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D-1]==AA[rivi][1]){
            if (B[C][D-2]==AA[rivi][2]){
              if (B[C][D-3]==AA[rivi][3]){
                if (B[C][D-4]==AA[rivi][4]){
		        			if (B[C][D-5]==AA[rivi][5]){
                    B[C][D-4]=V2;
                    B[C][D-5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C][D-4]=0;
                    B[C][D-5]=0;
                    if(uhka==9)PRIORITY[C][D-4]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D-1]==AA[rivi][1]){
            if (B[C-2][D-2]==AA[rivi][2]){
              if (B[C-3][D-3]==AA[rivi][3]){
                if (B[C-4][D-4]==AA[rivi][4]){
		        			if (B[C-5][D-5]==AA[rivi][5]){
                    B[C-4][D-4]=V2;
                    B[C-5][D-5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C-4][D-4]=0;
                    B[C-5][D-5]=0;
                    if(uhka==9)PRIORITY[C-4][D-4]=1000;
                  }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D+1]==AA[rivi][1]){
            if (B[C-2][D+2]==AA[rivi][2]){
              if (B[C-3][D+3]==AA[rivi][3]){
                if (B[C-4][D+4]==AA[rivi][4]){
		         			if (B[C-5][D+5]==AA[rivi][5]){
                    B[C-4][D+4]=V2;
                    B[C-5][D+5]=V2;
                    uhka=pikahaku(B, V2, 2);
                    B[C-4][D+4]=0;
                    B[C-5][D+5]=0;
                    if(uhka==9)PRIORITY[C-4][D+4]=1000;
                  }
                }
              }
            }
          }
        }

      }
    }
  }
  return 0;
}







int rekursiohaku(int syvyys, int syvyysmax, int AA[16][8], int BB[16][8], int B[64][64], long int PRIORITY[64][64], int rivi0, int suunta0, int X, int Y, int puoli2){

  int C, D, i, voitto=0; /*, j;*/
  int rivi; /*, loyty=0;*/

  if(syvyys>syvyysmax) return 0;

/*  j=0; */
  for(rivi=0;rivi<12;rivi++){
    if(syvyys==0)rivi0=rivi;
    for(D=8;D<laudankoko+8;D++){
      if(syvyys==0)Y=D;
      for(C=8;C<laudankoko+8;C++){
	    	if(syvyys==0)X=C;
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D]==AA[rivi][1]){
            if (B[C+2][D]==AA[rivi][2]){
              if (B[C+3][D]==AA[rivi][3]){
                if (B[C+4][D]==AA[rivi][4]){
			        	  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
                    syvyysmax=syvyys-1;
  				          haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=0;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D+1]==AA[rivi][1]){
            if (B[C][D+2]==AA[rivi][2]){
              if (B[C][D+3]==AA[rivi][3]){
                if (B[C][D+4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
 	           		    syvyysmax=syvyys-1;
					          haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=1;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C][D+i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D+1]==AA[rivi][1]){
            if (B[C+2][D+2]==AA[rivi][2]){
              if (B[C+3][D+3]==AA[rivi][3]){
                if (B[C+4][D+4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
				         	  syvyysmax=syvyys-1;
	          		    haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=2;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D+i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D-1]==AA[rivi][1]){
            if (B[C+2][D-2]==AA[rivi][2]){
              if (B[C+3][D-3]==AA[rivi][3]){
                if (B[C+4][D-4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
					          syvyysmax=syvyys-1;
					          haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=3;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D-i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C+i][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D]==AA[rivi][1]){
            if (B[C-2][D]==AA[rivi][2]){
              if (B[C-3][D]==AA[rivi][3]){
                if (B[C-4][D]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
	          			  syvyysmax=syvyys-1;
	           			  haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=4;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D-1]==AA[rivi][1]){
            if (B[C][D-2]==AA[rivi][2]){
              if (B[C][D-3]==AA[rivi][3]){
                if (B[C][D-4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
					          syvyysmax=syvyys-1;
		                haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=5;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C][D-i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D-1]==AA[rivi][1]){
            if (B[C-2][D-2]==AA[rivi][2]){
              if (B[C-3][D-3]==AA[rivi][3]){
                if (B[C-4][D-4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
					          syvyysmax=syvyys-1;
				         	  haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=6;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D-i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D+1]==AA[rivi][1]){
            if (B[C-2][D+2]==AA[rivi][2]){
              if (B[C-3][D+3]==AA[rivi][3]){
                if (B[C-4][D+4]==AA[rivi][4]){
                  if((rivi<3)&&(syvyys>0)){
                    voitto=pikahaku(B, puoli2, 1);
                    if(voitto==9){voitto=0;return 0;}
				         	  syvyysmax=syvyys-1;
					          haepriority(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                    return syvyys;
                  }
                  if(syvyys==0)suunta0=7;
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D+i]=BB[rivi][i];
		              voitto=0;
                  voitto=rekursiohaku(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                  for(i=0;i<5;i++)if(BB[rivi][i]!=5)B[C-i][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                
                }
              }
            }
          }
        }

      }
    }
  }

return 0;
}


/* ei suoralle uhkalle */
int rekursiohaku2(int syvyys, int syvyysmax, int AA[16][8], int BB[16][8], int B[64][64], long int PRIORITY[64][64], int rivi0, int suunta0, int X, int Y, int puoli2){

  int C, D, i, voitto=0; /*,j, */
  int rivi;/*, loyty=0;*/
  if(syvyys>syvyysmax) return 0;

/*  j=0; */
  for(rivi=0;rivi<7;rivi++){
    if(syvyys==0)rivi0=rivi;
    for(D=8;D<laudankoko+8;D++){
      if(syvyys==0)Y=D;
      for(C=8;C<laudankoko+8;C++){
		if(syvyys==0)X=C;
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D]==AA[rivi][1]){
            if (B[C+2][D]==AA[rivi][2]){
              if (B[C+3][D]==AA[rivi][3]){
                if (B[C+4][D]==AA[rivi][4]){
                  if (B[C+5][D]==AA[rivi][5]){
					          if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/
                      syvyysmax=syvyys-1;
					            haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=0;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D+1]==AA[rivi][1]){
            if (B[C][D+2]==AA[rivi][2]){
              if (B[C][D+3]==AA[rivi][3]){
                if (B[C][D+4]==AA[rivi][4]){
                  if (B[C][D+5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/ 
					            syvyysmax=syvyys-1;
					            haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=1;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C][D+i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D+1]==AA[rivi][1]){
            if (B[C+2][D+2]==AA[rivi][2]){
              if (B[C+3][D+3]==AA[rivi][3]){
                if (B[C+4][D+4]==AA[rivi][4]){
                  if (B[C+5][D+5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/ 
			          		  syvyysmax=syvyys-1;
					            haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=2;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D+i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C+1][D-1]==AA[rivi][1]){
            if (B[C+2][D-2]==AA[rivi][2]){
              if (B[C+3][D-3]==AA[rivi][3]){
                if (B[C+4][D-4]==AA[rivi][4]){
                  if (B[C+5][D-5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/ 
				          	  syvyysmax=syvyys-1;
				          	  haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=3;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D-i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C+i][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D]==AA[rivi][1]){
            if (B[C-2][D]==AA[rivi][2]){
              if (B[C-3][D]==AA[rivi][3]){
                if (B[C-4][D]==AA[rivi][4]){
                  if (B[C-5][D]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/ 
				          	  syvyysmax=syvyys-1;
				          	  haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=4;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C][D-1]==AA[rivi][1]){
            if (B[C][D-2]==AA[rivi][2]){
              if (B[C][D-3]==AA[rivi][3]){
                if (B[C][D-4]==AA[rivi][4]){
                  if (B[C][D-5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/ 
					            syvyysmax=syvyys-1;
			          		  haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=5;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C][D-i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D-1]==AA[rivi][1]){
            if (B[C-2][D-2]==AA[rivi][2]){
              if (B[C-3][D-3]==AA[rivi][3]){
                if (B[C-4][D-4]==AA[rivi][4]){
                  if (B[C-5][D-5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/  
				           	  syvyysmax=syvyys-1;
				          	  haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=6;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D-i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D-i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }
        if (B[C][D]==AA[rivi][0]){
          if (B[C-1][D+1]==AA[rivi][1]){
            if (B[C-2][D+2]==AA[rivi][2]){
              if (B[C-3][D+3]==AA[rivi][3]){
                if (B[C-4][D+4]==AA[rivi][4]){
                  if (B[C-5][D+5]==AA[rivi][5]){
                   if((rivi<2)&&(syvyys>0)){
/*                      voitto=pikahaku(B, puoli2);
                      if(voitto==9){voitto=0;return 0;}
*/
					            syvyysmax=syvyys-1;
					            haepriority2(syvyys, rivi0, suunta0, (X), (Y), PRIORITY);
                      return syvyys;
                   }
                   if(syvyys==0)suunta0=7;
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D+i]=BB[rivi][i];
		               voitto=0;
                   voitto=rekursiohaku2(syvyys+1, syvyysmax, AA, BB, B, PRIORITY, rivi0, suunta0, X, Y, puoli2);
                   for(i=0;i<6;i++)if(BB[rivi][i]!=5)B[C-i][D+i]=0;
/*                   if((voitto!=0)&&(syvyys>voitto-1)) return 0;
                   if((voitto!=0)&&(syvyys>0)){ return voitto;}
                   if((syvyys==0)&&(voitto!=0)){};
*/                }
                }
              }
            }
          }
        }

      }
    }
  }

return 0;
}








/* AA on kaavataulukko, PISTEET on pistetaulukko kaavoille               */
/* A on pelilaudan taulukko, PRIORITY on pisteytys pelilaudalle taulukko */




/* TÄMÄ ON OHJELMAN YDIN, (engine) joka laskee parhaan paikan laittaa  */
/* tietokoneen pelimerkki.                                             */
/* This program looks the best place for computer's move               */
/* for THO-MOKU, a Go-Moku like game.                                  */

/* Tämä uudempi toimii samalla periaatteella kuin vanhempi, mutta      */
/* on paljon nopeampi ja vaikeampi tajuta... Vois kommentoida, mutta   */
/* ei jaksa...                                                         */

/* Rekursiojutska poistettu, se oli surkee.                                         */
/* Nyt on niin vaikea mitata kun 450MHz koneella kaikki menee                       */
/* alle sekunnissa... Laskun nopeus ei valitettavasti ole enää vakio. :(            */
/* Mutta maksimiarvo ei ylitä missään tapauksessa edellisen laskimen normaaliarvoa. */
/* (Rekursiivisuus on oikeesti hidasta) */


void lisaapriority(long int PRIORITY[64][64],  int AA[64][8],  int C,  int D,  int suunta,  int B[64][64],  int rivi, long int PISTEET[64]){
   int n;

   /* NYT TULEE KOODIN RUMA OSUUS... tätä ei ole optimoitu vielä... */
	if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
	  if (((AA[rivi][1]==3)&&(B[C+1][D]==0))||(AA[rivi][1]==5)||(B[C+1][D]==AA[rivi][1])){
	    if (((AA[rivi][2]==3)&&(B[C+2][D]==0))||(AA[rivi][2]==5)||(B[C+2][D]==AA[rivi][2])){
	      if (((AA[rivi][3]==3)&&(B[C+3][D]==0))||(AA[rivi][3]==5)||(B[C+3][D]==AA[rivi][3])){
      		if (((AA[rivi][4]==3)&&(B[C+4][D]==0))||(AA[rivi][4]==5)||(B[C+4][D]==AA[rivi][4])){
		        if (((AA[rivi][5]==3)&&(B[C+5][D]==0))||(AA[rivi][5]==5)||(B[C+5][D]==AA[rivi][5])){
		          if (((AA[rivi][6]==3)&&(B[C+6][D]==0))||(AA[rivi][6]==5)||(B[C+6][D]==AA[rivi][6])){
		            if (((AA[rivi][7]==3)&&(B[C+7][D]==0))||(AA[rivi][7]==5)||(B[C+7][D]==AA[rivi][7])){
                  for(n=0;n<=7;n++){
			              if((AA[rivi][n]==0)&&(B[C+n][D]==0)){
                      if((C+n>-1)&&(C+n<laudankoko+8)&&(D>-1)&&(D<laudankoko+8)){
                        PRIORITY[C+n][D]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
        }
	    }
	  }
  }
  if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
    if (((AA[rivi][1]==3)&&(B[C][D+1]==0))||(AA[rivi][1]==5)||(B[C][D+1]==AA[rivi][1])){
	    if (((AA[rivi][2]==3)&&(B[C][D+2]==0))||(AA[rivi][2]==5)||(B[C][D+2]==AA[rivi][2])){
	      if (((AA[rivi][3]==3)&&(B[C][D+3]==0))||(AA[rivi][3]==5)||(B[C][D+3]==AA[rivi][3])){
          if (((AA[rivi][4]==3)&&(B[C][D+4]==0))||(AA[rivi][4]==5)||(B[C][D+4]==AA[rivi][4])){
            if (((AA[rivi][5]==3)&&(B[C][D+5]==0))||(AA[rivi][5]==5)||(B[C][D+5]==AA[rivi][5])){
		          if (((AA[rivi][6]==3)&&(B[C][D+6]==0))||(AA[rivi][6]==5)||(B[C][D+6]==AA[rivi][6])){
		            if (((AA[rivi][7]==3)&&(B[C][D+7]==0))||(AA[rivi][7]==5)||(B[C][D+7]==AA[rivi][7])){
                  for(n=0;n<=7;n++){
                    if((AA[rivi][n]==0)&&(B[C][D+n]==0)){
                      if((C>-1)&&(C<laudankoko+8)&&(D+n>-1)&&(D+n<laudankoko+8)){
                        PRIORITY[C][D+n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
	      }
      }
    }
	}
  if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
	  if (((AA[rivi][1]==3)&&(B[C+1][D+1]==0))||(AA[rivi][1]==5)||(B[C+1][D+1]==AA[rivi][1])){
      if (((AA[rivi][2]==3)&&(B[C+2][D+2]==0))||(AA[rivi][2]==5)||(B[C+2][D+2]==AA[rivi][2])){
        if (((AA[rivi][3]==3)&&(B[C+3][D+3]==0))||(AA[rivi][3]==5)||(B[C+3][D+3]==AA[rivi][3])){
	      	if (((AA[rivi][4]==3)&&(B[C+4][D+4]==0))||(AA[rivi][4]==5)||(B[C+4][D+4]==AA[rivi][4])){
		        if (((AA[rivi][5]==3)&&(B[C+5][D+5]==0))||(AA[rivi][5]==5)||(B[C+5][D+5]==AA[rivi][5])){
              if (((AA[rivi][6]==3)&&(B[C+6][D+6]==0))||(AA[rivi][6]==5)||(B[C+6][D+6]==AA[rivi][6])){
                if (((AA[rivi][7]==3)&&(B[C+7][D+7]==0))||(AA[rivi][7]==5)||(B[C+7][D+7]==AA[rivi][7])){
	            		for(n=0;n<=7;n++){
                    if((AA[rivi][n]==0)&&(B[C+n][D+n]==0)){
                      if((C+n>-1)&&(C+n<laudankoko+8)&&(D+n>-1)&&(D+n<laudankoko+8)){
  			                PRIORITY[C+n][D+n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
	  }
	}
  if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
    if (((AA[rivi][1]==3)&&(B[C+1][D-1]==0))||(AA[rivi][1]==5)||(B[C+1][D-1]==AA[rivi][1])){
      if (((AA[rivi][2]==3)&&(B[C+2][D-2]==0))||(AA[rivi][2]==5)||(B[C+2][D-2]==AA[rivi][2])){
        if (((AA[rivi][3]==3)&&(B[C+3][D-3]==0))||(AA[rivi][3]==5)||(B[C+3][D-3]==AA[rivi][3])){
          if (((AA[rivi][4]==3)&&(B[C+4][D-4]==0))||(AA[rivi][4]==5)||(B[C+4][D-4]==AA[rivi][4])){
	      	  if (((AA[rivi][5]==3)&&(B[C+5][D-5]==0))||(AA[rivi][5]==5)||(B[C+5][D-5]==AA[rivi][5])){
		          if (((AA[rivi][6]==3)&&(B[C+6][D-6]==0))||(AA[rivi][6]==5)||(B[C+6][D-6]==AA[rivi][6])){
	      	      if (((AA[rivi][7]==3)&&(B[C+7][D-7]==0))||(AA[rivi][7]==5)||(B[C+7][D-7]==AA[rivi][7])){
			            for(n=0;n<=7;n++){
		            	  if((AA[rivi][n]==0)&&(B[C+n][D-n]==0)){
                      if((C+n>-1)&&(C+n<laudankoko+8)&&(D-n>-1)&&(D-n<laudankoko+8)){
                        PRIORITY[C+n][D-n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
	      }
	    }
    }
  }
  if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
	  if (((AA[rivi][1]==3)&&(B[C-1][D]==0))||(AA[rivi][1]==5)||(B[C-1][D]==AA[rivi][1])){
	    if (((AA[rivi][2]==3)&&(B[C-2][D]==0))||(AA[rivi][2]==5)||(B[C-2][D]==AA[rivi][2])){
        if (((AA[rivi][3]==3)&&(B[C-3][D]==0))||(AA[rivi][3]==5)||(B[C-3][D]==AA[rivi][3])){
          if (((AA[rivi][4]==3)&&(B[C-4][D]==0))||(AA[rivi][4]==5)||(B[C-4][D]==AA[rivi][4])){
            if (((AA[rivi][5]==3)&&(B[C-5][D]==0))||(AA[rivi][5]==5)||(B[C-5][D]==AA[rivi][5])){
      		    if (((AA[rivi][6]==3)&&(B[C-6][D]==0))||(AA[rivi][6]==5)||(B[C-6][D]==AA[rivi][6])){
		            if (((AA[rivi][7]==3)&&(B[C-7][D]==0))||(AA[rivi][7]==5)||(B[C-7][D]==AA[rivi][7])){
                  for(n=0;n<=7;n++){
                    if((AA[rivi][n]==0)&&(B[C-n][D]==0)){
                      if((C-n>-1)&&(C-n<laudankoko+8)&&(D>-1)&&(D<laudankoko+8)){
  			                PRIORITY[C-n][D]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
	      }
	    }
	  }
	}
	if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
    if (((AA[rivi][1]==3)&&(B[C][D-1]==0))||(AA[rivi][1]==5)||(B[C][D-1]==AA[rivi][1])){
      if (((AA[rivi][2]==3)&&(B[C][D-2]==0))||(AA[rivi][2]==5)||(B[C][D-2]==AA[rivi][2])){
	      if (((AA[rivi][3]==3)&&(B[C][D-3]==0))||(AA[rivi][3]==5)||(B[C][D-3]==AA[rivi][3])){
          if (((AA[rivi][4]==3)&&(B[C][D-4]==0))||(AA[rivi][4]==5)||(B[C][D-4]==AA[rivi][4])){
		        if (((AA[rivi][5]==3)&&(B[C][D-5]==0))||(AA[rivi][5]==5)||(B[C][D-5]==AA[rivi][5])){
		          if (((AA[rivi][6]==3)&&(B[C][D-6]==0))||(AA[rivi][6]==5)||(B[C][D-6]==AA[rivi][6])){
                if (((AA[rivi][7]==3)&&(B[C][D-7]==0))||(AA[rivi][7]==5)||(B[C][D-7]==AA[rivi][7])){
	            		for(n=0;n<=7;n++){
			              if((AA[rivi][n]==0)&&(B[C][D-n]==0)){
                      if((C>-1)&&(C<laudankoko+8)&&(D-n>-1)&&(D-n<laudankoko+8)){
                        PRIORITY[C][D-n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
	  }
	}
  if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
    if (((AA[rivi][1]==3)&&(B[C-1][D-1]==0))||(AA[rivi][1]==5)||(B[C-1][D-1]==AA[rivi][1])){
	    if (((AA[rivi][2]==3)&&(B[C-2][D-2]==0))||(AA[rivi][2]==5)||(B[C-2][D-2]==AA[rivi][2])){
	      if (((AA[rivi][3]==3)&&(B[C-3][D-3]==0))||(AA[rivi][3]==5)||(B[C-3][D-3]==AA[rivi][3])){
          if (((AA[rivi][4]==3)&&(B[C-4][D-4]==0))||(AA[rivi][4]==5)||(B[C-4][D-4]==AA[rivi][4])){
		        if (((AA[rivi][5]==3)&&(B[C-5][D-5]==0))||(AA[rivi][5]==5)||(B[C-5][D-5]==AA[rivi][5])){
              if (((AA[rivi][6]==3)&&(B[C-6][D-6]==0))||(AA[rivi][6]==5)||(B[C-6][D-6]==AA[rivi][6])){
                if (((AA[rivi][7]==3)&&(B[C-7][D-7]==0))||(AA[rivi][7]==5)||(B[C-7][D-7]==AA[rivi][7])){
                  for(n=0;n<=7;n++){
			              if((AA[rivi][n]==0)&&(B[C-n][D-n]==0)){
                      if((C-n>-1)&&(C-n<laudankoko+8)&&(D-n>-1)&&(D-n<laudankoko+8)){
  			                PRIORITY[C-n][D-n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
        }
	    }
	  }
  }
	if (((AA[rivi][0]==3)&&(B[C][D]==0))||(AA[rivi][0]==5)||(B[C][D]==AA[rivi][0])){
	  if (((AA[rivi][1]==3)&&(B[C-1][D+1]==0))||(AA[rivi][1]==5)||(B[C-1][D+1]==AA[rivi][1])){
      if (((AA[rivi][2]==3)&&(B[C-2][D+2]==0))||(AA[rivi][2]==5)||(B[C-2][D+2]==AA[rivi][2])){
        if (((AA[rivi][3]==3)&&(B[C-3][D+3]==0))||(AA[rivi][3]==5)||(B[C-3][D+3]==AA[rivi][3])){
	       	if (((AA[rivi][4]==3)&&(B[C-4][D+4]==0))||(AA[rivi][4]==5)||(B[C-4][D+4]==AA[rivi][4])){
            if (((AA[rivi][5]==3)&&(B[C-5][D+5]==0))||(AA[rivi][5]==5)||(B[C-5][D+5]==AA[rivi][5])){
		          if (((AA[rivi][6]==3)&&(B[C-6][D+6]==0))||(AA[rivi][6]==5)||(B[C-6][D+6]==AA[rivi][6])){
		            if (((AA[rivi][7]==3)&&(B[C-7][D+7]==0))||(AA[rivi][7]==5)||(B[C-7][D+7]==AA[rivi][7])){
                  for(n=0;n<=7;n++){
                    if((AA[rivi][n]==0)&&(B[C-n][D+n]==0)){
                      if((C-n>-1)&&(C-n<laudankoko+8)&&(D+n>-1)&&(D+n<laudankoko+8)){
                        PRIORITY[C-n][D+n]+=PISTEET[rivi];
                      }
                    }
                  }
                }
              }
            }
          }
	      }
	    }
	  }
	}

}



/* Tähän erilliset opetetut aloitukset, jos ei käytetä normaalia */
int alotus(void){
  int B[64][64];
  int x, y, x2, y2, uhka;
  /* int x3, y3, kakkonen; */
/*  kakkonen=arpa(); */
	for(y=0;y<64;y++){
		for(x=0;x<64;x++)B[x][y]=0;
	}
	for(y=0;y<(laudankoko);y++){
		for(x=0;x<(laudankoko);x++)
			B[x+8][y+8]=A[x][y];
	}

  switch(kokovuoro){
    case 0:
	    PRIORITY[laudankoko/2+8][laudankoko/2+8]=500;
      return 1;
    case 1:
      if((alotyyli==2)||(alotyyli==3)){
        for(y=8;y<laudankoko+8;y++){
          for(x=8;x<laudankoko+8;x++){
            if(B[x][y]==1){
		          if(x<laudankoko/2+7){
                if(y<laudankoko/2+7)PRIORITY[x+1][y+1]=500; else PRIORITY[x+1][y-1]=500;
              } else {
                if(y<laudankoko/2+7)PRIORITY[x-1][y+1]=500; else PRIORITY[x-1][y-1]=500;
              }
            }
          }
        }
		    return 1;
      }
      if(alotyyli==4){
        for(y=8;y<laudankoko+8;y++){
          for(x=8;x<laudankoko+8;x++){
            if(B[x][y]==1){
		          if(x<laudankoko/2+7){
			        	if(y<laudankoko/2+7){PRIORITY[x+2][y+1]=500;PRIORITY[x+1][y+2]=500;}else{PRIORITY[x+2][y-1]=500;PRIORITY[x+1][y-2]=500;}
              } else {
	        			if(y<laudankoko/2+7){PRIORITY[x-2][y+1]=500;PRIORITY[x-1][y+2]=500;}else{PRIORITY[x-2][y-1]=500;PRIORITY[x-1][y-2]=500;}
              }
            }
          }
        }
		    return 1;
      }
      return 0;
    case 2:
      for(y=8;y<laudankoko+8;y++){
        for(x=8;x<laudankoko+8;x++){
          if(B[x][y]==1){
            switch(alotyyli){
              case 2:
                if(B[x-1][y-1]==2){PRIORITY[x-2][y-2]=500;return 1;}
                if(B[x][y-1]==2){PRIORITY[x][y-2]=500;return 1;}
                if(B[x+1][y-1]==2){PRIORITY[x+2][y-2]=500;return 1;}
                if(B[x-1][y]==2){PRIORITY[x-2][y]=500;return 1;}
                if(B[x+1][y]==2){PRIORITY[x+2][y]=500;return 1;}
                if(B[x-1][y+1]==2){PRIORITY[x-2][y+2]=500;return 1;}
                if(B[x][y+1]==2){PRIORITY[x][y+2]=500;return 1;}
                if(B[x+1][y+1]==2){PRIORITY[x+2][y+2]=500;return 1;}
				  
                if(B[x-2][y-2]==2){PRIORITY[x-3][y-3]=500;return 1;}
                if(B[x+2][y-2]==2){PRIORITY[x+3][y-3]=500;return 1;}
                if(B[x+2][y+2]==2){PRIORITY[x+3][y+3]=500;return 1;}
                if(B[x-2][y+2]==2){PRIORITY[x-3][y+3]=500;return 1;}

                if(B[x][y-2]==2){PRIORITY[x-2][y-2]=500;PRIORITY[x+2][y-2]=500;return 1;}
                if(B[x-2][y]==2){PRIORITY[x-2][y-2]=500;PRIORITY[x-2][y+2]=500;return 1;}
                if(B[x+2][y]==2){PRIORITY[x+2][y-2]=500;PRIORITY[x+2][y+2]=500;return 1;}
                if(B[x][y+2]==2){PRIORITY[x-2][y+2]=500;PRIORITY[x+2][y+2]=500;return 1;}

                if(B[x-1][y-2]==2){PRIORITY[x-2][y-2]=500;PRIORITY[x+1][y-1]=500;PRIORITY[x-2][y]=100;return 1;}
                if(B[x-1][y+2]==2){PRIORITY[x-2][y+2]=500;PRIORITY[x+1][y+1]=500;PRIORITY[x][y+2]=100;return 1;}
                if(B[x+1][y-2]==2){PRIORITY[x+2][y-2]=500;PRIORITY[x-1][y-1]=500;PRIORITY[x+2][y]=100;return 1;}
                if(B[x+1][y+2]==2){PRIORITY[x+2][y+2]=500;PRIORITY[x-1][y+1]=500;PRIORITY[x][y+2]=100;return 1;}

                if(B[x+2][y+1]==2){PRIORITY[x+2][y+2]=500;PRIORITY[x+1][y-1]=500;PRIORITY[x][y+2]=100;return 1;}
                if(B[x-2][y-1]==2){PRIORITY[x-2][y-2]=500;PRIORITY[x-1][y+1]=500;PRIORITY[x][y-2]=100;return 1;}
			        	if(B[x+2][y-1]==2){PRIORITY[x+2][y-2]=500;PRIORITY[x+1][y+1]=500;PRIORITY[x][y-2]=100;return 1;}
                if(B[x-2][y+1]==2){PRIORITY[x-2][y+2]=500;PRIORITY[x-1][y-1]=500;PRIORITY[x][y+2]=100;return 1;}

                return 0;
              case 3:
                if(B[x-1][y-1]==2){PRIORITY[x+1][y+1]=500;return 1;}
                if(B[x+1][y-1]==2){PRIORITY[x-1][y+1]=500;return 1;}
                if(B[x-1][y+1]==2){PRIORITY[x+1][y-1]=500;return 1;}
                if(B[x+1][y+1]==2){PRIORITY[x-1][y-1]=500;return 1;}
                if(B[x][y-1]==2){PRIORITY[x-1][y+1]=500;PRIORITY[x+1][y+1]=500;return 1;}
                if(B[x-1][y]==2){PRIORITY[x+1][y+1]=500;PRIORITY[x+1][y-1]=500;return 1;}
                if(B[x+1][y]==2){PRIORITY[x-1][y+1]=500;PRIORITY[x-1][y-1]=500;return 1;}
                if(B[x][y+1]==2){PRIORITY[x+1][y-1]=500;PRIORITY[x-1][y-1]=500;return 1;}

                for(y2=8;y2<laudankoko+8;y2++){
                   for(x2=8;x2<laudankoko+8;x2++){
                     if(B[x2][y2]==2){
          					   if(x2>x){
					            	 if(y2>y){PRIORITY[x-1][y-1]=500;return 1;}else{PRIORITY[x-1][y+1]=500;return 1;}
                       }else{
						             if(y2>y){PRIORITY[x+1][y-1]=500;return 1;}else{PRIORITY[x+1][y+1]=500;return 1;}
                       }
                     }
                   }
                }
				
				        return 0;
              case 4:
                for(y2=8;y2<laudankoko+8;y2++){
                  for(x2=8;x2<laudankoko+8;x2++){
                     if(B[x2][y2]==2){
	          				   if(x2>x){
						             if(y2>y){
							             if((x+1==x2)&&(y+1==y2)){PRIORITY[x][y-2]=500;PRIORITY[x-2][y]=500;return 1;}
							             PRIORITY[x-2][y-2]=500;return 1;
                         }else{
							             if((x+1==x2)&&(y-1==y2)){PRIORITY[x][y+2]=500;PRIORITY[x-2][y]=500;return 1;}
							             PRIORITY[x-2][y+2]=500;return 1;
                         }
                       }else{
            						 if(y2>y){
						            	 if((x-1==x2)&&(y+1==y2)){PRIORITY[x][y-2]=500;PRIORITY[x+2][y]=500;return 1;}
							             PRIORITY[x+2][y-2]=500;return 1;
                         }else{
							             if((x-1==x2)&&(y-1==y2)){PRIORITY[x][y+2]=500;PRIORITY[x+2][y]=500;return 1;}
							             PRIORITY[x+2][y+2]=500;return 1;
                         }
                       }
                     }
                  }
                }
				        return 0;
	            default: return 0;
            }
          }
        }
      }
    case 3:
      if(alotyyli==2){
        for(y=8;y<laudankoko+8;y++){
          for(x=8;x<laudankoko+8;x++){
            if(B[x][y]==2){
              if(((B[x-1][y-1]==1)&&(B[x+1][y+1]==1))||((B[x+1][y-1]==1)&&(B[x-1][y+1]==1))){
			       	  PRIORITY[x][y+1]=500;PRIORITY[x][y-1]=500;PRIORITY[x+1][y]=500;PRIORITY[x-1][y]=500;
			          return 1;
              }
            }
          }
        }
      }
      if(alotyyli==3){
        for(y=8;y<laudankoko+8;y++){
          for(x=8;x<laudankoko+8;x++){
            if(B[x][y]==2){
              if((B[x-1][y-1]==1)||(B[x-1][y-1]==0))if(B[x+1][y+1]==0){PRIORITY[x+1][y+1]=500;return 1;}
              if((B[x-1][y+1]==1)||(B[x-1][y+1]==0))if(B[x+1][y-1]==0){PRIORITY[x+1][y-1]=500;return 1;}
              if((B[x+1][y-1]==1)||(B[x+1][y-1]==0))if(B[x-1][y+1]==0){PRIORITY[x-1][y+1]=500;return 1;}
              if((B[x+1][y+1]==1)||(B[x+1][y+1]==0))if(B[x-1][y-1]==0){PRIORITY[x-1][y-1]=500;return 1;}
            }
          }
        }
      }
    case 4:
      for(y=8;y<laudankoko+8;y++){
        for(x=8;x<laudankoko+8;x++){
          if(B[x][y]==1){
            switch(alotyyli){
              case 2:
                for(y2=8;y2<laudankoko+8;y2++){
                  for(x2=8;x2<laudankoko+8;x2++){
                    if(B[x2][y2]==2){

                      if(B[x+2][y+2]==1){
                        if((x2!=x+1)||(y2!=y+1)){
                         if(x2>y2){
                           if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                         }else{
                           if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                         }
                        }
                      }
                      if(B[x-2][y+2]==1){
                        if((x2!=x-1)||(y2!=y+1)){
                         if(x2>(laudankoko+12-y2)){
                           if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                         }else{
                           if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                         }
                        }
                      }


                      if(B[x][y+2]==1){
                        if((x2!=x)||(y2!=y+1)){
                         if(x2>x){
                           if(B[x-1][y+1]==0){PRIORITY[x-1][y+1]=500;return 1;}
                         }else{
                           if(B[x+1][y+1]==0){PRIORITY[x+1][y+1]=500;return 1;}
                         }							
                        }
                      }
                      if(B[x+2][y]==1){
                        if((x2!=x+1)||(y2!=y)){
                         if(y2>y){
                           if(B[x+1][y-1]==0){PRIORITY[x+1][y-1]=500;return 1;}
                         }else{
                           if(B[x+1][y+1]==0){PRIORITY[x+1][y+1]=500;return 1;}
                         }
                        }
                      }
					
                    }
                  }
                }
                return 0;
			        case 3:
                if(B[x-1][y-1]==1){
                  if(B[x-2][y-2]==2){
                    if(B[x+1][y+1]==0){PRIORITY[x+1][y+1]=500;return 1;}
                  }
	        			if(B[x+1][y+1]==2){
                    if(B[x-2][y-2]==0){PRIORITY[x-2][y-2]=500;return 1;}
                  }
                }	
                if(B[x-1][y+1]==1){
                  if(B[x-2][y+2]==2){
                    if(B[x+1][y-1]==0){PRIORITY[x+1][y-1]=500;return 1;}
                  }
           	    if(B[x+1][y-1]==2){
                    if(B[x-2][y+2]==0){PRIORITY[x-2][y+2]=500;return 1;}
                  }
                }
                if(B[x+1][y-1]==1){
                  if(B[x+2][y-2]==2){
                    if(B[x-1][y+1]==0){PRIORITY[x-1][y+1]=500;return 1;}
                  }
        		    if(B[x-1][y+1]==2){
                    if(B[x+2][y-2]==0){PRIORITY[x+2][y-2]=500;return 1;}
                  }
                }
                if(B[x+1][y+1]==1){
                  if(B[x+2][y+2]==2){
                    if(B[x-1][y-1]==0){PRIORITY[x-1][y-1]=500;return 1;}
                  }
   	            if(B[x-1][y-1]==2){
                    if(B[x+2][y+2]==0){PRIORITY[x+2][y+2]=500;return 1;}
                  }
                }
                return 0;
			        case 4:
                if(B[x+2][y]==1){
                  if(B[x-1][y+1]==2){
                    if(B[x][y-2]==0){PRIORITY[x][y-2]=500;return 1;}
                    if((B[x][y-2]!=0)&&(B[x][y+2]==0)){PRIORITY[x][y+2]=400;return 1;}
                  }
                  if(B[x-1][y-1]==2){
                    if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                    if((B[x][y+2]!=0)&&(B[x][y-2]==0)){PRIORITY[x][y-2]=400;return 1;}
                  }
                  if(B[x+3][y+1]==2){
                    if(B[x+2][y-2]==0){PRIORITY[x+2][y-2]=500;return 1;}
                    if((B[x+2][y-2]!=0)&&(B[x+2][y+2]==0)){PRIORITY[x+2][y+2]=400;return 1;}
                  }
                  if(B[x+3][y-1]==2){
                    if(B[x+2][y+2]==0){PRIORITY[x+2][y+2]=500;return 1;}
                    if((B[x+2][y+2]!=0)&&(B[x+2][y-2]==0)){PRIORITY[x+2][y-2]=400;return 1;}
                  }
                }

                if(B[x-2][y]==1){
                  if(B[x+1][y+1]==2){
                    if(B[x][y-2]==0){PRIORITY[x][y-2]=500;return 1;}
                    if((B[x][y-2]!=0)&&(B[x][y+2]==0)){PRIORITY[x][y+2]=400;return 1;}
                  }
                  if(B[x+1][y-1]==2){
                    if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                    if((B[x][y+2]!=0)&&(B[x][y-2]==0)){PRIORITY[x][y-2]=400;return 1;}
                  }
                  if(B[x-3][y+1]==2){
                    if(B[x-2][y-2]==0){PRIORITY[x-2][y-2]=500;return 1;}
                    if((B[x-2][y-2]!=0)&&(B[x-2][y+2]==0)){PRIORITY[x-2][y+2]=400;return 1;}
                  }
                  if(B[x-3][y-1]==2){
                    if(B[x-2][y+2]==0){PRIORITY[x-2][y+2]=500;return 1;}
                    if((B[x-2][y+2]!=0)&&(B[x-2][y-2]==0)){PRIORITY[x-2][y-2]=400;return 1;}
                  }
                }
                if(B[x][y+2]==1){
                  if(B[x+1][y-1]==2){
                    if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                    if((B[x-2][y]!=0)&&(B[x+2][y]==0)){PRIORITY[x+2][y]=400;return 1;}
                  }
                  if(B[x-1][y-1]==2){
                    if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                    if((B[x+2][y]!=0)&&(B[x-2][y]==0)){PRIORITY[x-2][y]=400;return 1;}
                  }
                  if(B[x+1][y+3]==2){
                    if(B[x-2][y+2]==0){PRIORITY[x-2][y]=500;return 1;}
                    if((B[x-2][y+2]!=0)&&(B[x+2][y]==0)){PRIORITY[x+2][y+2]=400;return 1;}
                  }
                  if(B[x-1][y+3]==2){
                    if(B[x+2][y+2]==0){PRIORITY[x+2][y+2]=500;return 1;}
                    if((B[x+2][y+2]!=0)&&(B[x-2][y+2]==0)){PRIORITY[x-2][y+2]=400;return 1;}
                  }
                }
                if(B[x][y-2]==1){
                  if(B[x+1][y+1]==2){
                    if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                    if((B[x-2][y]!=0)&&(B[x+2][y]==0)){PRIORITY[x+2][y]=400;return 1;}
                  }
                  if(B[x-1][y+1]==2){
                    if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                    if((B[x+2][y]!=0)&&(B[x-2][y]==0)){PRIORITY[x-2][y]=400;return 1;}
                  }
                  if(B[x+1][y-3]==2){
                    if(B[x-2][y-2]==0){PRIORITY[x-2][y-2]=500;return 1;}
                    if((B[x-2][y-2]!=0)&&(B[x+2][y-2]==0)){PRIORITY[x+2][y-2]=400;return 1;}
                  }
                  if(B[x-1][y-3]==2){
                    if(B[x+2][y-2]==0){PRIORITY[x+2][y-2]=500;return 1;}
                    if((B[x+2][y-2]!=0)&&(B[x-2][y-2]==0)){PRIORITY[x-2][y-2]=400;return 1;}
                  }
                }
                for(y2=8;y2<laudankoko+8;y2++){
                  for(x2=8;x2<laudankoko+8;x2++){
                    if(B[x2][y2]==2){
                      if(B[x-2][y-2]==1){
					  
                        if(x2>y2){
                          if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                        }else{
                          if(B[x][y-2]==0){PRIORITY[x][y-2]=500;return 1;}
                        }
                      }
                      if(B[x-2][y+2]==1){
					  
                        if(x2>(laudankoko+14-y2)){
                          if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                        }else{
                          if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                        }
                      }
                      if(B[x+2][y-2]==1){
					  
                        if(x2>(laudankoko+14-y2)){
                          if(B[x][y-2]==0){PRIORITY[x][y-2]=500;return 1;}
                        }else{
                          if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                        }
                      }
                      if(B[x+2][y+2]==1){
					  
                        if(x2>y2){
                          if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                        }else{
                          if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                        }
                      }

                    }
                  }
                }
                return 0;
	            default: 
                return 0;
            }
          }
        }
      }
      return 0;
    case 5: /* varottava vastustajan 3 suoraa */
  	  if(alotyyli==2){
        uhka=pikahaku(B, 1, 2);
        if(uhka==9)return 0;
        for(y=8;y<laudankoko+8;y++){
           for(x=8;x<laudankoko+8;x++){
	      		 if(B[x][y]==2){
               if((B[x-1][y-1]==1)&&(B[x+1][y-1]==1)&&(B[x+1][y+1]==1)){
                 if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                 if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
               }
               if((B[x-1][y-1]==1)&&(B[x-1][y+1]==1)&&(B[x+1][y+1]==1)){
                 if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                 if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
               }
               if((B[x-1][y-1]==1)&&(B[x+1][y-1]==1)&&(B[x-1][y+1]==1)){
                 if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                 if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
               }
               if((B[x+1][y-1]==1)&&(B[x-1][y+1]==1)&&(B[x+1][y+1]==1)){
                 if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                 if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
               }
             }
           }			   
        }
        return 0;
      }
	    if(alotyyli==3){
          uhka=pikahaku(B, 1, 2);
          if(uhka==9)return 0;
          for(y=8;y<laudankoko+8;y++){
            for(x=8;x<laudankoko+8;x++){
              if(B[x][y]==2){
          
                for(y2=8;y2<laudankoko+8;y2++){
                  for(x2=8;x2<laudankoko+8;x2++){
                    if(B[x2][y2]==1){
                      if(B[x-1][y-1]==2){
                        if(((x2!=x-2)||(y2!=y-2))&&((x2!=x+1)||(y2!=y+1))){
                          if(B[x-2][y-2]==1){
                            if(x2>y2){
                              if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
                            }else{
                              if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                            }
                          }
			            			  if(B[x+1][y+1]==1){
                            if(x2>y2){
                              if(B[x-2][y-1]==0){PRIORITY[x-2][y-1]=500;return 1;}
                            }else{
                              if(B[x-1][y-2]==0){PRIORITY[x-1][y-2]=500;return 1;}
                            }
                          }
                        }	
                      }
                      if(B[x-1][y+1]==2){
                        if(((x2!=x-2)||(y2!=y+2))&&((x2!=x+1)||(y2!=y-1))){

                          if(B[x-2][y+2]==1){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
                            }else{
                              if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                            }
                          }
					            	  if(B[x+1][y-1]==1){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x-2][y+1]==0){PRIORITY[x-2][y+1]=500;return 1;}
                            }else{
                              if(B[x-1][y+2]==0){PRIORITY[x-1][y+2]=500;return 1;}
                            }
                          }
						
                        }
                      }
                      if(B[x+1][y-1]==2){
                        if(((x2!=x+2)||(y2!=y-2))&&((x2!=x-1)||(y2!=y+1))){

                          if(B[x+2][y-2]==1){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                            }else{
                              if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
                            }
                          }
				            		  if(B[x-1][y+1]==1){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x+1][y-2]==0){PRIORITY[x+1][y-2]=500;return 1;}
                            }else{
                              if(B[x+2][y-1]==0){PRIORITY[x+2][y-1]=500;return 1;}
                            }
                          }
						
                        }
                      }
                      if(B[x+1][y+1]==2){
                        if(((x2!=x+2)||(y2!=y+2))&&((x2!=x-1)||(y2!=y-1))){

                          if(B[x+2][y+2]==1){
                            if(x2>y2){
                              if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                            }else{
                              if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
                            }
                          }
						              if(B[x-1][y-1]==1){
                            if(x2>y2){
                              if(B[x+1][y+1]==0){PRIORITY[x+1][y+2]=500;return 1;}
                            }else{
                              if(B[x+2][y+1]==0){PRIORITY[x+2][y+1]=500;return 1;}
                            }
                          }
						
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          return 0;

      
      }
      return 0;
	  case 6:
      switch(alotyyli){

        case 2:
          uhka=pikahaku(B, 2, 2);
          if(uhka==9)return 0;
          for(y=8;y<laudankoko+8;y++){
            for(x=8;x<laudankoko+8;x++){
              if(B[x][y]==1){
                if((B[x-2][y]==1)&&(B[x][y-2]==1)&&(B[x-1][y-1]==2)){
                  if((B[x][y-1]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x-1][y]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y-2]==1)&&(B[x+1][y-1]==2)){
                  if((B[x+1][y]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                  if((B[x][y-1]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y+2]==1)&&(B[x+1][y+1]==2)){
                  if((B[x+1][y]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x][y+1]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x-2][y]==1)&&(B[x][y+2]==1)&&(B[x-1][y+1]==2)){
                  if((B[x-1][y]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                  if((B[x][y+1]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                }

                if((B[x-2][y]==1)&&(B[x][y-2]==1)&&(B[x-1][y-1]==2)){
                  if((B[x-3][y-1]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x-1][y-3]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y-2]==1)&&(B[x+1][y-1]==2)){
                  if((B[x+1][y-3]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                  if((B[x+3][y-1]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y+2]==1)&&(B[x+1][y+1]==2)){
                  if((B[x+1][y-3]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x+3][y+1]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x-2][y]==1)&&(B[x][y+2]==1)&&(B[x-1][y+1]==2)){
                  if((B[x-1][y+3]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                  if((B[x-3][y+1]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                }

                if((B[x-2][y]==1)&&(B[x][y-2]==1)&&(B[x-1][y-1]==2)){
                  if((B[x-2][y-1]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x-1][y-2]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y-2]==1)&&(B[x+1][y-1]==2)){
                  if((B[x+1][y-2]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                  if((B[x+2][y-1]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                }
                if((B[x+2][y]==1)&&(B[x][y+2]==1)&&(B[x+1][y+1]==2)){
                  if((B[x+1][y-2]==2)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  if((B[x+2][y+1]==2)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                }
                if((B[x-2][y]==1)&&(B[x][y+2]==1)&&(B[x-1][y+1]==2)){
                  if((B[x-1][y+2]==2)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                  if((B[x-2][y+1]==2)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                }

                

                if((B[x-1][y+1]==1)&&(B[x+1][y+1]==1)&&(B[x][y+1]==2)){
                  if(B[x][y+2]==0){PRIORITY[x][y+2]=500;return 1;}
                  if(B[x][y+2]==2){
                    if((B[x+2][y+2]==0)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                    if((B[x-2][y+2]==0)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                  }
                }
                if((B[x+1][y-1]==1)&&(B[x+1][y+1]==1)&&(B[x+1][y]==2)){
                  if(B[x+2][y]==0){PRIORITY[x+2][y]=500;return 1;}
                  if(B[x+2][y]==2){
                    if((B[x+2][y-2]==0)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                    if((B[x+2][y+2]==0)&&(B[x-1][y-1]==0)){PRIORITY[x-1][y-1]=500;return 1;}
                  }
                }
                if((B[x+1][y-1]==1)&&(B[x-1][y-1]==1)&&(B[x][y-1]==2)){
                  if(B[x][y-2]==0){PRIORITY[x][y-2]=500;return 1;}
                  if(B[x][y-2]==2){
                    if((B[x+2][y-2]==0)&&(B[x-1][y+1]==0)){PRIORITY[x-1][y+1]=500;return 1;}
                    if((B[x-2][y-2]==0)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                  }
                }
                if((B[x-1][y+1]==1)&&(B[x-1][y-1]==1)&&(B[x-1][y]==2)){
                  if(B[x-2][y]==0){PRIORITY[x-2][y]=500;return 1;}
                  if(B[x-2][y]==2){
                    if((B[x+2][y-2]==0)&&(B[x+1][y-1]==0)){PRIORITY[x+1][y-1]=500;return 1;}
                    if((B[x-2][y-2]==0)&&(B[x+1][y+1]==0)){PRIORITY[x+1][y+1]=500;return 1;}
                  }
                }
              
              }
            }
          }

          return 0;
        case 3:
          uhka=pikahaku(B, 2, 2);
          if(uhka==9)return 0;
          for(y=8;y<laudankoko+8;y++){
            for(x=8;x<laudankoko+8;x++){
              if(B[x][y]==1){
          
                for(y2=8;y2<laudankoko+8;y2++){
                  for(x2=8;x2<laudankoko+8;x2++){
                    if(B[x2][y2]==2){
                      if(B[x-1][y-1]==1){
                        if(((x2!=x-2)||(y2!=y-2))&&((x2!=x+1)||(y2!=y+1))){
                          if(B[x-2][y-2]==2){
                            if(x2>y2){
                              if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
                            }else{
                              if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                            }
                          }
			            			  if(B[x+1][y+1]==2){
                            if(x2>y2){
                              if(B[x-2][y-1]==0){PRIORITY[x-2][y-1]=500;return 1;}
                            }else{
                              if(B[x-1][y-2]==0){PRIORITY[x-1][y-2]=500;return 1;}
                            }
                          }
                        }	
                      }
                      if(B[x-1][y+1]==1){
                        if(((x2!=x-2)||(y2!=y+2))&&((x2!=x+1)||(y2!=y-1))){

                          if(B[x-2][y+2]==2){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
                            }else{
                              if(B[x+1][y]==0){PRIORITY[x+1][y]=500;return 1;}
                            }
                          }
					            	  if(B[x+1][y-1]==2){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x-2][y+1]==0){PRIORITY[x-2][y+1]=500;return 1;}
                            }else{
                              if(B[x-1][y+2]==0){PRIORITY[x-1][y+2]=500;return 1;}
                            }
                          }
						
                        }
                      }
                      if(B[x+1][y-1]==1){
                        if(((x2!=x+2)||(y2!=y-2))&&((x2!=x-1)||(y2!=y+1))){

                          if(B[x+2][y-2]==2){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                            }else{
                              if(B[x][y+1]==0){PRIORITY[x][y+1]=500;return 1;}
                            }
                          }
				            		  if(B[x-1][y+1]==2){
                            if(x2>(laudankoko+14-y2)){
                              if(B[x+1][y-2]==0){PRIORITY[x+1][y-2]=500;return 1;}
                            }else{
                              if(B[x+2][y-1]==0){PRIORITY[x+2][y-1]=500;return 1;}
                            }
                          }
						
                        }
                      }
                      if(B[x+1][y+1]==1){
                        if(((x2!=x+2)||(y2!=y+2))&&((x2!=x-1)||(y2!=y-1))){

                          if(B[x+2][y+2]==2){
                            if(x2>y2){
                              if(B[x-1][y]==0){PRIORITY[x-1][y]=500;return 1;}
                            }else{
                              if(B[x][y-1]==0){PRIORITY[x][y-1]=500;return 1;}
                            }
                          }
						              if(B[x-1][y-1]==2){
                            if(x2>y2){
                              if(B[x+1][y+2]==0){PRIORITY[x+1][y+2]=500;return 1;}
                            }else{
                              if(B[x+2][y+1]==0){PRIORITY[x+2][y+1]=500;return 1;}
                            }
                          }
						
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          return 0;

        case 4:
          uhka=pikahaku(B, 2, 2);
          if(uhka==9)return 0;
          for(y=8;y<laudankoko+8;y++){
            for(x=8;x<laudankoko+8;x++){
              if(B[x][y]==1){
                if((B[x-2][y-2]==0)&&(B[x][y-2]==1)&&(B[x-2][y]==1)){PRIORITY[x-2][y-2]=500;return 1;}
                if((B[x-2][y-2]==1)&&(B[x][y-2]==0)&&(B[x-2][y]==1)){PRIORITY[x][y-2]=500;return 1;}
                if((B[x-2][y-2]==1)&&(B[x][y-2]==1)&&(B[x-2][y]==0)){PRIORITY[x-2][y]=500;return 1;}

                if((B[x][y-2]==0)&&(B[x+2][y-2]==1)&&(B[x+2][y]==1)){PRIORITY[x][y-2]=500;return 1;}
                if((B[x][y-2]==1)&&(B[x+2][y-2]==0)&&(B[x+2][y]==1)){PRIORITY[x+2][y-2]=500;return 1;}
                if((B[x][y-2]==1)&&(B[x+2][y-2]==1)&&(B[x+2][y]==0)){PRIORITY[x+2][y-2]=500;return 1;}

                if((B[x+2][y]==0)&&(B[x+2][y+2]==1)&&(B[x][y+2]==1)){PRIORITY[x+2][y]=500;return 1;}
                if((B[x+2][y]==1)&&(B[x+2][y+2]==0)&&(B[x][y+2]==1)){PRIORITY[x+2][y+2]=500;return 1;}
                if((B[x+2][y]==1)&&(B[x+2][y+2]==1)&&(B[x][y+2]==0)){PRIORITY[x][y+2]=500;return 1;}

                if((B[x-2][y]==0)&&(B[x-2][y+2]==1)&&(B[x+2][y+2]==1)){PRIORITY[x-2][y]=500;return 1;}
                if((B[x-2][y]==1)&&(B[x-2][y+2]==0)&&(B[x+2][y+2]==1)){PRIORITY[x-2][y+2]=500;return 1;}
                if((B[x-2][y]==1)&&(B[x-2][y+2]==1)&&(B[x+2][y+2]==0)){PRIORITY[x+2][y+2]=500;return 1;}
              
              }
            }
          }
          return 0;
      default: return 0;
    }
	  default: return 0;
  }
	return 0;
}


/* pieni parannus erääseen skenarioon, ettei se lopu heti */
int ansamodu(void){
	puoli=2;
	kokovuoro++;
	undo2x=0;undo2y=0;
	undo1x=0;undo1y=0;
	if((A[31][27]==0)){A[edellx=undo1x=31][edelly=undo1y=27]=1; return 0;}
	if((A[35][27]==0)){A[edellx=undo1x=35][edelly=undo1y=27]=1; return 0;}
	if((A[31][27]==1)){if(A[30][27]==0){A[edellx=undo1x=30][edelly=undo1y=27]=1; return 0;}}
	if((A[35][27]==1)){if(A[36][27]==0){A[edellx=undo1x=36][edelly=undo1y=27]=1; return 0;}}
	if((A[21][30]==0)){A[edellx=undo1x=21][edelly=undo1y=30]=1; return 0;}
	if((A[21][29]==0)&&(A[20][29]==0)){A[edellx=undo1x=20][edelly=undo1y=29]=1; return 0;}
	if((A[21][29]==0)&&(A[20][29]==1)){A[edellx=undo1x=21][edelly=undo1y=29]=1; return 0;}
	if((A[20][29]==1)&&(A[22][30]==0)&&(A[22][31]==0)){A[edellx=undo1x=22][edelly=undo1y=31]=1; return 0;}
	if((A[20][29]==1)&&(A[21][30]==1)&&(A[22][31]==1)&&(A[23][32]==0)){A[edellx=undo1x=23][edelly=undo1y=32]=1; return 0;}
	if((A[20][29]==1)&&(A[21][30]==1)&&(A[22][31]==1)&&(A[19][28]==0)){A[edellx=undo1x=19][edelly=undo1y=28]=1; return 0;}
	if((A[19][28]==1)&&(A[18][27]==0)){A[edellx=undo1x=18][edelly=undo1y=27]=1; return 0;}
	if((A[23][32]==1)&&(A[24][33]==0)){A[edellx=undo1x=24][edelly=undo1y=33]=1; return 0;}
	if((A[22][30]==0)&&(A[22][31]==0)){A[edellx=undo1x=22][edelly=undo1y=31]=1; return 0;}
	if((A[22][30]==0)&&(A[22][31]==1)){A[edellx=undo1x=22][edelly=undo1y=30]=1; return 0;}
	if((A[23][30]==0)&&(A[22][31]==1)){A[edellx=undo1x=23][edelly=undo1y=30]=1; return 0;}
	if((A[23][30]==1)&&(A[21][32]==0)){A[edellx=undo1x=21][edelly=undo1y=32]=1; return 0;}
	if((A[23][30]==1)&&(A[26][27]==0)){A[edellx=undo1x=26][edelly=undo1y=27]=1; return 0;}
	if((A[21][29]==0)&&(A[20][29]==1)){A[edellx=undo1x=21][edelly=undo1y=29]=1; return 0;}
	if((A[20][29]==1)&&(A[22][30]==2)&&(A[21][29]==2)&&(A[23][26]==0)){A[edellx=undo1x=23][edelly=undo1y=26]=1; return 0;}
	if((A[20][29]==1)&&(A[22][31]==2)&&(A[21][29]==2)&&(A[23][26]==0)){A[edellx=undo1x=23][edelly=undo1y=26]=1; return 0;}
	if((A[23][26]==1)&&(A[21][28]==0)){A[edellx=undo1x=21][edelly=undo1y=28]=1; return 0;}
	if((A[23][26]==1)&&(A[21][28]==1)&&(A[19][30]==0)){A[edellx=undo1x=19][edelly=undo1y=30]=1; return 0;}
	if((A[23][26]==1)&&(A[21][28]==1)&&(A[24][25]==0)){A[edellx=undo1x=24][edelly=undo1y=25]=1; return 0;}
	if((A[22][25]==0)&&(A[23][26]==1)){A[edellx=undo1x=22][edelly=undo1y=25]=1; return 0;}
	if((A[22][25]==1)&&(A[26][29]==0)){A[edellx=undo1x=26][edelly=undo1y=29]=1; return 0;}
	if((A[22][25]==1)&&(A[20][25]==0)){A[edellx=undo1x=20][edelly=undo1y=25]=1; return 0;}
	if((A[20][25]==1)&&(A[19][25]==0)){A[edellx=undo1x=19][edelly=undo1y=25]=1; return 0;}
	if((A[19][25]==1)&&(A[18][25]==0)){A[edellx=undo1x=18][edelly=undo1y=25]=1; return 0;}
	if((A[19][25]==1)&&(A[23][25]==2)&&(A[20][26]==0)){A[edellx=undo1x=20][edelly=undo1y=26]=1; return 0;}
	if((A[20][26]==1)&&(A[21][27]==0)){A[edellx=undo1x=21][edelly=undo1y=27]=1; return 0;}
	if((A[20][26]==1)&&(A[20][23]==0)){A[edellx=undo1x=20][edelly=undo1y=23]=1; return 0;}
	if((A[20][26]==1)&&(A[20][23]==1)&&(A[20][22]==0)){A[edellx=undo1x=20][edelly=undo1y=22]=1; return 0;}
	if((A[20][26]==1)&&(A[20][27]==0)){A[edellx=undo1x=20][edelly=undo1y=27]=1; return 0;}
	if((A[20][26]==1)&&(A[20][27]==1)&&(A[20][28]==0)){A[edellx=undo1x=20][edelly=undo1y=28]=1; return 0;}
	if((A[19][25]==2)&&(A[20][25]==1)&&(A[20][27]==0)){A[edellx=undo1x=20][edelly=undo1y=27]=1; return 0;}
	if((A[21][28]==1)&&(A[12][19]==1)&&(A[19][26]==0)&&(A[18][25]==0)){A[edellx=undo1x=19][edelly=undo1y=26]=1; return 0;}
	if((A[21][28]==1)&&(A[12][19]==1)&&(A[19][26]==1)&&(A[18][25]==0)){A[edellx=undo1x=18][edelly=undo1y=25]=1; return 0;}
	if((A[21][28]==1)&&(A[12][19]==1)&&(A[19][26]==1)&&(A[23][30]==0)){A[edellx=undo1x=23][edelly=undo1y=30]=1; return 0;}
	if((A[19][28]==0)&&(A[12][19]==1)){A[edellx=undo1x=19][edelly=undo1y=28]=1; return 0;}
	if((A[19][28]==1)&&(A[21][26]==0)){A[edellx=undo1x=21][edelly=undo1y=26]=1; return 0;}
	if((A[19][28]==1)&&(A[21][26]==1)&&(A[18][29]==0)){A[edellx=undo1x=18][edelly=undo1y=29]=1; return 0;}
	if((A[19][28]==1)&&(A[21][26]==1)&&(A[23][24]==0)){A[edellx=undo1x=23][edelly=undo1y=24]=1; return 0;}
	if((A[19][28]==1)&&(A[18][27]==0)){A[edellx=undo1x=18][edelly=undo1y=27]=1; return 0;}
	if((A[18][27]==1)&&(A[17][26]==0)){A[edellx=undo1x=17][edelly=undo1y=26]=1; return 0;}
	if((A[18][27]==1)&&(A[22][31]==0)){A[edellx=undo1x=22][edelly=undo1y=31]=1; return 0;}
	if((A[22][31]==0)){A[edellx=undo1x=22][31]=1; return 0;}
	if((A[22][31]==1)&&(A[23][32]==0)){A[edellx=undo1x=23][edelly=undo1y=32]=1; return 0;}
	if((A[22][31]==1)&&(A[18][27]==0)){A[edellx=undo1x=18][edelly=undo1y=27]=1; return 0;}
	if((A[25][31]==0)&&(A[24][30]==0)){A[edellx=undo1x=25][edelly=undo1y=31]=1; return 0;}
	if((A[25][31]==1)&&(A[24][30]==0)){A[edellx=undo1x=24][edelly=undo1y=30]=1; return 0;}
	if((A[25][31]==1)&&(A[24][30]==1)&&(A[26][32]==0)){A[edellx=undo1x=26][edelly=undo1y=32]=1; return 0;}
	if((A[25][31]==1)&&(A[24][30]==1)&&(A[21][27]==0)){A[edellx=undo1x=21][edelly=undo1y=27]=1; return 0;}
	if((A[25][31]==1)&&(A[28][28]==0)){A[edellx=undo1x=28][edelly=undo1y=28]=1; return 0;}
	pelvscomp=0;
	kokovuoro--;
	puoli=1;
	return 0;
}







int haeparas(void){
	int laskuri1, laskuri2, x, y;
	long int iso=0;
  long int PRIORITY2[64][64];
	int arvottu=0;
	int vakio=0;
	isoinx=0;
	isoiny=0;
/*
  if((kokovuoro==0)&&((pelvscomp==0)||(pelvscomp==3))){
		sijoitanappi((laudankoko/2), (laudankoko/2));
		return 0;
	};
*/
	for(y=0;y<64;y++){
		for(x=0;x<64;x++)PRIORITY2[x][y]=0;
	}
	for(y=0;y<(laudankoko);y++){
		for(x=0;x<(laudankoko);x++)
			PRIORITY2[x][y]=PRIORITY[x+8][y+8];
	}

	for(laskuri2=0;laskuri2<(laudankoko);laskuri2++){
		for(laskuri1=0;laskuri1<(laudankoko);laskuri1++){
    	arvottu=arpa();
			if(arpamaara==0)vakio=0;
			if(arpamaara==1)vakio=7+arvottu;
			if(arpamaara==2)vakio=0.1f*iso;
			if(PRIORITY2[laskuri1][laskuri2]<-1)PRIORITY2[laskuri1][laskuri2] = 32768 * 2 + PRIORITY2[laskuri1][laskuri2];
			if((PRIORITY2[laskuri1][laskuri2]+arvottu+vakio)>iso){
				iso=PRIORITY2[laskuri1][laskuri2];
				isoinx=laskuri1;
				isoiny=laskuri2;
			}
		}
	}
	sijoitanappi(isoinx, isoiny);
	return 0;
}







int mietipaikka(void){
  int x, y, rivi, C[64][64], AA[64][8], CC[16][8], BB[16][8], suunta=0, vpuoli=2, voitto=0;
  long int PISTEET[64], piste=0;
  int tempx, tempy, suunta0=0, rivi0=0, X=0, Y=0, uhka;
  int syvyysmax, sanonta;
  int B[64][64];
  int puoli2=1;

  if(puoli==1)puoli2=2; /* miksihän tämä pitää vaihtaa, joku hämärä bugi */
  if(puoli==2)puoli2=1; /* muuten luulee että kone on nolla kun se on risti... */

  syvyysmax=maxsyvyys;
  sanonta=arpa4();
	for(y=0;y<64;y++){
		for(x=0;x<64;x++)B[x][y]=0;
	}
	for(y=0;y<(laudankoko);y++){
		for(x=0;x<(laudankoko);x++)
			B[x+8][y+8]=A[x][y];
	}

 /* 1. kerros, suoran voitton haku*/

  fastend(PISTEET);
  if(puoli2==1)taulukko1(AA, 1, 2);
  if(puoli2==2)taulukko1(AA, 2, 1); 

  for(rivi=0;rivi<6;rivi++){
    for(y=8;y<laudankoko+8;y++){ 
      for(x=8;x<laudankoko+8;x++){
	      lisaapriority(PRIORITY, AA, x, y, suunta, B, rivi, PISTEET);
      }
    }
  }
  for(y=8;y<laudankoko+8;y++){
    for(x=8;x<laudankoko+8;x++){
      if(PRIORITY[x][y]>1){
	      if((PRIORITY[x][y]<1000)&&(puhe==1)){

          if(sanonta<=1)sprintf(tekstijutska5, "Computer: I have no options...");
          if(sanonta==2)sprintf(tekstijutska5, "Computer: Now you had the straight four. :(");
          if(sanonta==3)sprintf(tekstijutska5, "Computer: I must place the mark here.");
          if(sanonta>=4)sprintf(tekstijutska5, "Computer: There are no question marks.");
        }
        return 2;
      }
    }
  }


 /* 2. kerros, rekursiohaku*/
  if(maxsyvyys!=0){
   if(puoli2==1){taulukko2(CC, BB, 1, 2);vpuoli=2;}
   if(puoli2==2){taulukko2(CC, BB, 2, 1);vpuoli=1;}

   for(tempy=0;tempy<64;tempy++){
    for(tempx=0;tempx<64;tempx++){
     C[tempy][tempx]=B[tempy][tempx];
    }
   }
   X=0;Y=0;
   voitto=rekursiohaku(0, syvyysmax, CC, BB, C, PRIORITY, rivi0, suunta0, X, Y, puoli2);
   for(y=8;y<laudankoko+8;y++){
     for(x=8;x<laudankoko+8;x++){
       if (PRIORITY[x][y]>1){
         if(PRIORITY[x][y]>piste)piste=PRIORITY[x][y];
       }
     }
   }
   for(y=8;y<laudankoko+8;y++){
     for(x=8;x<laudankoko+8;x++){
       if (PRIORITY[x][y]>1){
         if(PRIORITY[x][y]<piste)PRIORITY[x][y]=0;
       }
     }
   }
   for(y=8;y<laudankoko+8;y++){
     for(x=8;x<laudankoko+8;x++){
       if (PRIORITY[x][y]>1){
		     if(puhe==1){
           if(sanonta<=1)sprintf(tekstijutska5, "Computer: GG!");
           if(sanonta==2)sprintf(tekstijutska5, "Computer: I won!!! :D");
           if(sanonta==3)sprintf(tekstijutska5, "Computer: Almost too easy...");
           if(sanonta>=4)sprintf(tekstijutska5, "Computer: You must be sleeping.");
         }
         return 1;
       }
     }
   }
   uhka=pikahaku(B, puoli2, 2);
   if(uhka==9){
     torjuntahaku(B, puoli2, PRIORITY);
     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           
           if(puhe==1)sprintf(tekstijutska5, "Computer: Hmm... Hmm...");
           return 1;
         }
       }
     }
     if(puhe==1){
	    if(sanonta<=1)sprintf(tekstijutska5, "Computer: I'm under a threat.");
	    if(sanonta==2)sprintf(tekstijutska5, "Computer: Ohh... Help me! :)");
	    if(sanonta==3)sprintf(tekstijutska5, "Computer: Tough game...");
	    if(sanonta>=4)sprintf(tekstijutska5, "Computer: You got the straight three.");
     }
   }
   if(uhka!=9){

	  
     rivi0=0;suunta0=0;X=0;Y=0; syvyysmax=maxsyvyys;
     for(tempy=0;tempy<64;tempy++){
      for(tempx=0;tempx<64;tempx++){
       C[tempy][tempx]=B[tempy][tempx];
      }
     }

     if(puoli2==1)taulukko2(CC, BB, 2, 1);
     if(puoli2==2)taulukko2(CC, BB, 1, 2);
     voitto=rekursiohaku(0, syvyysmax, CC, BB, C, PRIORITY, rivi0, suunta0, X, Y, vpuoli);
     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           if(PRIORITY[x][y]>piste)piste=PRIORITY[x][y];
         }
       }
     }
     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           if(PRIORITY[x][y]<piste)PRIORITY[x][y]=0;
         }
       }
     }

     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){ 
         if (PRIORITY[x][y]>1){
           if(puhe==1){
             if(sanonta<=1)sprintf(tekstijutska5, "Computer: Let's ruin your game here...");
             if(sanonta==2)sprintf(tekstijutska5, "Computer: Hmm... I see your point.");
             if(sanonta==3)sprintf(tekstijutska5, "Computer: I can read your mind. ;)");
             if(sanonta>=4)sprintf(tekstijutska5, "Computer: You have a little trap here.");
           }
           return 1;
         }
       }
     }

	  
	   rivi0=0;suunta0=0;X=0;Y=0;  syvyysmax=maxsyvyys;
     for(tempy=0;tempy<64;tempy++){
      for(tempx=0;tempx<64;tempx++){
       C[tempy][tempx]=B[tempy][tempx];
      }
     }
     if(puoli2==1)taulukko3(CC, BB, 1, 2);
     if(puoli2==2)taulukko3(CC, BB, 2, 1);
     voitto=rekursiohaku2(0, syvyysmax, CC, BB, C, PRIORITY, rivi0, suunta0, X, Y, puoli2);

     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           if(PRIORITY[x][y]>piste)piste=PRIORITY[x][y];
         }
       }
     }
     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           if(PRIORITY[x][y]<piste)PRIORITY[x][y]=0;
         }
       }
     }
    
     for(y=8;y<laudankoko+8;y++){
       for(x=8;x<laudankoko+8;x++){
         if (PRIORITY[x][y]>1){
           if(puhe==1){
             if(sanonta<=1)sprintf(tekstijutska5, "Computer: Now be careful!");
             if(sanonta==2)sprintf(tekstijutska5, "Computer: Hehehee...");
             if(sanonta==3)sprintf(tekstijutska5, "Computer: Hmmmmm... Let's see...");
             if(sanonta>=4)sprintf(tekstijutska5, "Computer: I recommend to concentrate.");
           }
         return 1;
         }
       }
     }
   }
  }
  uhka=0;

/* 3. kerros, prioriteettihaku*/
  if(tyyli==1)attacker(PISTEET);
  if(tyyli==2)defender(PISTEET);
  if(tyyli==3)preasure(PISTEET);
  if(tyyli==4)tbuilder(PISTEET);
  if(puoli2==1)taulukko4(AA, 1, 2);
  if(puoli2==2)taulukko4(AA, 2, 1);
  for(rivi=0;rivi<40;rivi++){ 
/*  for(rivi=29;rivi<32;rivi++){ *//* vain rekursion testaukseen */
    for(y=8;y<laudankoko+8;y++){
      for(x=8;x<laudankoko+8;x++){
        lisaapriority(PRIORITY, AA, x, y, suunta, B, rivi, PISTEET);
      }
    }
  }
  for(y=8;y<laudankoko+8;y++){
    for(x=8;x<laudankoko+8;x++){
      if (PRIORITY[x][y]>1){
        return 1;
      }
    }
  }

  for(y=9;y<laudankoko+7;y++){
    for(x=9;x<laudankoko+7;x++){
       if(B[x][y]==0){
          PRIORITY[x][y]=50;
          if(B[x+1][y]!=0)PRIORITY[x][y]+=100;
          if(B[x-1][y]!=0)PRIORITY[x][y]+=100;
          if(B[x][y-1]!=0)PRIORITY[x][y]+=100;
          if(B[x][y+1]!=0)PRIORITY[x][y]+=100;
          if(B[x+1][y+1]!=0)PRIORITY[x][y]+=100;
          if(B[x-1][y+1]!=0)PRIORITY[x][y]+=100;
          if(B[x+1][y-1]!=0)PRIORITY[x][y]+=100;
          if(B[x-1][y-1]!=0)PRIORITY[x][y]+=100;
       }
    }
  }
  if(puhe==1){
    if(sanonta<=1)sprintf(tekstijutska5, "Computer: You have no good places!");
    if(sanonta==2)sprintf(tekstijutska5, "Computer: Try to create something, please.");
    if(sanonta==3)sprintf(tekstijutska5, "Computer: Not going very well...");
    if(sanonta>=4)sprintf(tekstijutska5, "Computer: Boring...");
  }

	return 0;
}







































/* <<<<<<<<   Random Boardin (scenario valikko) teko >>>>>>>>>>*/

void luoarvottulauta(void){
	int silmu, onnistu;
	int rarvont1=0, rarvont2=0, rarvont3=0, rarvont4=0, rarvont5=0;
	time_t aika2;
	struct tm aika;
	aika2=time(&aika2);
	aika=*localtime(&aika2);
	srand(arvottu3*10+arvottu2*10+(aika.tm_sec+1));
	nollaus(A, PRIORITY);
	kokovuoro=0;
	rarvont1=((rand()%(20))+10);
	srand((aika.tm_sec+1)*(rarvont1+3));
	rarvont2=((rand()%(20))+10);

	if(randpuoli==0){
		pelvscomp=1;
		puoli=1;
	}
	if(randpuoli==1){
		pelvscomp=0;
		puoli=2;
		A[rarvont1][rarvont2]=1;
	}
	srand((aika.tm_sec+1)*(rarvont1+rarvont2+2));
	rarvont5=(rand()%(3)+1);
	for(silmu=1;silmu<((randmaara+2)*4+1+rarvont5);silmu++){
		srand(rarvont1*rarvont1+1+arvottu3);
		if(randvaik==0)rarvont2=((rand()%(20))+10);
		if((randvaik>0)&&(randpuoli==0))rarvont2=((rand()%(20))+10);
		if((randvaik==1)&&(randpuoli==1))rarvont2=((rand()%(15))+15);
		if((randvaik==2)&&(randpuoli==1))rarvont2=((rand()%(10))+20);
		srand(rarvont2*rarvont2+rarvont1);
		if(randvaik==0)rarvont3=((rand()%(20))+10);
		if((randvaik>0)&&(randpuoli==0))rarvont3=((rand()%(20))+10);
		if((randvaik==1)&&(randpuoli==1))rarvont3=((rand()%(15))+15);
		if((randvaik==2)&&(randpuoli==1))rarvont3=((rand()%(10))+20);
		srand(rarvont3*rarvont3+rarvont1);
		if(randvaik==0)rarvont4=((rand()%(20))+10);
		if((randvaik>0)&&(randpuoli==1))rarvont4=((rand()%(20))+10);
		if((randvaik==1)&&(randpuoli==0))rarvont4=((rand()%(15))+15);
		if((randvaik==2)&&(randpuoli==0))rarvont4=((rand()%(10))+20);
		srand(rarvont4*rarvont4+rarvont2);
		if(randvaik==0)rarvont1=((rand()%(20))+10);
		if((randvaik>0)&&(randpuoli==1))rarvont1=((rand()%(20))+10);
		if((randvaik==1)&&(randpuoli==0))rarvont1=((rand()%(15))+15);
		if((randvaik==2)&&(randpuoli==0))rarvont1=((rand()%(10))+20);
		srand(rarvont3+12);
		onnistu=0;
		while(onnistu==0){
			if((A[rarvont2][rarvont3]==0)&&A[rarvont1][rarvont4]==0){
				A[rarvont2][rarvont3]=1;
				A[rarvont3][rarvont4]=2;
				kokovuoro+=2;
				onnistu=1;
			}
			if(onnistu==0){
				if((A[rarvont3][rarvont2]==0)&&A[rarvont4][rarvont1]==0){
					A[rarvont3][rarvont2]=1;
					A[rarvont4][rarvont1]=2;
					kokovuoro+=2;
					onnistu=1;
				}
			}
			srand(rarvont1*rarvont4);
			if(randvaik==0)rarvont2=((rand()%(20))+10);
			if((randvaik>0)&&(randpuoli==1))rarvont2=((rand()%(20))+10);
			if((randvaik==1)&&(randpuoli==0))rarvont2=((rand()%(15))+15);
			if((randvaik==2)&&(randpuoli==0))rarvont2=((rand()%(10))+20);
			srand(rarvont2*rarvont1);
			if(randvaik==0)rarvont3=((rand()%(20))+10);
			if((randvaik>0)&&(randpuoli==1))rarvont3=((rand()%(20))+10);
			if((randvaik==1)&&(randpuoli==0))rarvont3=((rand()%(15))+15);
			if((randvaik==2)&&(randpuoli==0))rarvont3=((rand()%(10))+20);
			srand(rarvont3*rarvont2+1);
			if(randvaik==0)rarvont4=((rand()%(20))+10);
			if((randvaik>0)&&(randpuoli==0))rarvont4=((rand()%(20))+10);
			if((randvaik==1)&&(randpuoli==1))rarvont4=((rand()%(15))+15);
			if((randvaik==2)&&(randpuoli==1))rarvont4=((rand()%(10))+20);
			srand(rarvont4*rarvont4+3);
			if(randvaik==0)rarvont1=((rand()%(20))+10);
			if((randvaik>0)&&(randpuoli==0))rarvont1=((rand()%(20))+10);
			if((randvaik==1)&&(randpuoli==1))rarvont1=((rand()%(15))+15);
			if((randvaik==2)&&(randpuoli==1))rarvont1=((rand()%(10))+20);
			srand(rarvont1);
		}
	}
}
























/*  <<<<<<<<<<<< Hiiren käsittely >>>>>>>>>  */



static void hiiri( int button, int state, int mouseX, int mouseY){
	double x=-1, y=1, z=0;
	double objx,objy,objz;
	double modelMatrix[16], projMatrix[16];
	int viewport[4];
	int paikkax=0, paikkay=0, apu=0, arvotus=0; /*, paluutark=0;*/

	z=getzeta();
	if((peliloppu==0)&&(viivaon==0)){
		if(((pelvscomp==2)||((pelvscomp==0)&&(puoli==2))||((pelvscomp==1)&&(puoli==1)))||pelvscomp>600) {
			if ((button==GLUT_LEFT_BUTTON)&&(state==GLUT_DOWN)) {

				/* kiva koordinaattimuutos, */
				glGetDoublev(GL_MODELVIEW_MATRIX, modelMatrix);
				glGetDoublev(GL_PROJECTION_MATRIX, projMatrix);
				glGetIntegerv(GL_VIEWPORT,viewport);
				if (gluUnProject(mouseX,mouseY,0,modelMatrix,projMatrix,viewport,&objx,&objy,&objz)==GL_TRUE){
				/* Se toimii! (uskomatonta)                  */
				/* Mutta toisaalta se ei toimi, jos käyttäjä */
				/* valitsee alussa väärän resoluution.       */
					
					for(y-=z*0.45f;y>objy;y-=z)paikkay++;
					for(x+=z*0.45f;x<objx;x+=z)paikkax++;
				
					sijoitanappi(paikkax, paikkay);
					if(pelvscomp==665){
            arvotus=ansamodu();
						if(puoli==1){apu=1;puoli=2;}
						if(puoli==2){apu=2;puoli=1;}
						voitontarkistus();
						puoli=apu;
					}
					/* ja jos tietokone mukana, niin se lataa saman tien omansa */
          if((akuva==0)&&(peliloppu==0)){
            if(((pelvscomp==0)&&(puoli==1))||((pelvscomp==1)&&(puoli==2))){
              sprintf (tekstijutska5, "Thinking. Wait a year. (Menu: Computer-Thinking time)");
            }
          }
					glutPostRedisplay();	/* refreshiä taas  */
				}
			}
		}
	}
}


















/*   <<<<<<<<<<<   Kivat pikku menut >>>>>>>>>>>>>>>> */



void restoredefaults(void); /* nämä on pakko alustaa */
void optionsluku(void);
void optionstallennus(void);




/* Näihin on tehty käsin näyttämään mikä on valittu tällä hetkellä, */
/* koska glutissa ei ollut siihen valmiita rutiineja, mutta se on   */
/* käyttäjän kannalta aivan ehdotonta nähdä.                        */

/* Pitkien testien jälkeen ilmeni, että 3 tyhjää merkkiä on yhtä    */
/* pitkä jono kuin yksi plus-merkki ja yksi tyhjä merkki.			*/
/* Niissä on edessä 3 näkymätöntä merkkiä (toimii myös SGI:ssä),    */
/* jotka eivät ole välilyöntejä. (Välilyönnit eivät näy menujen		*/
/* alussa). Valitun menun tekstin edessä on siis plus-merkki.       */

static void menu_teema(int arvo){
  menuvalinta[0]=arvo;
	if(akuva==1){
		akuva=0;
	}
	glutSetMenu(teema);
	glutChangeToMenuEntry(1, "   Default", 1);
	glutChangeToMenuEntry(2, "   Light", 2);
	glutChangeToMenuEntry(3, "   Stone", 3);
	glutChangeToMenuEntry(4, "   Wood", 4);
	glutChangeToMenuEntry(5, "   Wall", 5);
	glutChangeToMenuEntry(6, "   Newspaper", 6);
	glutChangeToMenuEntry(7, "   Black", 7);
    switch (arvo) {
      case 1:
		  glutChangeToMenuEntry(1, "+ Default", 1);
		  ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		  break;
      case 2:
		  glutChangeToMenuEntry(2, "+ Light", 2);
		  ruudunpiirto("tumviol.thp","puunorm.thp","hiekkat.thp",0);
		  break;
      case 3:
		  glutChangeToMenuEntry(3, "+ Stone", 3);
		  ruudunpiirto("pilvi.thp","puutumm.thp","kivimarm.thp",0);
		  break;
      case 4:
		  glutChangeToMenuEntry(4, "+ Wood", 4);
		  ruudunpiirto("kivigran.thp","puuvaal.thp","hiekkav.thp",0);
		  break;
      case 5:
		  glutChangeToMenuEntry(5, "+ Wall", 5);
		  ruudunpiirto("tiili.thp","tiili.thp","tiili.thp",1);
		  break;
      case 6:
		  glutChangeToMenuEntry(6, "+ Newspaper", 6);
		  ruudunpiirto("lehti.thp","lehti.thp","lehti.thp",1);
		  break;
	  case 7:
		  glutChangeToMenuEntry(7, "+ Black", 7);
		  if(leiaut==0)ruudunpiirto("musta.thp","musta.thp","musta.thp",1);
		  break;
	}
	glutPostRedisplay();
}




static void menu_pelaajat(int arvo){
	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
		akuva=0;
	}
	peliloppu=0;
	nollaus(A, PRIORITY);
	viivaon=0;
  kokovuoro=0;
	tekstit=0;
	puoli=1;
    switch (arvo) {
      case 21:
   		  nollaus(A, PRIORITY);
		  kokovuoro=0;
		  puoli=1;
		  pelvscomp=0;
		  break;
      case 22:
   		  nollaus(A, PRIORITY);
		  kokovuoro=0;
		  puoli=1;
		  pelvscomp=1;
		  break;
      case 23:
   		  nollaus(A, PRIORITY);
		  kokovuoro=0;
		  puoli=1;
		  pelvscomp=2;
		  break;
      case 24:
   		  nollaus(A, PRIORITY);
		  kokovuoro=0;
		  puoli=1;
		  pelvscomp=3;
		  break;
	}
	glutPostRedisplay ();
}


static void menu_settinki(int arvo){
    switch (arvo) {
      case 201:
        optionsluku();
      break;
      case 202:
        optionstallennus();
      break;
      case 203:
        restoredefaults();
      break;
	}
  glutPostRedisplay();
}

static void menu_lopetus(int arvo){
    switch (arvo) {
      case 31:
		  glutPostRedisplay();
		  break;
      case 32:
		  if(akuva==1){
			  ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
			  glutPostRedisplay();
			  akuva=0;
		  }
		  nollaus(A, PRIORITY);
		  kokovuoro=0;
		  puoli=1;
		  viivaon=0;
      if(alotyyli==5)alotyyli=arpa4();
		  peliloppu=0;
		  tekstit=0;
		  glutPostRedisplay();
		  break;
      case 33:
  		  printf("\n Ok.\n\n\n Thanks for using THO-MOKU OpenGL\n\n");
        optionstallennus();
		  exit(0);
      case 34:
  		  printf("\n Ok.\n\n\n Thanks for using THO-MOKU OpenGL\n\n");
		  exit(0);
	}
}

static void menu_lautakoko(int arvo){
	/* int scroll=0; */
  menuvalinta[1]=arvo;
	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
		akuva=0;
	}
	nollaus(A, PRIORITY);
	glutSetMenu(lautakoko);
	glutChangeToMenuEntry(1, "   40x40 (Default)", 11);
	glutChangeToMenuEntry(2, "   20x20", 12);
	glutChangeToMenuEntry(3, "   16x16", 13);
	glutChangeToMenuEntry(4, "   15x15 (Standard)", 14);
/*	glutChangeToMenuEntry(5, "     3x3", 15);  */
/*	glutChangeToMenuEntry(6, "   Unlimited (Scroll)", 16);  */

	kokovuoro=0;
	puoli=1;
    switch (arvo) {
      case 11:
		  laudankoko=40;
/*		  scroll=0; */
		  glutChangeToMenuEntry(1, "+ 40x40 (Default)", 11);
		  kokovuoro=0;
		  viivaon=0;

		  puoli=1;
		  break;
      case 12:
		  laudankoko=20;
/*		  scroll=0; */
  		  nollaus(A, PRIORITY);
		  viivaon=0;

		  glutChangeToMenuEntry(2, "+ 20x20", 12);
		  kokovuoro=0;
		  puoli=1;
		  break;
      case 13:
		  laudankoko=16;
/*		  scroll=0; */
  		  nollaus(A, PRIORITY);
		  viivaon=0;

		  glutChangeToMenuEntry(3, "+ 16x16", 13);
		  kokovuoro=0;
		  puoli=1;
		  break;
      case 14:
		  laudankoko=15;
/*		  scroll=0; */
  		  nollaus(A, PRIORITY);
		  viivaon=0;

      glutChangeToMenuEntry(4, "+ 15x15 (Standard)", 14);
		  kokovuoro=0;
		  puoli=1;
		  break;
/*      case 15:
		  laudankoko=3;
		  scroll=0;
		  glutChangeToMenuEntry(2, "+  3x3", 12);
		  viivaon=0;
		  kokovuoro=0;
		  puoli=1;
		  break; */
/*      case 16:
		  laudankoko=40;
		  scroll=1;
		  viivaon=0;
		  glutChangeToMenuEntry(2, "+ Unlimited (Scroll)", 12);
		  kokovuoro=0;
		  puoli=1;
		  break; */
	}
	glutPostRedisplay ();
}

static void menu_tyyli(int arvo){
  menuvalinta[2]=arvo;
	glutSetMenu(ktyyli);
	glutChangeToMenuEntry(1, "   Aggressive Attacker", 41);
	glutChangeToMenuEntry(2, "   Defence Master", 42);
	glutChangeToMenuEntry(3, "   Passive Pressurer", 43);
	glutChangeToMenuEntry(4, "   Nasty Trap Builder", 44);
	glutChangeToMenuEntry(5, "   Random", 45);
/*	glutChangeToMenuEntry(6, "   Custom: player1.cmp", 46);  */
/*	glutChangeToMenuEntry(7, "   Custom: player2.cmp", 46);  */
/*	glutChangeToMenuEntry(8, "   Custom: player3.cmp", 46);  */
    switch (arvo) {
      case 41:
		  glutChangeToMenuEntry(1, "+ Aggressive Attacker", 41);
		  tyyli=1;
		  break;
      case 42:
		  glutChangeToMenuEntry(2, "+ Defence Master", 42);
		  tyyli=2;
		  break;
      case 43:
		  glutChangeToMenuEntry(3, "+ Passive Pressurer", 43);
		  tyyli=3;
		  break;
      case 44:
		  glutChangeToMenuEntry(4, "+ Nasty Trap Builder", 44);
		  tyyli=4;
		  break;
      case 45:
		  glutChangeToMenuEntry(5, "+ Random", 45);
		  tyyli=arpa();
		  break;
/*      case 46:
		  glutChangeToMenuEntry(6, "+ Custom: player1.cmp", 45);
		  tyyli=5;
		  break; */
/*      case 47:
		  glutChangeToMenuEntry(7, "+ Custom: player2.cmp", 46);
		  tyyli=6;
		  break; */
/*      case 48:
		  glutChangeToMenuEntry(8, "+ Custom: player3.cmp", 47);
		  tyyli=7;
		  break; */
	}
	glutPostRedisplay();
}

static void menu_styyli(int arvo){
  menuvalinta[3]=arvo;
	glutSetMenu(kstyyli);
	glutChangeToMenuEntry(1, "   Random", 55);
	glutChangeToMenuEntry(2, "   Basic", 51);
	glutChangeToMenuEntry(3, "   THO-MOKU Special", 52);
	glutChangeToMenuEntry(4, "   Breadfan's special", 53);
	glutChangeToMenuEntry(5, "   Playsite Big X", 54);
    switch (arvo) {
      case 51:
		  glutChangeToMenuEntry(2, "+ Basic", 51);
		  alotyyli2=1;
		  alotyyli=1;
		  break;
      case 52:
		  glutChangeToMenuEntry(3, "+ THO-MOKU Special", 52);
		  alotyyli2=2;
		  alotyyli=2;
		  break;
      case 53:
		  glutChangeToMenuEntry(4, "+ Breadfan's special", 53);
		  alotyyli2=3;
		  alotyyli=3;
		  break;
      case 54:
		  glutChangeToMenuEntry(5, "+ Playsite Big X", 54);
		  alotyyli2=4;
		  alotyyli=4;
		  break;
      case 55:
		  glutChangeToMenuEntry(1, "+ Random", 54);
		  alotyyli2=5;
      alotyyli=2;
		  break;
	}
}

static void menu_arvonta(int arvo){
  menuvalinta[4]=arvo;
	glutSetMenu(karvonta);
	glutChangeToMenuEntry(1, "   Normal", 61);
	glutChangeToMenuEntry(2, "   High (too easy)", 62);
	glutChangeToMenuEntry(3, "   Very High (too easy)", 63);
	switch (arvo) {
      case 61:
		  glutChangeToMenuEntry(1, "+ Normal", 61);
		  arpamaara=0;
		  break;
      case 62:
		  glutChangeToMenuEntry(2, "+ High (too easy)", 62);
		  arpamaara=1;
		  break;
      case 63:
		  glutChangeToMenuEntry(3, "+ Very High (too easy)", 63);
		  arpamaara=2;
		  break;
	}
}

static void menu_ajatus(int arvo){
  menuvalinta[5]=arvo;
	glutSetMenu(kajatus);
	glutChangeToMenuEntry(1, "   Easy: Only this move (Fast)", 71);
	glutChangeToMenuEntry(2, "   Easy: 2 moves (Fast)", 72);
	glutChangeToMenuEntry(3, "   Normal: 3 moves (Medium)", 73);
	glutChangeToMenuEntry(4, "   Hard: 4 moves (Medium)", 74);
	glutChangeToMenuEntry(5, "   Hard: 5 moves (Slow)", 75);
	glutChangeToMenuEntry(6, "   Hard: 6 moves (Slow)", 76);
	glutChangeToMenuEntry(7, "   Hard: 7 moves (Slow)", 77);
	glutChangeToMenuEntry(8, "   Hard: 8 moves (Slow)", 78);
    switch (arvo) {
      case 71:
		  glutChangeToMenuEntry(1, "+ Easy: Only this move (Fast)", 71);
      maxsyvyys=0;
		  break;
      case 72:
		  glutChangeToMenuEntry(2, "+ Easy: 2 moves (Fast)", 72);
      maxsyvyys=2;
		  break;
      case 73:
		  glutChangeToMenuEntry(3, "+ Normal: 3 moves (Medium)", 73);
      maxsyvyys=3;
		  break;
      case 74:
		  glutChangeToMenuEntry(4, "+ Hard: 4 moves (Medium)", 74);
      maxsyvyys=4;
		  break;
      case 75:
		  glutChangeToMenuEntry(5, "+ Hard: 5 moves (Slow)", 75);
      maxsyvyys=5;
		  break;
      case 76:
		  glutChangeToMenuEntry(6, "+ Hard: 6 moves (Slow)", 76);
      maxsyvyys=6;
		  break;
      case 77:
		  glutChangeToMenuEntry(7, "+ Hard: 7 moves (Slow)", 77);
      maxsyvyys=7;
		  break;
      case 78:
		  glutChangeToMenuEntry(8, "+ Hard: 8 moves (Slow)", 78);
      maxsyvyys=8;
		  break;
	}
}

static void menu_oppi(int arvo){
  menuvalinta[6]=arvo;
	glutSetMenu(koppi);
    switch (arvo) {
      case 81:
		  glutChangeToMenuEntry(1, "+ Disabled", 81);
		  glutChangeToMenuEntry(2, "   Enabled", 82);
      puhe=0;
		  break;
      case 82:
		  glutChangeToMenuEntry(1, "   Disabled", 81);
		  glutChangeToMenuEntry(2, "+ Enabled", 82);
      puhe=1;
		  break;
	}
}

static void menu_looppi(int arvo){
  menuvalinta[7]=arvo;
	glutSetMenu(klooppi);
    switch (arvo) {
      case 83:
		  glutChangeToMenuEntry(1, "+ Disabled", 83);
		  glutChangeToMenuEntry(2, "   Enabled", 84);
		  loopmode=0;
		  break;
      case 84:
		  glutChangeToMenuEntry(1, "   Disabled", 83);
		  glutChangeToMenuEntry(2, "+ Enabled", 84);
		  loopmode=1;
		  break;
	}
}

static void menu_rulet(int arvo){
  menuvalinta[8]=arvo;
	glutSetMenu(saannot);
	glutChangeToMenuEntry(1, "   Free-style Go-moku (THO-MOKU DEFAULT)", 160);
	glutChangeToMenuEntry(2, "   Standard Go-moku (Only line of exactly 5 wins)", 161);
	glutChangeToMenuEntry(3, "   Renju (cf. Sakata and Ikawa 1981)", 162);
	glutChangeToMenuEntry(4, "   Professional Go-moku (Learn the rules first)", 163);
	glutChangeToMenuEntry(5, "   Professional Renju (Side swapping, etc.)", 164);
/*	glutChangeToMenuEntry(6, "   Custom Rules, 165); */
    switch (arvo) {
      case 160:
		  rulet=1;
		  glutChangeToMenuEntry(1, "+ Free-style Go-moku (THO-MOKU DEFAULT)", 160);
		  break;
      case 161:
		  rulet=2;
		  glutChangeToMenuEntry(2, "+ Standard Go-moku (Only line of exactly 5 wins)", 161);
		  break;
      case 162:
		  rulet=3;
		  glutChangeToMenuEntry(3, "+ Renju (cf. Sakata and Ikawa 1981)", 162);
		  break;
      case 163:
		  rulet=4;
		  glutChangeToMenuEntry(4, "+ Professional Go-moku (Learn the rules first)", 163);
		  break;
      case 164:
		  rulet=5;
		  glutChangeToMenuEntry(5, "+ Professional Renju (Side swapping, etc.)", 164);
		  break;
/*    case 165:
		  rulet=6;
		  glutChangeToMenuEntry(6, "+ Custom Rules, 165);
		  break;
*/
	}
	glutPostRedisplay();
}

static void menu_vsken(int arvo){
	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
	}
  akuva=0;
	glutSetMenu(lautakoko);
	glutChangeToMenuEntry(1, "+ 40x40 (Default)", 11);
	glutChangeToMenuEntry(2, "   20x20", 12);
	glutChangeToMenuEntry(3, "   16x16", 13);
	glutChangeToMenuEntry(4, "   15x15 (Standard)", 14);
/*	glutChangeToMenuEntry(5, "     3x3", 15);  */
/*	glutChangeToMenuEntry(6, "   Unlimited (Scroll)", 16);  */
	laudankoko=40;
	peliloppu=0;
	kokovuoro=0;
	tekstit=0;
    switch (arvo) {
      case 100:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen001.tsc");
		  break;
      case 101:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen002.tsc");
		  break;
      case 102:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen003.tsc");
		  break;
      case 103:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen004.tsc");
		  break;
      case 104:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen005.tsc");
		  break;
      case 105:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen006.tsc");
		  break;
      case 106:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen007.tsc");
		  break;
      case 107:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen008.tsc");
		  break;
      case 108:
   		  nollaus(A, PRIORITY);
		  viivaon=0;
		  skenaarionluku("scen009.tsc");
		  break;
      case 109:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen010.tsc");
		  break;
      case 110:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("scen011.tsc");
		  break;
	}
	glutPostRedisplay();
}

static void menu_msken(int arvo){
	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
		akuva=0;
	}
	glutSetMenu(lautakoko);
	glutChangeToMenuEntry(1, "+ 40x40 (Default)", 11);
	glutChangeToMenuEntry(2, "   20x20", 12);
	glutChangeToMenuEntry(3, "   16x16", 13);
	glutChangeToMenuEntry(4, "   15x15 (Standard)", 14);
/*	glutChangeToMenuEntry(5, "     3x3", 15);  */
/*	glutChangeToMenuEntry(6, "   Unlimited (Scroll)", 16);  */
	laudankoko=40;
	peliloppu=0;
	kokovuoro=0;
	tekstit=0;
    switch (arvo) {
      case 111:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("ownscen1.tsc");
		  break;
      case 112:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("ownscen.tsc");
		  break;
      case 113:
		  viivaon=0;
   		  nollaus(A, PRIORITY);
		  skenaarionluku("ownscen3.tsc");
		  break;
      case 114:
   		  nollaus(A, PRIORITY);
		  viivaon=0;
		  skenaarionluku("ownscen4.tsc");
		  break;
	}
	glutPostRedisplay();
}

static void menu_huijaus(int arvo){

	int apu=0;
  menuvalinta[9]=arvo;
	glutSetMenu(huijaus);
    switch (arvo) {
      case 91:
		  glutChangeToMenuEntry(1, "+ Disable cheats", 91);
		  glutChangeToMenuEntry(2, "   Enable cheats", 92);
		  glutChangeToMenuEntry(3, "   (Undo last move)", 93);
		  glutChangeToMenuEntry(4, "   (Show computers thoughts)", 93);
		  primatriisi=0;
		  break;
      case 92:
		  glutChangeToMenuEntry(1, "   Disable cheats", 91);
		  glutChangeToMenuEntry(2, "+ Enable cheats", 92);
		  glutChangeToMenuEntry(3, "   Undo last move!", 94);
		  glutChangeToMenuEntry(4, "   Show computers thoughts!", 95);
		  break;
      case 93:
		  break;
      case 94:
		  if((undo1x!=0)&&(undo1y!=0)&&(undo2x!=0)&&(undo2y!=0)&&(kokovuoro>1)){
			  A[undo1x][undo1y]=0;
			  A[undo2x][undo2y]=0;
			  kokovuoro-=2;
		  }
		  break;
      case 95:
		  apu=primatriisi;
		  primatriisi=1;
		  glutChangeToMenuEntry(4, "+ Show computers thoughts!", 95);
		  if(apu==1){
			  primatriisi=0;
			  glutChangeToMenuEntry(4, "   Show computers thoughts!", 95);
		  }
		  break;
	}
	glutPostRedisplay();
}

static void menu_leiaut(int arvo){
  menuvalinta[10]=arvo;

	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
		akuva=0;
	}
	glutSetMenu(leiautti);
    switch (arvo) {
      case 85:
		  glutChangeToMenuEntry(1, "+ THO-MOKU (Default)", 85);
		  glutChangeToMenuEntry(2, "   Go-Moku/Renju", 86);
		  glutSetMenu(pelaajapuol);
		  if(randpuoli==0){
			  glutChangeToMenuEntry(1, "+ X", 156);
			  glutChangeToMenuEntry(2, "   0", 157);
		  }
		  if(randpuoli==1){
			  glutChangeToMenuEntry(1, "   X", 156);
			  glutChangeToMenuEntry(2, "+ 0", 157);
		  }
		  leiaut=0;
		  break;
      case 86:
		  glutChangeToMenuEntry(1, "   THO-MOKU (Default)", 85);
		  glutChangeToMenuEntry(2, "+ Go-Moku/Renju", 86);
		  glutSetMenu(pelaajapuol);
		  if(randpuoli==0){
			  glutChangeToMenuEntry(1, "+ Black", 156);
			  glutChangeToMenuEntry(2, "   White", 157);
		  }
		  if(randpuoli==1){
			  glutChangeToMenuEntry(1, "   Black", 156);
			  glutChangeToMenuEntry(2, "+ White", 157);
		  }
		  leiaut=1;
		  if(taustakv3[0]=='m'){
			  ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
			  glutSetMenu(teema);
			  glutChangeToMenuEntry(1, "+ Default", 1);
			  glutChangeToMenuEntry(2, "   Light", 2);
			  glutChangeToMenuEntry(3, "   Stone", 3);
			  glutChangeToMenuEntry(4, "   Wood", 4);
			  glutChangeToMenuEntry(5, "   Wall", 5);
			  glutChangeToMenuEntry(6, "   Newspaper", 6);
			  glutChangeToMenuEntry(7, "   Black", 7);

		  }
		  break;
	}
	glutPostRedisplay();
}

static void menu_vuoroja(int arvo){
  menuvalinta[11]=arvo;
	glutSetMenu(vuoroja);
	glutChangeToMenuEntry(1, "   Few", 150);
	glutChangeToMenuEntry(2, "   Normal", 151);
	glutChangeToMenuEntry(3, "   Many", 152);
    switch (arvo) {
      case 150:
		  glutChangeToMenuEntry(1, "+ Few", 150);
		  randmaara=0;
		  break;
      case 151:
		  glutChangeToMenuEntry(2, "+ Normal", 151);
		  randmaara=1;
		  break;
      case 152:
		  glutChangeToMenuEntry(3, "+ Many", 152);
		  randmaara=2;
		  break;
	}
}

static void menu_vaikeus(int arvo){
  menuvalinta[12]=arvo;
	glutSetMenu(taso);
	glutChangeToMenuEntry(1, "   Hard", 153);
	glutChangeToMenuEntry(2, "   Medium", 154);
	glutChangeToMenuEntry(3, "   Easy", 155);
    switch (arvo) {
      case 153:
		  glutChangeToMenuEntry(1, "+ Hard", 153);
		  randvaik=0;
		  break;
      case 154:
		  glutChangeToMenuEntry(2, "+ Medium", 154);
		  randvaik=1;
		  break;
      case 155:
		  glutChangeToMenuEntry(3, "+ Easy", 155);
		  randvaik=2;
		  break;
	}
	glutPostRedisplay();
}

static void menu_nappi(int arvo){
  menuvalinta[13]=arvo;
	glutSetMenu(pelaajapuol);
    switch (arvo) {
      case 156:
		  if(leiaut==0){
			  glutChangeToMenuEntry(1, "+ X", 156);
			  glutChangeToMenuEntry(2, "   O", 157);
		  }
		  if(leiaut==1){
			  glutChangeToMenuEntry(1, "+ Black", 156);
			  glutChangeToMenuEntry(2, "   White", 157);
		  }
		  randpuoli=0;
		  break;
      case 157:
		  if(leiaut==0){
			  glutChangeToMenuEntry(1, "   X", 156);
			  glutChangeToMenuEntry(2, "+ O", 157);
		  }
		  if(leiaut==1){
			  glutChangeToMenuEntry(1, "   Black", 156);
			  glutChangeToMenuEntry(2, "+ White", 157);
		  }

		  randpuoli=1;
		  break;
	}
	glutPostRedisplay();
}

static void menu_random(int arvo){
	if(akuva==1){
		ruudunpiirto("kivibron.thp","tumviol.thp","harmaa.thp",0);
		glutPostRedisplay();
		akuva=0;
	}
    switch (arvo) {
      case 180:
		  glutSetMenu(lautakoko);
		  glutChangeToMenuEntry(1, "+ 40x40 (Default)", 11);
		  glutChangeToMenuEntry(2, "   20x20", 12);
		  glutChangeToMenuEntry(3, "   16x16", 13);
		  glutChangeToMenuEntry(4, "   15x15 (Standard)", 14);
/*		  glutChangeToMenuEntry(5, "     3x3", 15);  */
/*		  glutChangeToMenuEntry(6, "   Unlimited (Scroll)", 16);  */
		  peliloppu=0;
		  nollaus(A, PRIORITY);
		  viivaon=0;

		  kokovuoro=0;
		  tekstit=0;
		  laudankoko=40;
		  luoarvottulauta();
		  break;
	}
	glutPostRedisplay();
}

static void menu_sken(int arvo){
}

static void menu_maisema(int arvo){
  menuvalinta[14]=arvo;
	glutSetMenu(maisemat);
    switch (arvo) {
      case 170:
		  distext=0;
		  glutChangeToMenuEntry(3, "+ Textures Enabled (Game is slow on software-OpenGL)", 170);
		  glutChangeToMenuEntry(4, "   Textures Disabled (Fast but graphics are unclear)", 171);
		  break;
      case 171:
		  distext=1;
		  glutChangeToMenuEntry(3, "   Textures Enabled (Game is slow if software-OpenGL)", 170);
		  glutChangeToMenuEntry(4, "+ Textures Disabled (Fast but graphics are unclear)", 171);
		  break;
	}
	glutPostRedisplay();
}

static void menu_pikaapu(int arvo){
    switch (arvo) {
      case 191:
		  helppi=1;
		  viivaon=0;
		  alkukuva("hrulesi.thp");
		  pelvscomp=2;
		  akuva=1;
		  break;
      case 192:
		  helppi=2;
		  viivaon=0;
		  alkukuva("hhowto2.thp");
		  pelvscomp=2;
		  akuva=1;
		  break;
	}

	glutPostRedisplay();
}


static void menu_tietsu(int arvo){
}

static void menu_valikko(int arvo){
}


void teemenu(){
	kuittaus = glutCreateMenu(menu_lopetus);
	glutAddMenuEntry("Cancel", 31);
	glutAddMenuEntry("Restart", 32);
	glutAddMenuEntry("Quit (Save settings)", 33);
	glutAddMenuEntry("Quit (Without saving)", 34);


	settinki = glutCreateMenu(menu_settinki);
	glutAddMenuEntry("Load settings", 201);
	glutAddMenuEntry("Save settings", 202);
	glutAddMenuEntry("Restore default settings", 203);


	pelaajat = glutCreateMenu(menu_pelaajat);
	glutAddMenuEntry(" Computer vs. Player", 21);
	glutAddMenuEntry(" Player vs. Computer", 22);
	glutAddMenuEntry(" Player vs. Player", 23);
	glutAddMenuEntry(" Computer vs. Computer", 24);


	lautakoko = glutCreateMenu(menu_lautakoko);
	glutAddMenuEntry("+ 40x40 (Default)", 11);
	glutAddMenuEntry("   20x20", 12);
	glutAddMenuEntry("   16x16", 13);
	glutAddMenuEntry("   15x15 (Standard)", 14);
/*	glutAddMenuEntry("   3x3", 15); */
/*	glutAddMenuEntry("   Unlimited (Scroll)", 16); */

	teema = glutCreateMenu(menu_teema);
	glutAddMenuEntry("+ Default", 1);
	glutAddMenuEntry("   Light", 2);
	glutAddMenuEntry("   Stone", 3);
	glutAddMenuEntry("   Wood", 4);
	glutAddMenuEntry("   Wall", 5);
	glutAddMenuEntry("   Newspaper", 6);
	glutAddMenuEntry("   Black", 7);


	ktyyli = glutCreateMenu(menu_tyyli);
	glutAddMenuEntry("+ Aggressive Attacker", 41);
	glutAddMenuEntry("   Defence Master", 42);
	glutAddMenuEntry("   Passive Pressurer", 43);
	glutAddMenuEntry("   Nasty Trap Builder", 44);
	glutAddMenuEntry("   Random", 45);
/*	glutAddMenuEntry("   Custom: player1.cmp", 46); */
/*	glutAddMenuEntry("   Custom: player2.cmp", 47); */
/*	glutAddMenuEntry("   Custom: player3.cmp", 48); */


	kstyyli = glutCreateMenu(menu_styyli);
	glutAddMenuEntry("+ Random", 55);
	glutAddMenuEntry("   Basic", 51);
	glutAddMenuEntry("   THO-MOKU Special", 52);
	glutAddMenuEntry("   Breadfan's special", 53);
	glutAddMenuEntry("   Playsite Big X", 54);
	
	karvonta = glutCreateMenu(menu_arvonta);
	glutAddMenuEntry("+ Normal", 61);
	glutAddMenuEntry("   High (too easy)", 62);
	glutAddMenuEntry("   Very High (too easy)", 63);

	kajatus = glutCreateMenu(menu_ajatus);
	glutAddMenuEntry("   Easy: Only this move (Fast)", 71);
	glutAddMenuEntry("   Easy: 2 moves (Fast)", 72);
	glutAddMenuEntry("+ Normal: 3 moves (Medium)", 73);
	glutAddMenuEntry("   Hard: 4 moves (Medium)", 74);
	glutAddMenuEntry("   Hard: 5 moves (Slow)", 75);
	glutAddMenuEntry("   Hard: 6 moves (Slow)", 76);
	glutAddMenuEntry("   Hard: 7 moves (Slow)", 77);
	glutAddMenuEntry("   Hard: 8 moves (Slow)", 78);

	koppi = glutCreateMenu(menu_oppi);
	glutAddMenuEntry("+ Disabled", 81);
	glutAddMenuEntry("   Enabled", 82);

	klooppi = glutCreateMenu(menu_looppi);
	glutAddMenuEntry("+ Disabled", 83);
	glutAddMenuEntry("   Enabled", 84);
	
	kone = glutCreateMenu(menu_tietsu);
	glutAddSubMenu("Thinking time", kajatus);
	glutAddSubMenu("Style", ktyyli);
	glutAddSubMenu("Start style", kstyyli);
	glutAddSubMenu("Randomness", karvonta);
	glutAddSubMenu("Talking", koppi);
	glutAddSubMenu("Loop-mode", klooppi);

	vuoroja = glutCreateMenu(menu_vuoroja);
	glutAddMenuEntry("   Few", 150);
	glutAddMenuEntry("+ Normal", 151);
	glutAddMenuEntry("   Many", 152);

	taso = glutCreateMenu(menu_vaikeus);
	glutAddMenuEntry("   Hard", 153);
	glutAddMenuEntry("+ Medium", 154);
	glutAddMenuEntry("   Easy", 155);

	pelaajapuol = glutCreateMenu(menu_nappi);
	glutAddMenuEntry("   X", 156);
	glutAddMenuEntry("+ O", 157);

	randomi = glutCreateMenu(menu_random);
	glutAddSubMenu("Player's Side", pelaajapuol);
	glutAddSubMenu("Turns before game", vuoroja);
	glutAddSubMenu("Level", taso);
	glutAddMenuEntry("Start!", 180);

	vakioscen = glutCreateMenu(menu_vsken);
	undo1x=0;undo1y=0;
	glutAddMenuEntry("Setting a Trap. (Hard)", 100);
	glutAddMenuEntry("The End of Greed. (Hard)", 101);
	glutAddMenuEntry("Defender.(Hard)", 102);
	glutAddMenuEntry("Ring of Death II. (Medium)", 103);
	glutAddMenuEntry("The Cross. (Medium)", 104);
	glutAddMenuEntry("Runaway. (Medium)", 105);
	glutAddMenuEntry("Rush X-ident. (Medium)", 106);
	glutAddMenuEntry("In the Beginning. (Easy)", 107);
	glutAddMenuEntry("The Line of Fire. (Easy)", 108);
	glutAddMenuEntry("Ring of Death. (Easy)", 109);
	glutAddMenuEntry("Gaze to Horizon. (Easy)", 110);

	muitten = glutCreateMenu(menu_msken);
	glutAddMenuEntry("Custom: ownscen1.tsc (Empty)", 111);
	glutAddMenuEntry("Custom: ownscen2.tsc (Empty)", 112);
	glutAddMenuEntry("Custom: ownscen3.tsc (Empty)", 113);
	glutAddMenuEntry("Custom: ownscen4.tsc (Empty)", 114);
	
	skenariot = glutCreateMenu(menu_sken);
	glutAddSubMenu("Standard Scenarios", vakioscen);
	glutAddSubMenu("Scenarios by other players", muitten);
	glutAddSubMenu("Random Board", randomi);


	huijaus = glutCreateMenu(menu_huijaus);
	glutAddMenuEntry("+ Disable cheats", 91);
	glutAddMenuEntry("   Enable cheats", 92);
	glutAddMenuEntry("   (Undo last move)", 93);
	glutAddMenuEntry("   (Show computers thoughts)", 93);

	saannot = glutCreateMenu(menu_rulet);
	glutAddMenuEntry("+ Free-style Go-moku (THO-MOKU DEFAULT)", 160);
	glutAddMenuEntry("   Standard Go-moku (Only line of exactly 5 wins)", 161);
	glutAddMenuEntry("* Renju (cf. Sakata and Ikawa 1981)", 162);
	glutAddMenuEntry("* Professional Go-moku (Learn the rules first)", 163);
	glutAddMenuEntry("* Professional Renju (Side swapping, etc.)", 164);
/*	glutAddMenuEntry("   Custom Rules, 165); */

	leiautti = glutCreateMenu(menu_leiaut);
	glutAddMenuEntry("+ THO-MOKU (Default)", 85);
	glutAddMenuEntry("   Go-Moku/Renju", 86);

	maisemat = glutCreateMenu(menu_maisema);
	glutAddSubMenu("   Graphics", teema);
	glutAddSubMenu("   Layout", leiautti);
	glutAddMenuEntry("+ Textures Enabled (Game is slow on software Open-GL)", 170);
	glutAddMenuEntry("   Textures Disabled (Fast but graphics are unclear)", 171);

	helppi=glutCreateMenu(menu_pikaapu);
	glutAddMenuEntry("Rules (More help in readme.txt)", 191);
	glutAddMenuEntry("How to win in Go-Moku", 192);

	glutCreateMenu(menu_valikko);
	glutAddSubMenu("New Game", pelaajat);
	glutAddSubMenu("Scenarios", skenariot);
	glutAddSubMenu("Menu settings", settinki);
	glutAddSubMenu("Computer", kone);
	glutAddSubMenu("Appearance", maisemat);
	glutAddSubMenu("Rules", saannot);
	glutAddSubMenu("Board", lautakoko);
	glutAddSubMenu("Cheat", huijaus);
	glutAddSubMenu("Help", helppi);
	glutAddSubMenu("Exit", kuittaus);

	glutAttachMenu(GLUT_RIGHT_BUTTON);
}


void restoredefaults(void){
        menu_teema(1);
        menu_lautakoko(11);
        menu_tyyli(41);
        menu_styyli(55);
        menu_arvonta(61);
        menu_ajatus(73);
        menu_oppi(81);
        menu_looppi(83);
        menu_rulet(160);
        menu_huijaus(91);
        menu_leiaut(85);
        menu_vuoroja(151);
        menu_vaikeus(154);
        menu_nappi(157);
        menu_maisema(170);
}

void optionsluku(){
	unsigned int P1;

	FILE *sisaan;
	sisaan=fopen("options.dat","r");

  for(P1=0;P1<32;P1++){
	  fscanf(sisaan,"%d ",&menuvalinta[P1]);
	}

  fclose(sisaan);

  menu_teema(menuvalinta[0]);
  menu_lautakoko(menuvalinta[1]);
  menu_tyyli(menuvalinta[2]);
  menu_styyli(menuvalinta[3]);
  menu_arvonta(menuvalinta[4]);
  menu_ajatus(menuvalinta[5]);
  menu_oppi(menuvalinta[6]);
  menu_looppi(menuvalinta[7]);
  menu_rulet(menuvalinta[8]);
  menu_huijaus(menuvalinta[9]);
  menu_leiaut(menuvalinta[10]);
  menu_vuoroja(menuvalinta[11]);
  menu_vaikeus(menuvalinta[12]);
  menu_nappi(menuvalinta[13]);
  menu_maisema(menuvalinta[14]);

}
/* menuvalinta={1, 11, 41, 55, 61, 73, 81, 83, 160, 91, 85, 151, 154, 157, 170} */

void optionstallennus(){
	unsigned int P1;

	FILE *ulos;
	ulos=fopen("options.dat","w");

  for(P1=0;P1<32;P1++){
   fprintf(ulos,"%d ",menuvalinta[P1]);
  }

	fclose(ulos);
}















/* Tämä on Init-funktio...                   */
/* Tässä on alustusjuttuja textuurista yms	 */
/* OpenGL funktioita.                        */

void alustus(void){

	float varitys[4] = { 1.0, 0.0, 1.0, 1.0 };

  menuvalinta[0]=1;menuvalinta[1]=11;menuvalinta[2]=41;menuvalinta[3]=55;
  menuvalinta[4]=61;menuvalinta[5]=73;menuvalinta[6]=81;menuvalinta[7]=83;
  menuvalinta[8]=160;menuvalinta[9]=91;menuvalinta[10]=85;menuvalinta[11]=151;
  menuvalinta[12]=154;menuvalinta[13]=157;menuvalinta[14]=170;

	teemenu(); /* Menut kuntoon */
/*  akuva=0;
  optionsluku(); */

	glShadeModel(GL_SMOOTH);
	glEnable(GL_NORMALIZE);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
	glEnable(GL_LINE_SMOOTH);

	/* sumu, jota ei juuri huomaa */
    glFogi(GL_FOG_MODE, GL_LINEAR);
    glFogf(GL_FOG_START, 0.1f);
    glFogf(GL_FOG_END, 4.0f);
    glFogfv(GL_FOG_COLOR, varitys);
    glHint(GL_FOG_HINT, GL_NICEST);

	glEnable(GL_DEPTH_TEST);
	glEnable(GL_CULL_FACE);
	glEnable(GL_TEXTURE_2D);

	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, (GLfloat)GL_REPEAT);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, (GLfloat)GL_REPEAT);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, (GLfloat)GL_LINEAR);
	glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, (GLfloat)GL_LINEAR);
	glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, (GLfloat)GL_DECAL);
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);

	alkukuva("alokuv.thp"); /* alotuskuva */

	Valotpaalle(); /* Valoa kansalle! */
}







/* Tämä on ruudunpäivitys funktio */
/* (  glutRefreshDisplay();  )                         */

void piirraruutu(void){
	int laskuri1, laskuri2;
	long int suurin=0;
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    glPushMatrix();
    kamera();                            /* kameran pyöritystä... */

	glEnable(GL_TEXTURE_2D);
	/*  piirtää textuurit (tämä on se joka hidastaa)  */
	if(distext!=1){
		glCallList(tausta);
		glCallList(lauta);
		glCallList(alunen);
	}

	/* Textuurit oli pakko disabloida vähäksi aikaa että värit näkyvät oikein */
	glDisable(GL_TEXTURE_2D);


  if(akuva==0){
	/* THomoku otsikko */
    glColor3f (0.0F, 1.0F, 0.0F);
	  sprintf (tekstijutska, "THO-MOKU");
    glRasterPos2f (-1.2F, 1.29F);
	  isojonoruudulle(tekstijutska);

  	glColor3f (0.5F, 0.0F, 1.0F);
	  sprintf (tekstijutska2, "Press right mouse button to menu...");
	  glRasterPos2f (-1.2F, -1.30F);
	  merkkijonoruudulle(tekstijutska2);
  	glColor3f (0.8F, 0.8F, 0.8F);
 	  glRasterPos2f (-0.4F, 1.29F);
    merkkijonoruudulle(tekstijutska5);


	/* Ja erinäisiä graafisia muutoksia ruudulle tietyissä tilanteissa */
  	if(puoli==1&&tekstit==0){
  		glColor3f (0.0F, 1.0F, 0.0F);
	  	if(leiaut==0)sprintf (tekstijutska3, "X's turn");
	  	if(leiaut==1)sprintf (tekstijutska3, "Black's turn");
    }
  	if(puoli==2&&tekstit==0){
  		glColor3f (0.5F, 0.0F, 1.0F);
	  	if(leiaut==0)sprintf (tekstijutska3, "0's turn");
	  	if(leiaut==1)sprintf (tekstijutska3, "White's turn");
    }
  	glRasterPos2f (0.8F, -1.31F);
  	isojonoruudulle(tekstijutska3);
  }

	/* Tässä piirretään the lauta (int A[][]) */
	glColor3f (0.5F, 0.0F, 1.0F);
  for(laskuri2=0;laskuri2<(laudankoko);laskuri2++){
	  for(laskuri1=0;laskuri1<(laudankoko);laskuri1++){
		  if(A[laskuri1][laskuri2]==2)nolla(laskuri1,laskuri2);
		}
	}
	glColor3f (0.0F, 1.0F, 0.0F);
	for(laskuri2=0;laskuri2<laudankoko;laskuri2++){
		for(laskuri1=0;laskuri1<laudankoko;laskuri1++){
			if(A[laskuri1][laskuri2]==1)risti(laskuri1,laskuri2);
		}
	}

	/* Ja ruudukko. Jos textuurit poissa niin hieman kirkkaampi, että näkyy... */
	if(distext==0)glColor3f (0.2F, 0.2F, 0.2F);
	if(distext==1)glColor3f (0.4F, 0.4F, 0.4F);
	piirraruudukko();
	

/* Tämä piirtää tietokoneen miettimiset jos cheat on päällä */
	if(primatriisi==1){
		for(laskuri2=0;laskuri2<laudankoko;laskuri2++){
			for(laskuri1=0;laskuri1<laudankoko;laskuri1++){
				if(PRIORITY[laskuri1][laskuri2]>suurin)suurin=PRIORITY[laskuri1][laskuri2];
			}
		}
		glDisable(GL_LINE_SMOOTH);
		for(laskuri2=0;laskuri2<laudankoko+8;laskuri2++){
			for(laskuri1=0;laskuri1<laudankoko+8;laskuri1++){
				if(PRIORITY[laskuri1][laskuri2]>0){
					piste(laskuri1-8,laskuri2-8, suurin);
				}
			}
		}
		glEnable(GL_LINE_SMOOTH);

	}

	/* Jos peli on loppu, näytetään voittajarivin päällä punainen viiva */
	glColor3f (1.0F, 0.0F, 0.0F);
	if(viivaon==1)pviiva();

	if(tekstit==1){
		glColor3f (0.0F, 1.0F, 0.0F);
		if(leiaut==0)sprintf (tekstijutska4, "X WON!");
		if(leiaut==1)sprintf (tekstijutska4, "BLACK WON!");
		glRasterPos2f (0.6F, 1.29F);
		isojonoruudulle(tekstijutska4);
	}
	if(tekstit==2){
		glColor3f (0.5F, 0.0F, 1.0F);
		if(leiaut==0)sprintf (tekstijutska4, "0 WON!");
		if(leiaut==1)sprintf (tekstijutska4, "WHITE WON!");
		glRasterPos2f (0.6F, 1.29F);
		isojonoruudulle(tekstijutska4);
	}

  neliox=undo1x;
  nelioy=undo1y;
	if(akuva==0)kirkastar(neliox, nelioy);

	glEnable(GL_TEXTURE_2D);

	glPopMatrix();
	glutSwapBuffers();     /* Noniin, raks raks, piirretään */

}






/* näytön resoluutiosähellys (Reshape) */
void ruudunasetus(int w, int h){
	glViewport(0, 0, korkeus, leveys);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(X_MINI, X_MAXI, Y_MINI, Y_MAXI, Z_MINI, Z_MAXI);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}


/* Koneen tyhjäkäynti-prosessi, ei tee oikeen mitään raskasta... */
void idle (void){
  int paluutark=0;
/*	unsigned int x=0, y=0;*/

  if(kokovuoro==0){
    if(alotyyli2==5){
      alotyyli=arpa4();
      if(alotyyli!=2)alotyyli=arpa();
    }
  }

	if((peliloppu==1)&&(loopmode==1)&&(pelvscomp==3)){
		peliloppu=0;
		viivaon=0;
		nollaus(A, PRIORITY);
		kokovuoro=0;
		puoli=1;
	}
  glutPostRedisplay();
  if(tietokone==0){tietokone=1;}
  if(tietokone==2){
    tietokone=0;

/* ...paitsi koneen siirto :DDD */
    if((peliloppu==0)&&(viivaon==0)){
  		if((pelvscomp==3)||((kokovuoro==0)&&(pelvscomp==0))){
	  		prionollaus();

        if(kokovuoro<10){
          paluutark=alotus();
        }
        sprintf (tekstijutska5, " ");
        if(paluutark==0)paluutark=mietipaikka();
        haeparas();
      }
  	  if(((pelvscomp==0)&&(puoli==1))||((pelvscomp==1)&&(puoli==2))){
        glutSetCursor(GLUT_CURSOR_WAIT);
			  prionollaus();
        if(kokovuoro<10){
          paluutark=alotus();
        }
        sprintf (tekstijutska5, " ");
        if(paluutark==0)paluutark=mietipaikka();
        haeparas();
        glutSetCursor(GLUT_CURSOR_LEFT_ARROW); 
      }
      glutPostRedisplay();
    }

  }
  if(tietokone==1){tietokone=2;}

	glutSetWindow (paaikkuna);
	arvottu2=arpa2();
/*	glutSwapBuffers();
	glutPostRedisplay ();
*/
}














/* pikku tutoriaali */
void tutorial(void){
  printf("\nA short tutorial for THO-MOKU\n\n");
  printf("You win the game if you have consecutive line of five marks in a row.\n");
  printf("This means that XXXXX -> X wins, 00000 -> 0 wins.\n");
  printf("Let * mean a place for a next mark. So 0XXXX* -> 0XXXXX -> X wins.\n");
  printf("The consecutive line of 5 can be in any of 4 directions:\n");
  printf("left, down, downleft or downright.\n");
  printf("Now, you will get consecutively 5 if you have 3 in line and opponent\n");
  printf("can't put a mark to prevent a line of 4: *XXX* or X*XX\n");
  printf("You will get a line of 3 from the line of 2 as one of following examples:\n");
  printf(" **XX** ,  *X*X* ,  X**X  \n");
  printf("As well, if your opponent have a 3 in line, you have to defence.\n");
  printf(" 000  -> *000  or  000*\n\n");
  printf(" * XX The only way to get 5 is that you need 2 lines of 3, so you   XX*X0\n");
  printf(" X    have to search for a place to get 2 lines of 3 at the same time  \n");
  printf(" X    Even better is to find a place for 3 and 4 (as in right).       X\n");
  printf("      Note: 2 more thinking seconds makes you a much better player.   X\n\n");
  printf("You much be greedy but not too much. Always be careful with opponent's \n");
  printf("place for a line of 4. There are some special cases too...\n");
  printf("Experience is the key. But think twice if you want to win.\n");

}




/* sit vähän näppisjutskaa */
/* mutta eihän tässä oo mitä niillä vois säätää... */

static void erikoiskey( int nappi, int x, int y ){
	switch (nappi) {

		case GLUT_KEY_UP:
      neliox++;
      kirkastar(neliox,nelioy);
			break;
		case GLUT_KEY_DOWN:
      neliox--;
      kirkastar(neliox,nelioy);
			break;
		case GLUT_KEY_LEFT:
      neliox--;
      kirkastar(neliox,nelioy);
			break;
		case GLUT_KEY_RIGHT:
      neliox++;
      kirkastar(neliox,nelioy);
			break;
	}
	glutPostRedisplay();

}


/* Normaalit näppisjutskat... */
void Keyboard(unsigned char nappi, int x, int y){
	switch (nappi){

    case 'q':
    case 'Q':
    case 27:  /* Esc:ki */
		printf("\n Ok.\n\n\n Thanks for using THO-MOKU OpenGL\n\n");
		exit(0);
		break;

/* Kameran pyöritys toimii, mutta siitä ei ole mitään hyötyä.   */
/* Joku avuton vain pyörittää sitä vahingossa eikä saa takaisin */
/* Joten päätin disabloida tämänkin ainakin toistaiseksi.       */		

	case 'a':
/*		kulma2+=1.0f; */    /* pyörittää kameraa */
		break;
	case 'd':
/*		kulma3 += 1.0f; */
/* if (kulma3 >= 360.0) kulma3 -= 360.0; */
		break;
	case 'w':
/*		etaisz+=0.5f; */
		break;
	case 's':
/*		etaisz-=0.5f; */
		break;
	}
	glutPostRedisplay();
}






/* glutin ihan alukujutut... */
void instaaglut(void){
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_DEPTH);
	glutInitWindowPosition (1, 1);
	glutInitWindowSize(320, 240);

	paaikkuna = glutCreateWindow (OTSIKKO);
	glutFullScreen();
/*  kokoruutu, toimii myös SGI:llä */
/*  Koulun SGI:den resoluutio on muuten 1280*1024    */

/*  Kokoruutu kursori, mutta toimii ilmankin.  */
  glutSetCursor(GLUT_CURSOR_LEFT_ARROW); 
}






/* Alkusetup, valitaan resoluutio, kaikki muut ovat */
/* hiiren oikean napin takana.                      */
int setuppi(void){
	int syote, paluu=0;
  printf("\n Choose resolution:\n\n");
	printf(" NOTE: PLEASE USE YOUR DEFAULT (WINDOWS) RESOLUTION!\n");
	printf(" (Or the mouse function won't work.)\n\n");
	printf("   1. 640*480\n\n");
	printf("   2. 800*600\n\n");
	printf("   3. 1024*768\n\n");
	printf("   4. 1280*1024\n\n");
	printf("   5. 1600*1200\n\n");
	printf("   6. Other\n\n");
  printf("   7. Quit\n\n");
	if (scanf("%d", &syote)!=1) return 2;
	if (syote==7) return 3;
	if (syote==1) {korkeus=640; leveys=480;}
	if (syote==2) {korkeus=800; leveys=600;}
	if (syote==3) {korkeus=1024; leveys=768;}
	if (syote==4) {korkeus=1280; leveys=1024;}
	if (syote==5) {korkeus=1600; leveys=1200;}
	if (syote==6) {    
  	printf("\n\n   Other common resolutions (width*height):\n\n");
  	printf("   848*480, 1152*864, 1600*1280,\n");
    printf("   1800*1440, 1920*1080, 1920*1200,\n");
    printf("   1920*1440, 2048*1536, 2304x1440\n\n");
    printf("\n   Custom resolution \n\n");
    printf("   Input width?\n   ");
    paluu=scanf("%d",&korkeus);
    if(paluu!=1)return 4;
    printf("\n   Input height?\n   "); 
    paluu=scanf("%d",&leveys);
    if(paluu!=1)return 4;
    printf("\n");
  }
	if (korkeus<100) return 4;
	if (leveys<100) return 4;
	printf("\n Ok.\n Loading...\n");
	return 0;
}




/* THE MAINI! */


int main(int argc, char** argv){

	int arvo=0;
  if(argc>1)tutorial();
  if(argc>1)return 0;
	arvottu3=arpa3();
	arvo=setuppi();

  if(arvo!=0) return 0;

	nollaus(A, PRIORITY);
	glutInit(&argc, argv); /* GLUT-juttu */


/* Glut alkujutut */
	instaaglut(); /* taas mennään */

/* OpenGL alkujutut */
	alustus(); /* textuuri */


/* näyttö */
	glutDisplayFunc(piirraruutu);
	glutReshapeFunc(ruudunasetus);


/* tietojen syöttö */
	glutKeyboardFunc( Keyboard );
	glutSpecialFunc(erikoiskey);
	glutMouseFunc(hiiri);


/* odottaja */
    glutIdleFunc (idle);

/* Ja sitten the looppi: */
	glutMainLoop();

	return 0;
}

/* Lisätietoja sähköpostilla: tuomas.hietanen@lut.fi */




/* Loppu (vihdoin) */
