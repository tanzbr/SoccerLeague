using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoccerLeague.Models;

namespace SoccerLeague.ViewModels
{
    public class TimeStatusViewModel
    {
        public Time Time { get; set; }

        public bool Apto { get; set; }

        public List<string> Pendencias { get; set; }
    }

}
