DOWNLOAD_URL	:= https://acpica.org/sites/acpica/files/acpica-unix-20170303.tar.gz
DOWNLOAD_FILE   := $(notdir $(DOWNLOAD_URL))
FOLDER_NAME		:= $(patsubst %.tar.gz,%,$(DOWNLOAD_FILE))
SOURCE_DIR		:= ./$(FOLDER_NAME)/source/
PATCH_DIR		:= ./patch

SRC 			:= $(wildcard $(SOURCE_DIR)components/*/*.c)
SRC				:= $(filter-out $(SOURCE_DIR)components/disassembler/%,$(SRC))
SRC				:= $(filter-out $(SOURCE_DIR)components/debugger/%,$(SRC))
OBJS			:= $(SRC:.c=.o)

CC 				:= i686-sharpen-gcc
AR				:= i686-sharpen-ar
CFLAGS 			:= -Wall -std=c99 -O2 -I$(SOURCE_DIR)include -I$(SOURCE_DIR)include/platform -nostdlib

.PHONY: all clean patch

$(DOWNLOAD_FILE):
	curl -O $(DOWNLOAD_URL)
	tar -xf $(DOWNLOAD_FILE)
	rm $(SOURCE_DIR)components/resources/rsdump.c $(SOURCE_DIR)components/resources/rsdumpinfo.c
	patch --verbose -p 0 -i $(PATCH_DIR)/acpica.patch
	cp -r $(PATCH_DIR)/include $(SOURCE_DIR)

patch: $(DOWNLOAD_FILE)

all: $(OBJS)
	$(AR) rcs acpica.a $(OBJS)

clean:
	rm $(OBJ) acpica.a $(DOWNLOAD_FILE)
	rm -rf $(FOLDER_NAME)

%.o: %.c
	$(CC) $(CFLAGS) -c -o $@ $<