namespace AuthService.Shared.DTOs
{
    //// Extended error response DTO
    //public class ErrorResponse : BaseResponse
    //{
    //    public string Details { get; set; }
    //    public string StackTrace { get; set; }
    //    public string Type { get; set; }
    //}

    public class ErrorResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
