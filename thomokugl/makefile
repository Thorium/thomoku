##### MACROS #####

CC = cc
CFLAGS =

# = comment only      You have to use right LIBS:

#LIBS = /some_path_here/libglut.a -lGLU -lGL -lXmu -lXext -lX11 -lm

# SGI Example: /usr/lib32/  (could be /usr/lib/ too...)
#LIBS = /usr/lib32/libglut.a -lGLU -lGL -lXmu -lXext -lX11 -lm

# Sun4 Example:
LIBS = -lglut -lGLU -lGL -lXmu -lXext -lX11 -lm



PROGS =

##### RULES #####

.SUFFIXES:
.SUFFIXES: .c

.c:
	$(CC) $(CFLAGS) $< $(LIBS) -o $@

##### TARGETS ######

default:
	@echo "Use:   make ristinolla"

clean:
	-rm $(PROGS) *.o *~ core
