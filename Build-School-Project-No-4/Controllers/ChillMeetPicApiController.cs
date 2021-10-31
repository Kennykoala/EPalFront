using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Build_School_Project_No_4.Controllers
{
    public class ChillMeetPicApiController : ApiController
    {
        private EPalContext db = new EPalContext();

        public IHttpActionResult PostMeetAvater([FromBody] ProfileViewModel Data)
        {
            Members member = db.Members.First(x => x.MemberId == Data.MemberId);

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    member.MeetPicture = Data.ProfilePicture;
                    db.SaveChanges();
                    tran.Commit();

                    var msg = "OK";
                    return Json(msg);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
