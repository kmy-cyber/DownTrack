using Xunit;
using FakeItEasy;
using System;
using DownTrack.Api.Controllers;
using DownTrack.Application.IRepository;
namespace Tests;

    public class DepartmentControllerTests{
        
        [Fact]
        public void CreateDepartmentTests(){
            //Arrange
              var data = A.Fake<IDepartmentRepository>();

            //  var controller  = new DepartmentController(data);
        }
    




    }
