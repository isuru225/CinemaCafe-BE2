using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MovieAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        [HttpGet("check-session")]
        public IActionResult CheckSession() 
        {
            var sessionObject = HttpContext.Request.Cookies["myKey"];
            //string sessionId = sessionObject.sessionID;


            // Check if the session identifier is valid (e.g., exists and is not expired)
            //Console.WriteLine("SIMMM"+sessionId);
            Debug.WriteLine("Hellooo" + HttpContext.Request.Cookies["myKey"]);
            if (true)
            {
                // Here, you would typically validate the session against your session store or database
                // For simplicity, this example just assumes the session is valid
                return Ok(new { message = "Session is still valid" });
            }
            else
            {
                // if the session identifier is missing or expired, return unauthorized status code
                return Unauthorized(new { message = "session has expired" });
            }
            //return Ok(new { SessionId = 1 });
        }

        [HttpGet("create-session")]
        public IActionResult CreateSession()
        {
            // Generate a unique session identifier (e.g., a GUID)
            Guid ID = Guid.NewGuid();
            string sessionID = ID.ToString();

            return Ok(new { sessionID = sessionID });   
        }
    }
}
