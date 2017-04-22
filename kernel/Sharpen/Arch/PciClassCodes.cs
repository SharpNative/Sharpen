namespace Sharpen.Arch
{
    enum PCIClassCombinations
    {
        /**
         * Class code 0x00
         */
        Any                     = 0x0000,
        VGADevice               = 0x0001,

        /**
         * Class code 0x01
         */
        SCSIController          = 0x0100,
        IDEController           = 0x0101,
        FloppyDiskController    = 0x0102,
        IPIBusController        = 0x0103,
        RAIDController          = 0x0104,
        ATAController           = 0x0105,
        SATAController          = 0x0106,
        SASController           = 0x0107,
        OtherStorageController  = 0x0180,

        /**
         * Class code 0x0C
         */
        IEEE1304Controller      = 0x0C00,
        ACCESSBusController     = 0x0C01,
        SSAController           = 0x0C02,
        USBController           = 0x0C03,
        FIbreChannelController  = 0x0C04,
        SMBusController         = 0x0C05,
        InfiniBandController    = 0x0C06,
        IPMIIterface            = 0x0C07,
        SERCOS                  = 0x0C08,
        CANBus                  = 0x0C09
    }
}
