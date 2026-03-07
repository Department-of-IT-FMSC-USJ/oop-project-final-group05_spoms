namespace SmartPostOffice.Models.Enums
{
public enum RequestStatus
{
PENDING = 0, // just submitted online
ACCEPTED = 1, // officer processed at counter
RECEIVED = 2, // at sorting center
IN_TRANSIT = 3, // on the way
OUT_FOR_DELIVERY = 4, // with delivery person
DELIVERED = 5 // done!
}
}