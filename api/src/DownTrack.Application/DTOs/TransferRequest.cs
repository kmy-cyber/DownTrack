using System;

namespace DownTrack.Application.DTO;

    public class TransferRequestDto
    {
        public int Id { get; set; } 

        public DateTime Date { get; set; } 

        public int UserId { get; set; } 
        public required string UserName { get; set; } 

        public int EquipmentId { get; set; } 
        public int DepartamentoId { get; set; } 
        public int SeccionId { get; set; } 
        public required string DepartamentoName { get; set; } 
        public required string  SeccionName { get; set; }
    }

