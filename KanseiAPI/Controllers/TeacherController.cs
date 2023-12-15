using KanseiAPI.Interface;
using KanseiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace KanseiAPI.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController
    {
        private List<Teacher> listTeacher;
        public TeacherController() {
            listTeacher = new List<Teacher>();
        }
        [HttpGet("", Name = "GetAllTeacher")]
        public async Task<ActionResult<ResponseInfo>> getAll()
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                response.statusCode = System.Net.HttpStatusCode.OK;
                response.data = listTeacher;
                return await Task.FromResult(response);
            }
            catch(Exception ex)
            {
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                return await Task.FromResult(response);
            }
        }
        [HttpPost("", Name = "CreateTeacher")]
        public async Task<ActionResult<ResponseInfo>> createTeacher(List<Teacher> listTeacher)
        {
            ResponseInfo response = new ResponseInfo();
            try
            {
                listTeacher.AddRange(listTeacher);
                response.statusCode = System.Net.HttpStatusCode.OK;
                response.data = listTeacher; return await Task.FromResult(response);
            }
            catch(Exception e)
            {
                response.statusCode = System.Net.HttpStatusCode.BadRequest;
                return await Task.FromResult(response);
            }
        }

    }
}
