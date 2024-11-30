namespace CoachFrika.Common
{

    public class BaseResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public T Data { get; set; }
        public BaseResponse()
        {
        }

        public BaseResponse(bool status, string message)
        {
            if(status)
            {
                Message = "Successful";
            }
            else
            {
                Message = message;
            }
            Status = status;
        }

        public BaseResponse(bool status, string message, T data)
        {
            if (status)
            {
                Message = "Successful";
            }
            else
            {
                Message = message;
            }
            Status = status;
            Data = data;


        }
        public BaseResponse(bool status, string message, T data, int pageNumber,int pageSize)
        {
            if (status)
            {
                Message = "Successful";
            }
            else
            {
                Message = message;
            }
            Status = status;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;


        }
    }

}
