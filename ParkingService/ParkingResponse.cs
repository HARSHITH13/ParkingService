namespace ParkingService
{
    public class ParkingResponse
    {
        public string? VehicleNum { get; set; }

        public int ParkingSlot { get; set;}

        public bool Success {  get; set; }

        public string? ErrorMessage { get; set; }

        public int AvailableSlots { get; set; }
    }
}
