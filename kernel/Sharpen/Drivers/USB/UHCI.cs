﻿using Sharpen.Arch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpen.Drivers.USB
{
    class UHCI
    {
        const ushort REG_USBCMD    = 0x00;
        const ushort REG_USBSTS    = 0x02;
        const ushort REG_USBINTR   = 0x04;
        const ushort REG_FRNUM     = 0x06;
        const ushort REG_FRBASEADD = 0x08;
        const ushort REG_SOFMOD    = 0x0C;
        const ushort REG_PORTSC1   = 0x10;
        const ushort REG_PORTSC2   = 0x12;
        const ushort REG_LEGSUP    = 0xC0;

        private static ushort m_ioBase = 0x00;


        const ushort PORTSC_CUR_STAT            = (1 << 0);
        const ushort PORTSC_STAT_CHNG           = (1 << 1);
        const ushort PORTSC_CUR_ENABLE          = (1 << 2);
        const ushort PORTSC_ENABLE_STAT         = (1 << 3);
        const ushort PORTSC_LINE_STAT           = (1 << 4);
        const ushort PORTSC_RESUME_DETECT       = (1 << 6);
        const ushort PORTSC_LOW_SPEED           = (1 << 8);
        const ushort PORTSC_RESET               = (1 << 9);
        const ushort PORTSC_SUSPEND             = (1 << 12);

        const ushort USBCMD_RS = (1 << 0);
        const ushort USBCMD_HCRESET = (1 << 1);
        const ushort USBCMD_GRESET = (1 << 2);
        const ushort USBCMD_EGSM = (1 << 3);
        const ushort USBCMD_FGR = (1 << 4);
        const ushort USBCMD_SWDBG = (1 << 5);
        const ushort USBCMD_CF = (1 << 6);
        const ushort USBCMD_MAXP = (1 << 7);

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            PciDevice dev = findEHCIDevice();

            if (dev == null)
                return;

            if((dev.BAR4.flags & PCI.BAR_IO) == 0)
            {
                Console.WriteLine("[UHCI] Only Portio supported");
            }

            m_ioBase = (ushort)dev.BAR4.Address;

            Console.WriteLine("[UHCI] Initalize");

            /**
             * Initalize framelist
             */
            PortIO.Out16((ushort)(m_ioBase + REG_FRNUM), 0);

            /**
             * We are going to poll!
             */
            PortIO.Out16((ushort)(m_ioBase + REG_USBINTR), 0x00);

            /**
             * Enable device
             */
            PortIO.Out16((ushort)(m_ioBase + REG_USBCMD), USBCMD_RS);

            probe();
        }

        /// <summary>
        /// Reset port
        /// </summary>
        /// <param name="port"></param>
        private static void resetPort(ushort port)
        {
            /**
             * Set reset bit
             */
            setPortBit(port, PORTSC_RESET);

            /**
             * Wait for 60 ms
             */
            Sleep(60);

            /**
             * Unset reset bit
             */
            unsetPortBit(port, PORTSC_RESET);

            /**
             * Wait for atleast 150ms for link to go up
             */
            for(int i =0; i < 15; i++)
            {
                Sleep(10);

                ushort status = PortIO.In16((ushort)(m_ioBase + port));

                /**
                 * Is it even connected?
                 */
                if(((status) & PORTSC_CUR_STAT) == 0)
                    break;

                /**
                 * Status changed?
                 */
                if(((status) & (PORTSC_STAT_CHNG | PORTSC_ENABLE_STAT)) > 0)
                {
                    unsetPortBit(port, PORTSC_STAT_CHNG | PORTSC_ENABLE_STAT);
                    continue;
                }

                /**
                 * Enabled?
                 */
                if((status & PORTSC_CUR_ENABLE) > 0)
                    break;

            }
        }

        /// <summary>
        /// Sleep for X ms
        /// </summary>
        /// <param name="cnt"></param>
        private static void Sleep(int cnt)
        {
            for (int i = 0; i < cnt; i++)
                PortIO.In32(0x80);
        }


        /// <summary>
        /// Set bit on port
        /// </summary>
        /// <param name="port"></param>
        /// <param name="bit"></param>
        private static void setPortBit(ushort port, ushort bit)
        {
            ushort status = PortIO.In16((ushort)(m_ioBase + port));
            status |= bit;
            PortIO.Out16((ushort)(m_ioBase + port), status);
        }

        /// <summary>
        /// Unset bit on port
        /// </summary>
        /// <param name="port"></param>
        /// <param name="bit"></param>
        private static void unsetPortBit(ushort port, ushort bit)
        {
            ushort status = PortIO.In16((ushort)(m_ioBase + port));
            status &= (ushort)~bit;
            PortIO.Out16((ushort)(m_ioBase + port), status);
        }

        private static void probe()
        {
            for(int i = 0; i < 2; i++)
            {
                ushort port = (i == 0 )? REG_PORTSC1 : REG_PORTSC2;

                resetPort(port);


                ushort status = PortIO.In16((ushort)(m_ioBase + port));

                bool lowSpeed = ((status & PORTSC_LOW_SPEED) > 0);

                /**
                 * Port enabled?
                 */
                if((status & PORTSC_CUR_STAT) > 0)
                {
                    Console.Write("[UHCI] port ");
                    Console.WriteNum(i);
                    Console.Write(" connect with ");
                    Console.WriteLine(lowSpeed ? "low speed" : "high speed");
                }
                else
                {
                    Console.Write("[UHCI] port ");
                    Console.WriteNum(i);
                    Console.WriteLine(" disconnected");
                }
            }
        }

        private static PciDevice findEHCIDevice()
        {
            /**
             * Note: this cycles through PCI devices!
             */
            for (int i = 0; i < PCI.DeviceNum; i++)
            {
                PciDevice dev = PCI.Devices[i];

                if (dev.CombinedClass == (int)PCIClassCombinations.USBController && dev.ProgIntf == 0x00)
                    return dev;
            }

            return null;
        }
    }
}
