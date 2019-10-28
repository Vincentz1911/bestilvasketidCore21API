using System.Collections.Generic;
using BestilVasketidCoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestilVasketidCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentController : ControllerBase
    {
        DBTools dbTools = new DBTools();
        ResidentCRUD rc = new ResidentCRUD();
        UserCRUD uc = new UserCRUD();
        AddressCRUD ac = new AddressCRUD();

        // GET: api/Resident
        [HttpGet]
        public List<DTO_Resident> Get()
        {
            List<Resident> list = rc.ResidentList();
            if (list == null) return null;

            List<DTO_Resident> dto_list = new List<DTO_Resident>();
            foreach (var item in list)
            {
                TimeStamp t = null;
                if (item.Timestamp == null) item.Timestamp = dbTools.CreateTimeStamp(); 
                t = dbTools.GetTimeStamp(item.Timestamp);
                User user = uc.GetUserById(item.User);
                Address address = ac.GetAddressById(item.Address);

                dto_list.Add(new DTO_Resident() { Resident = item, User = user, Address = address, Timestamp = t });
            }
            return dto_list;
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetResidentById")]
        public DTO_User GetUser(int id)
        {
            User u = uc.GetUserById(id);
            TimeStamp t = dbTools.GetTimeStamp(u.Timestamp);
            DTO_User user = new DTO_User() { user = u, timestamp = t};

            return user;
        }

        // POST: api/User (Returns ID of user created)
        [HttpPost]
        public int Post([FromBody] DTO_User dto_user)
        {
            return uc.CreateUser(dto_user.user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] DTO_User dto_user)
        {
            uc.UpdateUser(id, dto_user.user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            uc.DeleteUser(id);
        }
    }
}
