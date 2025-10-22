namespace ChatNest.Models.Common
{
    public class BaseResponse
    {
        public int MessageID { get; set; } // int for the message
        public string MessageDescription { get; set; } = string.Empty; // Description of the message, can be used for error messages or status updates
    }
}
