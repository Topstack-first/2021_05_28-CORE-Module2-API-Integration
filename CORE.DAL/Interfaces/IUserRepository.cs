using Core.DAL.Models;
using System.Collections.Generic;

namespace Core.DAL.Services
{
	public interface IUserRepository
	{
		ICollection<User> GetAllUsers();
		User GetUserById(int id);
	}
}
