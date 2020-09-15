using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication12.Utilites
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private  readonly string allowedDomain;

        public ValidEmailDomainAttribute(string _allowedDomain)
        {
            this.allowedDomain = _allowedDomain;
        }
        
        public override bool IsValid(object value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowedDomain.ToUpper();
        }
    }
}
