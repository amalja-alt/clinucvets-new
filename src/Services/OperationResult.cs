namespace ClinicVets.Services;

// a simple wrapper to represent the result of an operation, including success/failure status, value (if successful), and error message (if failed)
public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string ErrorMessage { get; }

    private OperationResult(bool isSuccess, T? value, string errorMessage)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static OperationResult<T> Success(T value) => new(true, value, string.Empty);

    public static OperationResult<T> Failure(string errorMessage) => new(false, default, errorMessage);
}
