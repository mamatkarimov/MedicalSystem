using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSystem.Application.Models.Results
{
    public class ServiceResult<T> : ServiceResult
    {
        public T Data { get; }

        private ServiceResult(T data, bool success, string message, IEnumerable<string> errors)
            : base(success, message, errors)
        {
            Data = data;
        }

        public static ServiceResult<T> Success(T data, string message = "")
            => new ServiceResult<T>(data, true, message, null);

        public new static ServiceResult<T> Failure(string error)
            => new ServiceResult<T>(default, false, string.Empty, new[] { error });

        public new static ServiceResult<T> Failure(IEnumerable<string> errors)
            => new ServiceResult<T>(default, false, string.Empty, errors);
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public IEnumerable<string> Errors { get; }

        protected ServiceResult(bool success, string message, IEnumerable<string> errors)
        {
            IsSuccess = success;
            Message = message;
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public static ServiceResult Success(string message = "")
            => new ServiceResult(true, message, null);

        public static ServiceResult Failure(string error)
            => new ServiceResult(false, string.Empty, new[] { error });

        public static ServiceResult Failure(IEnumerable<string> errors)
            => new ServiceResult(false, string.Empty, errors);
    }

    //public class ValidationServiceResult : ServiceResult
    //{
    //    public Dictionary<string, List<string>> FieldErrors { get; }
    //}

    //public class PagedServiceResult<T> : ServiceResult<T>
    //{
    //    public int TotalCount { get; }
    //    public int PageSize { get; }
    //    public int CurrentPage { get; }
    //}

}
