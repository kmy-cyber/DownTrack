

using System.ComponentModel.DataAnnotations;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO.Authentication;

public class RegisterUserDto
{
    public string Name{get;set;}
    public string Email{get;set;}
    public string Password{get;set;}
    public Role userRole {get;set;}
}