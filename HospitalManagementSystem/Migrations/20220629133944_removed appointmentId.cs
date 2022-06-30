using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalManagementSystem.Migrations
{
    public partial class removedappointmentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Department_id",
                table: "Appointment",
                newName: "DepartmentId");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Appointment",
                newName: "Department_id");
        }
    }
}
