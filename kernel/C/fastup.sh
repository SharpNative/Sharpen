sudo /etc/qemu-ifdown tap0
sudo /etc/qemu-ifup tap0
sudo /usr/local/src/qemu/debug/i386-softmmu/qemu-system-i386 ../../os.img -soundhw ac97 -net nic,model=pcnet,vlan=0 -net tap,ifname=tap0,script=no,downscript=no -serial file:test.txt -m 256 -D qemu.log
