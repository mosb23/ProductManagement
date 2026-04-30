namespace ProductManagement_V2.Application.Common.Results
{
    public enum ResultStatus
    {
        Success = 0,
        BadRequest = 1,
        NotFound = 2,
        Conflict = 3,
        Validation = 4,
        Unauthorized = 5,
        Forbidden = 6
    }
}