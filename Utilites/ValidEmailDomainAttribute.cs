using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.String;

namespace WebApplication12.Utilites
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private  readonly string _allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain)
        {
            this._allowedDomain = allowedDomain;
        }
        
        public override bool IsValid(object value)
        {
            var strings = value.ToString().Split('@');
            return string.Equals(strings[1], _allowedDomain, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
