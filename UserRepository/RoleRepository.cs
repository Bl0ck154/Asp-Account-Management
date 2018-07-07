using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRepositoryLib
{
	public class RoleRepository
	{
		List<Role> list = new List<Role>() {
			new Role(){ Id = 1, Name="User" },
			new Role(){ Id = 2, Name="Admin" } };
		static RoleRepository _instance;
		static public RoleRepository Instance => _instance ?? (_instance = new RoleRepository());
		private RoleRepository() { }

		public void Create(Role role)
		{
			role.Id = (list.LastOrDefault()?.Id ?? 0) + 1;
			list.Add(role);
		}
		public bool Remove(Role role) => list.Remove(role);
		public IEnumerable<Role> GetAll() => list;
		public Role GetById(int id) => list.Find(role => role.Id == id);
		public void Edit(int id, Role role)
		{
			list[list.FindIndex(u => u.Id == id)] = role;
		}
	}
}
