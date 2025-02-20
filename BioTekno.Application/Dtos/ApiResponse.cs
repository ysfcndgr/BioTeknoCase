using System;
using BioTekno.Domain.Enums;

namespace BioTekno.Application.Dtos
{
    public class ApiResponse<T>
    {
        public ApiStatus Status { get; set; }
        public string ResultMessage { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }
    }
}

