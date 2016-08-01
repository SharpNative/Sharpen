﻿using Sharpen.Arch;
using Sharpen.Collections;
using Sharpen.FileSystem;

namespace Sharpen.Drivers.Char
{
    public sealed class SerialPort
    {
        private static SerialPortComport[] comports = new SerialPortComport[4];

        /// <summary>
        /// Init device
        /// </summary>
        /// <param name="num"></param>
        private static void initDevice(int num)
        {
            if (comports[num].Address == 0)
                return;

            ushort port = comports[num].Address;

            PortIO.Out8((ushort)(port + 1), 0x00);
            PortIO.Out8((ushort)(port + 3), 0x80);
            PortIO.Out8((ushort)(port + 0), 0x01);
            PortIO.Out8((ushort)(port + 1), 0x00);
            PortIO.Out8((ushort)(port + 3), 0x03);
            PortIO.Out8((ushort)(port + 2), 0xC7);
            PortIO.Out8((ushort)(port + 4), 0x0B);
            PortIO.Out8((ushort)(port + 1), 0x01);

            comports[num].Buffer = new Fifo(256);
            
            Device dev = new Device();
            dev.Name = comports[num].Name;
            dev.node = new Node();
            
            dev.node.Cookie = (uint)num;
            dev.node.Flags = NodeFlags.FILE;
            dev.node.Write = writeImpl;
            dev.node.Read = readImpl;

            DevFS.RegisterDevice(dev);
        }

        /// <summary>
        /// Write method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes written</returns>
        private static uint writeImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            uint i = 0;
            if (comports[node.Cookie].Address == 0)
                return 0;

            while(i < size)
            {
                write(buffer[i], comports[node.Cookie].Address);

                i++;
            }

            return i;
        }

        /// <summary>
        /// Read method for filesystem
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="offset">The offset</param>
        /// <param name="size">The size</param>
        /// <param name="buffer">The buffer</param>
        /// <returns>The amount of bytes read</returns>
        private static uint readImpl(Node node, uint offset, uint size, byte[] buffer)
        {
            if (comports[node.Cookie].Address == 0)
                return 0;

            return comports[node.Cookie].Buffer.Read(buffer, (ushort)size);
        }

        /// <summary>
        /// Is the transmit empty?
        /// </summary>
        /// <param name="port">The serialport</param>
        /// <returns>If the transmit is empty</returns>
        private static bool isTransmitEmpty(int port)
        {
            return (PortIO.In8((ushort)(port + 0x05)) & 0x20) == 0;
        }

        /// <summary>
        /// Byte received on port?
        /// </summary>
        /// <param name="port">The serialport</param>
        /// <returns>If there are bytes received</returns>
        private static bool hasReceived(int port)
        {
            return (PortIO.In8((ushort)(port + 0x05)) & 1) > 0;
        }

        /// <summary>
        /// Read from serial port
        /// </summary>
        /// <param name="port">The serial port</param>
        /// <returns>Read data</returns>
        private static byte read(ushort port)
        {
            while (!hasReceived(port))
                CPU.HLT();

            return PortIO.In8(port);
        }

        /// <summary>
        /// Write to serial port
        /// </summary>
        /// <param name="d">The data</param>
        /// <param name="port">The port</param>
        private static void write(byte d, ushort port)
        {
            while (isTransmitEmpty(port))
                CPU.HLT();

            PortIO.Out8(port, d);
        }
        
        /// <summary>
        /// Read bios data area for addresses
        /// </summary>
        private static unsafe void readBda()
        {
            ushort* bda = (ushort*)0x00000400;

            comports[0].Address = *(bda + 0);   // COM1
            comports[1].Address = *(bda + 1);   // COM2
            comports[2].Address = *(bda + 2);   // COM3
            comports[3].Address = *(bda + 3);   // COM4
        }

        /// <summary>
        /// Handler for comport 1 and 3
        /// </summary>
        private static unsafe void Handler13(Regs* regsPtr)
        {
            SerialPortComport port;

            if (comports[0].Address != 0 && hasReceived(comports[0].Address))
                port = comports[0];
            else
                port = comports[2];

            if (port.Address == 0)
                return;

            while(hasReceived(port.Address))
                port.Buffer.WriteByte(read(port.Address));
        }

        /// <summary>
        /// Handler for comports 2 and 4
        /// </summary>
        private static unsafe void Handler24(Regs* regsPtr)
        {
            SerialPortComport port;

            if (comports[1].Address != 0 && hasReceived(comports[1].Address))
                port = comports[1];
            else
                port = comports[3];

            if (port.Address == 0)
                return;

            while (hasReceived(port.Address))
                port.Buffer.WriteByte(read(port.Address));
        }

        /// <summary>
        /// Initialize serialport
        /// </summary>
        public static unsafe void Init()
        {
            comports[0] = new SerialPortComport();
            comports[0].Name = "COM1";
            comports[1] = new SerialPortComport();
            comports[1].Name = "COM2";
            comports[2] = new SerialPortComport();
            comports[2].Name = "COM3";
            comports[3] = new SerialPortComport();
            comports[3].Name = "COM4";

            readBda();

            initDevice(0);
            initDevice(1);
            initDevice(2);
            initDevice(3);
            
            IRQ.SetHandler(3, Handler24);
            IRQ.SetHandler(4, Handler13);
        }
    }
}
