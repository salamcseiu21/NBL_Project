﻿using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
   
    public class UserManager
    {
        readonly UserGateway _userGateway = new UserGateway();
        public IEnumerable<User> GetAll => _userGateway.GetAll;
       public User GetUserInformationByUserId(int userId)
        {
            return _userGateway.GetUserInformationByUserId(userId);
        }
        public IEnumerable<User> GetAllUserForAutoComplete()
        {
            return _userGateway.GetAllUserForAutoComplete();
        }
        public ViewUser GetUserByUserNameAndPassword(string userName, string password)
        {
            return _userGateway.GetUserByUserNameAndPassword(userName, password);
        }

        public bool ChangeLoginStatus(ViewUser user, int status)
        {
            return _userGateway.ChangeLoginStatus(user, status);
        }

        public string AddNewUser(User user)
        {
            int rowAffected = _userGateway.AddNewUser(user);
            if (rowAffected > 0)
            {
                return "User Added Successfully!";
            }
            return "Failed to add user";
        }
        public User GetUserByUserName(string userName)
        {
            return _userGateway.GetUserByUserName(userName);
        }
    }
}