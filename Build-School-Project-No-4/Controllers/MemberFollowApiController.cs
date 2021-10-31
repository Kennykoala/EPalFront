using Build_School_Project_No_4.DataModels;
using Build_School_Project_No_4.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Build_School_Project_No_4.Controllers
{
    public class MemberFollowApiController : ApiController
    {
        private EPalContext db = new EPalContext();

        public IHttpActionResult FollowResult([FromBody] ChillMeetLikeViewModel query)
        {
            //Create
            var testFollowId = query.FollowingId;
            var testOwnId = query.UserId;
            Followings memberFollow = new Followings();


            //Delete
            Followings RemoveFollow = db.Followings.FirstOrDefault(x => x.FollowingId == query.FollowingId);
            var isFollow = db.Followings.FirstOrDefault(x => x.FollowingId == query.FollowingId && x.MemberId == query.UserId);



            if (isFollow == null)
            {
                //Create
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
            else
            {
                //Delete
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {

                        db.Entry(RemoveFollow).State = EntityState.Deleted;
                        db.SaveChanges();
                        tran.Commit();

                        return Ok(RemoveFollow);

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
}
