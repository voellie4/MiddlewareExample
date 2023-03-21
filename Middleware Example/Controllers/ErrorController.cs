using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using WindowAuthDemo;

namespace Middleware_Example.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("401")]
        public ActionResult Return401Error()
        {
            return Unauthorized();
        }

        [HttpGet("DivideByZero")]
        public ActionResult DivideByZero(int a, int b)
        {
            if (b == 0)
                throw new DivideByZeroException("b cannot be zero");
            else
                return Ok(JsonConvert.SerializeObject(new DivideObject { number1 = a, number2 = b}));
        }

        [HttpGet("PrintAB")]
        public ActionResult PrintAB(int a, int b)
        {
            return Ok(JsonConvert.SerializeObject(new DivideObject { number1 = a, number2 = b }));
        }

        [HttpGet("ThrowRandomError")]
        public ActionResult ThrowRandomError()
        {
            throw new Exception("throwing random error");
        }

        [HttpPost]
        [Route("UploadFileToWIP")]
        public IActionResult Post()
        {
            string networkPath = @"\\txhdcfile01\ProdData\Document Control\WIP";


            using (new ConnectToSharedFolder(networkPath))
            {
                //create empty file
                System.IO.File.Create(networkPath + "\\MTR-BDY-KB9E").Dispose();
            }

            return Ok("created file successfully.");
        }
    }

    public class DivideObject
    {
        public int number1 { get; set; }
        public int number2 { get; set; }
    }
}
