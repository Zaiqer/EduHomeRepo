using EduHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome.ViewModels
{
    public class FooterVM
    {
        public Bio Bio { get; internal set; }
        public List<Social> Socials { get; internal set; }
    }
}
