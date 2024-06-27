using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.ServiceResults
{
    public static class ServiceResult
    {
        public static void SuccessResult(string message)
        {
            Result.Success(message);
        }

        public static void FailureResult(string message)
        {
            Result.Failure(message);
        }

        public static string RegisterSuccess() => "عملیات با موفقیت انجام شد";
        public static string RegisterFail() => "عملیات با شکست مواجه شد";
    }
}
