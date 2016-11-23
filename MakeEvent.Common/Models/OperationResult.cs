﻿using System.Collections.Generic;

namespace MakeEvent.Common.Models
{
    public class OperationResult<T>
    {
        public T Result { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static OperationResult Success()
            => new OperationResult { Succeeded = true };

        public static OperationResult<T> Success<T>(T result)
            => new OperationResult<T> { Result = result, Succeeded = true };

        public static OperationResult Fail(IEnumerable<string> errors)
            => new OperationResult { Errors = errors };

        public static OperationResult<T> Fail<T>(IEnumerable<string> errors)
            => new OperationResult<T> { Errors = errors };
    }
}