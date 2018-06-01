using DataLayer.Models;
using Explosives.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Explosives.Mapping
{
    public static class UserMapper
    {
        public static UserPO MapDOToPO(UserDO from)
        {
            UserPO to = new UserPO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.RoleID = from.RoleID;

            return to;
        }

        public static UserDO MapPOtoDO(UserPO from)
        {
            UserDO to = new UserDO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.RoleID = from.RoleID;

            return to;
        }

        public static List<UserPO> MapDOToPO(List<UserDO> from)
        {
            List<UserPO> to = new List<UserPO>();

            if (from != null)
            {
                foreach (UserDO item in from)
                {
                    UserPO mappedItem = MapDOToPO(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }

        public static List<UserDO> MapPOtoDO(List<UserPO> from)
        {
            List<UserDO> to = new List<UserDO>();

            if (from != null)
            {
                foreach (UserPO item in from)
                {
                    UserDO mappedItem = MapPOtoDO(item);
                    to.Add(mappedItem);
                }
            }
            return to;
        }

    }
}