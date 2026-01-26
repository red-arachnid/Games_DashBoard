using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games_DashBoard.Model
{
    public class StoredData
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Game> Games { get; set; } = new List<Game>();
    }
}
