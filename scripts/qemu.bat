echo Starting QEMU...
rem "C:\Program Files\qemu\qemu-system-i386.exe" os.img -soundhw ac97 -net nic,model=rtl8139 -serial file:test.txt -net tap,ifname=TAP -m 256 -D qemu.log
"C:\Program Files\qemu\qemu-system-i386.exe" ..\..\os.img -soundhw ac97 -net nic,model=e1000 -serial file:serial.txtd  -m 256 -D qemu.log -net tap,ifname=TAP  -usbdevice mouse
rem net dump,file=netdump.wcap