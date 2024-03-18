namespace InventarioMVC.Models.Models.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = default!;


    }

    public class BaseResponseGeneric<T>:BaseResponse
    {
        public T Data { get; set; } = default!;
    }

    public class PaginationBaseResponse<T>: BaseResponse
    {
        public ICollection<T> Data { get; set; } = default!;
        public int Total { get; set; }
    }
}
