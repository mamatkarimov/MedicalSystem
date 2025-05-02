namespace AuthService.Shared.DTOs.User
{
    public class DeviceListResult : BaseResponse
    {
        public IEnumerable<UserDeviceDto> Devices { get; set; }
    }
}