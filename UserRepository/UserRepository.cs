using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepositoryLib
{
	public class UserRepository
	{
		List<User> list = new List<User>();
		static UserRepository _instance;
		static public UserRepository Instance => _instance ?? (_instance = new UserRepository());
		private UserRepository() { }

		public void Create(User user)
		{
			user.Id = (list.LastOrDefault()?.Id ?? 0) + 1;
			list.Add(user);
		}
		public bool Remove(User user) => list.Remove(user);
		public IEnumerable<User> GetAll() => list;
		public User GetById(int id) => list.Find(user => user.Id == id);
		public void Edit(int id, User user)
		{
			list[list.FindIndex(u => u.Id == id)] = user;
		}
	}
}
