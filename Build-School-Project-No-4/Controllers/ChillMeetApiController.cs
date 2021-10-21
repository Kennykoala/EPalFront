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
    public class ChillMeetApiController : ApiController
    {
        private EPalContext db = new EPalContext();

        public IHttpActionResult PostSwiperResult([FromBody] ChillMeetViewModel swiperData)
        {
            var testLikeId = swiperData.LikeId;
            var testMemberId = swiperData.UserId;

            MeetLikes meetLikes = new MeetLikes();
            //MeetLikes meetLikes = db.MeetLikes.First(x => x.MemberId == swiperData.LikeId);


            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    meetLikes.LikeId = testLikeId;
                    meetLikes.MemberId = testMemberId;
                    db.MeetLikes.Add(meetLikes);
                    //db.Members.Add();
                    db.SaveChanges();
                    tran.Commit();

                    return Ok(meetLikes);
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
