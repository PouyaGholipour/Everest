using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace DomainServices.Exception
{
    public class ServiceException : System.Exception
    {
        public ServiceException(string type, string title, string detail, IReadOnlyList<string>? errors)
            :base(message:"request failed")
        {
            Type = type;
            Title = title;
            Detail = detail;
            Errors = errors;
        }

        public string Type { get; }
        public string Title { get; }
        public string Detail { get; }

        public readonly IReadOnlyList<string>? Errors;

        public static ServiceException Create
            (
                string type = "ValidationFailure",
                string title = "Validation failure",
                string detail = "One or more validation errors have occured",
                List<string>? errors = null
            ) => new (type: type, title: title, detail: detail, errors : errors);
    }
}
