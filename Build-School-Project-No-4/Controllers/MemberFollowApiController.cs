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
    public class MemberFollowApiController : ApiController
    {
        private EPalContext db = new EPalContext();

        public IHttpActionResult PostFollowResult([FromBody] MemberViewModel followData)
        {
            var testFollowId = followData.FollowingId;
            var testOwnId = followData.UserId;

            Followings memberFollow = new Followings();

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    memberFollow.MemberId = testOwnId;
                    memberFollow.FollowingId = testFollowId;
                    db.Followings.Add(memberFollow);
                    db.SaveChanges();
                    tran.Commit();

                    return Ok(memberFollow);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
        }


    }
}
