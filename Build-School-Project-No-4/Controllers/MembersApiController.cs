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
            // = MemberAvatar.MemberAvatar.MemberId;
            //int MemberId, string ProfilePicture

            //string aaa = JsonConvert.DeserializeObject(data);

            Members member = db.Members.First(x => x.MemberId == Data.MemberId);

            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    //Members result = new Members()
                    //{
                    //    ProfilePicture = Data.ProfilePicture
                    //};

                    member.ProfilePicture = Data.ProfilePicture;
                    //db.Members.Add();
                    db.SaveChanges();
                    tran.Commit();

                    return Ok(member);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }

        }












        //// GET: api/MembersApi
        //public IQueryable<Members> GetMembers()
        //{
        //    return db.Members;
        //}

        //// GET: api/MembersApi/5
        //[ResponseType(typeof(Members))]
        //public IHttpActionResult GetMembers(int id)
        //{
        //    Members members = db.Members.Find(id);
        //    if (members == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(members);
        //}

        //// PUT: api/MembersApi/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMembers(int id, Members members)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != members.MemberId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(members).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MembersExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/MembersApi
        //[ResponseType(typeof(Members))]
        //public IHttpActionResult PostMembers(Members members)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Members.Add(members);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = members.MemberId }, members);
        //}

        //// DELETE: api/MembersApi/5
        //[ResponseType(typeof(Members))]
        //public IHttpActionResult DeleteMembers(int id)
        //{
        //    Members members = db.Members.Find(id);
        //    if (members == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Members.Remove(members);
        //    db.SaveChanges();

        //    return Ok(members);
        //}

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