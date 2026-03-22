namespace SmartPostOffice.Models.Enums
{
    public enum StampOrderStatus
    {
        PENDING            = 0,  // just placed (before payment)
        PAYMENT_CONFIRMED  = 1,  // payment received
        PROCESSING         = 2,  // officer is preparing
        DISPATCHED         = 3,  // handed to postal delivery
        DELIVERED          = 4   // customer received
    }
}