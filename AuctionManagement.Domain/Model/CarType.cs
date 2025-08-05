using System.ComponentModel;

namespace AuctionManagement.Domain.Model
{
    public enum CarType
    {
        [Description("Hatchback")]
        HATCHBACK,

        [Description("Sudan")]
        SUDAN,

        [Description("SUV")]
        SUV,

        [Description("Truck")]
        TRUCK
    }
}
