namespace HospitalManagementSystem.Models
{
    public class BillModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public string Condition { get; set; } = "Pending";
    }
    public class BillViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AppointmentId { get; set; }

        public decimal Amount { get; set; }

        public string AppointmentDescription { get; set; }

        public string Condition { get; set; } = "Pending";
    }
}
