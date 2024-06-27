namespace ParkingService
{
    public class ParkingSlot
    {
        private static int nextAvailableSlot = 0;
        private int _totalSlots = 0;
        public int AvailableSlots = 0;

        private readonly SortedSet<int> departedSlots;

        public ParkingSlot(int totalSlot)
        {
            departedSlots = new();
            _totalSlots = totalSlot;
            AvailableSlots = _totalSlots;
        }

        public int AssignParkingSlot()
        {
            int slot = GetParkingSlot();

            if (_totalSlots < slot)
            {
                return -1; // Indicate no slot available
            }
            AvailableSlots--;
            return slot;
        }

        public string VehicleDeparted(int slot)
        {
            if (slot <= 0 || slot > _totalSlots)
            {
                //throw new ArgumentOutOfRangeException(nameof(slot), "Invalid slot number");
                return "Invalid slot number";
            }
            AvailableSlots++;
            // Insert the departed slot back into the list in its ascending order
            departedSlots.Add(slot);
            return "Vehicle exit from parking bay";            
        }

        private int GetParkingSlot()
        {
            int slot;
            if (departedSlots != null && departedSlots.Any())
            {
                slot = departedSlots.First();
                departedSlots.Remove(slot);
            }
            else
            {
                slot = ++nextAvailableSlot;
            }

            return slot;

        }
    }
}
