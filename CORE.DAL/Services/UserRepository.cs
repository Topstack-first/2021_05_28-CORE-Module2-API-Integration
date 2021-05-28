using Core.DAL.Contexts;
using Core.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.DAL.Services
{
	public class UserRepository : IUserRepository
	{
		private CoreDBContext _dbContext;

		public UserRepository(CoreDBContext context)
		{
			_dbContext = context;
		}

		public ICollection<User> GetAllUsers()
		{
			return _dbContext.User
				.Where(a=>a.UserApprovalStatus == "Approved" && (bool)a.UserAccountStatus && !(bool)a.DeleteStatus)
				.ToList();
		}
		
		public User GetUserById(int id)
		{
			return _dbContext.User.FirstOrDefault(a => a.UserId == id);
		}
	}
}
