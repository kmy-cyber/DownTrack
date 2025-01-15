using System;

namespace DownTrack.Application.DTO;

    public class TransferRequestDto
    {
        public required int Id { get; set; } 
        public DateTime Date { get; set; } 
        public required int EmployeeId { get; set; } 
        // public required string UserName { get; set; } 
        public required int EquipmentId { get; set; } 
        public required int DepartmentId { get; set; } 
        public required int SectionId { get; set; } 
        // public required string DepartamentoName { get; set; } 
        // public required string  SeccionName { get; set; }
    }

