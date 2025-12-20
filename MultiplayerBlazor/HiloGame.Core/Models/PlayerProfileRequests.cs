namespace HiloGame.Api.Models
{
    public class DisplayNameUpdateRequest
    {
        public Guid PlayerId { get; set; }
        public string NewDisplayName { get; set; } = string.Empty;
    }
}
