using Microsoft.AspNetCore.Mvc;
using ParkingService.DBContext;
using ParkingService.Services;

namespace ParkingService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParkingController : ControllerBase
    {        
        private readonly ILogger<ParkingController> _logger;
        private readonly ParkingContext parkingContext;
        private readonly ParkingSlot parkingSlot;
        private readonly SmsService smsService;
        public static int availableSlot = 100;

        public ParkingController(ILogger<ParkingController> logger, ParkingContext parkingContext, ParkingSlot parkingSlot, SmsService smsService)
        {
            _logger = logger;
            this.parkingContext = parkingContext;
            this.parkingSlot = parkingSlot;
            this.smsService = smsService;
        }

        [HttpGet(Name = "GetParkingSlot")]

        public ParkingResponse GetParkingSlot(int inputRFID)
        {
            int slot = -1;
            var response = parkingContext.VehicleDetails.Where(x => x.RFID == inputRFID).FirstOrDefault();
            ParkingResponse pr = new();            

            if (response == null)
            {
                pr.Success = false;
                pr.ErrorMessage = "Vechicle not registered. Get the RFID tag";
                pr.AvailableSlots = availableSlot;
            }
            else 
            {
                slot = parkingSlot.AssignParkingSlot();
                if (slot == -1)
                {
                    pr.Success = false;
                    pr.ErrorMessage = "Parking lot is full";
                    pr.AvailableSlots = 0;                    
                }
                else
                {
                    response.Slot = slot;
                    parkingContext.SaveChanges();
                    pr.VehicleNum = response.VehicleNum;
                    pr.Success = true;
                    pr.ParkingSlot = slot;
                    pr.ErrorMessage = "Vehicle entered the parking bay";
                    availableSlot--;
                    pr.AvailableSlots = availableSlot;

                    string mobNum = "+917996823996";
                    string msg = "Your vehicle is parked under Slot number - " + response.Slot + "\n Please click on the link to view your parked vehicle - \"https://parkinglayout.blob.core.windows.net/parkinglayout/sampleLayout2.png\"";
                    smsService.SendSms(mobNum, msg);
                }
                //pr.AvailableSlots = parkingSlot.AvailableSlots;
                //pr.AvailableSlots = availableSlot;
            }

            return pr;
        }

        [HttpPost(Name ="ExitParkingSlot")]
        //public ParkingResponse ExitParkingSlot(int inputRFID)
        public ParkingResponse ExitParkingSlot([FromBody] RFIDRequest request)
        {

            int inputRFID = request.inputRFID;
            var response = parkingContext.VehicleDetails.Where(x => x.RFID == inputRFID).FirstOrDefault();
            ParkingResponse pr = new();
            if (response == null)
            {
                pr.ErrorMessage = "Invalid Slot number";
                pr.Success = false;
                pr.AvailableSlots = availableSlot;
            }
            else
            {
                var departStatus = parkingSlot.VehicleDeparted(response.Slot);               

                pr.ErrorMessage = departStatus.ToString();
                pr.Success = true;
                pr.VehicleNum = response.VehicleNum;
                pr.ParkingSlot = 0;
                //pr.AvailableSlots = parkingSlot.AvailableSlots;                        
                if (pr.ErrorMessage.Equals("Invalid slot number"))
                {
                    pr.AvailableSlots = availableSlot;
                    pr.Success = false;
                }
                else
                {
                    availableSlot++;
                    pr.AvailableSlots = availableSlot;
                    pr.Success = true;
                }

                response.Slot = 0;
                parkingContext.SaveChanges();
            }

            return pr;
        }

        public class RFIDRequest
        {
            public int inputRFID { get; set; }
        }
    }
}
