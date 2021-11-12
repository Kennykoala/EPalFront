using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Security;
using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using Newtonsoft.Json;

namespace Build_School_Project_No_4.Controllers
{
    public class MembersApiController : ApiController
    {
        private EPalContext db = new EPalContext();


        public IHttpActionResult PostAvatar([FromBody] MemberAvatarViewModel Data)
        {
            Members member = db.Members.First(x => x.MemberId == Data.MemberId);

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    member.ProfilePicture = Data.ProfilePicture;

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

        private bool MembersExists(int id)
        {
            return db.Members.Count(e => e.MemberId == id) > 0;
        }
    }
}