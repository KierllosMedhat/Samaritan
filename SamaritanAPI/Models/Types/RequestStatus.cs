namespace SamaritanAPI.Models.Types
{
    public enum RequestStatus
    {
    Pending,           // Request created, waiting for action
    SubleaderAssigned,    // A Subleader has been assigned to the request
    DiallerAssigned,    // A Dialer has been assigned to the request
    NoDiallerFound,    // A Dialer has been assigned to the request
    DonorAssigned,     // A donor has been assigned to the request
    NoDonorFound,      // No available donor for this request
    Completed,         // Request successfully fulfilled
    Canceled          // Request canceled by admin or user
    }
}
