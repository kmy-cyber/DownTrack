

using System.ComponentModel.DataAnnotations;
using DownTrack.Domain.Enum;

namespace DownTrack.Application.DTO.Authentication;

public class RegisterUserDto
{
    public string Name{get;set;} = null!;
    public string Email{get;set;}= null!;
    public string Password{get;set;}= null!;
    public string UserRole {get;set;}= null!;
}