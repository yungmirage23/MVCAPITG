using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestWebAppl.Models;

namespace ClassLibrary.Models.JwtToken
{
    public  interface IJwtGenerator
    {
        string CreateToken(ApplicationUser user);
    }
}
